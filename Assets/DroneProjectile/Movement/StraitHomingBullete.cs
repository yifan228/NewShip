using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraitHomingBullete : AProjectileMovement
{
    float speed;
    float rotationSpeed;
    float searchAngle;

    public StraitHomingBullete(ProjectileMovementData movementData, float speed, float rotationSpeed, float searchAngle)
    {
        data = movementData;
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        this.searchAngle = searchAngle;
        data.ThisProjectileRB.velocity = data.InitWorldSpeed;
        // if(data.TargetTransform == null){
        //     UnityEngine.Debug.Log("strait homing target trans is null");
        // }
    }

    public override void Moving()
    {
        Vector2 direction = Vector2.zero;
        
        if (data.TargetTransform != null)
        {
            direction = ((Vector2)data.TargetTransform.position - data.ThisProjectileRB.position);
            float angle = Vector2.SignedAngle(direction.normalized, data.ThisProjectileRB.transform.up);

            // 只有在搜索角度內的目標才會追蹤
            if (Mathf.Abs(angle) <= searchAngle)
            {
                // 以固定的轉速朝向目標旋轉
                data.ThisProjectileRB.rotation += -Mathf.Sign(angle) * rotationSpeed * Time.deltaTime;
            }
        }

        // 以固定速度前進
        data.ThisProjectileRB.velocity = data.InitWorldSpeed + (Vector2)data.ThisProjectileRB.transform.up * speed;
    }
}
