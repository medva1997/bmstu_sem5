using System;
using System.Windows.Media.Media3D;


namespace WpfApp3
{
   

    class Model
    {
        private readonly ModelVisual3D _modelVis = new ModelVisual3D();
        /// <summary>
        /// список материалов
        /// </summary>
        private readonly MaterialGroup _myMaterials = new MaterialGroup();
        /// <summary>
        /// список трансформаций
        /// </summary>
        private readonly Transform3DGroup _transforms = new Transform3DGroup();

        /// <summary>
        /// Возвращат текущие значения модели после примениея всех трансформаций
        /// </summary>
        /// <returns></returns>
        public Point3DCollection GetTransformdPoints3D()
        {
            Point3DCollection currentPoints= new Point3DCollection();
            foreach (var model3D in ModelObj.Children)
            {
                var element = (GeometryModel3D) model3D;
                MeshGeometry3D va = (MeshGeometry3D)element.Geometry;
                Transform3D trans = ModelObj.Transform;
                foreach (Point3D point in va.Positions)
                {
                    currentPoints.Add(trans.Transform(point));
                }
            }
            return currentPoints;
        }

        public Point3DCollection GetTransformdPoints3DLast3()
        {
            Point3DCollection currentPoints = new Point3DCollection();
            var model3D = ModelObj.Children[ModelObj.Children.Count - 1];
            var element = (GeometryModel3D)model3D;
            MeshGeometry3D va = (MeshGeometry3D)element.Geometry;
            Transform3D trans = ModelObj.Transform;
            foreach (Point3D point in va.Positions)
            {
                currentPoints.Add(trans.Transform(point));
            }
            
            return currentPoints;
        }
        /// <summary>
        /// Возвращат оригинальные значения модели
        /// </summary>
        /// <returns></returns>
        public Point3DCollection GetOriginalPoint3D()
        {
            return GetMesh.Positions;
        }

        private MeshGeometry3D GetMesh
        {
            get
            {
                var model3D = ModelObj.Children[0];
                var element = (GeometryModel3D) model3D;
                return (MeshGeometry3D) element.Geometry;
            }
        }

        /// <summary>
        /// Получение оригинальных размеров модели
        /// </summary>
        public Rect3D GetOriginalBounds
        {
            get
            {
                var model3D = ModelObj.Children[0];
                var element = (GeometryModel3D)model3D;
                return element.Geometry.Bounds;
            }
        }
        /// <summary>
        /// Получение размеров модели после трансформации
        /// </summary>
        public Rect3D GetTransformdBounds => ModelObj.Bounds;

        
        public double Length => GetTransformdBounds.SizeX;
        public double Width => GetTransformdBounds.SizeZ;
        public double Hight => GetTransformdBounds.SizeY;



        
        public ModelVisual3D ModelVis => _modelVis;
        public Model3DGroup ModelObj { get; private set; }

        public Material AddMaterial
        {
            set => _myMaterials.Children.Add(value);
        }

        public Transform3D AddTransform
        {
            set => _transforms.Children.Add(value);
        }

        public void LoadModel(string path)
        {
            Reader3Ds reader3Ds = new Reader3Ds {DefaultMaterial = _myMaterials};
            ModelObj = reader3Ds.ReadFile(path);
            ModelObj.Transform = _transforms;
            _modelVis.Content = ModelObj;
            
        }
        
        private Vector3D CreateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }

        private Vector3D _moveToZeroVector3D(Rect3D rect)
        {
            Point3D centr= rect.Location;
            centr.X += rect.SizeX / 2;
            centr.Y += rect.SizeY / 2;
            centr.Z += rect.SizeZ / 2;
            return  new Vector3D(-centr.X,-centr.Y,-centr.Z);
        }

        private double _vectorLen(Vector3D vect)
        {
            return Math.Abs(Math.Sqrt(vect.X * vect.X + vect.Y * vect.Y + vect.Z * vect.Z));
        }
        public void Mover(WaterGenerator wt)
        {
            Point3DCollection points = GetTransformdPoints3DLast3();


            Point3D p1 = points[points.Count-1];
            Point3D p2 = points[points.Count-2];
            Point3D p3 = points[points.Count-3];
            
            //Trace.WriteLine("p1 "+p1);
            //Trace.WriteLine("p2 "+ p2);
            //Trace.WriteLine("p3 "+p3);

            
            Vector3D aa= CreateNormal(p1,p2,p3);

            double lenA = _vectorLen(aa);

            //Trace.WriteLine("aa " + aa);

            Point3D sea1 = wt.GetWaterHeightPoint(p1);
            Point3D sea2 = wt.GetWaterHeightPoint(p2);
            Point3D sea3 = wt.GetWaterHeightPoint(p3);

            //Trace.WriteLine("sea1 " + sea1);
            //Trace.WriteLine("sea2 " + sea2);
            //Trace.WriteLine("sea3 " + sea3);

            Vector3D bb = CreateNormal(sea1,sea2,sea3);
            double lenB = _vectorLen(bb);

            double c = Vector3D.DotProduct( bb, aa);
            double an2 = c / (lenA * lenB);
            //Trace.WriteLine("bb " + bb);

            Vector3D ccN=Vector3D.CrossProduct(aa,bb);
            //Trace.WriteLine("ccN " + bb);

            //Vector3D zero = MoveToZeroVector3D(GetTransformdBounds);

            //AddTransform = new TranslateTransform3D(zero);
            //double angle = Vector3D.AngleBetween(aa, bb);
            AxisAngleRotation3D ang = new AxisAngleRotation3D(ccN, an2);
            AddTransform = new RotateTransform3D(ang);
            //AddTransform = new TranslateTransform3D(new Vector3D(0,-2,0));
            //zero.Negate();
            //AddTransform = new TranslateTransform3D(zero);


            //Quaternion q= new Quaternion(aa,angle );
            //Rotation3D rt = new QuaternionRotation3D(q);
            ////AxisAngleRotation3D ang= new AxisAngleRotation3D(Vector3D.CrossProduct(bb,aa), -angle);
            ////RotateTransform3D rotate = new RotateTransform3D(ang);

            //AddTransform = rotate;
            //Trace.WriteLine(sea1);
            //Trace.WriteLine(sea2);
            //Trace.WriteLine(sea3);


            //Trace.WriteLine("ang "+angle);
            //Trace.WriteLine("ang2 " + an2);
        }

    }
}