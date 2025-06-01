using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ArbitraryTileCreaterManager : MonoBehaviour
{
    protected ArbitraryTileMapCreater creater;
    protected TileType firsttype;
    [SerializeField] GameObject testTile;
    [SerializeField] SpriteAtlas atla;
    [SerializeField] bool useGameObjectPrefabRatherthanatla;
    [SerializeField] bool test;
    [SerializeField] float RotationDegreeAfterCreateMap = 0;
    [Header("生成的大小")]
    [SerializeField] int MaxLayer;
    [SerializeField] protected float gameObjectSize;
    [SerializeField] protected float spriteObjectSize;

    protected List<GameObject> CreatedmapTiles;

    private void Start()
    {
        if (test)
        {
            StartCreateMap();
        }
    }
    /// <summary>
    /// 生成地圖的觸發口 要先去設定 MaxLayer
    /// </summary>
    public virtual void StartCreateMap(Action<List<GameObject>> onCreatedEnd = null)
    {

    }
    protected void Initialize()
    {
        CreatedmapTiles = new List<GameObject>();
        Dictionary<Vector2, ArbitraryTileStateBase> dic = new Dictionary<Vector2, ArbitraryTileStateBase>();
        dic = creater.CreateTiles(firsttype, MaxLayer);
        foreach (var item in dic)
        {
            if (useGameObjectPrefabRatherthanatla)
            {
                GameObject go;
                go = CreateGOWithType(item);
                if (go != null)
                {
                    CreatedmapTiles.Add(go);
                }
            }
            else
            {
                GameObject go = Instantiate(testTile, spriteObjectSize * item.Key,Quaternion.identity, transform);
                go.transform.localScale = new Vector2(spriteObjectSize, spriteObjectSize);
                go.GetComponent<SpriteRenderer>().sprite = atla.GetSprite(item.Value.GetTileType().Name);
            }
        }
        transform.rotation = Quaternion.Euler(0,0, RotationDegreeAfterCreateMap);
    }

    protected virtual GameObject CreateGOWithType(KeyValuePair<Vector2, ArbitraryTileStateBase> item)
    {
        return null;
    }
}
