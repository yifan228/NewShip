
using UnityEditor;
using UnityEngine;


namespace EnemyNameSpace
{
    [CustomEditor(typeof(EnemyData))]
    public class EnemyDataEditor : Editor
    {
        SerializedProperty dataProperty;
        EnemyData enemyData;
        //private void OnValidate()
        //{
        //    Debug.Log("123456789");
        //    enemyData = (EnemyData)target;
        //    if(enemyData.Data.Equals(default))
        //    {
        //        enemyData.Data = new EnemyDataStruct();
        //    }

        //    EditorUtility.SetDirty(enemyData); // 標記為已變更，確保保存
        //    dataProperty = serializedObject.FindProperty("Data");
        //}
        public override void OnInspectorGUI()
        {
            enemyData = (EnemyData)target;
            serializedObject.Update();

            dataProperty = serializedObject.FindProperty("Data");

            EditorGUILayout.LabelField("Enemy Settings", EditorStyles.boldLabel);

            // 基本屬性
            enemyData.Data.Name = EditorGUILayout.TextField("名稱(編輯器易讀)", enemyData.Data.Name);
            EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("KeyString"));
            EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("Sprite"));
            EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("AIData"));

            // Motion Pattern
            // SerializedProperty motionPattern = dataProperty.FindPropertyRelative("MotionPattern");
            // EditorGUILayout.PropertyField(motionPattern);

            // if ((EnemyMotionPattern)motionPattern.enumValueIndex == EnemyMotionPattern.ChasePlayer)
            // {
            //     SerializedProperty moveAndRotateData = dataProperty.FindPropertyRelative("MoveAndRotateData");
            //     EditorGUILayout.LabelField("移動與旋轉數據", EditorStyles.boldLabel);

            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("MaxMoveSpeed"), new GUIContent("最大速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("MinMoveSpeed"), new GUIContent("最小速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("Acceleration"), new GUIContent("加速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("Deceleration"), new GUIContent("減速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("Range"), new GUIContent("判斷加速或減速的距離"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("MaxAngularSpeed"), new GUIContent("最大旋轉速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("MinAngularSpeed"), new GUIContent("最小旋轉速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("RotateAcceleration"), new GUIContent("旋轉加速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("RotateDeceleration"), new GUIContent("旋轉減速度"));
            //     EditorGUILayout.PropertyField(moveAndRotateData.FindPropertyRelative("AngularRange"), new GUIContent("判斷角加速或減速的距離"));
            // }

            // Pick Pattern
            // SerializedProperty picktarget = dataProperty.FindPropertyRelative("PickTargetType");
            // EditorGUILayout.PropertyField(picktarget);

            // Attack Pattern
            // SerializedProperty attackPattern = dataProperty.FindPropertyRelative("AttackPattern");
            // EditorGUILayout.PropertyField(attackPattern);

            // if ((EnemyAttackPattern)attackPattern.enumValueIndex == EnemyAttackPattern.Bullete)
            // {
            //     EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("DroneKeyString"));
            //     EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("FirePosition"));
            //     EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("FusilladeCount"));
            //     EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("DroneCount")); 
            //     EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("DroneAngle"));
            // }
            // else if ((EnemyAttackPattern)attackPattern.enumValueIndex == EnemyAttackPattern.Homing)
            // {

            // }

            // 通用屬性
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
            enemyData.Data.HPdata.MaxHp = EditorGUILayout.FloatField("Hp", enemyData.Data.HPdata.MaxHp);
            enemyData.Data.HPdata.Armormax = EditorGUILayout.FloatField("最大護甲值", enemyData.Data.HPdata.Armormax);
            enemyData.Data.HPdata.ArmorrecoverSpd = EditorGUILayout.FloatField("護甲回復速度/秒", enemyData.Data.HPdata.ArmorrecoverSpd);
            enemyData.Data.HPdata.ArmorminReSpd = EditorGUILayout.FloatField("最低護甲回復速度/秒", enemyData.Data.HPdata.ArmorminReSpd);
            enemyData.Data.HPdata.ArmorstartRecoverTime = EditorGUILayout.FloatField("護甲回復延遲時間", enemyData.Data.HPdata.ArmorstartRecoverTime);
            enemyData.Data.HpDamage = EditorGUILayout.FloatField("對生命攻擊", enemyData.Data.HpDamage);
            enemyData.Data.ArmorDamage = EditorGUILayout.FloatField("對護甲攻擊", enemyData.Data.ArmorDamage);
            enemyData.Data.Crash = EditorGUILayout.FloatField("擊破時造成的倍率", enemyData.Data.Crash);

            EditorGUILayout.Space();

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(enemyData);
            }

        }
    }
}