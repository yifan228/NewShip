using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DroneProjectileDataBase))]
public class DroneProjectileDataBaseEditor : Editor
{
    private SerializedProperty dataList;
    private bool showList = true;
    private List<bool> foldouts = new List<bool>(); // 控制每個項目的折疊狀態
    private string newKeyString = ""; // 用於輸入 KeyString

    private void OnEnable()
    {
        dataList = serializedObject.FindProperty("Data");
        for (int i = 0; i < dataList.arraySize; i++)
        {
            foldouts.Add(true); // 預設所有項目展開
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Drone Projectile Database", EditorStyles.boldLabel);

        // 主列表的折疊控制
        showList = EditorGUILayout.Foldout(showList, "Projectile Data List", true, EditorStyles.foldout);
        if (showList)
        {
            EditorGUILayout.BeginVertical("box");

            for (int i = 0; i < dataList.arraySize; i++)
            {
                if (i >= foldouts.Count) foldouts.Add(true); // 確保列表同步

                SerializedProperty item = dataList.GetArrayElementAtIndex(i);
                DrawProjectileData(item, i);
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("➕ Add New Projectile", GUILayout.Height(25)))
            {
                AddNewProjectile();
            }

            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProjectileData(SerializedProperty item, int index)
    {
        SerializedProperty key = item.FindPropertyRelative("ProjectileKeyString");
        SerializedProperty movementType = item.FindPropertyRelative("MovementType");
        SerializedProperty triggerDmgType = item.FindPropertyRelative("TriggerDmgType");
        SerializedProperty onHitSkill = item.FindPropertyRelative("onHitSkill");
        SerializedProperty hitfx = item.FindPropertyRelative("HitFX");
        // 外框線（上）
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginVertical("HelpBox");

        // 折疊標題
        EditorGUILayout.BeginHorizontal();
        foldouts[index] = EditorGUILayout.Foldout(foldouts[index], $"Projectile {index + 1}: {key.stringValue}", true, EditorStyles.foldout);

        if (GUILayout.Button("❌", GUILayout.Width(25), GUILayout.Height(20)))
        {
            dataList.DeleteArrayElementAtIndex(index);
            foldouts.RemoveAt(index);
            return;
        }
        EditorGUILayout.EndHorizontal();

        if (foldouts[index])
        {
            EditorGUILayout.Space();
            key.stringValue = EditorGUILayout.TextField("Key String", key.stringValue);
            EditorGUILayout.PropertyField(item.FindPropertyRelative("DroneProjectileType"), new GUIContent("Drone Projectile Type"));
            EditorGUILayout.PropertyField(item.FindPropertyRelative("LifeTime"), new GUIContent("Life Time"));
            // Trigger Dmg Type
            EditorGUILayout.PropertyField(item.FindPropertyRelative("TriggerDmgType"), new GUIContent("Trigger Dmg Type"));
            if((TriggerDmgType)triggerDmgType.enumValueIndex !=TriggerDmgType.OnTriggerEnter){
                EditorGUILayout.PropertyField(item.FindPropertyRelative("DurationDamageCD"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("DurationDamageMaxCount"));
            }
            // Movement Type
            movementType.enumValueIndex = (int)(ProjectileMovementType)EditorGUILayout.EnumPopup("Movement Type", (ProjectileMovementType)movementType.enumValueIndex);
            if ((ProjectileMovementType)movementType.enumValueIndex == ProjectileMovementType.Stright ||
                (ProjectileMovementType)movementType.enumValueIndex == ProjectileMovementType.HomingMissle)
            {
                EditorGUILayout.PropertyField(item.FindPropertyRelative("MaxSpeed"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("Acceleration"));
            }
            if ((ProjectileMovementType)movementType.enumValueIndex == ProjectileMovementType.HomingMissle)
            {
                EditorGUILayout.PropertyField(item.FindPropertyRelative("MaxAngularSpeed"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("AngularAcceleration"));
            }
            if ((ProjectileMovementType)movementType.enumValueIndex == ProjectileMovementType.StraitHoming)
            {
                EditorGUILayout.PropertyField(item.FindPropertyRelative("MaxAngularSpeed"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("MaxSpeed"));
            }

            // permission count
            EditorGUILayout.PropertyField(item.FindPropertyRelative("PermissionCount"));
            
            // On Hit Skill
            onHitSkill.enumValueIndex = (int)(OnHitSkill)EditorGUILayout.EnumPopup("On Hit Skill", (OnHitSkill)onHitSkill.enumValueIndex);
            hitfx.objectReferenceValue = EditorGUILayout.ObjectField("Hit FX", hitfx.objectReferenceValue, typeof(VFXBase), false);
            if ((OnHitSkill)onHitSkill.enumValueIndex == OnHitSkill.Explode)
            {
                EditorGUILayout.PropertyField(item.FindPropertyRelative("ExplodeProjectileKeyString"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("ExplodeRange"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("Out_EplodeHpDmg"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("Out_EplodeArmorDmg"));
            }
            else if ((OnHitSkill)onHitSkill.enumValueIndex == OnHitSkill.Split)
            {
                EditorGUILayout.PropertyField(item.FindPropertyRelative("SplitProjectileKeyString"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("SplitAcount"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("AngleFrom"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("AngleTo"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("Out_SplitHpDmg"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("Out_SplitArmorDmg"));
            }
            else if ((OnHitSkill)onHitSkill.enumValueIndex == OnHitSkill.Field)
            {
                EditorGUILayout.PropertyField(item.FindPropertyRelative("FieldProjectileKeyString"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("FieldRange"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("FieldTime"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("Out_FieldHpDmg"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("Out_FieldArmorDmg"));
            }

            // 物件大小
            EditorGUILayout.PropertyField(item.FindPropertyRelative("ObjSize"), new GUIContent("Object Size"));

            // 特效預覽
            EditorGUILayout.PropertyField(item.FindPropertyRelative("View"), new GUIContent("View"));
        }

        EditorGUILayout.EndVertical();

        // 外框線（下）
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
    }

    private void AddNewProjectile()
    {
        // 彈出輸入框，讓使用者輸入 KeyString
        string inputKey = EditorInputDialog.Show("Add New Projectile", "Enter the KeyString of the projectile you want to copy (leave blank for default):", "");

        if (string.IsNullOrEmpty(inputKey))
        {
            CopyLastProjectile();
        }
        else
        {
            CopyExistingProjectile(inputKey);
        }
    }

    private void CopyExistingProjectile(string key)
    {
        for (int i = 0; i < dataList.arraySize; i++)
        {
            SerializedProperty existingItem = dataList.GetArrayElementAtIndex(i);
            if (existingItem.FindPropertyRelative("ProjectileKeyString").stringValue == key)
            {
                dataList.arraySize++;
                SerializedProperty newItem = dataList.GetArrayElementAtIndex(dataList.arraySize - 1);

                CopyProjectileData(existingItem, newItem, key + "_Copy");
                foldouts.Add(true);
                return;
            }
        }
        CopyLastProjectile();
    }

    private void CopyLastProjectile()
    {
        if (dataList.arraySize > 0)
        {
            SerializedProperty lastItem = dataList.GetArrayElementAtIndex(dataList.arraySize - 1);
            dataList.arraySize++;
            SerializedProperty newItem = dataList.GetArrayElementAtIndex(dataList.arraySize - 1);

            CopyProjectileData(lastItem, newItem, lastItem.FindPropertyRelative("ProjectileKeyString").stringValue + "_Copy");
        }
        else
        {
            dataList.arraySize++;
        }
        foldouts.Add(true);
    }

    private void CopyProjectileData(SerializedProperty source, SerializedProperty target, string newKey)
    {
        // 複製基本屬性
        target.FindPropertyRelative("ProjectileKeyString").stringValue = newKey;
        target.FindPropertyRelative("LifeTime").floatValue = source.FindPropertyRelative("LifeTime").floatValue;
        target.FindPropertyRelative("MovementType").enumValueIndex = source.FindPropertyRelative("MovementType").enumValueIndex;

        target.FindPropertyRelative("MaxSpeed").floatValue = source.FindPropertyRelative("MaxSpeed").floatValue;
        target.FindPropertyRelative("Acceleration").floatValue = source.FindPropertyRelative("Acceleration").floatValue;
        target.FindPropertyRelative("MaxAngularSpeed").floatValue = source.FindPropertyRelative("MaxAngularSpeed").floatValue;
        target.FindPropertyRelative("AngularAcceleration").floatValue = source.FindPropertyRelative("AngularAcceleration").floatValue;

        target.FindPropertyRelative("PermissionCount").intValue = source.FindPropertyRelative("PermissionCount").intValue;
        target.FindPropertyRelative("HitFX").objectReferenceValue = source.FindPropertyRelative("HitFX").objectReferenceValue;

        // 複製 OnHitSkill 相關
        target.FindPropertyRelative("onHitSkill").enumValueIndex = source.FindPropertyRelative("onHitSkill").enumValueIndex;
        target.FindPropertyRelative("ExplodeRange").floatValue = source.FindPropertyRelative("ExplodeRange").floatValue;

        CopyHpDmgData(source.FindPropertyRelative("Out_EplodeHpDmg"), target.FindPropertyRelative("Out_EplodeHpDmg"));
        CopyArmorDmgData(source.FindPropertyRelative("Out_EplodeArmorDmg"), target.FindPropertyRelative("Out_EplodeArmorDmg"));

        // Explode 相關
        target.FindPropertyRelative("ExplodeProjectileKeyString").stringValue = source.FindPropertyRelative("ExplodeProjectileKeyString").stringValue;
        target.FindPropertyRelative("Out_EplodeHpDmg").floatValue = source.FindPropertyRelative("Out_EplodeHpDmg").floatValue;
        target.FindPropertyRelative("Out_EplodeArmorDmg").floatValue = source.FindPropertyRelative("Out_EplodeArmorDmg").floatValue;

        // Split 相關
        target.FindPropertyRelative("SplitAcount").intValue = source.FindPropertyRelative("SplitAcount").intValue;
        target.FindPropertyRelative("AngleFrom").floatValue = source.FindPropertyRelative("AngleFrom").floatValue;
        target.FindPropertyRelative("AngleTo").floatValue = source.FindPropertyRelative("AngleTo").floatValue;

        CopyHpDmgData(source.FindPropertyRelative("Out_SplitHpDmg"), target.FindPropertyRelative("Out_SplitHpDmg"));
        CopyArmorDmgData(source.FindPropertyRelative("Out_SplitArmorDmg"), target.FindPropertyRelative("Out_SplitArmorDmg"));

        // Field 相關
        target.FindPropertyRelative("FieldRange").floatValue = source.FindPropertyRelative("FieldRange").floatValue;
        target.FindPropertyRelative("FieldTime").floatValue = source.FindPropertyRelative("FieldTime").floatValue;

        CopyHpDmgData(source.FindPropertyRelative("Out_FieldHpDmg"), target.FindPropertyRelative("Out_FieldHpDmg"));
        CopyArmorDmgData(source.FindPropertyRelative("Out_FieldArmorDmg"), target.FindPropertyRelative("Out_FieldArmorDmg"));

    }

    // 封裝 HpDmgData 的複製
    private void CopyHpDmgData(SerializedProperty source, SerializedProperty target)
    {
        target.FindPropertyRelative("Dmg").floatValue = source.FindPropertyRelative("Dmg").floatValue;
        target.FindPropertyRelative("CriticalDamage_Percentage").floatValue = source.FindPropertyRelative("CriticalDamage_Percentage").floatValue;
        target.FindPropertyRelative("Critical_Percentage").floatValue = source.FindPropertyRelative("Critical_Percentage").floatValue;
        target.FindPropertyRelative("IgnoreArmor").floatValue = source.FindPropertyRelative("IgnoreArmor").floatValue;
    }

    // 封裝 ArmorDmgData 的複製
    private void CopyArmorDmgData(SerializedProperty source, SerializedProperty target)
    {
        target.FindPropertyRelative("Dmg").floatValue = source.FindPropertyRelative("Dmg").floatValue;
        target.FindPropertyRelative("OnCrash").floatValue = source.FindPropertyRelative("OnCrash").floatValue;
        target.FindPropertyRelative("SuperCrashDmg_Percentage").floatValue = source.FindPropertyRelative("SuperCrashDmg_Percentage").floatValue;
    }

}

public class EditorInputDialog : EditorWindow
{
    private static EditorInputDialog window;
    private string inputText = "";
    private string titleText;
    private string messageText;
    private static string returnValue = "";

    public static string Show(string title, string message, string defaultValue = "")
    {
        returnValue = "";
        window = CreateInstance<EditorInputDialog>();
        window.titleText = title;
        window.messageText = message;
        window.inputText = defaultValue;
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 150);
        window.ShowModal(); // **這行確保對話框是模態的**
        return returnValue;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField(titleText, EditorStyles.boldLabel);
        EditorGUILayout.LabelField(messageText);
        inputText = EditorGUILayout.TextField(inputText);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Confirm"))
        {
            returnValue = inputText;
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            returnValue = "";
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}