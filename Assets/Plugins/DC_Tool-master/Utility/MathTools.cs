using System.Collections.Generic;
using UnityEngine;

namespace DC_Tool.Utility
{
    // P = Point, Ls = LineSegment, L = Line, V = Vector
    public static class MathTools
    {
        private const float MINI_ADJUSTMENT = 0.001f;

        public static Vector2 VECTOR_NAN = new Vector2(float.NaN, float.NaN);

        #region Float
        public static float FloatClamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }
        public static int ChanceToInt(float value)
        {
            int resultInt = (int)value;
            float additionalChance = value % 1;
            if (additionalChance > Random.value)
            {
                resultInt++;
            }
            return resultInt;
        }
        public static float FloatMove(float value, float target, float moveDistance)
        {
            float diff = target - value;
            if (Mathf.Abs(diff) <= moveDistance)
            {
                return target;
            }
            else if (diff > 0)
            {
                return value + moveDistance;
            }
            else
            {
                return value - moveDistance;
            }
        }
        public static float Remap(float a, Vector2 from, Vector2 to, bool clamp)
        {
            float t = (a - from.x) / (from.y - from.x);
            return FloatLerp(to.x, to.y, t, clamp);
        }
        public static float FloatLerp(float a, float b, float t, bool clamp = true)
        {
            if (clamp)
            {
                t = FloatClamp(t, 0, 1);
            }
            return a + (b - a) * t;
        }
        public static float FloatRound(float value, float roundUnit)
        {
            int valueUnit = Mathf.RoundToInt(value / roundUnit);
            return roundUnit * valueUnit;
        }
        public static bool IsInRange(float value, Vector2 range)
        {
            return value > range.x && value < range.y;
        }
        public static float IncAverage(int number, float numberInc)
        {
            float total = 1 + (number - 1) * numberInc;
            return total / number;
        }
        #endregion

