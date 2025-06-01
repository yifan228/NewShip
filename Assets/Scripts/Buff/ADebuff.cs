using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADebuff 
{
    protected float max;
    protected float speed;
    protected float minSpd;

    protected DebuffAttribute data;

    public  virtual void OnUpdate(float time)
    {

    }
}

public enum DebufEnum
{
    Curse,
    Burn,
    
}
[Serializable]
public struct DebuffAttribute
{
    public DebufEnum type;
    public float Velocity;
    public float Position;
    public float HitRate;
}