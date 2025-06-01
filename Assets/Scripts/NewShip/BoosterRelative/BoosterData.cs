using System;
using UnityEngine;

[Serializable]
public struct BoosterData
{
    /// <summary>
    /// public is just used for testing  
    /// </summary>
    public BoosterDriveName driveName { get; set; }
    public string BoosterName;
    public string KeyString;
    public float DeltaSpeedPower;
    public float DeltaAngularPower;
    public float MaxSpeed;
    public float MaxAngularSpeed;
    [Obsolete]
    public float ImplusForce;
    [Obsolete]
    public float ImplusTourque;
    public AnimationCurve Accelerationcurve;
}

