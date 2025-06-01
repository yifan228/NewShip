using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableDatabase/BulleteDatabase", menuName = "Data/BulleteDatabase")]
public class BulleteDatabase : ScriptableObject
{
    [SerializeField]List<BulleteDataSetupData> bulletes;
    public List<BulleteDataSetupData> Bulletes { get { return bulletes; } }
}

[Serializable]
public class BulleteDataSetupData : AbstractWeaponData
{
    public float InitialSpeed;
    public float TinderCost;
    public BulleteBehaviourType BehaviourType;
    public BulleteType BulleteDisplayType;
    public int InitialMaxDamageEnemyAmount;
    public float Radius;
    public string ExplodeFXName;
    public string BulleteDisplayFXName;
}

public class DTOBulleteData : AbstractWeaponData
{
    public float DTOSpeed;
    public int DTOMaxDamagedEnemyAmount;
    public float Damage;
    public float Radius;
    public BulleteType DisplayType;
    public BulleteBehaviourType BehaviourType;
    public string ExplodeFxName;
    public string BulleteDisplayFXName;
}


public abstract class AbstractWeaponData
{
    public string Name;
    public string WeaponEnglishIntro;
    public string WeaponChineseIntro;
    public string SavedName;
    public int Id;
    [HideInInspector] public int Amount = 1;

    public float InitialPower;//in charge case this is the shoot power after calculate
    public float BaseDamage;
    public Sprite sprite;
    public string UiSpriteName;
   // public WeaponEmitAmount stage;
    //public MerchandiseData MerchandiseData;
}

public enum BulleteBehaviourType
{
    BossDefault,
    SplitCargoBullete
}

public enum BulleteType
{
    SpriteBullete,
    ParticleSystemBullete,
    Laser,
    Rope,
}

