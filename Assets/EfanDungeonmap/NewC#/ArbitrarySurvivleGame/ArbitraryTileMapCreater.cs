using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ArbitraryTileMapCreater
{
    public virtual Dictionary<Vector2, ArbitraryTileStateBase> CreateTiles(TileType originalType,int maxLayer)
    {
        return null;
    }
}
public class ArbitraryTileMapCreater<T>: ArbitraryTileMapCreater where T:ArbitraryTileStateBase,new()
{
    List<List<ArbitraryTileStateBase>> tiles = new List<List<ArbitraryTileStateBase>>();
    Dictionary<Vector2, ArbitraryTileStateBase> positionToGetTileDictionary = new Dictionary<Vector2, ArbitraryTileStateBase>(new CompareWithVector());
    public override Dictionary<Vector2, ArbitraryTileStateBase> CreateTiles(TileType originalType,int maxLayer)
    {
        tiles.Add(new List<ArbitraryTileStateBase>());
        tiles[0].Add(new T());//第零層第一個，座標00
        tiles[0][0].tilePosition = Vector2.zero;
        tiles[0][0].SetOriginalType(originalType);
        positionToGetTileDictionary.Add(tiles[0][0].tilePosition, tiles[0][0]);
        for (int i = 1; i <= maxLayer; i++)
        {
            List<Vector2> layerPositions = CreateLayer(tiles[i-1]);
            //Debug.Log("第"+i+"層的上一層 : "+layerPositions.Count);
            tiles.Add(new List<ArbitraryTileStateBase>());
            for (int w = 0; w < layerPositions.Count; w++)
            {
                ArbitraryTileStateBase tile = new T();
                tiles[i].Add( tile);
                tile.tilePosition = layerPositions[w];
                //Debug.Log(tile.tilePosition);
                positionToGetTileDictionary.Add(tile.tilePosition, tile);
                SetType(tile);
            }
        }
        return positionToGetTileDictionary;
    }

    private List<Vector2> CreateLayer(List<ArbitraryTileStateBase> previousLayer)
    {
        List<Vector2> tmpList = new List<Vector2>();
        foreach (var tile in previousLayer)
        {
            if (tile.tilePosition.x == 0 && tile.tilePosition.y == 0)
            {
                tmpList.Add(new Vector2(1, 0));
                tmpList.Add(new Vector2(-1, 0));
                tmpList.Add(new Vector2(0,1));
                tmpList.Add(new Vector2(0,-1));
            }
            else if (tile.tilePosition.x > 0 && tile.tilePosition.y == 0)
            {
                tmpList.Add(new Vector2(tile.tilePosition.x + 1, 0));
                tmpList.Add(new Vector2(tile.tilePosition.x , tile.tilePosition.y + 1));
                tmpList.Add(new Vector2(tile.tilePosition.x , tile.tilePosition.y - 1));
            }
            else if (tile.tilePosition.x == 0 && tile.tilePosition.y > 0)
            {
                tmpList.Add(new Vector2(0, tile.tilePosition.y + 1));
                tmpList.Add(new Vector2(tile.tilePosition.x + 1, tile.tilePosition.y ));
                tmpList.Add(new Vector2(tile.tilePosition.x - 1, tile.tilePosition.y ));
            }
            else if (tile.tilePosition.x < 0 && tile.tilePosition.y == 0)
            {
                tmpList.Add(new Vector2(tile.tilePosition.x - 1, 0));
                tmpList.Add(new Vector2(tile.tilePosition.x , tile.tilePosition.y + 1));
                tmpList.Add(new Vector2(tile.tilePosition.x , tile.tilePosition.y - 1));
            }
            else if (tile.tilePosition.x == 0 && tile.tilePosition.y < 0)
            {
                tmpList.Add(new Vector2(0, tile.tilePosition.y - 1));
                tmpList.Add(new Vector2(tile.tilePosition.x + 1, tile.tilePosition.y ));
                tmpList.Add(new Vector2(tile.tilePosition.x - 1, tile.tilePosition.y ));
            }
            else if (tile.tilePosition.x > 0 )
            {
                tmpList.Add(new Vector2(tile.tilePosition.x + 1, tile.tilePosition.y));
            }
            else if (tile.tilePosition.x<0)
            {
                tmpList.Add(new Vector2(tile.tilePosition.x - 1, tile.tilePosition.y));
            }
        }
        List<Vector2> tmphashSet = tmpList.Distinct<Vector2>().ToList();
        return tmphashSet;
    }

    private void SetType(ArbitraryTileStateBase tile)
    {
        List<TileType> adjacentList = new List<TileType>();
        if (tile.tilePosition.x > 0 && tile.tilePosition.y == 0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(tile.tilePosition.x-1, 0)];
            adjacentList.Add(objectTile.GetTileType());
        }
        else if (tile.tilePosition.x == 0 && tile.tilePosition.y > 0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(0,tile.tilePosition.y -1)];
            adjacentList.Add(objectTile.GetTileType());
        }
        else if (tile.tilePosition.x < 0 && tile.tilePosition.y == 0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(tile.tilePosition.x + 1, 0)];
            adjacentList.Add(objectTile.GetTileType());
        }
        else if (tile.tilePosition.x == 0 && tile.tilePosition.y < 0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(0, tile.tilePosition.y + 1)];
            adjacentList.Add(objectTile.GetTileType());
        }
        else if (tile.tilePosition.x > 0 && tile.tilePosition.y>0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(tile.tilePosition.x-1, tile.tilePosition.y )];
            ArbitraryTileStateBase objectTile2 = positionToGetTileDictionary[new Vector2(tile.tilePosition.x, tile.tilePosition.y-1)];
            adjacentList.Add(objectTile.GetTileType());
            adjacentList.Add(objectTile2.GetTileType());
        }
        else if (tile.tilePosition.x < 0 && tile.tilePosition.y > 0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(tile.tilePosition.x + 1, tile.tilePosition.y)];
            ArbitraryTileStateBase objectTile2 = positionToGetTileDictionary[new Vector2(tile.tilePosition.x, tile.tilePosition.y - 1)];
            adjacentList.Add(objectTile.GetTileType());
            adjacentList.Add(objectTile2.GetTileType());
        }
        else if (tile.tilePosition.x < 0 && tile.tilePosition.y < 0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(tile.tilePosition.x + 1, tile.tilePosition.y)];
            ArbitraryTileStateBase objectTile2 = positionToGetTileDictionary[new Vector2(tile.tilePosition.x, tile.tilePosition.y + 1)];
            adjacentList.Add(objectTile.GetTileType());
            adjacentList.Add(objectTile2.GetTileType());
        }
        else if (tile.tilePosition.x > 0 && tile.tilePosition.y < 0)
        {
            ArbitraryTileStateBase objectTile = positionToGetTileDictionary[new Vector2(tile.tilePosition.x - 1, tile.tilePosition.y)];
            ArbitraryTileStateBase objectTile2 = positionToGetTileDictionary[new Vector2(tile.tilePosition.x, tile.tilePosition.y + 1)];
            adjacentList.Add(objectTile.GetTileType());
            adjacentList.Add(objectTile2.GetTileType());
        }
        tile.SetType(adjacentList);
    }
}

public class CompareWithVector : IEqualityComparer< Vector2>
{
    public bool Equals(Vector2 x, Vector2 y)
    {
        return x == y;
    }

    public int GetHashCode(Vector2 obj)
    {
        int hash = obj.ToString().GetHashCode();
        return hash;
    }
}



