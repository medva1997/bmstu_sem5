using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Core
{
    abstract class AffineTransform3D
    {
        internal AffineTransform3D() { }

       

        /// <summary>
        ///     Determines if this is an affine transformation.
        /// </summary>
        public  bool IsAffine => true;


      

        
        public new AffineTransform3D Clone()
        {
            return (AffineTransform3D)this;
        }

        
        public new AffineTransform3D CloneCurrentValue()
        {
            return (AffineTransform3D)this;
        }
        
    }


}
