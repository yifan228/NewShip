using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ScriptableDatabase/BulleteDatabase", menuName = "Data/TangentCargoUpgradeData")]
public class TangentCargoDroneSO : ScriptableObject
{
    public List<CargoWeaponTangentLvData> Data;
}
[Serializable]
public class CargoWeaponTangentLvData
{
    public int Lv;
    public Sprite testSprite;
    public Vector2 Scale;
    public float CastInterval;
    public float Damage;
    public float Speed;
    public int MaxDamageCount;
    public float detectRadius;
}
