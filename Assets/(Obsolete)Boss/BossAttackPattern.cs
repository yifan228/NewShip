using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;


#region Patterns
public abstract class BossAttackPattern
{
    protected Transform target;
    protected ABoss boss;

    public void Settings(Transform target, ABoss boss)
    {
        this.target = target;
        this.boss = boss;
    }
    /// <summary>
    /// return isEnd
    /// </summary>
    /// <param name="deltatime"></param>
    /// <param name="thisBoss"></param>
    /// <returns></returns>
    public abstract bool RunningPattern(float deltatime,ABoss thisBoss);
    public abstract void Reset();
}

public class CircleThrowPattern : BossAttackPattern
{
    float timer;
    float bulletesIntervalTimer;
    float bulletesIntervalTimeSettings = 0.3f;
    float bulletesIntervalTime ;//全部子彈發射完成4.5秒
    float durationSettings = 9;
    float duration ;
    float theta;
    float startThrowTime = 2;
    //List<int> bulletesIndex = new List<int>() { 0, 1 };
    bool isbattleTalking = false;
    public CircleThrowPattern(ABoss thisBoss,float bulletesIntervalTimeSettings, float durationSettings)
    {
        this.bulletesIntervalTimeSettings = bulletesIntervalTimeSettings;
        bulletesIntervalTime = bulletesIntervalTimeSettings;
        this.durationSettings = durationSettings;
        boss = thisBoss;
    }

    public override void Reset()
    {
        timer = 0;
        bulletesIntervalTimer = 0;
        bulletesIntervalTime = bulletesIntervalTimeSettings;
        duration = durationSettings;
        theta = 0;
    }

