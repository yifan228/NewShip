using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABoss : MonoBehaviour,AttackedTarget
{
    public Transform Target;
    protected BossAttackPattern currentPattern;
    protected BossBattleState state =  BossBattleState.Idle;
    public BulleteDatabase BossBulleteDatabase;
    protected List<DTOBulleteData> bossBulleteDatas;
    [SerializeField] Image bossHpBar;
    [SerializeField] protected BulletePool bulletePool;
    [SerializeField] protected float Damage;
    [SerializeField] protected float totalHp;
    public Action OnDie;
    protected bool isDefeated;
    protected float currentHp;

    public virtual void ActivateBoss()
    {
        
    }
    public void DeactivateBoss()
    {
        ChangeState(BossBattleState.Idle);
    }
    protected void SetPattern(BossAttackPattern pattern)
    {
        currentPattern = pattern;
        currentPattern.Settings(Target,this);
        currentPattern.Reset();
    }

    protected void SetHpUI()
    {
        float amount = currentHp / totalHp;
        bossHpBar.fillAmount = amount;
    }

    public virtual void GetHurt(float damage)
    {
        
    }
    public virtual void CreateBullete(int bulleteIndex, Vector2 emitPoint, Vector2 direction)
    {
        BulleteView bullete = bulletePool.GetBullete();
        bullete.Initialize(TagsEnum.Boss, new DefaultBulleteDamageStrategy(), bossBulleteDatas[bulleteIndex],Damage, emitPoint, direction);
    }

    public virtual void DrawAttackRange(Vector2 origin, Vector2 direction)
    {

    }

    public virtual void TalkDuringBattle(string content)
    {
        
    }

    public virtual void EndBattleTalk()
    {

    }
    protected void ChangeState(BossBattleState state)
    {
        this.state = state;
    }

    protected DTOBulleteData CreateDTOBulleteData(BulleteDataSetupData setupData)
    {
        if (setupData == null)
        {
            Debug.LogError("丟進CreateDTOBulleteData的setup data 是空的");
            return null;
        }
        DTOBulleteData data = new DTOBulleteData();
        data.Name = setupData.Name;
        data.Id = setupData.Id;
        data.ExplodeFxName = setupData.ExplodeFXName;
        data.BulleteDisplayFXName = setupData.BulleteDisplayFXName;
        data.DTOSpeed = setupData.InitialSpeed;
        data.InitialPower = setupData.InitialPower;

        data.Damage = setupData.BaseDamage;

        //data.sprite = setupData.sprite;
        data.BehaviourType = setupData.BehaviourType;
        data.DTOMaxDamagedEnemyAmount = setupData.InitialMaxDamageEnemyAmount;
        data.DisplayType = setupData.BulleteDisplayType;
        data.Radius = setupData.Radius;
        return data;
    }

    public void BeAttacked(DamageStruct damage, string attackerName,AttackedType type)
    {
        if (attackerName == TagsEnum.Player.ToString()) GetHurt(damage.Damage);
    }
}
public enum BossBattleState
{
    Idle,
    Battle,
    TrashTalk
}
