using System.Collections;
using System.Collections.Generic;
using EnemyNameSpace;
using UnityEngine;
using System.Linq;

public class EnemyTargetPicker : MonoBehaviour
{
    [SerializeField] EnemiesBornManager enemyManager;
    [SerializeField] NewShipController player;

    static EnemiesBornManager staticEnemiesManager;
    static NewShipController staticplayer;

    public static EnemyTargetPicker Instance;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        staticEnemiesManager = enemyManager;
        staticplayer = player;
        Instance=this;
    }

    public Transform PickNearestTargetTransformWithAngleAndRange(Vector2 myPosition,Vector2 faceDir, float angleWithDegree, float range)
    {
        if (staticEnemiesManager == null || staticEnemiesManager.Enemies == null)
            return null;

        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (var enemy in staticEnemiesManager.Enemies)
        {
            if (enemy == null) continue;

            Vector2 directionToTarget = ((Vector2)enemy.transform.position - myPosition).normalized;
            Vector2 forwardDirection = faceDir; // 假設向右為前方，可以根據需要調整
            float angle = Vector2.Angle(forwardDirection, directionToTarget);
            float distance = Vector2.Distance(myPosition, enemy.transform.position);

            if (angle <= angleWithDegree / 2 && distance <= range && distance < nearestDistance)
            {
                nearestTarget = enemy.transform;
                nearestDistance = distance;
            }
        }

        return nearestTarget;
    }

    public static Vector2 PickNearestTargetPositionWithAngleAndRange(Vector2 myPosition, Vector2 faceDir,float angleWithDegree, float range, Vector2 randomSizeWithCircle)
    {
        Transform target = Instance. PickNearestTargetTransformWithAngleAndRange(myPosition,faceDir, angleWithDegree, range);
        
        if (target == null)
            return default; // 如果沒有找到目標，返回自己的位置

        Vector2 targetPosition = target.position;
        
        // 在目標位置附近生成一個隨機點
        float randomAngle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(0f, Mathf.Min(randomSizeWithCircle.x, randomSizeWithCircle.y));
        
        Vector2 randomOffset = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius,
            Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius
        );

        return targetPosition + randomOffset;
    }

    public static List<Vector2> PickEnemies(Vector2 myPosition,Vector2 faceDir, float angleWithDegree, float range,List<GameObject> canNotDamageList = null)
    {
        List<Vector2> enemies = new List<Vector2>();
        if (staticEnemiesManager == null || staticEnemiesManager.Enemies == null){
            UnityEngine.Debug.LogError("staticEnemiesManager is null");
            return enemies;
        }
        foreach (var enemy in staticEnemiesManager.Enemies)
        {
            if (enemy == null) continue;
            Vector2 directionToTarget = ((Vector2)enemy.transform.position - myPosition).normalized;
            float angle = Vector2.Angle(faceDir, directionToTarget);
            float distance = Vector2.Distance(myPosition, enemy.transform.position);
            if (angle <= angleWithDegree / 2 && distance <= range && !canNotDamageList.Contains(enemy.gameObject))
            {
                enemies.Add(enemy.transform.position);
            }
        }
        return enemies;
    }

    public static Transform PickPlayer_TransformWithAngleAndRange(Vector2 myPosition,Vector2 faceDir, float angleWithDegree, float range)
    {
        if (staticEnemiesManager == null || staticEnemiesManager.Enemies == null)
            return null;

        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

            Vector2 directionToTarget = ((Vector2)staticplayer.transform.position - myPosition).normalized;
            Vector2 forwardDirection = faceDir; // 假設向右為前方，可以根據需要調整
            float angle = Vector2.Angle(forwardDirection, directionToTarget);
            float distance = Vector2.Distance(myPosition, staticplayer.transform.position);

            if (angle <= angleWithDegree / 2 && distance <= range && distance < nearestDistance)
            {
                nearestTarget = staticplayer.transform;
                nearestDistance = distance;
            }

        return nearestTarget;
    }

    public static Vector2 PickPlayer_PositionWithAngleAndRange(Vector2 myPosition, Vector2 faceDir,float angleWithDegree, float range, Vector2 randomSizeWithCircle)
    {
        Transform target = PickPlayer_TransformWithAngleAndRange(myPosition,faceDir, angleWithDegree, range);
        
        if (target == null)
            return default; // 如果沒有找到目標，返回自己的位置

        Vector2 targetPosition = target.position;
        
        // 在目標位置附近生成一個隨機點
        float randomAngle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(0f, Mathf.Min(randomSizeWithCircle.x, randomSizeWithCircle.y));
        
        Vector2 randomOffset = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius,
            Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius
        );

        return targetPosition + randomOffset;
    }
}
