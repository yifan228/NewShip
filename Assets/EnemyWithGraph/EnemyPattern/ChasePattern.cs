using EnemyNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.EventSystems;

public class ChasePattern : AEnemyMovePattern
{
    
    public override void Execute(EnemyController controller)
    {
        Vector2 r = (Vector2)controller.Player.position - (Vector2)controller.transform.position;
        Rotate(controller.enemyData.MoveAndRotateData, controller, r);
        Move(controller.enemyData.MoveAndRotateData, controller, r);
    }

    void Move(MoveAndRotateData d,EnemyController controller,Vector2 r)
    {
        float currentSpeed=0;
        if (r.magnitude < d.Range)
        {
            currentSpeed += d.Acceleration * GlobalTimeManager.Global_Enemy_Deltatime; // 加速
        }
        else if (r.magnitude <= d.Range)
        {
            currentSpeed -= d.Deceleration * GlobalTimeManager.Global_Enemy_Deltatime; // 減速
        }
        currentSpeed = Mathf.Clamp(currentSpeed, d.MinMoveSpeed, d.MaxMoveSpeed); // 限制速度範圍

        controller.transform.position += controller.transform.up.normalized * currentSpeed * Time.deltaTime; // 移動
    }

    void Rotate(MoveAndRotateData d,EnemyController controller,Vector2 r)
    {
        float rotateSpeed=0;
        // 計算當前方向與目標方向的夾角
        float angle = Vector2.SignedAngle(controller.transform.up, r.normalized);

        // 旋轉速度隨時間變化
        if (Mathf.Abs(angle) > 5f) // 避免小幅擺動
        {
            if (rotateSpeed < d.AngularRange)
            {
                rotateSpeed += d.RotateAcceleration * Time.deltaTime; // 加速旋轉
            }
            else if (rotateSpeed > d.MaxAngularSpeed)
            {
                rotateSpeed -= d.RotateDeceleration * Time.deltaTime; // 減速旋轉
            }
        }
        else
        {
            rotateSpeed = d.MinAngularSpeed; // 轉向已完成，回到最小旋轉速度
        }

        rotateSpeed = Mathf.Clamp(rotateSpeed, d.MinAngularSpeed, d.MaxAngularSpeed); // 限制旋轉速度範圍

        // 計算新的旋轉角度
        float rotationStep = rotateSpeed * Time.deltaTime * Mathf.Sign(angle);
        controller.transform.Rotate(0, 0, rotationStep); // 旋轉物件
    }
}

public struct ChasePatternData
{
    public float DetectInterval;
    public float MaxDetectInterval;
    public float Speed;
}
