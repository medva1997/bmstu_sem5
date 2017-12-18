using System;
using System.Windows.Media.Media3D;

namespace WpfApp3.Core
{

    public struct Vector3D 
    {
       
        public static bool operator ==(Vector3D vector1, Vector3D vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y &&
                   vector1.Z == vector2.Z;
        }

       
        public static bool operator !=(Vector3D vector1, Vector3D vector2)
        {
            return !(vector1 == vector2);
        }
       
        public static bool Equals(Vector3D vector1, Vector3D vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y) &&
                   vector1.Z.Equals(vector2.Z);
        }

       
        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector3D))
            {
                return false;
            }

            Vector3D value = (Vector3D)o;
            return Vector3D.Equals(this, value);
        }

       
        public bool Equals(Vector3D value)
        {
            return Vector3D.Equals(this, value);
        }

        public Vector3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

       


        /// <summary>
        /// Length of the vector.
        /// </summary>
        public double Length => Math.Sqrt(_x * _x + _y * _y + _z * _z);

        /// <summary>
        /// Length of the vector squared.
        /// </summary>
        public double LengthSquared => _x * _x + _y * _y + _z * _z;

        /// <summary>
        /// Updates the vector to maintain its direction, but to have a length
        /// of 1. Equivalent to dividing the vector by its Length.
        /// Returns NaN if length is zero.
        /// </summary>
        public void Normalize()
        {
            // Computation of length can overflow easily because it
            // first computes squared length, so we first divide by
            // the largest coefficient.
            double m = Math.Abs(_x);
            double absy = Math.Abs(_y);
            double absz = Math.Abs(_z);
            if (absy > m)
            {
                m = absy;
            }
            if (absz > m)
            {
                m = absz;
            }

            _x /= m;
            _y /= m;
            _z /= m;

            double length = Math.Sqrt(_x * _x + _y * _y + _z * _z);
            this /= length;
        }
        

        /// <summary>
        /// Operator -Vector (unary negation).
        /// </summary>
        /// <param name="vector">Vector being negated.</param>
        /// <returns>Negation of the given vector.</returns>
        public static Vector3D operator -(Vector3D vector)
        {
            return new Vector3D(-vector._x, -vector._y, -vector._z);
        }

        /// <summary>
        /// Negates the values of X, Y, and Z on this Vector3D
        /// </summary>
        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
        }

        /// <summary>
        /// Vector addition.
        /// </summary>
        /// <param name="vector1">First vector being added.</param>
        /// <param name="vector2">Second vector being added.</param>
        /// <returns>Result of addition.</returns>
        public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        /// <summary>
        /// Vector addition.
        /// </summary>
        /// <param name="vector1">First vector being added.</param>
        /// <param name="vector2">Second vector being added.</param>
        /// <returns>Result of addition.</returns>
        public static Vector3D Add(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        /// <summary>
        /// Vector subtraction.
        /// </summary>
        /// <param name="vector1">Vector that is subtracted from.</param>
        /// <param name="vector2">Vector being subtracted.</param>
        /// <returns>Result of subtraction.</returns>
        public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        public static explicit operator Vector3D(Point3D v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Vector subtraction.
        /// </summary>
        /// <param name="vector1">Vector that is subtracted from.</param>
        /// <param name="vector2">Vector being subtracted.</param>
        /// <returns>Result of subtraction.</returns>
        public static Vector3D Subtract(Vector3D vector1, Vector3D vector2)
        {
            return new Vector3D(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        /// <summary>
        /// Vector3D + Point3D addition.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of addition.</returns>
        public static Point3D operator +(Vector3D vector, Point3D point)
        {
            return new Point3D(x: vector._x + point.X,
                               y: vector._y + point.Y,
                               z: vector._z + point.Z);
        }

        /// <summary>
        /// Vector3D + Point3D addition.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of addition.</returns>
        public static Point3D Add(Vector3D vector, Point3D point)
        {
            return new Point3D(vector._x + point.X,
                               vector._y + point.Y,
                               vector._z + point.Z);
        }

        /// <summary>
        /// Vector3D - Point3D subtraction.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of subtraction.</returns>
        public static Point3D operator -(Vector3D vector, Point3D point)
        {
            return new Point3D(vector._x - point.X,
                               vector._y - point.Y,
                               vector._z - point.Z);
        }

        /// <summary>
        /// Vector3D - Point3D subtraction.
        /// </summary>
        /// <param name="vector">Vector by which we offset the point.</param>
        /// <param name="point">Point being offset by the given vector.</param>
        /// <returns>Result of subtraction.</returns>
        public static Point3D Subtract(Vector3D vector, Point3D point)
        {
            return new Point3D(vector._x - point.X,
                               vector._y - point.Y,
                               vector._z - point.Z);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="vector">Vector being multiplied.</param>
        /// <param name="scalar">Scalar value by which the vector is multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D operator *(Vector3D vector, double scalar)
        {
            return new Vector3D(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="vector">Vector being multiplied.</param>
        /// <param name="scalar">Scalar value by which the vector is multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D Multiply(Vector3D vector, double scalar)
        {
            return new Vector3D(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="scalar">Scalar value by which the vector is multiplied</param>
        /// <param name="vector">Vector being multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D operator *(double scalar, Vector3D vector)
        {
            return new Vector3D(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="scalar">Scalar value by which the vector is multiplied</param>
        /// <param name="vector">Vector being multiplied.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector3D Multiply(double scalar, Vector3D vector)
        {
            return new Vector3D(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        /// <summary>
        /// Scalar division.
        /// </summary>
        /// <param name="vector">Vector being divided.</param>
        /// <param name="scalar">Scalar value by which we divide the vector.</param>
        /// <returns>Result of division.</returns>
        public static Vector3D operator /(Vector3D vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        /// <summary>
        /// Scalar division.
        /// </summary>
        /// <param name="vector">Vector being divided.</param>
        /// <param name="scalar">Scalar value by which we divide the vector.</param>
        /// <returns>Result of division.</returns>
        public static Vector3D Divide(Vector3D vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

       

        /// <summary>
        /// Vector dot product.
        /// </summary>
        /// <param name="vector1">First vector.</param>
        /// <param name="vector2">Second vector.</param>
        /// <returns>Dot product of two vectors.</returns>
        public static double DotProduct(Vector3D vector1, Vector3D vector2)
        {
            return DotProduct(ref vector1, ref vector2);
        }

        /// <summary>
        /// Faster internal version of DotProduct that avoids copies
        ///
        /// vector1 and vector2 to a passed by ref for perf and ARE NOT MODIFIED
        /// </summary>
        internal static double DotProduct(ref Vector3D vector1, ref Vector3D vector2)
        {
            return vector1._x * vector2._x +
                   vector1._y * vector2._y +
                   vector1._z * vector2._z;
        }

        /// <summary>
        /// Vector cross product.
        /// </summary>
        /// <param name="vector1">First vector.</param>
        /// <param name="vector2">Second vector.</param>
        /// <returns>Cross product of two vectors.</returns>
        public static Vector3D CrossProduct(Vector3D vector1, Vector3D vector2)
        {
            Vector3D result;
            CrossProduct(ref vector1, ref vector2, out result);
            return result;
        }

        /// <summary>
        /// Faster internal version of CrossProduct that avoids copies
        ///
        /// vector1 and vector2 to a passed by ref for perf and ARE NOT MODIFIED
        /// </summary>
        internal static void CrossProduct(ref Vector3D vector1, ref Vector3D vector2, out Vector3D result)
        {
            result._x = vector1._y * vector2._z - vector1._z * vector2._y;
            result._y = vector1._z * vector2._x - vector1._x * vector2._z;
            result._z = vector1._x * vector2._y - vector1._y * vector2._x;
        }

        
       


        /// <summary>
        ///     X - double.  Default value is 0.
        /// </summary>
        public double X
        {
            get => _x;

            set => _x = value;
        }

        /// <summary>
        ///     Y - double.  Default value is 0.
        /// </summary>
        public double Y
        {
            get => _y;

            set => _y = value;
        }

        /// <summary>
        ///     Z - double.  Default value is 0.
        /// </summary>
        public double Z
        {
            get => _z;

            set => _z = value;
        }

        

       
      
        internal double _x;
        internal double _y;
        internal double _z;
    }
}
