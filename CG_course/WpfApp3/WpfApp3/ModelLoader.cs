using System;

using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    class ModelLoader
    {


        public static Model Loader3DS(string file)
        {
            if(file=="")
                file = @"barrel.3ds";
            Model m=  new Model();
            
            //file = @"C:\Users\medva\Downloads\qs78rgoddkw0-pirateship\pirateships.3ds";
            m.AddMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(255, 152, 118, 84)));
            m.AddTransform=new ScaleTransform3D(1,1,1);
           
            m.LoadModel(file);

            if (file.Contains("barrel.3ds"))
            {
                m.AddTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90));
                m.AddTransform = new TranslateTransform3D(0, m.Hight / 4, 0);
            }
            else
            {
                m.AddTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(-1, 0, 0), 90));
                m.AddTransform = new TranslateTransform3D(0, m.Hight / 8+5, 0);
            }
            
            m.AddTransform= new TranslateTransform3D(0, m.Hight / 4,0);
            m.AddMaterial= new EmissiveMaterial(new SolidColorBrush(Color.FromRgb(152, 118, 84)));
            return m;

        }

        public static Model3DGroup testLoad()
        {
            Model3DGroup newModel3DGroup;
            Reader3Ds newReader3ds;
            newReader3ds = new Reader3Ds();
            string file = @"C:\Users\medva\Downloads\Курсовик\Бочка\ln5gjg95fj-barrel_3ds\ton_3ds\barrel.3ds";
            file = @"C:\Users\medva\Downloads\qs78rgoddkw0-pirateship\pirateships.3ds";

              
            MaterialGroup myMaterialGroup = new MaterialGroup();
            Material mt1 = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(255, 152, 118, 84)));
            //Material mt2 = new SpecularMaterial(new SolidColorBrush(Color.FromRgb(191, 239, 255)), 50);
            Material mt3 = new EmissiveMaterial(new SolidColorBrush(Color.FromRgb(152, 118, 84)));
            myMaterialGroup.Children.Add(mt1);
            myMaterialGroup.Children.Add(mt3);
            newReader3ds.DefaultMaterial= myMaterialGroup;
            newModel3DGroup = newReader3ds.ReadFile(file);

           

           
            TranslateTransform3D move = new TranslateTransform3D(50, 0, 50);
            Transform3DGroup transform3D = new Transform3DGroup();
            transform3D.Children.Add(move);
            //transform3D.Children.Add(rotate);
            newModel3DGroup.Transform = transform3D;
           

         
            return newModel3DGroup;
        }
        public static ModelVisual3D LoadModel(string patchToFolder)
        {
            ModelVisual3D model= new ModelVisual3D();
            GeometryModel3D model3D =new GeometryModel3D();
            MeshGeometry3D mesh= new MeshGeometry3D();
            Trace.WriteLine("Start Load GetNormals");
            mesh.Normals = LoadNormals(patchToFolder + @"\GetNormals.txt");
            Trace.WriteLine("Start Load Positions");
            mesh.Positions = LoadPositions(patchToFolder + @"\Positions.txt");
            //mesh.TextureCoordinates = LoadTextureCoordinates(patchToFolder + @"\TextureCoordinates.txt");
            Trace.WriteLine("Start Load TriangleIndices");
            mesh.TriangleIndices = LoadTriangleIndices(patchToFolder + @"\TriangleIndices.txt");
            model3D.Geometry = mesh;

            MaterialGroup myMaterialGroup = new MaterialGroup();
            Material mt1 = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(255, 152, 118, 84)));
            //Material mt2 = new SpecularMaterial(new SolidColorBrush(Color.FromRgb(191, 239, 255)), 50);
            Material mt3 = new EmissiveMaterial(new SolidColorBrush(Color.FromRgb(152, 118,84)));

            if (patchToFolder.Contains("ring"))
            {
                mt1 = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)));
                //Material mt2 = new SpecularMaterial(new SolidColorBrush(Color.FromRgb(191, 239, 255)), 50);
               mt3 = new EmissiveMaterial(new SolidColorBrush(Color.FromRgb(128, 128, 128)));
            }

            myMaterialGroup.Children.Add(mt1);
            myMaterialGroup.Children.Add(mt3);
            model3D.Material = myMaterialGroup;

            //ImageBrush br = new ImageBrush();
            //br.ImageSource = new BitmapImage(new Uri((@"C:\Users\medva\Documents\ln5gjg95fj-barrel_3ds\ton_3ds\PlanksNe.jpg")));
            //br.ViewboxUnits = BrushMappingMode.Absolute;
            //br.TileMode = TileMode.Tile;
            //br.Transform = Transform.Parse("1,0,0,-1,0,1");
            //Material mt2 = new DiffuseMaterial(br);

            //ImageBrush brback = new ImageBrush();
            //br.ImageSource = new BitmapImage(new Uri((@"C:\Users\medva\Documents\ln5gjg95fj-barrel_3ds\ton_3ds\PlanksNe.jpg")));
            //br.ViewboxUnits = BrushMappingMode.Absolute;
            //br.TileMode = TileMode.Tile;
            //br.Transform = Transform.Parse("1,0,0,-1,0,1");
            //Material mt2back = new DiffuseMaterial(brback);

            //model3D.Material = mt2;
            //model3D.BackMaterial = mt2back;
            //// Apply a transform to the object. In this sample, a rotation transform is applied,   
            //RotateTransform3D myRotateTransform3D = new RotateTransform3D();
            //AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D
            //{
            //    Axis = new Vector3D(1, 0, 0),
            //    Angle = 180
            //};
            //myRotateTransform3D.Rotation = myAxisAngleRotation3d;
            //myGeometryModel.Transform = myRotateTransform3D;
            ScaleTransform3D scale =new ScaleTransform3D(15,15,15);
            TranslateTransform3D move = new TranslateTransform3D(50, 0, 50);
            Transform3DGroup transform3D=new Transform3DGroup();
            transform3D.Children.Add(scale);
            transform3D.Children.Add(move);
            model.Transform = transform3D;

            model.Content = model3D;

            return model;

        }


        private static void  ReadFile(string patch, ref Point3DCollection collection)
        {
            FileStream fileStream = new FileStream(patch, FileMode.Open, FileAccess.Read);
            byte i = 0;
            double temp=0;
            Point3D point= new Point3D();
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line == "")
                        continue;
                    //Trace.WriteLine(line);
                    line = line.Replace(" ", "");
                    line = line.Replace(",", ".");
                    try
                    {
                        temp = double.Parse(line);
                    }
                    catch
                    {
                        Trace.WriteLine("Fail" + line);
                        continue;
                    }
                        //Trace.WriteLine(temp);
                    if (i == 0) point.X = temp;
                    else if (i == 1) point.Y = temp;
                    else if (i == 2)
                    {
                        point.Z = temp;
                        collection.Add(point);
                        point=new Point3D();
                    }
                    i++;
                    if (i == 3) i = 0;
                }
            }
        }

        private static void ReadFile(string patch, ref Vector3DCollection collection)
        {
            FileStream fileStream = new FileStream(patch, FileMode.Open, FileAccess.Read);
            byte i = 0;
            double temp = 0;
            Vector3D point = new Vector3D();

            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line == "")
                        continue;
                    //Trace.WriteLine(line);
                    line = line.Replace(" ", "");
                    line = line.Replace(",", ".");
                    try
                    {
                        temp = double.Parse(line);
                    }
                    catch
                    {
                        Trace.WriteLine("Fail" + line);
                        continue;
                    }
                    //Trace.WriteLine(temp);
                    if (i == 0) point.X = temp;
                    else if (i == 1) point.Y = temp;
                    else if (i == 2)
                    {
                        point.Z = temp;
                        collection.Add(point);
                        point = new Vector3D();
                    }
                    i++;
                    if (i == 3) i = 0;
                }
            }
        }

        public static Vector3DCollection LoadNormals(string patch)
        {
            Vector3DCollection normals = new Vector3DCollection();
            ReadFile(patch, ref normals);
            return normals;
        }

        public static Point3DCollection LoadPositions(string patch)
        {
            Point3DCollection positions = new Point3DCollection();
            ReadFile(patch, ref positions);
            return positions;
        }

        public static Point3DCollection LoadTextureCoordinates(string patch)
        {
            Point3DCollection texture = new Point3DCollection();
            ReadFile(patch, ref texture);
            return texture;

        }

        public static Int32Collection LoadTriangleIndices(string patch)
        {
            Int32Collection indices = new Int32Collection();
            FileStream fileStream = new FileStream(patch, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        int temp = Convert.ToInt32(line);
                        indices.Add(temp);
                    }
                    catch
                    {
                        Trace.WriteLine("Fail" + line + ";");
                    }
                }
            }
            return indices;

        }


    }
}