    public override bool RunningPattern(float deltatime,ABoss thisBoss)
    {
        if (timer < startThrowTime)
        {
            if (!isbattleTalking)
            {
                thisBoss.TalkDuringBattle("呼拉屋哈胡啦!");
                isbattleTalking = true;
            }
        }
        else
        {
            if (isbattleTalking)
            {
                thisBoss.EndBattleTalk();
                isbattleTalking = false;
            }

            if (bulletesIntervalTimer>=bulletesIntervalTime && theta < 2 * Mathf.PI)
            {
                Vector2 direction = new Vector2(Mathf.Cos(theta),Mathf.Sin(theta));
                boss.CreateBullete(0, boss.transform.position, direction);
                theta += 2*Mathf.PI/15f;
                bulletesIntervalTimer = 0;
            }
        }
        timer += deltatime;
        bulletesIntervalTimer += deltatime;
        if (timer >=duration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum AttackRangeType
{
    X,
    Ten,
    Eight
}
public class AttackRangeBulletePattern : BossAttackPattern
{
    private AttackRangeType type;
    float timer;
    float startAttackTimeSettings = 3;//之前是預判線的時間
    float startAttackTime;
    bool isDrawAttackRange;
    
    float durationSettings = 8;
    float duration;
    float attackIntervalTime = 0.5f;
    float attackIntervalTimer;
    /// <summary>
    /// boss bullete data index
    /// </summary>
    List<int> bulleteIndex;
    public AttackRangeBulletePattern(ABoss thisBoss,AttackRangeType type,float startAttackTimeSettings, float durationSettings, List<int> bulleteIndex)
    {
        this.startAttackTimeSettings = startAttackTimeSettings;
        this.durationSettings = durationSettings;
        boss = thisBoss;
        this.bulleteIndex = bulleteIndex;
        this.type = type;
    }

    public override void Reset()
    {
        timer = 0;
        startAttackTime = startAttackTimeSettings;
        duration = durationSettings;
        isDrawAttackRange = false;
        attackIntervalTimer = 0;
    }

    public override bool RunningPattern(float deltatime,ABoss thisBoss)
    {
        if (timer >= startAttackTime)
        {
            if (attackIntervalTimer>attackIntervalTime)
            {
                switch (type)
                {
                    case AttackRangeType.X:
                        AttackX();
                        break;
                    case AttackRangeType.Ten:
                        AttackTen();
                        break;
                    case AttackRangeType.Eight:
                        break;
                    default:
                        break;
                }
                
                attackIntervalTimer = 0;
            }
            attackIntervalTimer += Time.deltaTime;
        }
        else
        {
            if (!isDrawAttackRange)
            {
                switch (type)
                {
                    case AttackRangeType.X:
                        ShowAttackRangeX();
                        break;
                    case AttackRangeType.Ten:
                        ShowAttackRangeTen();
                        break;
                    case AttackRangeType.Eight:
                        break;
                    default:
                        break;
                }
            }
        }
        timer += deltatime;

        if (timer >= duration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #region Ten
    private void AttackTen()
    {
        int rng1 = UnityEngine.Random.Range(0,bulleteIndex.Count);
        int rng2 = UnityEngine.Random.Range(0, bulleteIndex.Count);
        int rng3 = UnityEngine.Random.Range(0, bulleteIndex.Count);
        int rng4 = UnityEngine.Random.Range(0, bulleteIndex.Count);
        boss.CreateBullete(bulleteIndex[rng1], boss.transform.position, Vector2.down);
        boss.CreateBullete(bulleteIndex[rng2], boss.transform.position, Vector2.up);
        boss.CreateBullete(bulleteIndex[rng3], boss.transform.position, Vector2.right);
        boss.CreateBullete(bulleteIndex[rng4], boss.transform.position, Vector2.left);
    }

    private void ShowAttackRangeTen()
    {
        boss.DrawAttackRange(boss.transform.position,Vector2.down);
        boss.DrawAttackRange(boss.transform.position, Vector2.up);
        boss.DrawAttackRange(boss.transform.position, Vector2.right);
        boss.DrawAttackRange(boss.transform.position, Vector2.left);
        isDrawAttackRange = true;
    }
    #endregion
    #region X
    private void AttackX()
    {
        int rng1 = UnityEngine.Random.Range(0, bulleteIndex.Count);
        int rng2 = UnityEngine.Random.Range(0, bulleteIndex.Count);
        int rng3 = UnityEngine.Random.Range(0, bulleteIndex.Count);
        int rng4 = UnityEngine.Random.Range(0, bulleteIndex.Count);
        boss.CreateBullete(bulleteIndex[rng1], boss.transform.position, Vector2.down+Vector2.right);
        boss.CreateBullete(bulleteIndex[rng2], boss.transform.position, Vector2.up + Vector2.right);
        boss.CreateBullete(bulleteIndex[rng3], boss.transform.position, Vector2.down + Vector2.left);
        boss.CreateBullete(bulleteIndex[rng4], boss.transform.position, Vector2.up + Vector2.left);
    }

    private void ShowAttackRangeX()
    {
        boss.DrawAttackRange(boss.transform.position, Vector2.down + Vector2.right);
        boss.DrawAttackRange(boss.transform.position, Vector2.up + Vector2.right);
        boss.DrawAttackRange(boss.transform.position, Vector2.down + Vector2.left);
        boss.DrawAttackRange(boss.transform.position, Vector2.up + Vector2.left);
        isDrawAttackRange = true;
    }
    #endregion
}

public class CircleAttackAndMove:BossAttackPattern
{
    float timer;
    float theta;
    float startThrowTime;
    float bulleteFireInterval;
    float lastFireTime;
    //List<int> bulletesIndex = new List<int>() { 0, 1 };
    Rigidbody2D thisRb2d;
    public CircleAttackAndMove(ABoss thisBoss, float startAttackTime,float bulleteFireInterval)
    {
        boss = thisBoss;
        thisRb2d = boss.GetComponent<Rigidbody2D>();
        if (thisRb2d is null)
        {
            Debug.Log("rb2d is null");
        }
        startThrowTime = startAttackTime;
        this.bulleteFireInterval = bulleteFireInterval;
    }

    public override void Reset()
    {
        timer = 0;
        theta = 0;
        lastFireTime = 0;
        tick = 0;
    }

    public override bool RunningPattern(float deltatime, ABoss thisBoss)
    {
        if (timer < startThrowTime)
        {
            moveSpeed = 3f;
        }
        else
        {
            if ((Time.time - lastFireTime)>bulleteFireInterval)
            {
                if (theta < 2 * Mathf.PI)
                {
                    moveSpeed = 0.5f;
                    Vector2 direction = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
                    boss.CreateBullete(0, boss.transform.position, direction);
                    theta += 2 * Mathf.PI / 15f;
                    lastFireTime = Time.time;
                }
                else
                {
                    return true;
                }
            }
        }
        timer += deltatime;
        Move();
        return false;
    }
    int tick = 0;
    float moveSpeed;
    private void Move()
    {
        tick++;
        if (tick >= 5)
        {
            Vector2 r = target.position - thisRb2d.transform.position;
            if (r.magnitude>8)
            {
                thisRb2d.velocity = r.normalized*moveSpeed;
            }
            else
            {
                thisRb2d.velocity = Vector2.zero;
            }
            tick = 0;
        }
    }
}
#endregion
