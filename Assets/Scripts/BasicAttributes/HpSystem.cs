using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    [SerializeField] Transform showHitDmgNumberPos;
    [SerializeField] Bar hpbar;
    [SerializeField] Bar armorbar;
    [SerializeField] GlobalTimerEnum timerEnmum=GlobalTimerEnum.Enemy;
    ArmorSystem armorSystem;
    float currentHp;
    float maxHp;
    Action die;
    //public float ArmorCrashResistance;
    public Vector2 ShowDmgNumPoint=>showHitDmgNumberPos.position;
    public float CurrentArmor => armorSystem.currentArmor;
    public float CurrentHP => currentHp;
    public void Init(HpData data,Action onDied)
    {
        armorSystem = new ArmorSystem(data.Armormax,data.ArmorrecoverSpd,data.ArmorminReSpd,data.ArmorstartRecoverTime, armorbar.BarPosition);
        currentHp = data.MaxHp;
        maxHp = data.MaxHp;
        die = onDied;
    }

    private void Update()
    {
        switch (timerEnmum)
        {
            case GlobalTimerEnum.Player:
                armorSystem.UpdateArmor(GlobalTimeManager.Global_Deltatime);
                break;
            case GlobalTimerEnum.Enemy:
                armorSystem.UpdateArmor(GlobalTimeManager.Global_Enemy_Deltatime);
                break;
            case GlobalTimerEnum.UI:
                break;
            default:
                break;
        }
    }

    public float ArmorBeDamaged(float damage)
    {
        //Debug.LogWarning("armor" + damage);
        return armorSystem.ArmorBeDamage(damage);
    }
    /// <summary>
    /// if return true then die
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool HpBeDamaged(float damage)
    {
        //Debug.LogWarning("hp"+damage);
        currentHp -= damage;
        if (currentHp>0)
        {
            hpbar.BarPosition(1-currentHp/maxHp);
            return false;
        }
        else
        {
            return true;
        }
    }

    public float ModifyReSpd(float amount)
    {
        return armorSystem.ModifyReSpd(amount);
    }

    public void ShowCrash()
    {

    }

    public void Die()
    {
        die();
    }
}
[Serializable]
public struct HpData
{
    public float Armormax;
    public float ArmorrecoverSpd;
    public float ArmorminReSpd;
    public float ArmorstartRecoverTime;

    public float ArmorResistance;

    public float MaxHp;
}
