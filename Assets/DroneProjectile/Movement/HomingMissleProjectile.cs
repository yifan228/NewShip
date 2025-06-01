using System;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissleProjectile : AProjectileMovement
{
    float rotationSpeed ;  
    float acceleration ;

    public HomingMissleProjectile(float rotationSpeed, float acceleration, ProjectileMovementData movementData)
    {
        this.rotationSpeed = rotationSpeed;
        this.acceleration = acceleration;
        data = movementData;
        data.ThisProjectileRB.velocity = data.InitWorldSpeed;
    }

    public override void Moving()
    {
        Vector2 direction = Vector2.zero;
        if (data.TargetTransform == null)
        {
            Debug.Log("need to use transform");
        }
        else
        {
            direction = ((Vector2)data.TargetTransform.position - data.ThisProjectileRB.position);
        }

        float angle = Vector2.SignedAngle(direction.normalized, data.ThisProjectileRB.transform.up);
        //Debug.Log(angle);

        data.ThisProjectileRB.rotation += -Mathf.Sign(angle) * rotationSpeed * Time.deltaTime;

        data.ThisProjectileRB.velocity += (Vector2)data.ThisProjectileRB.transform.up* acceleration * Time.deltaTime;
    }
}
