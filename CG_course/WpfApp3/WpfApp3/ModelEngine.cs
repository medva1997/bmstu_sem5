using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

namespace WpfApp3
{
    class ModelEngine
    {
        public float density = 500; //плотность
        public int slicesPerAxis = 2;
        public bool isConcave = false; //является вогнутым
        public int voxelsLimit = 16;

        private const float DAMPFER = 0.1f;
        private const float WATER_DENSITY = 1000; //Плотность воды

        private double voxelHalfHeight;
        private Vector3D localArchimedesForce;
        private List<Vector3D> voxels;
        private bool isMeshCollider;
        private List<Vector3D[]> forces; // For drawing force gizmos


        public struct transforming
        {
            public dynamic rotation;
            public Point3D position;
        }
        public struct Body
        {
            public Point3D centerOfMass;
            public double mass;
        }

        private transforming transform;
        private Model model;
        private WaterGenerator water;
        public Body rigidbody;

        /// <summary>
        /// Provides initialization.
        /// </summary>
        private void Start()
        {
            forces = new List<Vector3D[]>(); // For drawing force gizmos
            transform= new transforming();
            rigidbody= new Body();
            // Store original rotation and position
            var originalRotation = transform.rotation;
            var originalPosition = transform.position;
            transform.rotation = Quaternion.Identity;
            transform.position = new Point3D(0,0,0);

            

            var bounds = model.GetTransformdBounds;
            voxelHalfHeight = Math.Max(bounds.SizeZ,(Math.Max(bounds.SizeX, bounds.SizeY)));
          
            voxelHalfHeight /= 2 * slicesPerAxis;

           
            rigidbody.centerOfMass = new Point3D(bounds.X+bounds.SizeX, bounds.Y + bounds.SizeY, bounds.Z + bounds.SizeZ);

            voxels = SliceIntoVoxels(isMeshCollider && isConcave);

            // Restore original rotation and position
            transform.rotation = originalRotation;
            transform.position = originalPosition;

            double volume = rigidbody.mass / density;

            WeldPoints(voxels, voxelsLimit);

            double archimedesForceMagnitude = WATER_DENSITY * 9.81 * volume;
            localArchimedesForce = new Vector3D(0, archimedesForceMagnitude, 0) / voxels.Count;

            Debug.WriteLine(string.Format("[Buoyancy.cs] volume={1:0.0}, mass={2:0.0}, density={3:0.0}", volume, rigidbody.mass, density));
        }

