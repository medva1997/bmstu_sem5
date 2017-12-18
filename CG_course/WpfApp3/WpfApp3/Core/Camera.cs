using System;

using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;


namespace WpfApp3.Core
{
    public abstract class Camera : Animatable
    {
        internal Camera() { }

        internal abstract RayHitTestParameters RayFromViewportPoint(Point point, Size viewSize, Rect3D boundingRect, out double distanceAdjustment);
        internal abstract Matrix3D GetViewMatrix();
        internal abstract Matrix3D GetProjectionMatrix(double aspectRatio);

        internal static void PrependInverseTransform(Transform3D transform, ref Matrix3D viewMatrix)
        {
           
                PrependInverseTransform(transform.Value, ref viewMatrix);
            
        }

        internal static void PrependInverseTransform(Matrix3D matrix, ref Matrix3D viewMatrix)
        {
           
                // If the matrix is non-invertable we return a NaN matrix.
                viewMatrix = new Matrix3D(
                    double.NaN, double.NaN, double.NaN, double.NaN,
                    double.NaN, double.NaN, double.NaN, double.NaN,
                    double.NaN, double.NaN, double.NaN, double.NaN,
                    double.NaN, double.NaN, double.NaN, double.NaN);
           
        }

        private static void TransformPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            

            Camera target = ((Camera)d);


            Transform3D oldV = (Transform3D)e.OldValue;
            Transform3D newV = (Transform3D)e.NewValue;
            System.Windows.Threading.Dispatcher dispatcher = target.Dispatcher;

            if (dispatcher != null)
            {
               
              
            }

           
        }


       
        public Transform3D Transform
        {
            get
            {
                return (Transform3D)GetValue(TransformProperty);
            }
            set
            {
                //SetValueInternal(TransformProperty, value);
            }
        }

       
        internal abstract int GetChannelCountCore();

        

       
       
