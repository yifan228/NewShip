using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSecondBoss : ABoss
{
    List<BossAttackPattern> attackPatterns;
    private bool isTrashTalking;

    public override void ActivateBoss()
    {
        gameObject.SetActive(true);
        currentHp = totalHp;
        ChangeState(BossBattleState.TrashTalk);
    }
    private void Start()
    {

        //子彈順序 0:
        attackPatterns = new List<BossAttackPattern>() { new CircleAttackAndMove(this,3f,0.25f) };

        bossBulleteDatas = new List<DTOBulleteData>();

        for (int i = 0; i < BossBulleteDatabase.Bulletes.Count; i++)
        {
            bossBulleteDatas.Add(CreateDTOBulleteData(BossBulleteDatabase.Bulletes[i]));
        }

        SetPattern(attackPatterns[0]);
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
                SetPattern(attackPatterns[0]);
            }
        }
        else if (state == BossBattleState.TrashTalk)
        {
            if (isTrashTalking)
            {
                return;
            }
            isTrashTalking = true;
        }
        else if (state == BossBattleState.Idle)
        {

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
        // this boss don't use attack range
    }

    private void Die()
    {
        isDefeated = true;
        bulletePool.AllReturn();
        OnDie();
    }
}
