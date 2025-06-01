using System;
using System.Collections.Generic;
using UnityEngine;

public class CargoWeaponSTangent : MonoBehaviour
{
    //[SerializeField] CargoProjectileObjPool pool;
    //[SerializeField] TangentCargoDroneSO data;
    //[SerializeField] int CurrentLvDataIndex;
    //[SerializeField] Rigidbody2D thisCargoRB;
    //[SerializeField] RopeDebuffSkill RopeDebuff;
    //float timer;
    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer > data.Data[CurrentLvDataIndex].CastInterval)
    //    {
    //        DroneProjectileObj obj = pool.GetObj();
    //        DamageStruct damage = new DamageStruct();
    //        CargoWeaponTangentLvData objData = data.Data[CurrentLvDataIndex];
    //        damage.Damage = objData.Damage;
    //        damage.TriggerDamageRadius = objData.detectRadius;
    //        damage.maxDamageCount = objData.MaxDamageCount;
    //        //todo: change debuff
    //        damage.debuffData = RopeDebuff.DebuffData;
    //        obj.Init(objData.testSprite,objData.Scale,thisCargoRB.position, thisCargoRB.velocity.normalized,thisCargoRB.velocity.magnitude+ objData.Speed,10,damage);
    //        timer = 0;
    //    }
    //}
    //public void ChangeLevel(int level)
    //{
    //    CurrentLvDataIndex = level;
    //}
}
