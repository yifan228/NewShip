using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneProjectileObjPool : MonoBehaviour
{
    [SerializeField] DroneProjectileObj objView;
    static Stack<DroneProjectileObj> objsInPool;
    static List<DroneProjectileObj> objsInActivating;

    private void Update()
    {
        //for (int i=0;i< objsInActivating.Count;i++)
        //{
        //    objsInActivating[i].Updating(Time.deltaTime);
        //}
    }

    private void Start()
    {
        objsInPool = new Stack<DroneProjectileObj>();
        objsInActivating = new List<DroneProjectileObj>();
        Prewarm(10);
    }

    private void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            InstantiateBullete();
        }
    }
    public DroneProjectileObj GetObj()
    {
        if (objsInPool.Count == 0)
        {
            InstantiateBullete();
        }
        DroneProjectileObj bullete = objsInPool.Pop(); ;
        objsInActivating.Add(bullete);
        return bullete;
    }
    public static void ObjReturn(DroneProjectileObj bulleteView)
    {
        objsInPool.Push(bulleteView);
        bulleteView.CloseDisPlay();
        objsInActivating.Remove(bulleteView);
    }

    public void AllReturn()
    {
        for (int i = 0; i < objsInActivating.Count; i++)
        {
            objsInActivating[i].CloseDisPlay();
            ObjReturn(objsInActivating[i]);
        }
    }

    private void InstantiateBullete()
    {
        DroneProjectileObj bv = Instantiate(objView,transform);
        bv.gameObject.SetActive(false);
        objsInPool.Push(bv);
    }

    private void OnDestroy()
    {
        objsInPool.Clear();
        objsInActivating.Clear();
    }
}
