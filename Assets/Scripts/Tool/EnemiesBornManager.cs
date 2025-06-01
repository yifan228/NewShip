using System.Collections.Generic;
using UnityEngine;

namespace EnemyNameSpace
{
    public class EnemiesBornManager : MonoBehaviour
    {
       private Transform player;
        public static EnemiesBornManager Instance { get; private set; }
        public List<EnemyController> Enemies { get; private set; } = new List<EnemyController>();
        private List<EnemiesSpawnPoint> spawnPoints = new List<EnemiesSpawnPoint>();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Init(Transform player)
        {
            this.player = player;
            // 自動尋找場景中所有 EnemiesSpawnPoint
            spawnPoints = new List<EnemiesSpawnPoint>(GetComponentsInChildren<EnemiesSpawnPoint>());
            Debugger.Log(DebugCategory.Enemy,$"spawnPoints: {spawnPoints.Count}");
            // 訂閱所有生成點的生成/死亡事件
            foreach (var sp in spawnPoints)
            {
                sp.Init(player);
                sp.OnEnemySpawned += RegisterEnemy;
                sp.OnEnemyDied += UnregisterEnemy;
                // 將已經存在的敵人也加入（如場景預設）
                foreach (var e in sp.GetCurrentEnemies())
                {
                    RegisterEnemy(e);
                }
            }
        }

        private void RegisterEnemy(EnemyController enemy)
        {
            Debugger.Log(DebugCategory.Enemy, "RegisterEnemy", $"RegisterEnemy: {enemy.name}");
            Enemies.Add(enemy);
        }

        private void UnregisterEnemy(EnemyController enemy)
        {
            Enemies.Remove(enemy);
            Debugger.Log(DebugCategory.Enemy, "UnregisterEnemy", $"UnregisterEnemy: {enemy.name}");
        }
    }
}
