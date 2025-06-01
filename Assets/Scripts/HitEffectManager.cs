using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneProjectileEffectManager : MonoBehaviour
{
    public static void Hit(GameObject currentTarget,DroneProjectileData d, GameObject owner, TagsEnum targetTag, Vector2 position, Vector2 forward)
    {
        switch (d.onHitSkill)
        {
            case OnHitSkill.None:
                break;
            case OnHitSkill.Explode:
                DoExplode(d, owner, targetTag, position, forward);
                break;
            case OnHitSkill.Split:
                DoSplit(currentTarget,d, owner, targetTag, position, forward);
                break;
            case OnHitSkill.Field:
                DoField(d, owner, targetTag, position, forward);
                break;
            default:
                break;
        }
    }

    private static void DoExplode(DroneProjectileData d, GameObject owner, TagsEnum targetTag, Vector2 position, Vector2 forward)
    {
        // 範圍傷害
        Debug.Log("DoExplode");
        DroneProjectileData explodeData = GlobalDatabase.Instance.DroneProjectileDataBase.GetData(d.ExplodeProjectileKeyString);
        explodeData.LifeTime = 0;
        explodeData.ObjSize = Vector3.one * d.ExplodeRange/explodeData.View.RealRangeDevidedSize;
        explodeData.PermissionCount = 999;//no limit
        explodeData.SecondaryHpDmg = d.Out_EplodeHpDmg;
        explodeData.SecondaryArmorDmg = d.Out_EplodeArmorDmg;
        //UnityEngine.Debug.Log($"explodeData.SecondaryHpDmg:{explodeData.SecondaryHpDmg.Dmg}");
        //UnityEngine.Debug.Log($"explodeData.SecondaryArmorDmg:{explodeData.SecondaryArmorDmg.Dmg}");
        DroneProjectileObj explodePrefab = GlobalDatabase.Instance.DroneProjectilePrefab;
        if (explodePrefab != null)
        {
            DroneProjectileObj explodeObj = Instantiate(explodePrefab, position, Quaternion.identity);
            explodeObj.Init(owner, targetTag, Vector2.zero, position, forward, explodeData);
        }else{
            Debug.LogWarning("explodePrefab is null");
        }
    }

    private static void DoSplit(GameObject currentTarget,DroneProjectileData d, GameObject owner, TagsEnum targetTag, Vector2 position, Vector2 forward)
    {
        // 搜尋附近敵人
        List<GameObject> canNotDamageList = new List<GameObject>();
        if(currentTarget != null){
            canNotDamageList.Add(currentTarget);
        }
        var enemyPositions = EnemyTargetPicker.PickEnemies(position, forward, d.AngleTo - d.AngleFrom, 1000,canNotDamageList);
        Debug.Log($"enemyPositions: {enemyPositions.Count}");
        DroneProjectileObj splitPrefab = GlobalDatabase.Instance.DroneProjectilePrefab;
        int enemyCount = enemyPositions != null ? enemyPositions.Count : 0;
        int splitCount = d.SplitAcount;
        int usedCount = 0;
        // 先對每個敵人發射一發
        if (enemyCount > 0)
        {
            int count = Mathf.Min(splitCount, enemyCount);
            for (int i = 0; i < count; i++)
            {
                Vector2 dir = (enemyPositions[i] - position).normalized;
                DroneProjectileData splitData = GlobalDatabase.Instance.DroneProjectileDataBase.GetData(d.SplitProjectileKeyString);
                splitData.Direction = dir;
                splitData.TargetWithPos = enemyPositions[i];
                if (splitPrefab != null)
                {
                    DroneProjectileObj splitObj = GameObject.Instantiate(splitPrefab, position, Quaternion.identity);
                    splitObj.Init(owner, targetTag, dir * d.MaxSpeed, position, dir, splitData,canNotDamageList);
                }
                usedCount++;
            }
        }
        // 剩餘子彈平均散射
        int remain = splitCount - usedCount;
        if (remain > 0)
        {
            float angleFrom = d.AngleFrom;
            float angleTo = d.AngleTo;
            for (int i = 0; i < remain; i++)
            {
                float t = (remain == 1) ? 0.5f : (float)i / (remain - 1);
                float angle = Mathf.LerpAngle(angleFrom, angleTo, t);
                angle = (angle + 360f) % 360f;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;
                DroneProjectileData splitData = GlobalDatabase.Instance.DroneProjectileDataBase.GetData(d.SplitProjectileKeyString);
                splitData.Direction = dir;
                splitData.TargetWithPos = position + dir;
                if (splitPrefab != null)
                {
                    DroneProjectileObj splitObj = GameObject.Instantiate(splitPrefab, position, Quaternion.identity);
                    splitObj.Init(owner, targetTag, dir * d.MaxSpeed, position, dir, splitData,canNotDamageList);
                }
            }
        }
    }

    private static void DoField(DroneProjectileData d, GameObject owner, TagsEnum targetTag, Vector2 position, Vector2 forward)
    {
        Debug.Log("DoField");
        DroneProjectileData fieldData = GlobalDatabase.Instance.DroneProjectileDataBase.GetData(d.FieldProjectileKeyString);
        fieldData.LifeTime = d.FieldTime;
        fieldData.ObjSize = Vector3.one * d.FieldRange/fieldData.View.RealRangeDevidedSize;//vfx size1是10
        fieldData.PermissionCount = 999;//no limit
        fieldData.SecondaryHpDmg = d.Out_FieldHpDmg;
        fieldData.SecondaryArmorDmg = d.Out_FieldArmorDmg;
        UnityEngine.Debug.Log($"fieldData.SecondaryHpDmg:{fieldData.SecondaryHpDmg.Dmg}");
        UnityEngine.Debug.Log($"fieldData.SecondaryArmorDmg:{fieldData.SecondaryArmorDmg.Dmg}");
        DroneProjectileObj fieldPrefab = GlobalDatabase.Instance.DroneProjectilePrefab;
        if (fieldPrefab != null)
        {
            DroneProjectileObj fieldObj = Instantiate(fieldPrefab, position, Quaternion.identity);
            fieldObj.Init(owner, targetTag, Vector2.zero, position, forward, fieldData);
            //Debug.LogWarning($"name:{fieldObj.GetComponentInChildren<VFXBase>().name}");
        }else{
            Debug.LogWarning("fieldPrefab is null");
        }
    }
}
