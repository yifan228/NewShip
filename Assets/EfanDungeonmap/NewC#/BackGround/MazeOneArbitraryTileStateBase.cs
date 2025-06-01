using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOneArbitraryTileStateBase : ArbitraryTileStateBase
{
    protected override TileType TypeStringToActualType(string typeName)
    {
        switch (typeName)
        {
            case "A":
                return new MazeOneA();
            case "B":
                return new MazeOneB();
            case "C":
                return new MazeOneC();
            case "D":
                return new MazeOneD();
            case "E":
                return new MazeOneE();
            case "F":
                return new MazeOneF();
            case "G":
                return new MazeOneG();
            default:
                return null;
        }
    }
}
