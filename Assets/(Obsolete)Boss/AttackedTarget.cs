using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Obsolete]
public interface AttackedTarget
{
    public void BeAttacked(DamageStruct damage, string attackerName,AttackedType attackedType);
    public void ExitAttack()
    {

    }

    public static bool Timer(float breakTime,ref float currentTime,float addTime,ref int hitCount,bool isUseFirstHit)
    {
        if (isUseFirstHit)
        {
            if (hitCount == 0)
            {
                hitCount++;
                currentTime = 0;
                return true;
            }
            else
            {
                currentTime += addTime;
                if (currentTime > breakTime)
                {
                    hitCount++;
                    currentTime = 0;
                    return true;
                }
                else
                {
                    currentTime += addTime;
                    return false;
                }
            }
        }
        else
        {
            if (currentTime>breakTime)
            {
                hitCount++;
                currentTime = 0;
                return true;
            }
            else
            {
                currentTime += addTime;
                return false;
            }
        }
    }
}

public enum AttackedType
{
    Hit,
    CriticalHit,
    Dot,
    Fire
}

[Obsolete]
public struct DamageStruct
{
    public float Damage;
    public float TriggerDamageRadius;
    /// <summary>
    /// -1 is infinity
    /// </summary>
    public int maxDamageCount;
}
