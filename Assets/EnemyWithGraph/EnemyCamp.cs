using EnemyNameSpace;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace EnemyNameSpace
{
    public class EnemyCamp : MonoBehaviour
    {
        Camp<EnemyController> enemiesPool;
        [SerializeField] Transform player;
        [SerializeField] float campHP;
        [SerializeField] EnemyController enemyControllerPrfab;
        [SerializeField] EnemiesDatas database;
        [SerializeField] float enemiesCountPerWave;
        [SerializeField] float bornEnemyWaveInterval;
        float timer=0;
        private void Start()
        {
            enemiesPool = new Camp<EnemyController>();
            enemiesPool.Initialize(enemyControllerPrfab,3);
        }

        public void BornEnemy()
        {
            EnemyController enemy = enemiesPool.Get();
            enemy.Init(player,database.GetData(""),EnemyDie);
        }

        private void EnemyDie(EnemyController enemy)
        {
            enemiesPool.ReturnToPool(enemy);
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag(TagsEnum.Player.ToString()))
            {
                timer=timer-Time.deltaTime;
                UnityEngine.Debug.Log($"timer:{timer}");
                if (timer<=0)
                {
                    for (int i = 0; i < enemiesCountPerWave; i++)
                    {
                        BornEnemy();
                    }
                    timer = bornEnemyWaveInterval;
                }
            }
        }

        //public void BeAttacked(DamageStruct damage, string attackerName, AttackedType attackedType)
        //{
        //    if (attackerName == TagsEnum.Player.ToString())
        //    {
        //        campHP-=damage.Damage;
        //    }
        //}
    }

}
