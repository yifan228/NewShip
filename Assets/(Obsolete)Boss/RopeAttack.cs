using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Obsolete]
public class RopeAttack : MonoBehaviour,Attacker
{
    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] Transform ship;
    [SerializeField] Transform cargo;
    [SerializeField] TagsEnum targetTag;
    [SerializeField] TagsEnum thisTag;
    [SerializeField] float damage;

    private HashSet<AttackedTarget> previousHitIds = new HashSet<AttackedTarget>(); // �e�@�V���������� ID
    private HashSet<AttackedTarget> currentHitIds = new HashSet<AttackedTarget>();  // ��e�V���������� ID
    public void Attack(AttackedTarget target)
    {
        if (!target.Equals(default))
        {
             
            currentHitIds.Add(target);
            target.BeAttacked(new DamageStruct() { Damage = damage}, thisTag.ToString(), AttackedType.Hit);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
        
        currentHitIds.Clear();
        RaycastHit2D[] ray = Physics2D.RaycastAll(ship.position, (cargo.position - ship.position).normalized, (cargo.position - ship.position).magnitude,targetLayerMask);
        //Debug.Log(ray.Length);
        if (ray != null)
        {
            for (int i=0;i<ray.Length;i++)
            {
                Attack(ray[i].collider.GetComponent<AttackedTarget>());
                
            }
        }

        HashSet<AttackedTarget> exitIds = new HashSet<AttackedTarget>(previousHitIds);
        exitIds.ExceptWith(currentHitIds);

        if (exitIds.Count!=0)
        {
            foreach (AttackedTarget item in exitIds)
            {
                item.ExitAttack();
            }
        }
    }
}
