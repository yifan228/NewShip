using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace EnemyNameSpace
{
    public enum SpawnType
    {
        固定,
        隨機
    }

    [System.Serializable]
    public class EnemySpawnInfo
    {
        [Tooltip("敵人資料ID")]
        public int enemyId;
        [Tooltip("出現機率權重（越大越容易出現）")]
        public int weight = 1;
    }

    [System.Serializable]
    public class FixedSpawnInfo
    {
        [Tooltip("生成點位置（Transform）")]
        public Transform spawnPoint;
        [Tooltip("敵人資料ID")]
        public int enemyId;
    }

    public class EnemiesSpawnPoint : MonoBehaviour
    {
        public event Action<EnemyController> OnEnemySpawned;
        public event Action<EnemyController> OnEnemyDied;

        [Header("生成設定")]
        public EnemyController enemyPrefab;
        public EnemiesDatas enemiesDatabase;
        [Tooltip("可設定多種敵人與出現機率（隨機模式用）")] 
        public List<EnemySpawnInfo> enemySpawnInfos = new List<EnemySpawnInfo>();
        [Tooltip("固定生成點與敵人ID（固定模式用）")]
        public List<FixedSpawnInfo> fixedSpawnInfos = new List<FixedSpawnInfo>();
        [Tooltip("生成模式：固定或隨機")]
        public SpawnType spawnType = SpawnType.固定;
        [Tooltip("每波生成數量（隨機模式用）")]
        public int spawnCount = 1;
        [Tooltip("生成間隔(秒)")]
        public float spawnInterval = 2f;
        [Tooltip("啟用此Spawner的延遲(秒)")]
        public float spawnerStartDelay = 0f;
        [Tooltip("每隻怪物生成的間隔(秒)")]
        public float spawnDelay = 0f;
        [Tooltip("同時存在最大敵人數")]
        public int maxEnemies = 5;

        [Tooltip("這個spawner能生成的敵人數量")]
        public int totalEnemies = 9999;

        [Tooltip("隨機生成半徑（僅隨機模式有效）")]
        public float randomRadius = 3f;
        [Tooltip("生成波數 (0為無限)")]
        public int waveCount = 1;

        [Header("觸發設定")]
        [Tooltip("觸發範圍半徑")]
        public float triggerRadius = 5f;
        [Tooltip("這個spawner結束後，此spawner才會被觸發")]
        [SerializeField]private EnemiesSpawnPoint dependentSpawner = null;

        private readonly List<EnemyController> _spawnedEnemies = new List<EnemyController>();
        private float _spawnTimer = 0f;
        private int _currentWave = 0;
        private bool _playerInRange = false;
        private Transform _player;
        private bool _spawnerStarted = false;
        private int isSpawnedEnemyCount = 0;

        public void Init(Transform player)
        {
            _player = player;
        }

        private void Start()
        {
            // 啟動流程：先等依賴Spawner結束，再等延遲秒數，最後啟動本Spawner
            if (dependentSpawner != null)
            {
                StartCoroutine(WaitForDependencyAndDelayThenStart());
            }
            else
            {
                StartCoroutine(DelayThenStart());
            }
        }

        /// <summary>
        /// 等待依賴Spawner結束，再等延遲秒數，最後啟動本Spawner
        /// </summary>
        private IEnumerator WaitForDependencyAndDelayThenStart()
        {
            // 等待依賴Spawner結束
            while (dependentSpawner != null && !dependentSpawner.IsFinished())
            {
                yield return new WaitForSeconds(0.2f);
            }
            // 等延遲秒數
            if (spawnerStartDelay > 0f)
            {
                yield return new WaitForSeconds(spawnerStartDelay);
            }
            _spawnerStarted = true;
        }

        /// <summary>
        /// 只等延遲秒數，然後啟動本Spawner
        /// </summary>
        private IEnumerator DelayThenStart()
        {
            if (spawnerStartDelay > 0f)
            {
                yield return new WaitForSeconds(spawnerStartDelay);
            }
            _spawnerStarted = true;
        }

        public bool IsFinished()
        {
            return isSpawnedEnemyCount >= totalEnemies && _spawnedEnemies.Count == 0;
        }

        private void Update()
        {
            if (!_spawnerStarted) return;
            CheckPlayerInRange();
            if (!_playerInRange) return;
            if (waveCount > 0 && _currentWave >= waveCount) return;
            if (_spawnedEnemies.Count >= maxEnemies) return;

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= spawnInterval)
            {
                SpawnEnemies();
                _spawnTimer = 0f;
            }
        }

        private void CheckPlayerInRange()
        {
            if (_player == null) return;
            float dist = Vector2.Distance(transform.position, _player.position);
            _playerInRange = dist <= triggerRadius;
        }

        private void SpawnEnemies()
        {
            if (spawnDelay > 0f)
            {
                StartCoroutine(DelayedSpawnCoroutine());
                return;
            }
            isSpawnedEnemyCount++;
            if (spawnType == SpawnType.固定)
            {
                foreach (var info in fixedSpawnInfos)
                {
                    if (info.spawnPoint == null) continue;
                    if (_spawnedEnemies.Count >= maxEnemies) break;
                    var newEnemy = Instantiate(enemyPrefab, info.spawnPoint.position, Quaternion.identity);
                    newEnemy.Init(_player, enemiesDatabase.GetDataBase()[info.enemyId], OnEnemyDeath);
                    _spawnedEnemies.Add(newEnemy);
                    OnEnemySpawned.Invoke(newEnemy);
                    info.spawnPoint.gameObject.SetActive(false);
                }
            }
            else // 隨機
            {
                int canSpawn = Mathf.Min(spawnCount, maxEnemies - _spawnedEnemies.Count);
                for (int i = 0; i < canSpawn; i++)
                {
                    Vector2 spawnPos = _player.position;
                    float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
                    float r = UnityEngine.Random.Range(0f, randomRadius);
                    spawnPos += new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
                    int enemyId = GetRandomEnemyIdByWeight();
                    var newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                    newEnemy.Init(_player, enemiesDatabase.GetDataBase()[enemyId], OnEnemyDeath);
                    _spawnedEnemies.Add(newEnemy);
                    OnEnemySpawned.Invoke(newEnemy);
                }
            }
            _currentWave++;
        }

        private IEnumerator DelayedSpawnCoroutine()
        {
            if (spawnType == SpawnType.固定)
            {
                foreach (var info in fixedSpawnInfos)
                {
                    if (info.spawnPoint == null) continue;
                    if (_spawnedEnemies.Count >= maxEnemies) break;
                    var newEnemy = Instantiate(enemyPrefab, info.spawnPoint.position, Quaternion.identity);
                    newEnemy.Init(_player, enemiesDatabase.GetDataBase()[info.enemyId], OnEnemyDeath);
                    _spawnedEnemies.Add(newEnemy);
                    OnEnemySpawned.Invoke(newEnemy);
                    info.spawnPoint.gameObject.SetActive(false);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
            else // 隨機
            {
                int canSpawn = Mathf.Min(spawnCount, maxEnemies - _spawnedEnemies.Count);
                for (int i = 0; i < canSpawn; i++)
                {
                    Vector2 spawnPos = _player.position;
                    float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
                    float r = UnityEngine.Random.Range(0f, randomRadius);
                    spawnPos += new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
                    int enemyId = GetRandomEnemyIdByWeight();
                    var newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                    newEnemy.Init(_player, enemiesDatabase.GetDataBase()[enemyId], OnEnemyDeath);
                    _spawnedEnemies.Add(newEnemy);
                    OnEnemySpawned.Invoke(newEnemy);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
            _currentWave++;
        }

        private int GetRandomEnemyIdByWeight()
        {
            if (enemySpawnInfos == null || enemySpawnInfos.Count == 0)
                return 0;
            int totalWeight = 0;
            foreach (var info in enemySpawnInfos)
                totalWeight += info.weight;
            int rand = UnityEngine.Random.Range(0, totalWeight);
            int sum = 0;
            foreach (var info in enemySpawnInfos)
            {
                sum += info.weight;
                if (rand < sum)
                    return info.enemyId;
            }
            return enemySpawnInfos[0].enemyId;
        }

        private void OnEnemyDeath(EnemyController enemy)
        {
            if (_spawnedEnemies.Contains(enemy))
                _spawnedEnemies.Remove(enemy);
            OnEnemyDied?.Invoke(enemy);
            Destroy(enemy.gameObject);
        }

        public List<EnemyController> GetCurrentEnemies()
        {
            return new List<EnemyController>(_spawnedEnemies);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
            if (spawnType == SpawnType.隨機)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, randomRadius);
            }
            if (spawnType == SpawnType.固定 && fixedSpawnInfos != null)
            {
                Gizmos.color = Color.cyan;
                foreach (var info in fixedSpawnInfos)
                {
                    if (info.spawnPoint != null)
                        Gizmos.DrawWireSphere(info.spawnPoint.position, 0.3f);
                }
            }
        }
    }
} 