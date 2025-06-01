using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSystem
{
    float max;
    float currentReSpd;
    float minReSpd;
    float startRecoverTime;
    bool isRecovering;
    bool isBeforeStartRecover;
    public float currentArmor { get; private set; }
    private Action<float> setBar;
    float CountDownStartReTimer;


    public ArmorSystem( float max, float recoverSpd, float minReSpd, float startRecoverTime, Action<float> setArmorBar)
    {
        this.max = max;
        this.currentReSpd = recoverSpd;
        this.minReSpd = minReSpd;
        this.startRecoverTime = startRecoverTime;
        Debug.Log("armor start recover time"+startRecoverTime);
        this.currentArmor = max;
        setBar = setArmorBar;
        CountDownStartReTimer = 0;
        isRecovering = false;
        isBeforeStartRecover = false;
    }

    public float ArmorBeDamage(float damage)
    {
        if (isRecovering)
        {
            return float.MaxValue;
        }

        if (isBeforeStartRecover)
        {
            CountDownStartReTimer = 0;
        }

        float result = 0;
        currentArmor -= damage;
        if (currentArmor<=0)
        {
            result = currentArmor;
            currentArmor = 0;
            StartCountDownStartReTime();
        }
        setBar(1-currentArmor/max);
        return result;
    }

    private void StartCountDownStartReTime()
    {
        isBeforeStartRecover = true;
    }

    private void StartRecover()
    {
        isRecovering = true;
    }

    float tmpRecoverArmor = 0;
    public void UpdateArmor(float deltatime)
    {
        if (isBeforeStartRecover)
        {
            CountDownStartReTimer += deltatime;
            if (CountDownStartReTimer> startRecoverTime)
            {
                StartRecover();
                CountDownStartReTimer = 0;
            }
        }

        if (isRecovering)
        {
            if (tmpRecoverArmor < max)
            {
                tmpRecoverArmor += currentReSpd * deltatime;
                setBar(1 - tmpRecoverArmor / max);
            }
            else
            {
                currentArmor = max;
                tmpRecoverArmor = 0;
                isRecovering = false;
            }
        }
    }

    public float ModifyReSpd(float amount)
    {
        currentReSpd-=amount;
        if (currentReSpd<minReSpd)
        {
            currentReSpd = minReSpd;
        }
        return currentReSpd;
    }
}
