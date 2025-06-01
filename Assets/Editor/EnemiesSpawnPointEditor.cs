using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EnemyNameSpace
{
    [CustomEditor(typeof(EnemiesSpawnPoint))]
    public class EnemiesSpawnPointEditor : Editor
    {
        SerializedProperty enemyPrefabProp;
        SerializedProperty enemiesDatabaseProp;
        SerializedProperty enemySpawnInfosProp;
        SerializedProperty fixedSpawnInfosProp;
        SerializedProperty spawnTypeProp;
        SerializedProperty spawnCountProp;
        SerializedProperty spawnIntervalProp;
        SerializedProperty maxEnemiesProp;
        SerializedProperty randomRadiusProp;
        SerializedProperty waveCountProp;
        SerializedProperty triggerRadiusProp;
        SerializedProperty totalEnemiesProp;
        SerializedProperty spawnerStartDelayProp;
        SerializedProperty spawnDelayProp;
        SerializedProperty dependentSpawnerProp;

        void OnEnable()
        {
            enemyPrefabProp = serializedObject.FindProperty("enemyPrefab");
            enemiesDatabaseProp = serializedObject.FindProperty("enemiesDatabase");
            enemySpawnInfosProp = serializedObject.FindProperty("enemySpawnInfos");
            fixedSpawnInfosProp = serializedObject.FindProperty("fixedSpawnInfos");
            spawnTypeProp = serializedObject.FindProperty("spawnType");
            spawnCountProp = serializedObject.FindProperty("spawnCount");
            spawnIntervalProp = serializedObject.FindProperty("spawnInterval");
            maxEnemiesProp = serializedObject.FindProperty("maxEnemies");
            randomRadiusProp = serializedObject.FindProperty("randomRadius");
            waveCountProp = serializedObject.FindProperty("waveCount");
            triggerRadiusProp = serializedObject.FindProperty("triggerRadius");
            totalEnemiesProp = serializedObject.FindProperty("totalEnemies");
            spawnerStartDelayProp = serializedObject.FindProperty("spawnerStartDelay");
            spawnDelayProp = serializedObject.FindProperty("spawnDelay");
            dependentSpawnerProp = serializedObject.FindProperty("dependentSpawner");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("生成設定", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(enemyPrefabProp, new GUIContent("敵人預製體"));
            EditorGUILayout.PropertyField(enemiesDatabaseProp, new GUIContent("敵人資料庫"));
            EditorGUILayout.PropertyField(spawnTypeProp, new GUIContent("生成模式"));

            var spawnType = (SpawnType)spawnTypeProp.enumValueIndex;

            if (spawnType == SpawnType.固定)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("固定生成點設定", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(fixedSpawnInfosProp, new GUIContent("生成點與敵人ID列表"), true);
                EditorGUILayout.HelpBox("每一項填寫生成點(Transform)與敵人ID。", MessageType.Info);
            }
            else // 隨機
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("多種敵人與機率設定", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(enemySpawnInfosProp, new GUIContent("敵人與機率列表"), true);
                EditorGUILayout.HelpBox("每一項填寫敵人ID與出現機率權重（例如：ID=0, 權重=50）", MessageType.Info);
                EditorGUILayout.PropertyField(spawnCountProp, new GUIContent("每波生成數量"));
                EditorGUILayout.PropertyField(randomRadiusProp, new GUIContent("隨機生成半徑"));
            }

            EditorGUILayout.PropertyField(spawnIntervalProp, new GUIContent("生成間隔(秒)"));
            EditorGUILayout.PropertyField(maxEnemiesProp, new GUIContent("同時存在最大敵人數"));
            EditorGUILayout.PropertyField(waveCountProp, new GUIContent("生成波數 (0為無限)"));
            EditorGUILayout.PropertyField(totalEnemiesProp, new GUIContent("這個spawner能生成的敵人數量"));
            EditorGUILayout.PropertyField(spawnerStartDelayProp, new GUIContent("啟用延遲(秒)"));
            EditorGUILayout.PropertyField(spawnDelayProp, new GUIContent("每隻怪物生成間隔(秒)"));
            EditorGUILayout.PropertyField(dependentSpawnerProp, new GUIContent("依賴的Spawner(結束後才會啟動)"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("觸發設定", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(triggerRadiusProp, new GUIContent("觸發範圍半徑"));

            serializedObject.ApplyModifiedProperties();
        }
    }
} 