using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulleteProjectile : AProjectileMovement
{
    float speed;
    public BulleteProjectile(ProjectileMovementData movementData,float speed)
    {
        data = movementData;
        this.speed = speed;
    }
    public override void Moving()
    {
        data.ThisProjectileRB.velocity = data.InitWorldSpeed+ (Vector2)data.ThisProjectileRB.transform.up * speed;
    }
}
