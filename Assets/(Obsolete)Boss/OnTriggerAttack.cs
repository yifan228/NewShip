using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OnTriggerAttack : MonoBehaviour,Attacker
{
    [SerializeField] TagsEnum targetTag;
    [SerializeField] TagsEnum thisAttackerName;
    [SerializeField] float damage;
    [Tooltip("any negative number represent no cd *the cd of the attacked integer is implement on attacked target")]
    [SerializeField] float attackCD;

    [Tooltip("if true this attacker will attack target at OnTriggerEnter")]
    [SerializeField] bool isAttackFirstime;
    float timer;
    public float Damage { set { damage = value; } }

    private void OnEnable()
    {
        if (isAttackFirstime)
        {
            timer = attackCD+1;
        }
        else
        {
            timer = 0;
        }
    }
    public void Attack(AttackedTarget target)
    {
        if (target is null)
        {
            return;
        }
        
        target.BeAttacked(new DamageStruct() { Damage = damage}, thisAttackerName.ToString(), AttackedType.Hit);
    }
    public void AddDebuff(GameObject gameObject)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {    
        AddDebuff(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (attackCD<0)
        {
            Attack(collision.GetComponent<AttackedTarget>());
        }
        else
        {
            timer += Time.deltaTime;
            if (timer>attackCD)
            {
                timer = 0;
                Attack(collision.GetComponent<AttackedTarget>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag.ToString()))
        {
            collision.GetComponent<AttackedTarget>().ExitAttack();
        }
        if (attackCD > 0)
        {
            if (isAttackFirstime)
            {
                timer = attackCD + 1;
            }
            else
            {
                timer = 0;
            }
        }
    }
}

[Obsolete]
/// <summary>
/// you can add OnTriggerAttack component if you want to use ontriggerEnter2D to trigger attack
/// </summary>
public interface Attacker
{
    /// <summary>
    /// target is the object that is attcked by this attacker
    /// </summary>
    /// <param name="target"></param>
    public void Attack(AttackedTarget target);
}
