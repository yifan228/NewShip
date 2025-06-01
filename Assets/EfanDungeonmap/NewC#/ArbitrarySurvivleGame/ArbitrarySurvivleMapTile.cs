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
    /// �̤j�a�ϧ֪��ƶq
    /// </summary>
    public int MaxLayer;
    /// <summary>
    /// �X�{�l�u�ɥR�]���ƶq
    /// </summary>
    public int RestoreAmmoPackCount = 10;
    /// <summary>
    /// monster born interval
    /// </summary>
    //public float MonsterBornInterval;
    /// <summary>
    /// �X�{�[�ɶ����]���ƶq
    /// </summary>
    public int RestoreTimePackCount;
    /// <summary>
    /// �X�{�s�l�u�]�����v
    /// </summary>
    public int NewBulleteRatio;
    /// <summary>
    /// ���a�����ĤH�綤�����v
    /// </summary>
    public int EnemyCampRatio;
    /// <summary>
    /// ���a�����ĤH�Q���ѫ� �������y
    /// </summary>
    public int EnemyDrop;
}
