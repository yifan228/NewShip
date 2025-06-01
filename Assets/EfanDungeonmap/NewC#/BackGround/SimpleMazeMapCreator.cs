using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMazeMapCreator : ArbitraryTileCreaterManager
{
    [SerializeField] GameObject A;
    [SerializeField] GameObject B;
    [SerializeField] GameObject C;
    [SerializeField] GameObject D;
    [SerializeField] GameObject E;
    [SerializeField] GameObject F;
    [SerializeField] GameObject G;
    public override void StartCreateMap(Action<List<GameObject>> onCreatedEnd = null)
    {
        creater = new ArbitraryTileMapCreater<MazeOneArbitraryTileStateBase>();
        firsttype = new MazeOneA();
        Initialize();
        onCreatedEnd?.Invoke(CreatedmapTiles);
    }
    protected override GameObject CreateGOWithType(KeyValuePair<Vector2, ArbitraryTileStateBase> item )
    {
        Debug.Log(item.Value.GetTileType().Name);
        GameObject go;
        switch (item.Value.GetTileType().Name)
        {
            case "A":
                go = Instantiate(A, gameObjectSize * item.Key, Quaternion.identity, transform);
                go.transform.localScale = new Vector2(gameObjectSize,gameObjectSize);
                break;
            case "B":
                go = Instantiate(B, gameObjectSize * item.Key, Quaternion.identity, transform);
                go.transform.localScale = new Vector2(gameObjectSize, gameObjectSize);
                break;
            case "C":
                go = Instantiate(C, gameObjectSize * item.Key, Quaternion.identity, transform);
                go.transform.localScale = new Vector2(gameObjectSize, gameObjectSize);
                break;
            case "D":
                go = Instantiate(D, gameObjectSize * item.Key, Quaternion.identity, transform);
                go.transform.localScale = new Vector2(gameObjectSize, gameObjectSize);
                break;
            case "E":
                go = Instantiate(E, gameObjectSize * item.Key, Quaternion.identity, transform);
                go.transform.localScale = new Vector2(gameObjectSize, gameObjectSize);
                break;
            case "F":
                go = Instantiate(F, gameObjectSize * item.Key, Quaternion.identity, transform);
                go.transform.localScale = new Vector2(gameObjectSize, gameObjectSize);
                break;
            case "G":
                go = Instantiate(G, gameObjectSize * item.Key, Quaternion.identity, transform);
                go.transform.localScale = new Vector2(gameObjectSize, gameObjectSize);
                break;
            default:
                Debug.LogWarning("隨機生成地圖沒有相應的tilebase");
                return null;
        }
        return go;
    }
}
