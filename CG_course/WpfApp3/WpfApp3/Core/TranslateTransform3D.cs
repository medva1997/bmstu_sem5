using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WpfApp3.Core
{
    class TranslateTransform3D
    {
        private double _cachedOffsetXValue;
        private double _cachedOffsetYValue;
        private double _cachedOffsetZValue;


        public TranslateTransform3D() { }

       
        public TranslateTransform3D(Vector3D offset)
        {
            OffsetX = offset.X;
            OffsetY = offset.Y;
            OffsetZ = offset.Z;
        }

        public double OffsetZ { get; set; }

        public double OffsetY { get; set; }

        public double OffsetX { get; set; }


        public TranslateTransform3D(double offsetX, double offsetY, double offsetZ)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
            OffsetZ = offsetZ;
        }

       

       

       
        public  Matrix3D Value
        {
            get
            {
              

                Matrix3D matrix = new Matrix3D();
                //Append(ref matrix);

                return matrix;
            }
        }

       

    }
}
