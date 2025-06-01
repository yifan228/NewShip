using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefaultBulltetAction : ABulleteAction
{
    private float accumulationTime=0;
    private bool isSetInitialSpeed;

    public override void Detect()
    {
       
    }
    public BossDefaultBulltetAction(BulleteBehaviourData data)
    {
        datas = data;
    }
    public override void Move(Rigidbody2D thisRbody2D)
    {
        if (accumulationTime >= 1.25f)
        {
            thisRbody2D.velocity = datas.Toward.normalized * datas.InitialSpeed / accumulationTime*0.8f;
        }
        else if (!isSetInitialSpeed)
        {
            thisRbody2D.velocity = datas.Toward.normalized * datas.InitialSpeed;
            isSetInitialSpeed = true;
        }
        accumulationTime += Time.deltaTime;
    }

    public override void Trigger(Collider2D target, Rigidbody2D thisRbody2D, Action<Collider2D> PlayExplodeFX, ref int max, Action<TagsEnum, GameObject, Vector2> damageNumberPopoutAnim, Action close)
    {
        if (target.CompareTag(TagsEnum.Player.ToString()))
        {
            PlayExplodeFX(target);
            damageNumberPopoutAnim(TagsEnum.Player, target.gameObject, Vector2.zero);
            close();
        }
        else if (target.CompareTag(TagsEnum.FrontCollision.ToString()))
        {
            if (!datas.CanGoThroughWall)
            {
                PlayExplodeFX(target);
                close();
            }
        }
    }
}
