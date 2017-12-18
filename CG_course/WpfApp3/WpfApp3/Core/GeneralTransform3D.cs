using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WpfApp3.Core
{
    public abstract class GeneralTransform3D
    {
        internal GeneralTransform3D()
        {
        }

      
        public abstract bool TryTransform(Point3D inPoint, out Point3D result);

       
        public Point3D Transform(Point3D point)
        {
            Point3D transformedPoint;

            if (!TryTransform(point, out transformedPoint))
            {
                
            }

            return transformedPoint;
        }

        /// <summary>
        /// Transforms the bounding box to the smallest axis aligned bounding box
        /// that contains all the points in the original bounding box
        /// </summary>
        /// <param name="rect">Bounding box</param>
        /// <returns>The transformed bounding box</returns>
        public abstract Rect3D TransformBounds(Rect3D rect);


        /// <summary>
        /// Returns the inverse transform if it has an inverse, null otherwise
        /// </summary>        
        public abstract GeneralTransform3D Inverse { get; }

        /// <summary>
        /// Returns a best effort affine transform
        /// </summary>
        internal abstract Transform3D AffineTransform
        {
          
            get;
        }

 
        public new GeneralTransform3D Clone()
        {
            return this;
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new GeneralTransform3D CloneCurrentValue()
        {
            return this;
        }




       
        /// <summary>
        /// Creates a string representation of this object based on the current culture.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public override string ToString()
        {
           
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(null /* format string */, null /* format provider */);
        }

       
        public string ToString(IFormatProvider provider)
        {
           
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(null /* format string */, provider);
        }

       
      

       
        internal virtual string ConvertToString(string format, IFormatProvider provider)
        {
            return base.ToString();
        }

    }
}
