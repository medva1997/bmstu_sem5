using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Вода
        /// </summary>
        private readonly WaterGenerator _water;

        private bool _rendering;
        private double _lastTimeRendered;
        

        /// <summary>
        /// Объект
        /// </summary>
        private  Model _model;

        // Values to try:
        //   GridSize=20, RenderPeriod=125
        //   GridSize=50, RenderPeriod=50
        private const int GridSize = 200;

        private const double RenderPeriodInMs = 30;
        private const double ZoomPctEachWheelChange = 0.02;

        Point3D _lookto = new Point3D(0, 0, 0);
        private Archimede archimede;

        public MainWindow()
        {
            InitializeComponent();

          
            _model = new Model();
            _water = new WaterGenerator(GridSize);
            archimede= new Archimede(_model,_water);

            meshMain.Positions = _water.GetPoints;
            meshMain.TriangleIndices = _water.TriangleIndices;


            _model = ModelLoader.Loader3DS("");
            //добавление модели на отрисовку
            viewport3D1.Children.Add(_model.ModelVis);
          
            

        }


        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomCamera(e.Delta > 0 ? 1 : 0);
        }
        

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".3ds";

            Nullable<bool> result = dlg.ShowDialog();

            string filename = "";
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                 filename = dlg.FileName;
                OpenButton.Content = filename;
            }
            else
            {
                return;
            }

            viewport3D1.Children.Remove(_model.ModelVis);
            _model = new Model();
            _model = ModelLoader.Loader3DS(filename);
            archimede = new Archimede(_model, _water);

            


            
           
            //добавление модели на отрисовку
            viewport3D1.Children.Add(_model.ModelVis);


        }
        // Start/stop animation
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            archimede = new Archimede(_model, _water);
            if (!_rendering)
            {
                //_water = new WaterGenerator(GridSize);    
                
                
                meshMain.Positions = _water.GetPoints;

                _lastTimeRendered = 0.0;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                btnStart.Content = "Stop";
                _rendering = true;
            }
            else
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                btnStart.Content = "Start";
                _rendering = false;
            }
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //meshMain.GetNormals = _water.GetNormals;
            RenderingEventArgs rargs = (RenderingEventArgs) e;
            if ((rargs.RenderingTime.TotalMilliseconds - _lastTimeRendered) > RenderPeriodInMs)
            {
                //_model.AddTransform = archimede.LoadWaterMatrix();
                _model.Mover(_water);
                //_model.AddTransform = new TranslateTransform3D(-_lookto.X, 0, -_lookto.Z);
                //_model.AddTransform = new RotateTransform3D(
                //    new AxisAngleRotation3D(new Vector3D(0, 1, 0), 2));
                //_model.AddTransform = new TranslateTransform3D(_lookto.X, 0, _lookto.Z);


                // Unhook Positions collection from our mesh, for performance
                // (see http://blogs.msdn.com/timothyc/archive/2006/08/31/734308.aspx)
                meshMain.Positions = null;
                

                // Do the next iteration on the water grid, propagating waves
                _water.ProcessWater();

                // Then update our mesh to use new Z values
                meshMain.Positions = _water.GetPoints;
                

                _lastTimeRendered = rargs.RenderingTime.TotalMilliseconds;
            }
        }

        //Угол в горизонтали в котором смотрит камера
        private double _angle;

        private void CountGorizintal(ref Point3D point, Point3D centre, int dAngle)
        {
            double x = point.X - centre.X;
            double z = point.Z - centre.Z;
            _angle += +dAngle * Math.PI / 180;
            //Trace.WriteLine(angle);

            double r = Math.Sqrt(x * x + z * z);
            point.X = Math.Cos(_angle) * r + centre.X;
            point.Z = Math.Sin(_angle) * r + centre.Z;
        }

        private void RotareCameraInGorizontal(int angle)
        {
            Point3D pos = camMain.Position;
            CountGorizintal(ref pos, _lookto, angle);
            camMain.Position = pos;
            camMain.LookDirection = new Vector3D(_lookto.X - camMain.Position.X, -camMain.Position.Y,
                _lookto.Z - camMain.Position.Z);
        }

        private void ZoomCamera(double k)
        {
            Vector3D zoomDelta = Vector3D.Multiply(ZoomPctEachWheelChange, camMain.LookDirection);
            if (k > 0)
            {
                // Zoom in
                camMain.Position = Point3D.Add(camMain.Position, zoomDelta);
            }
            else
            {
                // Zoom out
                camMain.Position = Point3D.Subtract(camMain.Position, zoomDelta);
            }

        

            camMain.LookDirection = new Vector3D(
                _lookto.X - camMain.Position.X,
                _lookto.Y - camMain.Position.Y,
                _lookto.Z - camMain.Position.Z);
            //Trace.WriteLine(camMain.Position.ToString());
        }

        private void RotareCameraInVerical(int dAngle)
        {
            double y = camMain.Position.Y - _lookto.Y;
            double x = camMain.Position.X - _lookto.X;
            double z = camMain.Position.Z - _lookto.Z;
            double r = Math.Sqrt(x * x + z * z); //горизонтальный радиус
            double lAngle = Math.Atan(y / r) * 180 / Math.PI;


            //Trace.WriteLine("Currenrt "+angle);
            if (dAngle > 0)
            {
                if (lAngle < 80)
                {
                    lAngle += dAngle;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (lAngle > -30)
                {
                    lAngle += dAngle;
                }
                else
                {
                    return;
                }
            }
            //Trace.WriteLine("New " + angle);
            //Trace.WriteLine(angle);
            lAngle = lAngle * Math.PI / 180;

            double r2 = Math.Sqrt(r * r + y * y); // Радиус в вертикали
            double newR = Math.Cos(lAngle) * r2; // радиус на горизонтали уменьшился
            double angle2 = Math.Atan(z / x); //страрое отношение на горизонтальной плоскости
            x = Math.Cos(angle2) * newR + _lookto.X;
            z = Math.Sin(angle2) * newR + _lookto.Z;
            y = Math.Sin(lAngle) * r2 + _lookto.Y;

            camMain.Position = new Point3D(x, y, z);
            camMain.LookDirection = new Vector3D(
                _lookto.X - camMain.Position.X,
                _lookto.Y - camMain.Position.Y,
                _lookto.Z - camMain.Position.Z);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                RotareCameraInGorizontal(5);
            }
            else if (e.Key == Key.Right || e.Key == Key.D)
            {
                RotareCameraInGorizontal(-5);
            }
            else if (e.Key == Key.Up || e.Key == Key.W)
            {
                RotareCameraInVerical(5);
            }
            else if (e.Key == Key.Down || e.Key == Key.D)
            {
                RotareCameraInVerical(-5);
            }
            else if (e.Key == Key.Subtract || e.Key == Key.E)
            {
                ZoomCamera(-1);
            }
            else if (e.Key == Key.Add || e.Key == Key.Q)
            {
                ZoomCamera(1);
            }

            Trace.WriteLine(camMain.Position.ToString());
        }

        private void ModelChanger_OnClick(object sender, RoutedEventArgs e)
        {
            Zoomer(1.5);
        }

        private double _currentScale = 1;

        private void Zoomer(double sc)
        {
            _currentScale *= sc;
            _model.AddTransform = new ScaleTransform3D(_currentScale, _currentScale, _currentScale);
        }

        private  void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
             _water.ShowSettings();
            
        }
    }
}