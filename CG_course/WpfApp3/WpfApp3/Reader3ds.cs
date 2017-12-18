using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    /// <summary>
    /// http://www.martinreddy.net/gfx/3d/3DS.spec
    /// </summary>
    class Reader3Ds
    {
        #region Properties

        private Model3DGroup _currentModelGroup;
        private MeshGeometry3D _currentMesh;
        public List<MeshGeometry3D> Meshes { get; }
        /// <summary>
        /// Set the material that will be applied to the read objects
        /// </summary>
        public MaterialGroup DefaultMaterial { get; set; }

        /// <summary>
        /// Add defult light (Direction=0, 0, -1) if there are no lights defined in 3ds file
        /// </summary>
        public bool AddDefaultLight { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Reader3Ds()
        {
            Meshes = new List<MeshGeometry3D>();
        }

        #endregion

        #region ReadFile

        /// <summary>
        /// Reads 3ds file and returns its data in MeshGeometry3D object
        /// </summary>
        /// <param name="fileName">3ds file Name</param>
        /// <returns>read MeshGeometry3D object</returns>
        public Model3DGroup ReadFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                BinaryReader r = new BinaryReader(fs);

                while (r.PeekChar() != -1) // Until the end
                    ReadChunk(r);

                r.Close();
            }

            _currentModelGroup = new Model3DGroup();
            _currentModelGroup.Children.Clear();


            MAxMin(out Point3D max, out Point3D min);

            Point3D midPoint3D= new Point3D((max.X+min.X)/2, (max.Y + min.Y) / 2, (max.Z + min.Z) / 2);
            int d_size = 30;
            Point3D scale= new Point3D(d_size/ (max.X - min.X), d_size / (max.Y - min.Y), d_size/ (max.Z - min.Z));
            double sc = Math.Min(scale.X, Math.Min(scale.Y, scale.Z));
            if (sc<1)
                sc = Math.Min(1/scale.X, Math.Min(1/scale.Y, 1/scale.Z));
            scale = new Point3D(sc,sc,sc);

            
            

            foreach (MeshGeometry3D t in Meshes)
            {
                for (int j = 0; j < t.Positions.Count; j++)
                {
                    t.Positions[j] = new Point3D(t.Positions[j].X - midPoint3D.X,
                        t.Positions[j].Y - midPoint3D.Y,
                        t.Positions[j].Z - midPoint3D.Z);

                    if (!Double.IsInfinity(scale.X) || !Double.IsInfinity(scale.Y) || !Double.IsInfinity(scale.Z))
                    {
                        t.Positions[j] = new Point3D(t.Positions[j].X *scale.X,
                            t.Positions[j].Y *scale.Y,
                            t.Positions[j].Z * scale.Z);
                    }
                }
            }

            //MAxMin(out Point3D max2, out Point3D min2);
            //double delta = ((max2.Z + min2.Z) / 2 - min2.Z)/2;
            //delta = ((max2.Z + min2.Z) / 2 - min2.Z);



            //foreach (MeshGeometry3D t in Meshes)
            //{
            //    for (int j = 0; j < t.Positions.Count; j++)
            //    {
            //        t.Positions[j] = new Point3D(t.Positions[j].X,
            //            t.Positions[j].Z + delta,
            //            t.Positions[j].Y);
            //    }

            //}
            //дополнительяная сеть
            MeshGeometry3D mesh = new MeshGeometry3D();
            int n = 10;
            mesh.Positions.Add(new Point3D(0, -n, n));
            mesh.Positions.Add(new Point3D(0, 0, -n));
            mesh.Positions.Add(new Point3D(0, n, n));

            //mesh.Positions.Add(new Point3D(0, 0, -10));
            //mesh.Positions.Add(new Point3D(0, 10, 0));
            //mesh.Positions.Add(new Point3D(0, 0, 10));

            Meshes.Add(mesh);

            foreach (MeshGeometry3D oneMesh in Meshes)
            {
                GeometryModel3D oneGeometryModel = new GeometryModel3D(oneMesh, DefaultMaterial);
                oneGeometryModel.Material = DefaultMaterial;

                _currentModelGroup.Children.Add(oneGeometryModel);
            }

            return _currentModelGroup;
        }

        private void MAxMin(out Point3D max, out Point3D min)
        {
            max = Meshes[0].Positions[0];
            min = Meshes[0].Positions[0];

            foreach (MeshGeometry3D oneMesh in Meshes)
            {
                foreach (Point3D oneMeshPosition in oneMesh.Positions)
                {
                    if (oneMeshPosition.X > max.X) max.X = oneMeshPosition.X;
                    if (oneMeshPosition.Y > max.Y) max.Y = oneMeshPosition.Y;
                    if (oneMeshPosition.Z > max.Z) max.Z = oneMeshPosition.Z;

                    if (oneMeshPosition.X < min.X) min.X = oneMeshPosition.X;
                    if (oneMeshPosition.Y < min.Y) min.Y = oneMeshPosition.Y;
                    if (oneMeshPosition.Z < min.Z) min.Z = oneMeshPosition.Z;
                }
            }
        }

        #endregion

        #region ReadChunk - main

        /// <summary>
        /// Main chunk reading method
        /// </summary>
        /// <param name="r">BinaryReader</param>
        /// <returns>readed chunk lenghth</returns>
        private void ReadChunk(BinaryReader r)
        {
            ushort chunkHeader = r.ReadUInt16();
            int chunkLength = r.ReadInt32();

            switch (chunkHeader)
            {
                case ChunkIds.M3DVersion:
                case ChunkIds.MeshVersion:
                case ChunkIds.MasterScale:
                    r.ReadInt32(); // just read
                    break;

                case ChunkIds.NamedObject:
                    _currentMesh = new MeshGeometry3D();
                    Meshes.Add(_currentMesh);
                    ReadString(r); // just read
                    break;

                case ChunkIds.PointArray:
                    Int16 vertexCount;

                    vertexCount = r.ReadInt16();

                    for (int i = 0; i < vertexCount; i++)
                    {
                        Point3D currentVertex = new Point3D
                        {
                            X = r.ReadSingle(),
                            Y = r.ReadSingle(),
                            Z = r.ReadSingle()
                        };


                        _currentMesh.Positions.Add(currentVertex);
                    }

                    break;

                case ChunkIds.FaceArray:
                    Int32 facesCount;

                    facesCount = r.ReadInt16();

                    for (int i = 0; i < facesCount; i++)
                    {
                        // indexed to Posiotions (points)
                        short vertex1 = r.ReadInt16();
                        short vertex2 = r.ReadInt16();
                        short vertex3 = r.ReadInt16();

                        r.ReadInt16(); //just read

                        _currentMesh.TriangleIndices.Add(vertex1);
                        _currentMesh.TriangleIndices.Add(vertex2);
                        _currentMesh.TriangleIndices.Add(vertex3);
                    }

                    break;

                case ChunkIds.TriMappingcoors:
                    int coordinatsCount;

                    coordinatsCount = r.ReadInt16();

                    for (int i = 0; i < coordinatsCount; i++)
                    {
                        float tu = r.ReadSingle();
                        float tv = r.ReadSingle();

                        _currentMesh.TextureCoordinates.Add(new Point(tu, tv));
                    }
                    break;

                case ChunkIds.NCamera:
                case ChunkIds.NDirectLight:
                case ChunkIds.M3Dmagic:
                case ChunkIds.Mmagic:
                case ChunkIds.NTriObj:
                    //Skip it
                    break;

                default:
                    r.ReadBytes(chunkLength - 6); // skip unhandled chunk
                    break;
            }
        }

        #endregion

        #region ReadString

        /// <summary>
        /// Reads characters until '\0' and packs them into string - unlimited size
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private void ReadString(BinaryReader r)
        {
            ReadString(r, int.MaxValue);
        }

        /// <summary>
        /// Reads characters until '\0' and packs them into string - max size
        /// </summary>
        /// <param name="r"></param>
        /// <param name="maxLength">max string length; maxInt for unlimited</param>
        /// <returns></returns>
        private static void ReadString(BinaryReader r, int maxLength)
        {
            char oneChar = r.ReadChar();
            while (oneChar != '\0' && maxLength-- > 0)
            {
                oneChar = r.ReadChar();
            }
        }
        #endregion
    }

    class ChunkIds
    {
        public const ushort Mmagic = 0x3D3D;	/* Mesh magic */
        public const ushort M3Dmagic = 0x4D4D;
        public const ushort M3DVersion = 0x0002;
        public const ushort Smagic = 0x2D2D;	/* Shaper magic */
        public const ushort Lmagic = 0x2D3D;	/* Lofter magic */
        public const ushort Mlibmagic = 0x3DAA;	/* Material library magic */
        public const ushort Matmagic = 0x3DFF;	/* Material magic */

        public const ushort MeshVersion = 0x3D3E;  /* version chunk */
        public const ushort MasterScale = 0x0100;

        public const ushort NamedObject = 0x4000;

        public const ushort NTriObj = 0x4100;

        public const ushort PointArray = 0x4110;
        public const ushort FaceArray = 0x4120;
        public const ushort TriMappingcoors = 0x4140; //TEX_VERTS

        public const ushort NDirectLight = 0x4600;
        public const ushort DlSpotlight = 0x4610;

        public const ushort NCamera = 0x4700;

        public const ushort ColorF = 0x0010; /* RGB */
        public const ushort Color24 = 0x0011; /* True color */
    }
}