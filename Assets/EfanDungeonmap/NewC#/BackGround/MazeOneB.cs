using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOneB : TileType
{
    public MazeOneB()
    {
        Name = "B";
    }

    public override List<string> DownProbableObject()
    {
        return new List<string>() { "A","B", "C", "D","E","F","G" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "A","B", "F", "E" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "A","B", "G", "D" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
    }
}
