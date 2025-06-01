using EnemyNameSpace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class StationaryPattern : AEnemyMovePattern
{
    float timer;
    public StationaryPattern() 
    {
        timer = 0;
    }

    public override void Execute(EnemyController controller)
    {
        // Vector2 r = controller.Player.position - controller.transform.position;
        // if (r.magnitude>controller.enemyData.DetectRange)
        // {
        //     return;
        // }
        // timer += GlobalTimeManager.Global_Enemy_Deltatime;
        // controller.transform.up= Vector2.Lerp(controller.transform.up, r.normalized, 1 * GlobalTimeManager.Global_Enemy_Deltatime);
        // if (timer>controller.enemyData.AttackCDT)
        // {
        //     controller.Shoot(Vector2.zero,controller.Player);
        //     timer = 0;
        // }
    }
}
