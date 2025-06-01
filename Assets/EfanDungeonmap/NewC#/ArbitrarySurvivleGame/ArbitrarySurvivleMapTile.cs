using System;
using System.Collections.Generic;
using UnityEngine;

public class ArbitrarySurvivleMapTile : MonoBehaviour
{
    //[SerializeField]private EnemyCamp[] enemyCamps;
    [SerializeField] private List<RestorePack> AmmoResorePack;
    [SerializeField] private List<RestorePack> HpResorePack;
    [SerializeField] private List<RestorePack> TimeResorePack;
    //public virtual void InitializeEnemyCamp(ArbitratySurvivleMapInitializeData data, Action<Transform, Vector2, int, Transform> instantiateEnemy)
    //{
    //    for (int i = 0; i < enemyCamps.Length; i++)
    //    {
    //        int randomNum = UnityEngine.Random.Range(1, 101);
    //        if (randomNum <=data.EnemyCampRatio)
    //        {
    //            enemyCamps[i].Initialize(DecideEnemyId(),instantiateEnemy);
    //        }
    //    }
    //}

    private int DecideEnemyId()
    {
        return 0;
    }

    public List<RestorePack>  GetRestoreAmmoPack()
    {
        return GetUnActiveRestorePack(AmmoResorePack);
    }

    private List<RestorePack> GetUnActiveRestorePack(List<RestorePack> data)
    {
        List<RestorePack> d = new List<RestorePack>();
        for (int i = 0; i < data.Count; i++)
        {
            if (!data[i].gameObject.activeInHierarchy)
            {
                d.Add(data[i]);
            }
        }
        return d;
    }

    public virtual List<RestorePack> GetRestoreHpPack()
    {
        return GetUnActiveRestorePack(HpResorePack);
    }

    public virtual List<RestorePack> GetRestoreTimePack()
    {
        return GetUnActiveRestorePack(TimeResorePack);
    }

}

[Serializable]
public class ArbitratySurvivleMapInitializeData
{
    /// <summary>
    /// 最大地圖快的數量
    /// </summary>
    public int MaxLayer;
    /// <summary>
    /// 出現子彈補充包的數量
    /// </summary>
    public int RestoreAmmoPackCount = 10;
    /// <summary>
    /// monster born interval
    /// </summary>
    //public float MonsterBornInterval;
    /// <summary>
    /// 出現加時間的包的數量
    /// </summary>
    public int RestoreTimePackCount;
    /// <summary>
    /// 出現新子彈包的機率
    /// </summary>
    public int NewBulleteRatio;
    /// <summary>
    /// 此地塊中敵人營隊的概率
    /// </summary>
    public int EnemyCampRatio;
    /// <summary>
    /// 此地塊中敵人被打敗後 掉的獎勵
    /// </summary>
    public int EnemyDrop;
}
