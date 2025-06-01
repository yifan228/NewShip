using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[Serializable]
[CreateAssetMenu(fileName = "WeaponName", menuName = "Data/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public WeaponData weaponData;
}
//[System.Flags]
//public enum WeaponDrives
//{
//    Left_MachineGun=1<<0,
//    Right_MachineGun = 1 << 1,
//    Left_HomingMissle = 1<<2,
//    Right_HomingMissle = 1<<3
//}


[CustomEditor(typeof(WeaponDataSO))]
public class WeaponSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WeaponDataSO so = (WeaponDataSO)target;
        //WeaponDrives currentOptions = so.driveNames;
        //so.driveNames = (WeaponDrives)EditorGUILayout.MaskField(
        //    "此武器支援的輸入模式",
        //(int)currentOptions,
        //    System.Enum.GetNames(typeof(WeaponDrives))
        //);

        so.weaponData.WeaponName = EditorGUILayout.TextField("名稱(只用於inspector易讀)", so.weaponData.WeaponName);
        so.weaponData.WeaponKeyString = EditorGUILayout.TextField("武器keystring", so.weaponData.WeaponKeyString);
        so.weaponData.pickTargetType = (PickTargetType)EditorGUILayout.EnumPopup("選擇目標方式", so.weaponData.pickTargetType);
        so.weaponData.BulleteKeyString = EditorGUILayout.TextField("子彈的KeyString", so.weaponData.BulleteKeyString);
        so.weaponData.CD = EditorGUILayout.FloatField("每發的間隔", so.weaponData.CD);
        so.weaponData.Magzine = EditorGUILayout.IntField("彈匣容量", so.weaponData.Magzine);
        //so.weaponData.RotateRecoil = EditorGUILayout.FloatField("影響轉動的後座力", so.weaponData.RotateRecoil);

        //so.weaponData.MoveRecoil = EditorGUILayout.FloatField("影響移動的後座力", so.weaponData.MoveRecoil);
        //so.weaponData.ShootAngleFrom = EditorGUILayout.FloatField("武器能夠轉動的夾角from", so.weaponData.ShootAngleFrom);
        //so.weaponData.ShootAngleTo = EditorGUILayout.FloatField("武器能夠轉動的夾角to", so.weaponData.ShootAngleTo);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