        #region Int
        public static int IntClamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }
        #endregion

        #region Graphy
        public static bool IsNaNVector(Vector2 vector)
        {
            return float.IsNaN(vector.x);
        }
        public static Quaternion DirectionToRotation(Vector2 direction)
        {
            float angle = Vector2.Angle(Vector2.up, direction);
            if (direction.x > 0)
            {
                angle *= -1;
            }
            return Quaternion.Euler(Vector3.forward * angle);
        }
        public static float DirectionToAngle(Vector2 dir)
        {
            float result = Mathf.Atan2(dir.y, dir.x);
            return result;
        }
        public static Vector2 AngleToDirection(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        public static float ProjectionPL(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 vL = lineEnd - lineStart;
            Vector2 vP = point - lineStart;
            return Vector2.Dot(vP, vL) / vL.magnitude;
        }
        public static float DistancePL(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 vL = lineEnd - lineStart;
            Vector2 vP = point - lineStart;
            return Vector3.Cross(vP, vL).z / vL.magnitude;
        }
        public static float DistancePLs(Vector2 point, Vector2 lsStart, Vector2 lsEnd)
        {
            Vector2 vLs = lsEnd - lsStart;
            Vector2 vLsP = point - lsStart;

            float projection = Vector2.Dot(vLs, vLsP) / vLs.magnitude;
            if (projection < 0)
            {
                return Vector2.Distance(point, lsStart);
            }
            else if (projection > vLs.magnitude)
            {
                return Vector2.Distance(point, lsEnd);
            }
            else
            {
                return Mathf.Abs(Vector3.Cross(vLsP, vLs).z) / vLs.magnitude;
            }
        }
        public static Vector2 RandomOnUnitCircle()
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector2 result = Vector2.right * Mathf.Sin(angle) + Vector2.up * Mathf.Cos(angle);
            return result;
        }
        public static bool DistanceLessThan(Vector2 pos1, Vector2 pos2, float distance)
        {
            float dx = pos1.x - pos2.x;
            float dy = pos1.y - pos2.y;
            return (dx * dx + dy * dy) < distance * distance;
        }
        public static Vector2 Rotation(Vector2 dir, float angle)
        {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);
            float tempX = dir.x * cos - dir.y * sin;
            float tempY = dir.x * sin + dir.y * cos;
            return new Vector2(tempX, tempY);
        }
        public static Vector2 GetBezierPosition(List<Vector2> path, float t)
        {
            if (path.Count == 1)
            {
                return path[0];
            }
            else
            {
                List<Vector2> nextPath = new List<Vector2>();
                for (int i = 0; i < path.Count - 1; i++)
                {
                    nextPath.Add(Vector2.Lerp(path[i], path[i + 1], t));
                }
                return GetBezierPosition(nextPath, t);
            }
        }
        #endregion

        #region program
        public static T WeightRandomSelect<T>(List<T> objPool) where T : IWeightObj
        {
            float totalWeight = 0;
            foreach (IWeightObj current in objPool)
            {
                totalWeight += current.Weight;
            }
            float random = Random.Range(0, totalWeight);
            foreach (IWeightObj current in objPool)
            {
                random -= current.Weight;
                if (random <= 0)
                {
                    return (T)current;
                }
            }
            return default(T);
        }
        public static List<T> RandomOrderList<T>(this List<T> originList)
        {
            originList = new List<T>(originList);
            List<T> result = new List<T>();
            while (originList.Count > 0)
            {
                int randomIndex = Random.Range(0, originList.Count);
                result.Add(originList[randomIndex]);
                originList.RemoveAt(randomIndex);
            }
            return result;
        }
        #endregion

        #region string
        public static string ToPercentString(this float value)
        {
            float displayValue;
            if (value > 0)
            {
                displayValue = value * 100 + MINI_ADJUSTMENT;
            }
            else
            {
                displayValue = value * 100 - MINI_ADJUSTMENT;
            }
            return ((int)(displayValue)).ToString() + "%";
        }
        #endregion
        public static ReflectionCollision CalReflectionCollision(Vector2 ballOriginPos, Vector2 ballNextPos, Vector2 lineEnd1, Vector2 lineEnd2, float ballRadius)
        {
            ReflectionCollision result = new ReflectionCollision();
            //Ball not move
            if (ballOriginPos == ballNextPos)
            {
                result.HasCollision = false;
                return result;
            }
            //Ball is too far
            if (DistancePLs(ballOriginPos, lineEnd1, lineEnd2) <= ballRadius - MINI_ADJUSTMENT)
            {
                result.HasCollision = false;
                return result;
            }

            Vector2 vLine = lineEnd2 - lineEnd1;
            Vector2 vBallOrigin = ballOriginPos - lineEnd1;
            Vector2 vBallNext = ballNextPos - lineEnd1;

            float originDistance = Vector3.Cross(vBallOrigin, vLine).z / vLine.magnitude;
            float nextDistance = Vector3.Cross(vBallNext, vLine).z / vLine.magnitude;

            if (originDistance < 0)
            {
                originDistance *= -1;
                nextDistance *= -1;
            }

            if (nextDistance > originDistance)
            {
                if (originDistance > ballRadius - MINI_ADJUSTMENT)
                {
                    result.HasCollision = false;
                    return result;
                }
            }

            float tempCollisionT = (originDistance - ballRadius) / (originDistance - nextDistance);

            if (tempCollisionT > 1)
            {
                result.HasCollision = false;
                return result;
            }

            Vector2 collisionBallPosition = Vector2.LerpUnclamped(ballOriginPos, ballNextPos, tempCollisionT);
            Vector2 vCollisionBall = collisionBallPosition - lineEnd1;
            float collisionBallProjection = Vector2.Dot(vLine, vCollisionBall) / vLine.magnitude;

            if (collisionBallProjection < 0)
            {
                CalBallToPointCollision(ref result, ballOriginPos, ballNextPos, lineEnd1, ballRadius);
            }
            else if (collisionBallProjection > vLine.magnitude)
            {
                CalBallToPointCollision(ref result, ballOriginPos, ballNextPos, lineEnd2, ballRadius);
            }
            else
            {
                result.T = tempCollisionT;
                result.collisionPoint = lineEnd1 + vLine.normalized * collisionBallProjection;
            }
            return result;
        }
        private static void CalBallToPointCollision(ref ReflectionCollision result, Vector2 ballOriginPos, Vector2 ballNextPos, Vector2 collisionPoint, float ballRadius)
        {
            Vector2 vBall = ballNextPos - ballOriginPos;
            Vector2 vPointBall = collisionPoint - ballOriginPos;
            float pointProjection = Vector2.Dot(vBall, vPointBall) / vBall.magnitude;

            float nearestDistance = Mathf.Abs(Vector3.Cross(vBall, vPointBall).z / vBall.magnitude);

            if (nearestDistance >= ballRadius)
            {
                result.HasCollision = false;
                return;
            }
            float collisionPointAdjust = Mathf.Sqrt(ballRadius * ballRadius - nearestDistance * nearestDistance);
            float collisionBallMoveDistance = pointProjection - collisionPointAdjust;
            float collisionTime = collisionBallMoveDistance / vBall.magnitude;

            result.T = collisionTime;
            result.collisionPoint = collisionPoint;
        }
    }

    public struct ReflectionCollision
    {
        private const float DEFAULT_NON_COLLISION_TIME = -1;

        private float t;
        public Vector2 collisionPoint;

        public bool HasCollision
        {
            get
            {
                return T >= 0 && T <= 1;
            }

            set
            {
                if (value == false)
                {
                    T = DEFAULT_NON_COLLISION_TIME;
                }
            }
        }

        public float T
        {
            get => t;
            set
            {
                //Adandon last number for avoid pierce edge bug
                t = (float)System.Math.Round(value, 3);
            }
        }
    }
    public interface IWeightObj
    {
        float Weight
        {
            get;
        }
    }
}
