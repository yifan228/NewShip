using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectileMovement 
{
    protected ProjectileMovementData data;
    public abstract void Moving();
}

public class ProjectileMovementData
{
    public Rigidbody2D ThisProjectileRB {  get; private set; }
    public Vector2 TargetPosition;
    public Transform TargetTransform;
    public Vector2 InitWorldSpeed { get; private set; }

    public ProjectileMovementData(Rigidbody2D thisProjectileRB, Vector2 initWorldSpeed, Vector2 targetPosition, Transform targetTransform)
    {
        ThisProjectileRB = thisProjectileRB;
        TargetPosition = targetPosition;
        TargetTransform = targetTransform;
        this.InitWorldSpeed = initWorldSpeed;
    }
}
