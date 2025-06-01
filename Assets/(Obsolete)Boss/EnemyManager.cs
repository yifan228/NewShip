using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    //[SerializeField] Transform target;
    //[SerializeField] EnemiesDatas database;// only for init
    //[SerializeField] EnemyView enemyViewTemplate;
    //[SerializeField] float bornPositionFarRadius = 100f;
    //[SerializeField] float bornPositionNearRadius = 50f;
    //[SerializeField] float bornEnemiesTime;
    //[SerializeField] float enemiesPerWave;
    //List<EnemyDataStruct> datas = new List<EnemyDataStruct>();
    //List<EnemyView> activateEnemies = new List<EnemyView>();
    //Queue<EnemyView> pool = new Queue<EnemyView>();
    //int updatingTick=0;
    //float bornEnemiesTimer;
    //void Start()
    //{
    //    datas = database.GetDatas();
    //}

    //private void Update()
    //{
    //    updatingTick++;
    //    if (updatingTick ==3)
    //    {
    //        updatingTick = 0;
    //        foreach (var item in activateEnemies)
    //        {
    //            item.MoveWithVelocity();
    //        }
    //    }
    //    bornEnemiesTimer += Time.deltaTime;
    //    if (bornEnemiesTimer > bornEnemiesTime)
    //    {
    //        bornEnemiesTimer = 0;
    //        for (int i = 0; i < enemiesPerWave; i++)
    //        {
    //            string index = Random.Range(0, datas.Count).ToString();
    //            BornEnemies(index);
    //        }
    //    }
    //}

    //[ContextMenu("test born enemy 0")]
    //public void TestBornEnemy0()
    //{
    //    BornEnemies("0");
    //}

    //private void BornEnemies(string id)
    //{
    //    EnemyDataStruct d = datas.Find(x => x.ID == id);
    //    if (d.Equals(default))
    //    {
    //        Debug.LogError("there is no enemyID : " + id);
    //        return;
    //    }

    //    EnemyView enemy;
    //    if (pool.Count<=0)
    //    {
    //        enemy = Instantiate(enemyViewTemplate);
    //    }
    //    else
    //    {
    //        enemy = pool.Dequeue();
    //    }
    //    activateEnemies.Add(enemy);
    //    enemy.Init(target, d, ReturnToPool);
    //    DecidePosition(enemy);
    //    enemy.gameObject.SetActive(true);
    //}

    //private void DecidePosition(EnemyView enemy)
    //{
    //    float R = Random.Range(bornPositionNearRadius, bornPositionFarRadius);
    //    float theta = Random.Range(0f, 2*Mathf.PI);
    //    Vector2 pos = new Vector2(R * Mathf.Cos(theta), R * Mathf.Sin(theta));
    //    enemy.transform.position = (Vector2)target.position + pos;
    //}

    //private void ReturnToPool(EnemyView view)
    //{
    //    view.gameObject.SetActive(false);
    //    activateEnemies.Remove(view);
    //    pool.Enqueue(view);
    //}
}


