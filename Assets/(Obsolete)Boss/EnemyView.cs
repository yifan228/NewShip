using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Obsolete]
public class EnemyView : MonoBehaviour,AttackedTarget
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] CircleCollider2D hitCircleCollider;
    [SerializeField] Rigidbody2D thisRigidBody;

    Action<EnemyView> returnToPool;
    Transform target;
    [SerializeField]float currentHp;
    
    [SerializeField] int exp = 1;
    float moveSpeed;

    float attackedtimer=0;
    int hittedCount=0;
    
    IExpGetter expGetter;

    public void BeAttacked(DamageStruct damage, string attackerName,AttackedType attackedType)
    {
        
        switch (attackedType)
        {
            case AttackedType.CriticalHit:
                break;

            case AttackedType.Dot:
                break;
            case AttackedType.Fire:
                if (attackerName == TagsEnum.Player.ToString())
                {
                    spriteRenderer.DOKill();
                    spriteRenderer.color = Color.red;
                    spriteRenderer.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
                    DamageTextManager.instance.ShowHpDamage(damage.Damage, thisRigidBody.position + Vector2.up);
                    currentHp -= damage.Damage;
                    if (currentHp < 0)
                    {
                        expGetter.Get(exp);
                        returnToPool(this);
                    }
                    
                }
                break;
            default:
                break;
        }
    }
    public void ExitAttack()
    {
        hittedCount = 0;
        attackedtimer = 0;
    }

    public void Init(Transform target,EnemyNameSpace. EnemyDataStruct d,Action<EnemyView> returnToPool)
    {
        this.target = target;
        spriteRenderer.DOKill();
        spriteRenderer.color = Color.red;
        spriteRenderer.sprite = d.Sprite;
        currentHp = d.HPdata.MaxHp;

        this.returnToPool = returnToPool;
        target.TryGetComponent(out expGetter);
    }

    public virtual void MoveWithVelocity()
    {
        thisRigidBody.velocity = moveSpeed* (target.position - thisRigidBody.transform.position).normalized;
    }

    
}
