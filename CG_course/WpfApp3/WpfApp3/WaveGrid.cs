using System;

using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    class WaveGrid
    {
        // Constants
        const int MinDimension = 5;

        const double Damping = 0.96;
        const double SmoothingFactor = 2.0; // Gives more weight to smoothing than to velocity

        // Private member data
        private Point3DCollection _ptBuffer1;
        private Point3DCollection _ptBuffer2;
        private Int32Collection _triangleIndices;

       

        public Triangle[] mTri;

        public Point3DCollection mVertex => _currBuffer;


        private int _dimension;

        // Pointers to which buffers contain:
        //    - Current: Most recent data
        //    - Old: Earlier data
        // These two pointers will swap, pointing to ptBuffer1/ptBuffer2 as we cycle the buffers
        private Point3DCollection _currBuffer;

        private Point3DCollection _oldBuffer;

        public WaveGrid(int Dimension)
        {
            if (Dimension < MinDimension)
                throw new ApplicationException(
                    string.Format("Dimension must be at least {0}", MinDimension.ToString()));

            _ptBuffer1 = new Point3DCollection(Dimension * Dimension);
            _ptBuffer2 = new Point3DCollection(Dimension * Dimension);
            _triangleIndices = new Int32Collection((Dimension - 1) * (Dimension - 1) * 2);

            _dimension = Dimension;

            InitializePointsAndTriangles();

            _currBuffer = _ptBuffer2;
            _oldBuffer = _ptBuffer1;
        }

        /// <summary>
        /// Access to underlying grid data
        /// </summary>
        public Point3DCollection Points
        {
            get { return _currBuffer; }
        }

        /// <summary>
        /// Access to underlying triangle index collection
        /// </summary>
        public Int32Collection TriangleIndices
        {
            get { return _triangleIndices; }
        }

        /// <summary>
        /// Dimension of grid--same dimension for both X & Y
        /// </summary>
        public int Dimension
        {
            get { return _dimension; }
        }


        public void SetCenterPeak(double PeakValue)
        {
            int nCenter = (int) _dimension / 2;

            // Change data in oldest buffer, then make newest buffer
            // become oldest by swapping
            Point3D pt = _oldBuffer[(nCenter * _dimension) + nCenter];
            pt.Y = (int) PeakValue;
            pt.Y = 0;
            _oldBuffer[(nCenter * _dimension) + nCenter] = pt;

            SwapBuffers();
        }

        /// <summary>
        /// Leave buffers in place, but change notation of which one is most recent
        /// </summary>
        private void SwapBuffers()
        {
            Point3DCollection temp = _currBuffer;
            _currBuffer = _oldBuffer;
            _oldBuffer = temp;
        }


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
                    _ptBuffer1.Add(new Point3D(col, 0.0, row));

                    // Completing new square, add 2 triangles
                    if ((row > 0) && (col > 0))
                    {
                        // Triangle 1
                        _triangleIndices.Add(nCurrIndex - _dimension - 1);
                        _triangleIndices.Add(nCurrIndex);
                        _triangleIndices.Add(nCurrIndex - _dimension);
                        

                        // Triangle 2
                        _triangleIndices.Add(nCurrIndex - _dimension - 1);
                        _triangleIndices.Add(nCurrIndex - 1);
                        _triangleIndices.Add(nCurrIndex);
                    }

                    nCurrIndex++;
                }

                mTri= new Triangle[_triangleIndices.Count/3];
                for (int i = 0; i < mTri.Length; i++)
                {
                    mTri[i]= new Triangle()
                    {
                        I0 = _triangleIndices[i*3+0],
                        I1 = _triangleIndices[i * 3 + 1],
                        I2 = _triangleIndices[i * 3 + 2],
                    };
                }
            }

            // 2nd buffer exists only to have 2nd set of Z values
            _ptBuffer2 = _ptBuffer1.Clone();
        }

        /// <summary>
        /// Determine next state of entire grid, based on previous two states.
        /// This will have the effect of propagating ripples outward.
        /// </summary>
        public void ProcessWater()
        {
            // Note that we write into old buffer, which will then become our
            //    "current" buffer, and current will become old. 
            // I.e. What starts out in _currBuffer shifts into _oldBuffer and we
            // write new data into _currBuffer.  But because we just swap pointers,
            // we don't have to actually move data around.

            // When calculating data, we don't generate data for the cells around
            // the edge of the grid, because data smoothing looks at all adjacent
            // cells.  So instead of running [0,n-1], we run [1,n-2].

            double velocity; // Rate of change from old to current
            double smoothed; // Smoothed by adjacent cells
            double newHeight;
            int neighbors;

            int nPtIndex = 0; // Index that marches through 1-D point array

            // Remember that Y value is the height (the value that we're animating)
            for (int row = 0; row < _dimension; row++)
            {
                for (int col = 0; col < _dimension; col++)
                {
                    velocity = -1.0 * _oldBuffer[nPtIndex].Y; // row, col
                    smoothed = 0.0;

                    neighbors = 0;
                    if (row > 0) // row-1, col
                    {
                        smoothed += _currBuffer[nPtIndex - _dimension].Y;
                        neighbors++;
                    }

                    if (row < (_dimension - 1)) // row+1, col
                    {
                        smoothed += _currBuffer[nPtIndex + _dimension].Y;
                        neighbors++;
                    }

                    if (col > 0) // row, col-1
                    {
                        smoothed += _currBuffer[nPtIndex - 1].Y;
                        neighbors++;
                    }

                    if (col < (_dimension - 1)) // row, col+1
                    {
                        smoothed += _currBuffer[nPtIndex + 1].Y;
                        neighbors++;
                    }

                    // Will always have at least 2 neighbors
                    smoothed /= (double) neighbors;

                    // New height is combination of smoothing and velocity
                    newHeight = smoothed * SmoothingFactor + velocity;

                    // Damping
                    newHeight = newHeight * Damping;

                    // We write new data to old buffer
                    Point3D pt = _oldBuffer[nPtIndex];
                    pt.Y = newHeight; // row, col

                    if (row == 20 && col == 50)
                    {
                        if (t > 25)
                        {
                            pt.Y += 5;
                          //  t = 0;
                        }
                        //t++;
                    }

                    if (row ==80 && col == 50 )
                    {
                        if (t > 25)
                        {
                            pt.Y += 5;
                            t = 0;
                        }
                        t++;
                    }
                    _oldBuffer[nPtIndex] = pt;

                    nPtIndex++;
                }
            }

            SwapBuffers();
        }

        private int t = 0;



    }
}
