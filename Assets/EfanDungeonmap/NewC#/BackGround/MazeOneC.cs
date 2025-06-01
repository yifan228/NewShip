using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOneC : TileType
{
    public MazeOneC()
    {
        Name = "C";
    }

    public override List<string> DownProbableObject()
    {
        return new List<string>() { "A", "C", "D", "E", };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "A",  "C",  "F", "G" };
    }
}
