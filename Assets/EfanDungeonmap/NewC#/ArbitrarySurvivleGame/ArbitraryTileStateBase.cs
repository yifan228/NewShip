using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public interface ImaptileBase
{

}
public class ArbitraryTileStateBase
{
    public Vector2 tilePosition = new Vector2();
    TileType type;

    public TileType GetTileType()
    {
        return type;
    }

    public virtual void SetOriginalType(TileType type)
    {
        this.type = type;
    }

    /// <summary>
    /// 若非軸上 集合的第一個是x相鄰的tileType
    /// </summary>
    /// <param name="adjacentTypes"></param>
    public virtual void SetType(List<TileType> adjacentTypes)
    {
        List<string> probableTypeName = new List<string>();
        if (tilePosition.x > 0 && tilePosition.y == 0)
        {
            probableTypeName.AddRange(adjacentTypes[0].RightProbableObject());
        }
        else if (tilePosition.x == 0 && tilePosition.y > 0)
        {
            probableTypeName.AddRange(adjacentTypes[0].UpProbableObject());
        }
        else if (tilePosition.x < 0 && tilePosition.y == 0)
        {
            probableTypeName.AddRange(adjacentTypes[0].LeftProbableObject());
        }
        else if (tilePosition.x == 0 && tilePosition.y < 0)
        {
            probableTypeName.AddRange(adjacentTypes[0].DownProbableObject());
        }
        else if (tilePosition.x > 0 && tilePosition.y > 0)
        {
            probableTypeName = adjacentTypes[0].RightProbableObject().Intersect(adjacentTypes[1].UpProbableObject()).ToList();
        }
        else if (tilePosition.x < 0 && tilePosition.y > 0)
        {
            probableTypeName = adjacentTypes[0].LeftProbableObject().Intersect(adjacentTypes[1].UpProbableObject()).ToList();
        }
        else if (tilePosition.x < 0 && tilePosition.y < 0)
        {
            probableTypeName = adjacentTypes[0].LeftProbableObject().Intersect(adjacentTypes[1].DownProbableObject()).ToList();
        }
        else if (tilePosition.x > 0 && tilePosition.y < 0)
        {
            probableTypeName = adjacentTypes[0].RightProbableObject().Intersect(adjacentTypes[1].DownProbableObject()).ToList();
        }

        SetTileType(probableTypeName);
    }

    private void SetTileType(List<string> typeNames)
    {
        int rngNum = Random.Range(0, typeNames.Count);
        type = TypeStringToActualType(typeNames[rngNum]);
    }
        

    protected virtual TileType TypeStringToActualType(string typeName)
    {
        switch (typeName)
        {
            case "A":
                return new TestTileTypeA();
            case "B":
                return new TestTileTypeB();
            case "C":
                return new TestTileTypeC();
            case "D":
                return new TestTileTypeD();
            default:
                return null;
        }
    }
}

public abstract class TileType
{
    public string Name = "Default";
    public abstract List<string> RightProbableObject();
    public abstract List<string> LeftProbableObject();
    public abstract List<string> UpProbableObject();
    public abstract List<string> DownProbableObject();
}
public class TestTileTypeA : TileType
{
    public TestTileTypeA()
    {
        Name = "TestA";
    }
    public override List<string> DownProbableObject()
    {
        return new List<string>() {"A", "C", "D" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "A", "B", "D" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "A", "B", "D" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "A", "C", "D" };
    }
}

public class TestTileTypeB : TileType
{
    public TestTileTypeB()
    {
        Name = "TestB";
    }
    public override List<string> DownProbableObject()
    {
        return new List<string>() { "D" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "A", "B", "D" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "A", "B", "D" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "D" };
    }
}
public class TestTileTypeC : TileType
{
    public TestTileTypeC()
    {
        Name = "TestC";
    }
    public override List<string> DownProbableObject()
    {
        return new List<string>() { "A", "C", "D" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "D" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "D" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "A", "C", "D" };
    }
}
public class TestTileTypeD : TileType
{
    public TestTileTypeD()
    {
        Name = "TestD";
    }
    public override List<string> DownProbableObject()
    {
        return new List<string>() { "B", "D" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "C", "D" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "C", "D" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "B", "D" };
    }
}
