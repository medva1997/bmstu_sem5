using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    class WaterGenerator
    {
        // Private member data
        /// <summary>
        /// Первый буфер для храния сети
        /// </summary>
        private readonly Point3DCollection _ptBuffer1;
        /// <summary>
        /// Второй буфер для хранения сети
        /// </summary>
        private Point3DCollection _ptBuffer2;
        /// <summary>
        /// Треугольники сети
        /// </summary>
        private readonly Int32Collection _triangleIndices;
        /// <summary>
        /// Размер поля по одной стороне
        /// </summary>
        private readonly int _dimension;

        private readonly Vector3DCollection _getNormals;

        
        // These two pointers will swap, pointing to ptBuffer1/ptBuffer2 as we cycle the buffers
        private Point3DCollection _currBuffer;
        private Point3DCollection _oldBuffer;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dimension">Размер поля по одной стороне</param>
        public WaterGenerator(int dimension)
        {
            _ptBuffer1 = new Point3DCollection(dimension * dimension);
            _ptBuffer2 = new Point3DCollection(dimension * dimension);
            _getNormals= new Vector3DCollection(dimension * dimension);
            _triangleIndices = new Int32Collection((dimension - 1) * (dimension - 1) * 2);

            _dimension = dimension;

            InitializePointsAndTriangles();

            _currBuffer = _ptBuffer2;
            _oldBuffer = _ptBuffer1;

           
        }

        /// <summary>
        /// Access to underlying grid data
        /// </summary>
        public Point3DCollection GetPoints => _currBuffer;

        public Vector3DCollection GetNormals => _getNormals;

        /// <summary>
        /// Access to underlying triangle index collection
        /// </summary>
        public Int32Collection TriangleIndices => _triangleIndices;

        /// <summary>
        /// Leave buffers in place, but change notation of which one is most recent
        /// </summary>
        private void SwapBuffers()
        {
            Point3DCollection temp = _currBuffer;
            _currBuffer = _oldBuffer;
            _oldBuffer = temp;
        }


        #region func5

        private List<WaveParams> wParams=new List<WaveParams>();


        private int _numOfWavesToSum;
       

        private void Init_M()
        {
            if (_numOfWavesToSum > 0) return;

            WaveParams tempW = new WaveParams(wParams.Count+1);
            tempW.SetParams(70,5,1,10,355);
            wParams.Add(tempW);

            tempW = new WaveParams(wParams.Count + 1);
            tempW.SetParams(30, 2, 2, 5, 40);
            wParams.Add(tempW);

            tempW = new WaveParams(wParams.Count + 1);
            tempW.SetParams(0.28, 0.06, 1, 0.04, 200);
            wParams.Add(tempW);

            tempW = new WaveParams(wParams.Count + 1);
            tempW.SetParams(0.5, 0.08, 3, 0.05, 5);
            wParams.Add(tempW);

            _numOfWavesToSum = wParams.Count;
        }

        public  void ShowSettings()
        {
            WaterSettings wt = new WaterSettings(wParams);
            wt.Show();
            
            //return Task.CompletedTask; 
        }

        private double func(int x, int z, double t)
        {
            Init_M();


            double rez =0;
            for (int k = 0; k < _numOfWavesToSum; k++)
            {
                rez += wParams[k].WaveGenFunc(x, z, _timer);
            }

            return rez;
        }

        #endregion


        private void InitializePointsAndTriangles()
        {
            _ptBuffer1.Clear();
            _ptBuffer2.Clear();
            _triangleIndices.Clear();

            int nCurrIndex = 0; // March through 1-D arrays

            for (int row = 0; row < _dimension; row++)
            {
                for (int col = 0; col < _dimension; col++)
                {
                    // In grid, X/Y values are just row/col numbers
                    _ptBuffer1.Add(new Point3D(col - _dimension / 2, func(col,row,0), row - _dimension / 2));

                    // Completing new square, add 2 triangles
                    if ((row > 0) && (col > 0))
                    {
                        // Triangle 1
                        _triangleIndices.Add(nCurrIndex - _dimension - 1);
                        _triangleIndices.Add(nCurrIndex);
                        _triangleIndices.Add(nCurrIndex - _dimension);

                        // Triangle 2
                        _triangleIndices.Add(nCurrIndex - 1);
                        _triangleIndices.Add(nCurrIndex);
                        _triangleIndices.Add(nCurrIndex - _dimension - 1);

                    }
                    _getNormals.Add(new Vector3D());
                    nCurrIndex++;
                }
            }

            // 2nd buffer exists only to have 2nd set of Z values
            _ptBuffer2 = _ptBuffer1.Clone();
        }

        private double _timer = 0;
        /// <summary>
        /// Determine next state of entire grid, based on previous two states.
        /// This will have the effect of propagating ripples outward.
        /// </summary>
        public  void ProcessWater()
        {
            int nPtIndex = 0; // Index that marches through 1-D point array

            // Remember that Y value is the height (the value that we're animating)
            for (int row = 0; row < _dimension; row++)
            {
                for (int col = 0; col < _dimension; col++)
                {
                    _oldBuffer[nPtIndex]=new Point3D(col-_dimension/2, func(col, row, _timer), row - _dimension / 2);
                    
                    nPtIndex++;
                }
            }

            for (int i = 0; i < _getNormals.Count; i++)
            {
                _getNormals[i] = CreateNormal(_oldBuffer[TriangleIndices[i*3]], _oldBuffer[TriangleIndices[i*3+1]],
                    _oldBuffer[TriangleIndices[i*3+2]]);
            }
            
            SwapBuffers();
            _timer+=0.1;
        }

        private static Vector3D CreateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            Vector3D res = Vector3D.CrossProduct(v0, v1);
               
            return res;
        }

       

      
        public Point3D GetWaterHeightPoint(int x, int z)
        {
            if (x > -_dimension && z > -_dimension && x < _dimension && z < _dimension)
            {
                return _currBuffer[(z + _dimension / 2) * _dimension + (x + _dimension / 2)];

            }
            return new Point3D(0, 0, 0);
        }
        public Point3D GetWaterHeightPoint(Point3D p)
        {
            int x = (int)(p.X);
            int z = (int)(p.Z);
            if (x > -_dimension && z > -_dimension && x < _dimension && z < _dimension)
            {
                return _currBuffer[(z+ _dimension/2) * _dimension + (x+ _dimension/2)];

            }
            return new Point3D(0, 0, 0);
        }



    }
}





