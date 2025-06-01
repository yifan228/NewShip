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
    [Header("�ͦ����j�p")]
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
    /// �ͦ��a�Ϫ�Ĳ�o�f �n���h�]�w MaxLayer
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
