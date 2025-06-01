using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace EnemyNameSpace
{
    public class EnemyController : MonoBehaviour
    {
        public string KeyString { get; private set; }
        public Transform Player { get; private set; }
        [SerializeField] Rigidbody2D rigidBody;
        [SerializeField] SpriteRenderer enemyViewRenderer;
        [SerializeField] Collider2D triggerCollider;
        [SerializeField] Transform muzzel;
        [SerializeField] HpSystem hpSystem;
        [Header("is test ?")]
        [SerializeField] bool isUsedStartToTest;

        [Header("drone")]
        [SerializeField] DroneProjectileDataBase projectileDataBase;
        [SerializeField] DroneProjectileObj droneProjectilePrefab;
        [SerializeField] Transform enemyDroneHeirarchyPlace;

        [Header("test enemy")]
        [SerializeField] EnemyData testEnemyData;
        List<DroneProjectileData> DroneProjectiledata;
        public EnemyDataStruct enemyData { get; private set; }

        //private AEnemyMovePattern pattern;
        private AiGraphRunner aiGraphRunner;
        private int currentAction = 0;
        private float actionTimer = 0f;

        public void Init(Transform player, EnemyDataStruct data, Action<EnemyController> die)
        {
            if (data.Equals(default))
            {
                Debug.LogError("enemy data is null");
            }
            KeyString = data.KeyString;
            Player = player;
            enemyData = data;
            enemyViewRenderer.sprite = data.Sprite;
            Tool.ResetCollider.ResetCollider2D(triggerCollider, enemyViewRenderer);
            DroneProjectiledata = new List<DroneProjectileData>();
            hpSystem.Init(data.HPdata,()=>die(this));
            //pattern = GetPattern();
            DroneProjectileData tmpdata = projectileDataBase.GetData(data.DroneKeyString);
            DroneProjectiledata.Add(projectileDataBase.GetData(data.DroneKeyString));
                    UnityEngine.Debug.Log($"init DroneProjectileData: \n" +
            $"DroneKeyString: {data.DroneKeyString}\n" +
            $"MovementType: {tmpdata.MovementType}\n" +
            $"DroneProjectileType: {tmpdata.DroneProjectileType}\n" + 
            $"TriggerDmgType: {tmpdata.TriggerDmgType}\n" +
            $"SecondaryHpDmg: {tmpdata.SecondaryHpDmg}\n" +
            $"SecondaryArmorDmg: {tmpdata.SecondaryArmorDmg}\n" +
            $"LifeTime: {tmpdata.LifeTime}\n" +
            $"ObjSize: {tmpdata.ObjSize}\n" +
            $"PermissionCount: {tmpdata.PermissionCount}\n" +
            $"Direction: {tmpdata.Direction}\n" +
            $"TargetWithPos: {tmpdata.TargetWithPos}\n" +
            $"TargetWithTrans: {tmpdata.TargetWithTrans}\n" +
            $"View: {tmpdata.View}\n" +
            $"HitFX: {tmpdata.HitFX}");
        
            aiGraphRunner = new AiGraphRunner();
            aiGraphRunner.Init(data.AIData, this);
        }

        private void Start()
        {
            if (isUsedStartToTest)
            {
                Debug.LogWarning("test enemy");
                Init(FindFirstObjectByType<NewShipController>().transform, testEnemyData.Data, null);
            }
        }

        private AEnemyMovePattern GetPattern()
        {
            switch (enemyData.MotionPattern)
            {
                case EnemyMotionPattern.Stationary:
                    return new StationaryPattern();
                case EnemyMotionPattern.ChasePlayer:
                    return new ChasePattern();
                case EnemyMotionPattern.OnlyStationary:
                    return new OnlyStationary();
                default:
                    return null;
            }
        }
       
        private void Update()
        {
            //rootNode.Execute(this);
            //pattern.Execute(this);
            aiGraphRunner.Tick(Time.deltaTime);
        }
        
        private void Shoot(Vector2 targetPos, Transform targetTransform,DroneProjectileData data)
        {
            //Debug.Log("enemyshoot");
            switch (enemyData.AttackPattern)
            {
                case EnemyAttackPattern.Bullete:
                    StartCoroutine(Fusillade(enemyData.FusilladeCount,data));
                    break;
                case EnemyAttackPattern.Homing:
                    break;
                default:
                    break;
            }
        }
        #region attack pattern bullete shoot
        IEnumerator Fusillade(int count,DroneProjectileData data)
        {
            for (int i = 0; i < count; i++)
            {
                BulleteFire(enemyData.DroneCount, enemyData.DroneAngle,data);
                yield return new WaitForSeconds(0.2f);
            }
        }
        private void BulleteShoot(DroneProjectileData projectileData, Vector2 firePoint, Vector2 dir)
        {
            DroneProjectileObj obj = Instantiate(droneProjectilePrefab, enemyDroneHeirarchyPlace);

            projectileData.Direction = dir;
            switch (enemyData.PickTargetType)
            {
                case PickTargetType.NoOperate:
                    projectileData.TargetWithPos = default;
                    projectileData.TargetWithTrans = null;
                    break;
                case PickTargetType.PickNearestTargetTrans:
                    projectileData.TargetWithTrans = EnemyTargetPicker.PickPlayer_TransformWithAngleAndRange(transform.position,transform.up,30f,10f);
                    break;
                case PickTargetType.PickNearestTargetPos:
                    projectileData.TargetWithPos = EnemyTargetPicker.PickPlayer_PositionWithAngleAndRange(transform.position,transform.up,30f,10f,Vector2.one*3f);
                break;
                default:
                    break;
            }
            obj.Init(gameObject, TagsEnum.Player, rigidBody.velocity, firePoint, dir, projectileData);
        }
        

        void BulleteFire(int spreadCount, float spreadAngle,DroneProjectileData data)
        {
            if (spreadCount == 1)
            {
                BulleteShoot(data, muzzel.position, muzzel.position + transform.up.normalized);
                return;
            }
            float angleStep = spreadAngle / spreadCount; 
            float startAngle = -spreadAngle / 2f; 

            for (int i = 0; i < spreadCount; i++)
            {
                float angle = startAngle + (i * angleStep);
                Vector2 fireDirection = CalculateMethod.RotateVector(transform.up.normalized, angle);

                BulleteShoot(data, muzzel.position, fireDirection);
            }
        }
        #endregion

        #region ai
        // AI 行為：供 AiGraphRunner 呼叫

        public float GetPlayerDistance()
        {
            return Vector2.Distance(transform.position, Player.position);
        }
        public void MoveToPlayer(float distance,float speed)
        {
            if (Player == null) return;

            // 取得玩家位置
            Vector2 playerPos = Player.position;
            Vector2 currentPos = transform.position;
            Vector2 dirToPlayer = (playerPos - currentPos).normalized;

            // 計算目標點 - 在玩家周圍distance距離的位置
            Vector2 targetPos = CalculateMethod.GetTargetRandomPosition(playerPos,distance-1, distance+1);

            // 計算移動方向
            Vector2 moveDir = (targetPos - currentPos).normalized;

            // 更新旋轉角度
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // 設定速度
            rigidBody.velocity = moveDir * speed;
        }

        public void StationaryAim()
        {
            // 範例：面向玩家但不移動
            if (Player == null) return;
            Vector2 dir = (Player.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            rigidBody.velocity = Vector2.zero;
        }

        public void StationaryNoAim()
        {
            rigidBody.velocity = Vector2.zero;
        }

        public void ShootForAI(string bulletKeyString)
        {
            // 範例：每次呼叫都射擊一次
            DroneProjectileData data = projectileDataBase.GetData(bulletKeyString);
            Shoot(Player != null ? (Vector2)Player.position : Vector2.zero, Player,data);
        }

        public void SpreadShot(float spreadAngle, int bulletCount,string bulletKeyString)
        {
            // 範例：直接呼叫 BulleteFire
            //Debug.Log("SpreadShot");
            DroneProjectileData data = projectileDataBase.GetData(bulletKeyString);
            BulleteFire(bulletCount, spreadAngle,data);
        }
        #endregion
    }
}
