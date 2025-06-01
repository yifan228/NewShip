using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOneA : TileType
{
    public MazeOneA()
    {
        Name = "A";
    }

    public override List<string> DownProbableObject()
    {
        return new List<string>() {"A", "E", "C", "D" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "A", "B", "E", "F" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "A", "B", "G", "D" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "A", "F", "C", "G" };
    }
}
