using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    public struct Triangle
    {
        public int I0;      // Indices
        public int I1;
        public int I2;
        public Color Color;
        public double FArea;
        public Vector3D VCg;
        public Vector3D VNormal;
    }

    public struct Force
    {
        public Vector3D Vector;
        public Vector3D Position;
        public double Magnitude;
    }
    public struct TriangleWetted
    {
        public int I0;              // Indices
        public int I1;
        public int I2;
        public Color Color;         // Видимый цвет в режиме отладки
        public bool BNoChange;      // это оригинальный треугольник сетки, иначе это новый треугольник, созданный пересечением с водой
        public double Depth;        // глубина
        public double FArea;         // Поверхность треугольника
        public Vector3D VNormal;     // Вектор нормали

        public Vector3D VPressure;   // Векторное давление
        public double FPressure;     // силы давления
        public Vector3D PCPressure;  // Точка приложения силы давления
    }
    class Archimedе
    {
        // Moments of inertia
        double _ixx = 0;
        double _iyy = 0;
        double _izz = 0;
        double _iyx = 0;
        double _izx = 0;
        double _izy = 0;
        double _areaWettedMax;
         double AreaXZ;
        double _areaXzRacCub;




        /// <summary>
        /// Центр тяжести
        /// </summary>
        public Point3D Centroid { get; private set; }
        public double Volume { get; private set; }

        public void Compute()
        {
            ComputeArchimede();
            ListMeshTriToTest();
            TestAllVertices();
            ListWaterTriToTest();
            ComputeAABB();
            ComputeIntersections();

        }


        public void ComputeProperties()
        {
            // Volume Moment of inertia
            // ===========================
            double m = 0; // Mass
            double cx = 0; // Centroid
            double cy = 0;
            double cz = 0;
            double xx = 0; // Moment of inertia tensor
            double yy = 0;
            double zz = 0;
            double yx = 0;
            double zx = 0;
            double zy = 0;


            _areaWettedMax = 0;

            MeshGeometry3D mesh= new MeshGeometry3D();
            Point3DCollection positions= new Point3DCollection();
            

            for (int index = 0; index < mesh.TriangleIndices.Count; index += 3)
            {
                Point3D a = positions[mesh.TriangleIndices[index + 0]];
                Point3D b = positions[mesh.TriangleIndices[index + 2]];
                Point3D c = positions[mesh.TriangleIndices[index + 1]];

                // Signed volume of this tetrahedron.
                double v = a.X * b.Y * c.Z + a.Y * b.Z * c.X + b.X * c.Y * a.Z -
                            (c.X * b.Y * a.Z + b.X * a.Y * c.Z + c.Y * b.Z * a.X);

                // Contribution to the mass
                m += v;
                // Contribution to the centroid
                Point3D d = new Point3D
                {
                    X = a.X + b.X + c.X,
                    Y = a.Y + b.Y + c.Y,
                    Z = a.Z + b.Z + c.Z
                };

                cy += (v * d.Y);
                cx += (v * d.X);
                cz += (v * d.Z);
                // Contribution to moment of inertia monomials
                xx += v * (a.X * a.X + b.X * b.X + c.X * c.X + d.X * d.X);
                yy += v * (a.Y * a.Y + b.Y * b.Y + c.Y * c.Y + d.Y * d.Y);
                zz += v * (a.Z * a.Z + b.Z * b.Z + c.Z * c.Z + d.Z * d.Z);
                yx += v * (a.Y * a.X + b.Y * b.X + c.Y * c.X + d.Y * d.X);
                zx += v * (a.Z * a.X + b.Z * b.X + c.Z * c.X + d.Z * d.X);
                zy += v * (a.Z * a.Y + b.Z * b.Y + c.Z * c.Y + d.Z * d.Y);

                Vector3D uv = b - a;
                Vector3D vv = c - a;
                Vector3D aa = Vector3D.CrossProduct(vv, uv);
                _areaWettedMax += 0.5 * Math.Sqrt(aa.X * aa.X + aa.Y * aa.Y + aa.Z * aa.Z);

            }


            // Centroid.  
            // The case _m = 0 needs to be addressed here.
            double r = 1f / (4f * m);
            Centroid = new Point3D(cx * r, cy * r, cz * r);

            // Volume
            Volume = m / 6f;

            // Moment of inertia about the centroid.
            r = 1f / 120f;
            _iyx = yx * r - Volume * Centroid.Y * Centroid.X;
            _izx = zx * r - Volume * Centroid.Z * Centroid.X;
            _izy = zy * r - Volume * Centroid.Z * Centroid.Y;

            xx = xx * r - Volume * Centroid.X * Centroid.X;
            yy = yy * r - Volume * Centroid.Y * Centroid.Y;
            zz = zz * r - Volume * Centroid.Z * Centroid.Z;

            _ixx = yy + zz;
            _iyy = zz + xx;
            _izz = xx + yy;

            // Divers
            //AreaXZ = Length * Width;
            _areaXzRacCub = (double)Math.Pow(AreaXZ, 1f / 3f);
        }





        public Force Archimede;
        List<Triangle> _mLMeshNewTri;
        List<Vector3D> _mLMeshNewVertex;
        List<int> _mLMeshTriSubmerged;
        List<double> _mLMeshNewHeight;
        const double Gravity = 9.81f;
        const double Waterdensity = 1027f;  // SI = kg / m3
        public List<TriangleWetted> MlAllTriWetted;
        public double AreaWetted;
        public Vector3D Position { get; set; }
        Triangle[] _mTri;

        /// <summary>
        /// mL_MeshNewTri нужно заполнить
        /// mL_MeshTriSubmerged нужно заполнить
        /// </summary>
        void ComputeArchimede()
        {
            MlAllTriWetted = new List<TriangleWetted>();
            AreaWetted = 0f;

            #region Слияние двух списков погруженных треугольников
            //т. е. новые, созданные путем пересечения с поверхностью воды и те, которые полностью погружены

            foreach (Triangle T in _mLMeshNewTri)
            {
                TriangleWetted tw = new TriangleWetted
                {
                    I0 = T.I0,
                    I1 = T.I1,
                    I2 = T.I2,
                    Color = T.Color,
                    BNoChange = false
                };
                Vector3D u = _mLMeshNewVertex[T.I1] - _mLMeshNewVertex[T.I0];
                Vector3D v = _mLMeshNewVertex[T.I2] - _mLMeshNewVertex[T.I0];
                Vector3D a = Vector3D.CrossProduct(v, u);
                tw.FArea = (double)(0.5 * Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z));
                AreaWetted += tw.FArea;

                // отбраковка
                if (tw.FArea != 0)
                {
                    Vector3D b = Vector3D.CrossProduct(u, v);
                    Vector3D va = a - T.VNormal;
                    Vector3D vb = b - T.VNormal;

                    // Если новый треугольник находится в одном вращении индексов,
                    // то векторы a и T.vNormal находятся в одном и том же направлении,
                    // поэтому разность должно быть практически нулевой.
                    // Если это не так, это связано с тем, что направление вращения индексов
                    // должно быть изменено путем замены двух индексов.
                    //
                    if (va.LengthSquared > vb.LengthSquared)
                    {
                        tw.I1 = T.I2;
                        tw.I2 = T.I1;
                        tw.VNormal = -1 * a;
                    }
                    else tw.VNormal = a;
                    tw.VNormal.Normalize();
                }
                else tw.VNormal = new Vector3D(0, 0, 0);

                tw.PCPressure = (_mLMeshNewVertex[T.I0] + _mLMeshNewVertex[T.I1] + _mLMeshNewVertex[T.I2]) / 3;
                MlAllTriWetted.Add(tw);
            }

            foreach (int i in _mLMeshTriSubmerged)
            {
                TriangleWetted TW = new TriangleWetted();
                TW.I0 = _mTri[i].I0;
                TW.I1 = _mTri[i].I1;
                TW.I2 = _mTri[i].I2;
                TW.Color = _mTri[i].Color;
                TW.BNoChange = true;
                TW.FArea = _mTri[i].FArea;
                AreaWetted += TW.FArea;
                Vector3D u = _mLMeshNewVertex[TW.I1] - _mLMeshNewVertex[TW.I0];
                Vector3D v = _mLMeshNewVertex[TW.I2] - _mLMeshNewVertex[TW.I0];
                Vector3D a = Vector3D.CrossProduct(v, u);
                a.Normalize();
                TW.VNormal = a;
                TW.PCPressure = (_mLMeshNewVertex[TW.I0] + _mLMeshNewVertex[TW.I1] + _mLMeshNewVertex[TW.I2]) / 3;
                MlAllTriWetted.Add(TW);
            }
            #endregion

            // Compute Archimede Force
            // =======================
            Archimede = new Force
            {
                Position = Position,
                Vector = new Vector3D(0, 0, 0),
                Magnitude = 0
            };
            if (MlAllTriWetted.Count > 0)
            {
                double tmpSumPressure = 0;
                for (int index = 0; index != MlAllTriWetted.Count; index++)
                {
                    TriangleWetted tw = MlAllTriWetted[index];
                    tw.Depth = -(_mLMeshNewHeight[tw.I0] + _mLMeshNewHeight[tw.I1] + _mLMeshNewHeight[tw.I2]) / 3f;
                    tw.FPressure = Waterdensity * Gravity * tw.Depth * tw.FArea;
                    tw.VPressure = -tw.VNormal * tw.FPressure;
                    MlAllTriWetted[index] = tw;

                    Archimede.Vector += tw.VPressure;
                    Archimede.Position += tw.PCPressure * tw.FPressure;
                    tmpSumPressure += tw.FPressure;
                }
                Archimede.Vector.X = Archimede.Vector.Z = 0;
                Archimede.Position /= tmpSumPressure;
                Archimede.Magnitude = Archimede.Vector.Length;
            }
        }

        private List<int> _mLMeshTriToTest, _mLMeshTriEmerged, _mLVerticesTested;
        void ListMeshTriToTest()
        {
            _mLMeshTriSubmerged = new List<int>();
            _mLMeshTriToTest = new List<int>();
            _mLMeshTriEmerged = new List<int>();

            for (int t = 0; t != _mTri.Length; t++)
            {
                // Если все точки находятся ниже воды, то треугольник находится под водой
                if (_mLVerticesTested[_mTri[t].I0] == 0 && _mLVerticesTested[_mTri[t].I1] == 0 && _mLVerticesTested[_mTri[t].I2] == 0)
                    _mLMeshTriSubmerged.Add(t);

                // В противном случае, если одна из точек находится ниже воды, тогда необходимо проверить треугольник
                else if ((_mLVerticesTested[_mTri[t].I0] == 0 || _mLVerticesTested[_mTri[t].I1] == 0 || _mLVerticesTested[_mTri[t].I2] == 0))
                    _mLMeshTriToTest.Add(t);

                //В противном случае все точки находятся над водой, поэтому треугольник не должен проверяться
                else _mLMeshTriEmerged.Add(t);
            }
        }

        Point3D[] mVertexWorld;

        void TestAllVertices()
        {
            _mLVerticesTested = new List<int>();
            _mLMeshNewHeight = new List<double>();
            int w;
            for (int v = 0; v != mVertexWorld.Length; v++)
            {
                double h = 0;
                //double h = WaterGenerator.GetWaterHeight(mVertexWorld[v]);
                if (mVertexWorld[v].Y <= h)
                    w = 0;      // Под водой
                else
                    w = 1;  // Над водой

                _mLVerticesTested.Add(w);
                _mLMeshNewHeight.Add(mVertexWorld[v].Y - h);
            }
        }



        Dictionary<int, List<Vector3D>> _mDTriPts;   // int = N ° треугольника, List = список точек пересечения
        List<Point3D> _mLVertexWorld;
        List<int> _mLWaterTriToTest;
        private WaveGrid water;
        void ComputeIntersections()
        {
            Point3D m0, m1, m2, w0, w1, w2;
            _mDTriPts = new Dictionary<int, List<Vector3D>>();

            _mLVertexWorld = new List<Point3D>();
             //for (int i = 0; i != GetTransformdPoints3D().Count; i++)
              //  mL_VertexWorld.Add(mVertexWorld[i]);

            foreach (int m in _mLMeshTriToTest)
            {
                foreach (int w in _mLWaterTriToTest)
                {
                    // Triangle du mesh du bateau
                    m0 = mVertexWorld[_mTri[m].I0];
                    m1 = mVertexWorld[_mTri[m].I1];
                    m2 = mVertexWorld[_mTri[m].I2];

                    // Triangle de l'eau
                    w0 = water.mVertex[water.mTri[w].I0];
                    w1 = water.mVertex[water.mTri[w].I1];
                    w2 = water.mVertex[water.mTri[w].I2];

                    // Si les 2 triangles ont une intersection
                    if (Tri_Intersect.Intersection_3d(m0, m1, m2, w0, w1, w2) == 1)
                    {
                        // Ajouter les 2 points d'intersection
                        if (Tri_Intersect.source != Tri_Intersect.target)
                        {
                            if (!_mDTriPts.ContainsKey(m))
                            {
                                List<Vector3D> L = new List<Vector3D>();
                                L.Add(Tri_Intersect.source);
                                L.Add(Tri_Intersect.target);
                                _mDTriPts.Add(m, L);
                            }
                            else
                            {
                                List<Vector3D> L = new List<Vector3D>();
                                _mDTriPts.TryGetValue(m, out L);
                                if (!L.Contains(Tri_Intersect.source)) L.Add(Tri_Intersect.source);
                                if (!L.Contains(Tri_Intersect.target)) L.Add(Tri_Intersect.target);
                                _mDTriPts[m] = L;
                            }
                        }
                    }
                }
            }
        }

        Vector3D mMin, mMax;

        public Archimedе(double areaXz, Triangle[] mTri, Point3D[] mVertexWorld, WaveGrid water)
        {
            AreaXZ = areaXz;
            this._mTri = mTri;
            this.mVertexWorld = mVertexWorld;
            this.water = water;
        }

        void ListWaterTriToTest()
        {
            _mLWaterTriToTest = new List<int>();
            int halfWidth = (int)((water.Dimension - 1) / 2);
            int OffsetX = (water.Dimension - 1) / 2;
            int OffsetZ = (water.Dimension - 1) / 2;
            int t;
            int deltaX = 1;
            int deltaY = 0;
            int debX = (int)mMin.X + OffsetX - deltaX;
            int finX = (int)mMax.X + OffsetX + deltaX;
            int debZ = (int)mMin.Z + OffsetZ - deltaY;
            int finZ = (int)mMax.Z + OffsetZ + deltaY;

            for (int x = debX; x != finX; x++)
            {
                for (int z = debZ; z != finZ; z++)
                {
                    t = 2 * (x * (halfWidth * 2) + z);

                    _mLWaterTriToTest.Add(t);
                    _mLWaterTriToTest.Add(t + 1);
                }
            }
        }


        void ComputeAABB()
        {
            // Compute Axis Aligned Boundig Box
            Point3D min = new Point3D(float.MaxValue, float.MaxValue, float.MaxValue);
            Point3D max = new Point3D(float.MinValue, float.MinValue, float.MinValue);

            for (int v = 0; v != mVertexWorld.Length; v++)
            {
                min = Min(min, mVertexWorld[v]);
                max = Max(max, mVertexWorld[v]);
            }

            // Arrondit Min aux entiers inférieurs et Max aux entiers supérieurs
            mMin.X = (float)Math.Floor(min.X);
            mMin.Y = (float)Math.Floor(min.Y);
            mMin.Z = (float)Math.Floor(min.Z);
            mMax.X = (float)Math.Ceiling(max.X);
            mMax.Y = (float)Math.Ceiling(max.Y);
            mMax.Z = (float)Math.Ceiling(max.Z);
        }


        private static Point3D Max(Point3D a, Point3D b)
        {
            return new Point3D(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        }
        private static Point3D Min(Point3D a, Point3D b)
        {
            return new Point3D(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        }
        public Model3DGroup ModelObj { get; private set; }
        private void ModelWaterLine()
        {
            double xmin = ModelObj.Bounds.X + ModelObj.Bounds.SizeX;
            double xmax = ModelObj.Bounds.X;
            double ymin = ModelObj.Bounds.Y + ModelObj.Bounds.SizeY;
            double ymax = ModelObj.Bounds.Y;

            foreach (GeometryModel3D element in ModelObj.Children)
            {
                MeshGeometry3D va = (MeshGeometry3D)element.Geometry;

                for (int i = 0; i < va.Positions.Count; i++)
                {
                    if (va.Positions[i].Z == 0)
                    {
                        if (va.Positions[i].X > xmax)
                            xmax = va.Positions[i].X;
                        if (va.Positions[i].Y > ymax)
                            ymax = va.Positions[i].Y;
                        if (va.Positions[i].X < xmin)
                            xmin = va.Positions[i].X;
                        if (va.Positions[i].Y < ymin)
                            ymin = va.Positions[i].Y;
                    }
                }


            }
            Point3D p1 = new Point3D(xmin, ymin, 0);
            Point3D p2 = new Point3D(xmax, ymin, 0);
            Point3D p3 = new Point3D(xmin, ymax, 0);
            Point3D p4 = new Point3D(xmax, ymax, 0);
            //Vector3D vec = CalculateNormal(p1, p2, p3);
        }

    }
}
