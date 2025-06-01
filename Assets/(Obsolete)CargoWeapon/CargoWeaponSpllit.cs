//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class CargoWeaponSpllit : MonoBehaviour
//{
//    [SerializeField] CargoProjectileObjPool pool;
//    [SerializeField] CargoWeaponSplitLvUpgradeData data;
//    [SerializeField] int CurrentLvDataIndex;
//    [SerializeField] Transform thisCargoTransform;
//    float timer;

//    private void Update()
//    {
//        timer += Time.deltaTime;
//        if (timer>data.Data[CurrentLvDataIndex].SpliteInterval)
//        {
//            Split(data.Data[CurrentLvDataIndex].SplitCount, 
//                data.Data[CurrentLvDataIndex].Speed, 
//                data.Data[CurrentLvDataIndex].Damage,
//                data.Data[CurrentLvDataIndex].Permission,
//                data.Data[CurrentLvDataIndex].detectRadius
//                );
//            timer = 0;
//        }
//    }

//    private void Split(int splitCount,float bulleteSpeed,float damage,int permission,float detectRadius)
//    {
//        for (int i = 0; i < splitCount; i++)
//        {
//            float degree = 360 / splitCount * i;
//            DroneProjectileObj b = pool.GetObj();
//            DamageStruct damageData = new DamageStruct();
//            damageData.Damage = damage;
//            damageData.TriggerDamageRadius = detectRadius;
//            b.Init(null,thisCargoTransform.position, VectorRotate( thisCargoTransform.up, degree).normalized,-1,bulleteSpeed,damageData);
//        }
//    }
//    private Vector2 VectorRotate(Vector2 originalVector, float angleInDegrees)
//    {
//        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

//        float cosAngle = Mathf.Cos(angleInRadians);
//        float sinAngle = Mathf.Sin(angleInRadians);

//        return new Vector2(
//            originalVector.x * cosAngle + originalVector.y * sinAngle, 
//            -originalVector.x * sinAngle + originalVector.y * cosAngle);
//    }
//}

//[Serializable]
//[CreateAssetMenu(fileName = "ScriptableDatabase/BulleteDatabase", menuName = "Data/SplitCargoUpgradeData")]
//public class CargoWeaponSplitLvUpgradeData:ScriptableObject
//{
//    public List<CargoWeaponSplitLvData> Data;
//}

//public class CargoWeaponSplitLvData
//{
//    public int Lv;
//    public float SpliteInterval;
//    public int SplitCount;
//    public float Damage;
//    public float Speed;
//    public int Permission;
//    public float detectRadius;
//}
