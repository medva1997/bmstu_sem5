using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WpfApp3.Core
{
    public abstract class Transform3D : GeneralTransform3D
    {
      
        public new Transform3D Clone()
        {
            return (Transform3D)base.Clone();
        }

       
        public new Transform3D CloneCurrentValue()
        {
            return (Transform3D)base.CloneCurrentValue();
        }




        
        
        internal Transform3D() { }

       
        public new Point3D Transform(Point3D point)
        {
            // this function is included due to forward compatability reasons            
            return base.Transform(point);
        }


       
        public Vector3D Transform(Vector3D vector)
        {
            return new Vector3D();
        }

      
        public Point4D Transform(Point4D point)
        {
            return Value.Transform(point);
        }

       
        public void Transform(Point3D[] points)
        {
            Value.Transform(points);
        }

       
       

       
        public void Transform(Point4D[] points)
        {
            Value.Transform(points);
        }

        
        public override bool TryTransform(Point3D inPoint, out Point3D result)
        {
            result = Value.Transform(inPoint);
            return true;
        }

       
        public override Rect3D TransformBounds(Rect3D rect)
        {
            return new Rect3D();
        }
    
        public override GeneralTransform3D Inverse
        {
            get
            {
                

                Matrix3D matrix = Value;

                if (!matrix.HasInverse)
                {
                    return null;
                }

                matrix.Invert();
                return null;
            }
        }

       

       

       
        public static MatrixTransform3D Identity
        {
            get
            {
                // Make sure identity matrix is initialized.
                if (s_identity == null)
                {
                    MatrixTransform3D identity = new MatrixTransform3D();
                    identity.Freeze();
                    s_identity = identity;
                }
                return s_identity;
            }
        }

       
        public abstract bool IsAffine { get; }


       
        public abstract Matrix3D Value { get; }

      

        internal abstract void Append(ref Matrix3D matrix);


        private static MatrixTransform3D s_identity;

      
    }
}