        public override string ToString()
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(null /* format string */, null /* format provider */);
        }

        
        public string ToString(IFormatProvider provider)
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(null /* format string */, provider);
        }

       
        

      
        internal virtual string ConvertToString(string format, IFormatProvider provider)
        {
            return base.ToString();
        }


        public static readonly DependencyProperty TransformProperty;

        



      

      
        static Camera()
        {
            // Initializations
            Type typeofThis = typeof(Camera);
            TransformProperty =
                  RegisterProperty("Transform",
                                   typeof(Transform3D),
                                   typeofThis,
                                   Transform3D.Identity,
                                   new PropertyChangedCallback(TransformPropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        }

        private static DependencyProperty RegisterProperty(string v1, Type type, Type typeofThis, MatrixTransform3D identity, PropertyChangedCallback propertyChangedCallback, object p1, bool v2, object p2)
        {
            return null;
        }
    }

    public abstract class ProjectionCamera : Camera
    {
        internal ProjectionCamera()
        {
        }
        internal override Matrix3D GetViewMatrix()
        {
            Point3D position = Position;
            Vector3D lookDirection = LookDirection;
            Vector3D upDirection = UpDirection;

            return CreateViewMatrix(Transform, ref position, ref lookDirection, ref upDirection);
        }
        internal static Matrix3D CreateViewMatrix(Transform3D transform, ref Point3D position, ref Vector3D lookDirection, ref Vector3D upDirection)
        {
            Vector3D zaxis = -lookDirection;
            zaxis.Normalize();

            Vector3D xaxis = Vector3D.CrossProduct(upDirection, zaxis);
            xaxis.Normalize();

            Vector3D yaxis = Vector3D.CrossProduct(zaxis, xaxis);

            Vector3D positionVec = (Vector3D)position;
            double cx = -Vector3D.DotProduct(xaxis, positionVec);
            double cy = -Vector3D.DotProduct(yaxis, positionVec);
            double cz = -Vector3D.DotProduct(zaxis, positionVec);

            Matrix3D viewMatrix = new Matrix3D(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                cx, cy, cz, 1);

            PrependInverseTransform(transform, ref viewMatrix);

            return viewMatrix;
        }

        public new ProjectionCamera Clone()
        {
            return (ProjectionCamera)base.Clone();
        }
        public new ProjectionCamera CloneCurrentValue()
        {
            return (ProjectionCamera)base.CloneCurrentValue();
        }

        private static void NearPlaneDistancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProjectionCamera target = ((ProjectionCamera)d);


            target.PropertyChanged(NearPlaneDistanceProperty);
        }
        private static void FarPlaneDistancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProjectionCamera target = ((ProjectionCamera)d);


            target.PropertyChanged(FarPlaneDistanceProperty);
        }
        private static void PositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProjectionCamera target = ((ProjectionCamera)d);


            target.PropertyChanged(PositionProperty);
        }
        private static void LookDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProjectionCamera target = ((ProjectionCamera)d);


            target.PropertyChanged(LookDirectionProperty);
        }
        private static void UpDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProjectionCamera target = ((ProjectionCamera)d);


            target.PropertyChanged(UpDirectionProperty);
        }

        private void PropertyChanged(DependencyProperty upDirectionProperty)
        {
            throw new NotImplementedException();
        }

        public double NearPlaneDistance
        {
            get
            {
                return (double)GetValue(NearPlaneDistanceProperty);
            }
            set
            {
                //SetValueInternal(NearPlaneDistanceProperty, value);
            }
        }

        /// <summary>
        ///     FarPlaneDistance - double.  Default value is (double)Double.PositiveInfinity.
        /// </summary>
        public double FarPlaneDistance
        {
            get
            {
                return (double)GetValue(FarPlaneDistanceProperty);
            }
            set
            {
                //SetValueInternal(FarPlaneDistanceProperty, value);
            }
        }

        /// <summary>
        ///     Position - Point3D.  Default value is new Point3D().
        /// </summary>
        public Point3D Position
        {
            get
            {
                return (Point3D)GetValue(PositionProperty);
            }
            set
            {
                //SetValueInternal(PositionProperty, value);
            }
        }

        /// <summary>
        ///     LookDirection - Vector3D.  Default value is new Vector3D(0,0,-1).
        /// </summary>
        public Vector3D LookDirection
        {
            get
            {
                return (Vector3D)GetValue(LookDirectionProperty);
            }
            set
            {
                //SetValueInternal(LookDirectionProperty, value);
            }
        }

        /// <summary>
        ///     UpDirection - Vector3D.  Default value is new Vector3D(0,1,0).
        /// </summary>
        public Vector3D UpDirection
        {
            get
            {
                return (Vector3D)GetValue(UpDirectionProperty);
            }
            set
            {
               // SetValueInternal(UpDirectionProperty, value);
            }
        }


        public static readonly DependencyProperty NearPlaneDistanceProperty;
        /// <summary>
        ///     The DependencyProperty for the ProjectionCamera.FarPlaneDistance property.
        /// </summary>
        public static readonly DependencyProperty FarPlaneDistanceProperty;
        /// <summary>
        ///     The DependencyProperty for the ProjectionCamera.Position property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty;
        /// <summary>
        ///     The DependencyProperty for the ProjectionCamera.LookDirection property.
        /// </summary>
        public static readonly DependencyProperty LookDirectionProperty;
        /// <summary>
        ///     The DependencyProperty for the ProjectionCamera.UpDirection property.
        /// </summary>
        public static readonly DependencyProperty UpDirectionProperty;

        




        internal const double c_NearPlaneDistance = (double)0.125;
        internal const double c_FarPlaneDistance = (double)Double.PositiveInfinity;
        internal static Point3D s_Position = new Point3D();
        internal static System.Windows.Media.Media3D.Vector3D s_LookDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -1);
        internal static System.Windows.Media.Media3D.Vector3D s_UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0);

      
        static ProjectionCamera()
        {
           
            // Initializations
            Type typeofThis = typeof(ProjectionCamera);
            NearPlaneDistanceProperty =
                  RegisterProperty("NearPlaneDistance",
                                   typeof(double),
                                   typeofThis,
                                   (double)0.125,
                                   new PropertyChangedCallback(NearPlaneDistancePropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
            FarPlaneDistanceProperty =
                  RegisterProperty("FarPlaneDistance",
                                   typeof(double),
                                   typeofThis,
                                   (double)Double.PositiveInfinity,
                                   new PropertyChangedCallback(FarPlaneDistancePropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
            PositionProperty =
                  RegisterProperty("Position",
                                   typeof(Point3D),
                                   typeofThis,
                                   new Vector3D(),
                                   new PropertyChangedCallback(PositionPropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
            LookDirectionProperty =
                  RegisterProperty("LookDirection",
                                   typeof(Vector3D),
                                   typeofThis,
                                   new System.Windows.Media.Media3D.Vector3D(0, 0, -1),
                                   new PropertyChangedCallback(LookDirectionPropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
            UpDirectionProperty =
                  RegisterProperty("UpDirection",
                                   typeof(Vector3D),
                                   typeofThis,
                                   new System.Windows.Media.Media3D.Vector3D(0, 1, 0),
                                   new PropertyChangedCallback(UpDirectionPropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        }

        private static DependencyProperty RegisterProperty(string position, Type type, Type typeofThis, Vector3D positiveInfinity, PropertyChangedCallback propertyChangedCallback, object p1, bool v2, object p2)
        {
            throw new NotImplementedException();
        }

        private static DependencyProperty RegisterProperty(string v1, Type type, Type typeofThis, double positiveInfinity, PropertyChangedCallback propertyChangedCallback, object p1, bool v2, object p2)
        {
            throw new NotImplementedException();
        }

        private static DependencyProperty RegisterProperty(string v1, Type type, Type typeofThis, System.Windows.Media.Media3D.Vector3D vector3D, PropertyChangedCallback propertyChangedCallback, object p1, bool v2, object p2)
        {
            throw new NotImplementedException();
        }

    }

    sealed  class PerspectiveCamera : ProjectionCamera
    {
        public new PerspectiveCamera Clone()
        {
            return (PerspectiveCamera)base.Clone();
        }
        private static void FieldOfViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PerspectiveCamera target = ((PerspectiveCamera)d);


            target.PropertyChanged(FieldOfViewProperty);
        }

        private void PropertyChanged(object fieldOfViewProperty)
        {
            throw new NotImplementedException();
        }
        public double FieldOfView
        {
            get
            {
                return (double)GetValue(FieldOfViewProperty);
            }
            set
            {
                //SetValueInternal(FieldOfViewProperty, value);
            }
        }
        protected override Freezable CreateInstanceCore()
        {
            return new PerspectiveCamera();
        }
       
        
        internal  void ReleaseOnChannelCore(dynamic channel)
        {
            ReleaseOnChannelAnimations(channel);



        }

        private void ReleaseOnChannelAnimations(dynamic channel)
        {
            
        }

        
       
      
        public static readonly DependencyProperty FieldOfViewProperty;

        
        internal const double c_FieldOfView = (double)45.0;
        static PerspectiveCamera()
        {
           


            // Initializations
            Type typeofThis = typeof(PerspectiveCamera);
            FieldOfViewProperty =
                RegisterProperty("FieldOfView",
                    typeof(double),
                    typeofThis,
                    (double)45.0,
                    new PropertyChangedCallback(FieldOfViewPropertyChanged),
                    null,
                    /* isIndependentlyAnimated  = */ true,
                    /* coerceValueCallback */ null);
        }

        private static DependencyProperty RegisterProperty(string v1, Type type, Type typeofThis, double v2, PropertyChangedCallback propertyChangedCallback, object p1, bool v3, object p2)
        {
            throw new NotImplementedException();
        }

        public PerspectiveCamera() { }
        public PerspectiveCamera(Point3D position, Vector3D lookDirection, Vector3D upDirection, double fieldOfView)
        {
            Position = position;
            LookDirection = lookDirection;
            UpDirection = upDirection;
            FieldOfView = fieldOfView;
        }

        internal Matrix3D GetProjectionMatrix(double aspectRatio, double zn, double zf)
        {
            double fov =(FieldOfView);

            // Note: h and w are 1/2 of the inverse of the width/height ratios:
            //
            //  h = 1/(heightDepthRatio) * (1/2)
            //  w = 1/(widthDepthRatio) * (1/2)
            //
            // Computation for h is a bit different than what you will find in
            //  because we have a horizontal rather
            // than vertical FoV.

            double halfWidthDepthRatio = Math.Tan(fov / 2);
            double h = aspectRatio / halfWidthDepthRatio;
            double w = 1 / halfWidthDepthRatio;

            double m22 = zf != Double.PositiveInfinity ? zf / (zn - zf) : -1;
            double m32 = zn * m22;

            return new Matrix3D(
                w, 0, 0, 0,
                0, h, 0, 0,
                0, 0, m22, -1,
                0, 0, m32, 0);
        }

        internal override Matrix3D GetProjectionMatrix(double aspectRatio)
        {
            return GetProjectionMatrix(aspectRatio, NearPlaneDistance, FarPlaneDistance);
        }

        internal override int GetChannelCountCore()
        {
            throw new NotImplementedException();
        }

        internal override RayHitTestParameters RayFromViewportPoint(Point p, Size viewSize, Rect3D boundingRect, out double distanceAdjustment)
        {
            // The camera may be animating.  Take a snapshot of the current value
            // and get the property values we need. (Window OS #992662)
            Point3D position = Position;
            Vector3D lookDirection = LookDirection;
            Vector3D upDirection = UpDirection;
            Transform3D transform = Transform;
            double zn = NearPlaneDistance;
            double zf = FarPlaneDistance;
            double fov = FieldOfView;

            //
            //  Compute rayParameters
            //

            // Find the point on the projection plane in post-projective space where
            // the viewport maps to a 2x2 square from (-1,1)-(1,-1).
            Point np =p;

            // Note: h and w are 1/2 of the inverse of the width/height ratios:
            //
            //  h = 1/(heightDepthRatio) * (1/2)
            //  w = 1/(widthDepthRatio) * (1/2)
            //
            // Computation for h is a bit different than what you will find in
            // D3DXMatrixPerspectiveFovRH because we have a horizontal rather
            // than vertical FoV.
            double aspectRatio =1;
            double halfWidthDepthRatio = Math.Tan(fov / 2);
            double h = aspectRatio / halfWidthDepthRatio;
            double w = 1 / halfWidthDepthRatio;

            // To get from projective space to camera space we apply the
            // width/height ratios to find our normalized point at 1 unit
            // in front of the camera.  (1 is convenient, but has no other
            // special significance.) See note above about the construction
            // of w and h.
            System.Windows.Media.Media3D.Vector3D rayDirection = new System.Windows.Media.Media3D.Vector3D(np.X / w, np.Y / h, -1);

            // Apply the inverse of the view matrix to our rayDirection vector
            // to convert it from camera to world space.
           
           

            Matrix3D viewMatrix = CreateViewMatrix(/* trasform = */ null, ref position, ref lookDirection, ref upDirection);
            Matrix3D invView = viewMatrix;
            invView.Invert();
            

            // The we have the ray direction, now we need the origin.  The camera's
            // position would work except that we would intersect geometry between
            // the camera plane and the near plane so instead we must find the
            // point on the project plane where the ray (position, rayDirection)
            // intersect (Windows OS #1005064):
            //
            //                     | _.>       p = camera position
            //                rd  _+"          ld = camera look direction
            //                 .-" |ro         pp = projection plane
            //             _.-"    |           rd = ray direction
            //         p +"--------+--->       ro = desired ray origin on pp
            //                ld   |
            //                     pp
           

            // Above we constructed the direction such that it's length projects to
            // 1 unit on the lookDirection vector.
            //
            //
            //                rd  _.>
            //                 .-"        rd = unnormalized rayDirection
            //             _.-"           ld = normalized lookDirection (length = 1)
            //           -"--------->
            //                 ld   
            //
            // So to find the desired rayOrigin on the projection plane we simply do:            
            Point3D rayOrigin = position + zn * rayDirection;
            rayDirection.Normalize();

            // Account for the Camera.Transform we ignored during ray construction above.
            if (transform != null )
            {
                Matrix3D m = transform.Value;
             

                PrependInverseTransform(m, ref viewMatrix);
            }

            RayHitTestParameters rayParameters = new RayHitTestParameters(rayOrigin, rayDirection);

            //
            //  Compute HitTestProjectionMatrix
            //

            Matrix3D projectionMatrix = GetProjectionMatrix(aspectRatio, zn, zf);

            // The projectionMatrix takes camera-space 3D points into normalized clip
            // space.

            // The viewportMatrix will take normalized clip space into
            // viewport coordinates, with an additional 2D translation
            // to put the ray at the rayOrigin.
            Matrix3D viewportMatrix = new Matrix3D();
            viewportMatrix.TranslatePrepend(new System.Windows.Media.Media3D.Vector3D(-p.X, viewSize.Height - p.Y, 0));
            viewportMatrix.ScalePrepend(new System.Windows.Media.Media3D.Vector3D(viewSize.Width / 2, -viewSize.Height / 2, 1));
            viewportMatrix.TranslatePrepend(new System.Windows.Media.Media3D.Vector3D(1, 1, 0));

           

            // 
            // Perspective camera doesn't allow negative NearPlanes, so there's
            // not much point in adjusting the ray origin. Hence, the
            // distanceAdjustment remains 0.
            //
            distanceAdjustment = 0.0;

            return rayParameters;
        }


    }
}
