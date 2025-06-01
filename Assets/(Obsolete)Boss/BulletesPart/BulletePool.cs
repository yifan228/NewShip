using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletePool : MonoBehaviour
{
    [SerializeField] BulleteView bulleteView;
    static Stack<BulleteView> bulletesInPool;
    static List<BulleteView> bulletesOutofPool;

    private void Start()
    {
        bulletesInPool = new Stack<BulleteView>();
        bulletesOutofPool = new List<BulleteView>();
        Prewarm(10);
    }

    private void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            InstantiateBullete();
        }
    }
    public BulleteView GetBullete()
    {
        if(bulletesInPool.Count == 0)
        {
            InstantiateBullete();
        }
        BulleteView bullete = bulletesInPool.Pop(); ;
        bulletesOutofPool.Add(bullete);
        return bullete;
    }
    public static void BulleteReturn(BulleteView bulleteView)
    {
        bulletesInPool.Push(bulleteView);
        bulletesOutofPool.Remove(bulleteView);
        bulleteView.gameObject.SetActive(false);
    }

    public void AllReturn()
    {
        for (int i = 0; i < bulletesOutofPool.Count; i++)
        {
            bulletesOutofPool[i].CloseDisPlay();
            BulleteReturn(bulletesOutofPool[i]);
        }
    }

    private void InstantiateBullete()
    {
        BulleteView bv = Instantiate(bulleteView);
        bv.gameObject.SetActive(false);
        bulletesInPool.Push(bv);
    }

    private void OnDestroy()
    {
        bulletesInPool.Clear(); 
        bulletesOutofPool.Clear() ;
    }
}
