using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateMethod : MonoBehaviour
{
    public static bool IsAngleBetween(float angle, float minAngle, float maxAngle)
    {
        // 確保所有角度都在 -180 到 180 度範圍內
        angle = NormalizeAngle(angle);
        minAngle = NormalizeAngle(minAngle);
        maxAngle = NormalizeAngle(maxAngle);

        // 如果最大角度小於最小角度，表示範圍跨越了 -180/180 的邊界
        if (maxAngle < minAngle)
        {
            return angle >= minAngle || angle <= maxAngle;
        }

        // 正常情況，範圍不跨邊界
        return angle >= minAngle && angle <= maxAngle;
    }
    private static float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    public static float GetClosestWithSign(float input, float A, float B)
    {
        // 計算與 A 和 B 的距離（保留正負差距）
        float distanceToA = input - A;
        float distanceToB = input - B;

        // 返回更接近的值，直接比較差距的絕對值
        return Mathf.Abs(distanceToA) <= Mathf.Abs(distanceToB) ? A : B;
    }

    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad; // 轉換為弧度
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
    
    public static List<Vector2> SpreadDirection(Vector2 originDirection, float angleFrom, float angleTo, int count)
    {
        List<Vector2> directions = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {   
            float t = (count == 1) ? 0.5f : (float)i / (count - 1);     
            float angle = Mathf.LerpAngle(angleFrom, angleTo, t);
            angle = (angle + 360f) % 360f;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * originDirection;
            directions.Add(dir);
        }
        return directions;
    }

    public static Vector2 GetTargetRandomPosition( Vector2 targetPosition, float nearRandomSizeWithCircle, float farRandomSizeWithCircle)
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(nearRandomSizeWithCircle, farRandomSizeWithCircle);
        Vector2 randomOffset = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius,
            Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius
        );
        return targetPosition + randomOffset;
    }
}