        /// <summary>
        /// Slices the object into number of voxels represented by their center points.
        /// <param name="concave">Whether the object have a concave shape.</param>
        /// <returns>List of voxels represented by their center points.</returns>
        /// </summary>
        private List<Vector3D> SliceIntoVoxels(bool concave)
        {
            var points = new List<Vector3D>(slicesPerAxis * slicesPerAxis * slicesPerAxis);

            if (concave)
            {
                //var meshCol =

                //var convexValue = meshCol.convex;
                //meshCol.convex = false;

                // Concave slicing
                var bounds = model.GetTransformdBounds;
                for (int ix = 0; ix < slicesPerAxis; ix++)
                {
                    for (int iy = 0; iy < slicesPerAxis; iy++)
                    {
                        for (int iz = 0; iz < slicesPerAxis; iz++)
                        {
                            double x = bounds.X + bounds.SizeX / slicesPerAxis * (0.5 + ix);
                            double y = bounds.Y + bounds.SizeY / slicesPerAxis * (0.5 + iy);
                            double z = bounds.Z + bounds.SizeZ / slicesPerAxis * (0.5 + iz);

                            //r p = transform.InverseTransformPoint(new Vector3D(x, y, z));

                            //if (PointIsInsideMeshCollider(meshCol, p))
                            //{
                            //    points.Add(p);
                            //}
                        }
                    }
                }
                if (points.Count == 0)
                {
                    points.Add(new Vector3D(bounds.X + bounds.SizeX, bounds.Y + bounds.SizeY, bounds.Z + bounds.SizeZ));
                }

                //shCol.convex = convexValue;
            }
            else
            {
                // Convex slicing
                //r bounds = GetComponent<Collider>().bounds;
                for (int ix = 0; ix < slicesPerAxis; ix++)
                {
                    for (int iy = 0; iy < slicesPerAxis; iy++)
                    {
                        for (int iz = 0; iz < slicesPerAxis; iz++)
                        {
                            //double x = bounds.X + bounds.SizeX / slicesPerAxis * (0.5 + ix);
                            //double y = bounds.Y + bounds.SizeY / slicesPerAxis * (0.5 + iy);
                            //double z = bounds.Z + bounds.SizeZ / slicesPerAxis * (0.5 + iz);

                            ////r p = transform.InverseTransformPoint(new Vector3D(x, y, z));

                            //ints.Add(p);
                        }
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// Returns whether the point is inside the mesh collider.
        /// </summary>
        /// <param name="c">Mesh collider.</param>
        /// <param name="p">Point.</param>
        /// <returns>True - the point is inside the mesh collider. False - the point is outside of the mesh collider. </returns>
        private static bool PointIsInsideMeshCollider(MeshGeometry3D c, Vector3D p)
        {
            Vector3D[] directions = { new Vector3D(0, 1, 0), new Vector3D(0, -1, 0), new Vector3D(-1, 0, 0),
                new Vector3D(1,0,0), new Vector3D(0,0,-1), new Vector3D(0,0,1)};

            foreach (var ray in directions)
            {
                
            }

            return true;
        }

        /// <summary>
        /// Returns two closest points in the list.
        /// </summary>
        /// <param name="list">List of points.</param>
        /// <param name="firstIndex">Index of the first point in the list. It's always less than the second index.</param>
        /// <param name="secondIndex">Index of the second point in the list. It's always greater than the first index.</param>
        private static void FindClosestPoints(IList<Vector3D> list, out int firstIndex, out int secondIndex)
        {
            double minDistance = float.MaxValue, maxDistance = float.MinValue;
            firstIndex = 0;
            secondIndex = 1;

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    double distance = Vector3D.DotProduct(list[i], list[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        firstIndex = i;
                        secondIndex = j;
                    }
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                }
            }
        }

        /// <summary>
        /// Welds closest points.
        /// </summary>
        /// <param name="list">List of points.</param>
        /// <param name="targetCount">Target number of points in the list.</param>
        private static void WeldPoints(IList<Vector3D> list, int targetCount)
        {
            if (list.Count <= 2 || targetCount < 2)
            {
                return;
            }

            while (list.Count > targetCount)
            {
                int first, second;
                FindClosestPoints(list, out first, out second);

                var mixed = (list[first] + list[second]) * 0.5f;
                list.RemoveAt(second); // the second index is always greater that the first => removing the second item first
                list.RemoveAt(first);
                list.Add(mixed);
            }
        }

        /// <summary>
        /// Returns the water level at given location.
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="z">z-coordinate</param>
        /// <returns>Water level</returns>
        private float GetWaterLevel(double x, double z)
        {
            //              return ocean == null ? 0.0f : ocean.GetWaterHeightAtLocation(x, z);
            return 0.0f;
        }

        /// <summary>
        /// Calculates physics.
        /// </summary>
        private void FixedUpdate()
        {
            forces.Clear(); // For drawing force gizmos

            foreach (var point in voxels)
            {
                var wp = transform.position;
                double waterLevel = GetWaterLevel(wp.X, wp.Z);

                if (wp.Y - voxelHalfHeight < waterLevel)
                {
                    double k = (waterLevel - wp.Y) / (2 * voxelHalfHeight) + 0.5f;
                    if (k > 1)
                    {
                        k = 1f;
                    }
                    else if (k < 0)
                    {
                        k = 0f;
                    }

                  
                   

                 
                }
            }
        }

        /// <summary>
        /// Draws gizmos.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (voxels == null || forces == null)
            {
                return;
            }

            const float gizmoSize = 0.05f;
            

        }
    }
}
