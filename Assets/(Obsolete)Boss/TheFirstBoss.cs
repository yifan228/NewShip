using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFirstBoss : ABoss
{
    int trashTalkIndex;
    private bool isTrashTalking;
    List<BossAttackPattern> attackPatterns;
    [SerializeField] AttackRange attackRangePrefab;
    Queue<AttackRange> attackRanges = new Queue<AttackRange>();
    List<AttackRange> activeAttackRanges = new List<AttackRange>();

    public override void ActivateBoss()
    {
        gameObject.SetActive(true);
        currentHp = totalHp;
        ChangeState(BossBattleState.TrashTalk);
        trashTalkIndex = 0;
    }
    private void Start()
    {

        //子彈順序 0:
        attackPatterns = new List<BossAttackPattern>() { new CircleThrowPattern(this,0.3f,9),
            new AttackRangeBulletePattern(this,AttackRangeType.X,3,8,new List<int>(){0}),
            new AttackRangeBulletePattern(this,AttackRangeType.Ten,3,8,new List<int>(){0 }) };

        bossBulleteDatas = new List<DTOBulleteData>();

        for (int i = 0; i < BossBulleteDatabase.Bulletes.Count; i++)
        {
            bossBulleteDatas.Add(CreateDTOBulleteData(BossBulleteDatabase.Bulletes[i]));
        }

        for (int i = 0; i < 8; i++)
        {
            PrewarmAttackRange();
        }
        SetPattern(attackPatterns[0]);

        state = BossBattleState.Battle;
    }
    private void PrewarmAttackRange()
    {
        AttackRange a = Instantiate(attackRangePrefab, transform);
        a.Recycle();
        attackRanges.Enqueue(a);
    }

    private void Update()
    {
        if (isDefeated)
        {
            return;
        }
        if (state == BossBattleState.Battle)
        {
            bool isEnd = currentPattern.RunningPattern(Time.deltaTime, this);
            if (isEnd)
            {
                if (currentPattern is AttackRangeBulletePattern)
                {
                    RecycleAttackRanges();
                }

                int rngnum = UnityEngine.Random.Range(0, attackPatterns.Count);
                SetPattern(attackPatterns[rngnum]);
            }
        }
        else if (state == BossBattleState.TrashTalk)
        {
            StartTalk();
        }
        else if (state == BossBattleState.Idle)
        {

        }
    }
    private void RecycleAttackRanges()
    {
        foreach (var item in activeAttackRanges)
        {
            item.Recycle();
            attackRanges.Enqueue(item);
        }
    }
    private void StartTalk()
    {
        if (isTrashTalking)
        {
            return;
        }

        switch (trashTalkIndex)
        {
            case 0:
                isTrashTalking = true;
                break;
            default:
                break;
        }
    }

    public override void GetHurt(float damage)
    {
        if (state == BossBattleState.Battle)
        {
            Debug.Log("Boss hurtedd : " + damage);
            currentHp -= damage;
            SetHpUI();
            if (currentHp < 0)
            {
                //先這樣
                Die();
            }
        }
    }


    public override void DrawAttackRange(Vector2 origin, Vector2 direction)
    {
        AttackRange ar = attackRanges.Dequeue();
        ar.SetScaleAndDirection(new Vector3(4, 10, 1), direction);
        ar.transform.position = origin;
        activeAttackRanges.Add(ar);
    }

    private void Die()
    {
        isDefeated = true;
        bulletePool.AllReturn();
        OnDie();
    }

    private void EndDialog1()
    {
        isTrashTalking = false;
        ChangeState(BossBattleState.Battle);
        trashTalkIndex++;
    }
}