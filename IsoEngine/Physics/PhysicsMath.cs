using System;
using System.Collections.Generic;

namespace IsoEngine.Physics
{
    public static class PhysicsMath
    {

        internal static ShapeProjection ProjectShapeOntoAxis(Vec2 axis, Shape shape)
        {
            SetCircleVerticesAlongAxis(axis, shape);

            float min = Vec2.Dot(axis, shape.Vertices[0]);
            float max = min;
            Vec2 colVertex = shape.Vertices[0];

            for (int i = 0; i < shape.Vertices.Length; i++)
            {
                float p = Vec2.Dot(axis, shape.Vertices[i]);
                if (p < min)
                {
                    min = p;
                    colVertex = shape.Vertices[i];
                }
                if (p > max)
                    max = p;
            }

            return new ShapeProjection(min, max, colVertex);
        }

        internal struct ShapeProjection
        {
            public float Min { get; set; }
            public float Max { get; set; }
            public Vec2 ColVertex { get; set; }

            public ShapeProjection(float min, float max, Vec2 colVertex)
            {
                Min = min;
                Max = max;
                ColVertex = colVertex;
            }
        }

        internal static int GetShapeAxes(Shape shape)
        {
            if (shape.GetType() == typeof(Circle) || shape.GetType() == typeof(Line)) return 1;
            return 2; //rectangle
        }

        internal static void SetCircleVerticesAlongAxis(Vec2 axis, Shape shape)
        {
            if (shape.GetType() == typeof(Circle))
            {
                Circle circle = (Circle)shape;
                circle.Vertices[0] = circle.Position + axis.Unit() * -circle.Radius;
                circle.Vertices[1] = circle.Position + axis.Unit() * circle.Radius;
            }
        }

        internal static Vec2 ClosestVertexToPoint(Shape shape, Vec2 p)
        {
            Vec2 closestVertex = null;
            float minDist = float.MaxValue;

            for (int i = 0; i < shape.Vertices.Length; i++)
            {
                if ((p - shape.Vertices[i]).Magnitude() < minDist)
                {
                    closestVertex = shape.Vertices[i];
                    minDist = (p - shape.Vertices[i]).Magnitude();
                }
            }

            return closestVertex;
        }

        internal static Vec2[] FindAxes(Shape a, Shape b)
        {
            List<Vec2> axes = new List<Vec2>();

            if (a.GetType() == typeof(Circle) && b.GetType() == typeof(Circle))
            {
                axes.Add((b.Position - a.Position).Unit());

                return axes.ToArray();
            }
            if (a.GetType() == typeof(Circle))
            {
                axes.Add((ClosestVertexToPoint(b, a.Position) - a.Position).Unit());
                axes.Add(b.Dir.Normal());

                if (b.GetType() == typeof(Rectangle))
                    axes.Add(b.Dir);

                return axes.ToArray();
            }
            if (b.GetType() == typeof(Circle))
            {
                axes.Add(a.Dir.Normal());

                if (a.GetType() == typeof(Rectangle))
                    axes.Add(a.Dir);

                axes.Add((ClosestVertexToPoint(a, b.Position) - b.Position).Unit());
                return axes.ToArray();
            }

            axes.Add(a.Dir.Normal());
            if (a.GetType() == typeof(Rectangle))
                axes.Add(a.Dir);

            axes.Add(b.Dir.Normal());
            if (b.GetType() == typeof(Rectangle))
                axes.Add(b.Dir);

            return axes.ToArray();
        }

        /// <summary>
        /// Check for intersection between two Bodies according to the Separate Axis Theorem.
        /// </summary>
        /// <param name="a">The first Body.</param>
        /// <param name="b">The second Body.</param>
        /// <returns></returns>
        public static SATResult SAT(Shape a, Shape b)
        {
            if (a == null || b == null)
                return new SATResult(false, float.MaxValue, null, null);

            float minOverlap = float.MaxValue;
            Vec2 smallestAxis = null;
            Shape vertexShape = null;

            var axes = FindAxes(a, b);
            ShapeProjection proj1;
            ShapeProjection proj2;
            int firstShapeAxes = GetShapeAxes(a);

            for (int i = 0; i < axes.Length; i++)
            {
                proj1 = ProjectShapeOntoAxis(axes[i], a);
                proj2 = ProjectShapeOntoAxis(axes[i], b);
                
                float overlap = Math.Min(proj1.Max, proj2.Max) - Math.Max(proj1.Min, proj2.Min);
                if (overlap < 0)
                    return new SATResult(false, 0, null, null);

                if ((proj1.Max > proj2.Max && proj1.Min < proj2.Min) ||
                    (proj1.Max < proj2.Max && proj1.Min > proj2.Max))
                {
                    float mins = Math.Abs(proj1.Min - proj2.Max);
                    float maxes = Math.Abs(proj2.Max - proj2.Max);
                    if (mins < maxes)
                        overlap += mins;
                    else
                    {
                        overlap += maxes;
                        axes[i] *= -1;
                    }
                }

                if (overlap < minOverlap)
                {
                    minOverlap = overlap;
                    smallestAxis = axes[i];

                    if (i < firstShapeAxes)
                    {
                        vertexShape = b;
                        if (proj1.Max > proj2.Max)
                            smallestAxis = axes[i] * -1;
                    }
                    else
                    {
                        vertexShape = a;
                        if (proj1.Max < proj2.Max)
                            smallestAxis = axes[i] * -1;
                    }
                }
            }

            if (vertexShape == null)
                return new SATResult(false, float.MaxValue, null, null);
            Vec2 contactVertex = ProjectShapeOntoAxis(smallestAxis, vertexShape).ColVertex;

            if (vertexShape == b)
                smallestAxis *= -1;

            return new SATResult(true, minOverlap, smallestAxis, contactVertex);
        }

        /// <summary>
        /// The result of a SAT calculation.
        /// </summary>
        public struct SATResult
        {
            public bool Overlap { get; set; }
            public float Penetration { get; set; }
            public Vec2 Axis { get; set; }
            public Vec2 Vertex { get; set; }

            /// <summary>
            /// Initialize a new SATResult.
            /// </summary>
            /// <param name="overlap">Indicates if the two Bodies are colliding.</param>
            /// <param name="penetration">The penetration depth.</param>
            /// <param name="axis">The axis.</param>
            /// <param name="vertex">The contact vertex.</param>
            public SATResult(bool overlap, float penetration, Vec2 axis, Vec2 vertex)
            {
                Overlap = overlap;
                Penetration = penetration;
                Axis = axis;
                Vertex = vertex;
            }
        }

        internal static Matrix CalculateRotMatrix(float angle)
        {
            Matrix matrix = new Matrix(2, 2);
            matrix.Data[0, 0] = (float) Math.Cos(angle);
            matrix.Data[0, 1] = (float) -Math.Sin(angle);
            matrix.Data[1, 0] = (float) Math.Sin(angle);
            matrix.Data[1, 1] = (float) Math.Cos(angle);
            return matrix;
        }

    }
}
