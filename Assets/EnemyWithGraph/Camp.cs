using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp<T> where T : MonoBehaviour
{
    // 池中的物件列表
    private Queue<T> pool = new Queue<T>();

    // 用於生成新物件的預製體
    private T prefab;

    // 是否允許池子擴展
    [SerializeField] private bool canExpand = true;

    // 初始化物件池
    public void Initialize(T prefab,int initialSize)
    {
        this.prefab = prefab;
        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }

    // 從池中取出一個物件
    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else if (canExpand)
        {
            return AddObjectToPool();
        }
        else
        {
            Debug.LogWarning("物件池已空且無法擴展！");
            return null;
        }
    }

    // 將物件歸還到池中
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    // 添加新物件到池中
    private T AddObjectToPool()
    {
        T obj = GameObject.Instantiate(prefab);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }
}
