using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ScriptableDatabase/BulleteDatabase", menuName = "Data/FieldCargoUpgradeData")]
public class FieldCargoDroneSO : ScriptableObject
{
    public List<CargoWeaponFieldLvData> Data;
}
[Serializable]
public class CargoWeaponFieldLvData
{
    public int Lv;
    public Sprite testSprite;
    public Vector2 Scale;
    public float CastInterval;
    public float Duration;
    public float Damage;
    public float Speed;
    public int MaxDamageCount;
    public float detectRadius;
}
