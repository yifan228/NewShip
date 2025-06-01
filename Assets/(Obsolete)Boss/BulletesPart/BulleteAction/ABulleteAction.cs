using System.Collections;
using System;
using UnityEngine;
using TMPro;
public abstract class ABulleteAction 
{
    protected BulleteBehaviourData datas;
    public CalculateDamageStrategy DamageStrategy { get; private set; }

    public void SetCalculateDamageStrategy(CalculateDamageStrategy strategy)
    {
        DamageStrategy = strategy;
    }


    public abstract void Move(Rigidbody2D thisRbody2D);
    public abstract void Detect();

    public abstract void Trigger(Collider2D target, Rigidbody2D thisRbody2D, Action<Collider2D> PlayExplodeFX,ref int max,Action<TagsEnum,GameObject,Vector2> damageNumberPopoutAnim,Action close);
}

public class BulleteBehaviourData
{
    public BulleteBehaviourType Type;
    public float InitialSpeed;
    public ContactFilter2D Filter;
    public Collider2D DetectCollider;
    public Vector3 Toward;
    public TagsEnum Owner ;
    public bool CanGoThroughWall;
    public string ExplodeFX;
}

//public class BulleteRealDamageDataForCalculate
//{
//    public BulleteRealDamageDataForCalculate(GameObject damageTarget, Vector2 triggerPoint,Vector2 direction,float criticalRadious = 0 )
//    {
//        target = damageTarget;
//        TriggerPoint = triggerPoint;
//        BulleteDirection = direction;
//        CriticalHitRadious = criticalRadious;
//    }

//    public GameObject target { get; private set; }
//    public Vector2 TriggerPoint { get; private set; }
//    public Vector2 BulleteDirection { get; private set; }

//    public float CriticalHitRadious { get; private set; }
//}

public interface CalculateDamageStrategy
{
    float Calculate(float originDamage);
    /// <summary>
    /// ¥]§tsetactive to true
    /// </summary>
    /// <param name="originDamage"></param>
    /// <param name="text"></param>
    void ShowDamage(float originalDamage, Vector2 textPosition);
}

public class DefaultBulleteDamageStrategy : CalculateDamageStrategy
{
    public float Calculate(float originDamage)
    {
        return originDamage;
    }

    public void ShowDamage(float originalDamage, Vector2 position)
    {
        DamageTextManager.instance.ShowHpDamage(originalDamage, position);
    }
}
