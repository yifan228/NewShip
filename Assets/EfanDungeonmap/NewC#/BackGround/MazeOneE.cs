using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOneE : TileType
{
    public MazeOneE()
    {
        Name = "E";
    }

    public override List<string> DownProbableObject()
    {
        return new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "A", "B", "D",  "G" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "A",  "C", "F", "G" };
    }
}
