using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GravityTest
{
    class CollisionUtils
    {
        public struct PolygonCollisionResult
        {
            public bool isIntersecting;
            public Vector2 resetVector;
            public bool polygonAContained;
            public bool polygonBContained;
            public float resetDistance;
            public Vector2 separation;
        }

        private struct ProjectedVertices
        {
            public float min;
            public float max;
        }

        static public PolygonCollisionResult TestForCollision(HitBox polygonA, HitBox polygonB)
        {
            PolygonCollisionResult testAB = PolygonCollision(polygonA, polygonB);
            if (!testAB.isIntersecting) return testAB;

            PolygonCollisionResult testBA = PolygonCollision(polygonB, polygonA);
            if (!testBA.isIntersecting) return testBA;

            PolygonCollisionResult result = (Math.Abs(testAB.resetDistance) < Math.Abs(testBA.resetDistance)) ? testAB : testBA;

            // hack the contained flag to be the union of the two
            result.polygonAContained = testAB.polygonAContained && testBA.polygonAContained;
            result.polygonBContained = testAB.polygonBContained && testBA.polygonBContained;

            return result;
        }

        static private PolygonCollisionResult PolygonCollision(HitBox polygonA,
                              HitBox polygonB)
        {
            float shortestDist = float.MaxValue;
            PolygonCollisionResult pCR = new PolygonCollisionResult();
            pCR.isIntersecting = true;
            pCR.resetVector = new Vector2(0, 0);
            pCR.separation = new Vector2(0, 0);
            pCR.polygonAContained = true;
            pCR.polygonBContained = true;

            Vector2 offset = new Vector2(
                polygonA.CenterPosition.X - polygonB.CenterPosition.X,
                polygonA.CenterPosition.Y - polygonB.CenterPosition.Y);

            List<Vector2> pointsA = new List<Vector2> {
                polygonA.TopLeftCorner,
                polygonA.TopRightCorner,
                polygonA.BottomLeftCorner,
                polygonA.BottomRightCorner
            };

            List<Vector2> pointsB = new List<Vector2> {
                polygonB.TopLeftCorner,
                polygonB.TopRightCorner,
                polygonB.BottomLeftCorner,
                polygonB.BottomRightCorner
            };

            for (int i = 0; i < pointsA.Count; i++)
            {
                Vector2 axis = GetPerpendicularAxis(pointsA, i);

                ProjectedVertices polyARange = ProjectVertsForMinMax(axis, pointsA);
                ProjectedVertices polyBRange = ProjectVertsForMinMax(axis, pointsB);

                //float scalarOffset = Vector2.Dot(axis, offset);

                //polyARange.min += scalarOffset;
                //polyARange.max += scalarOffset;

                if ((polyARange.min - polyBRange.max > 0) || (polyBRange.min - polyARange.max > 0))
                {
                    pCR.isIntersecting = false;
                    return pCR;
                }

                CheckRangesForContainment(polyARange, polyBRange, ref pCR);

                float distMin = (polyBRange.max - polyARange.min) * -1;

                float distMinAbs = Math.Abs(distMin);
                if (distMinAbs < shortestDist)
                {
                    shortestDist = distMinAbs;

                    pCR.resetDistance = distMin;
                    pCR.resetVector = axis;
                }

            }

            pCR.separation = new Vector2(pCR.resetVector.X * pCR.resetDistance, pCR.resetVector.Y * pCR.resetDistance);

            return pCR;
        }

        static private void CheckRangesForContainment(ProjectedVertices rangeA, ProjectedVertices rangeB, ref PolygonCollisionResult pCR)
        {
            if (rangeA.max > rangeB.max || rangeA.min < rangeB.min) pCR.polygonAContained = false;
            if (rangeB.max > rangeA.max || rangeB.min < rangeA.min) pCR.polygonAContained = false;
        }

        static private ProjectedVertices ProjectVertsForMinMax(Vector2 axis, List<Vector2> vertices)
        {
            ProjectedVertices pV = new ProjectedVertices();
            pV.min = Vector2.Dot(axis, vertices[0]);
            pV.max = pV.min;

            for (int i = 1; i < vertices.Count; i++)
            {
                float temp = Vector2.Dot(axis, vertices[i]);
                if (temp < pV.min) pV.min = temp;
                if (temp > pV.max) pV.max = temp;
            }

            return pV;
        }

        static private Vector2 GetPerpendicularAxis(List<Vector2> points, int index)
        {
            Vector2 point1 = points[index];
            Vector2 point2 = index >= points.Count - 1 ? points[0] : points[index + 1];

            Vector2 axis = new Vector2(-(point2.Y - point1.Y), point2.X - point1.X);
            NormalizeAxis(ref axis);
            return axis;
        }

        static private void NormalizeAxis(ref Vector2 axis)
        {
            float length = (float)Math.Sqrt(axis.X * axis.X + axis.Y * axis.Y);
            if (length == 0)
                return;

            float ratio = 1 / length;
            axis.X *= ratio;
            axis.Y *= ratio;
        }
    }
}
