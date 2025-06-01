using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOneD : TileType
{
    public MazeOneD()
    {
        Name = "D";
    }

    public override List<string> DownProbableObject()
    {
        return new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
    }

    public override List<string> LeftProbableObject()
    {
        return new List<string>() { "A", "B", "E", "F",};
    }

    public override List<string> RightProbableObject()
    {
        return new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
    }

    public override List<string> UpProbableObject()
    {
        return new List<string>() { "A", "C", "F", "G" };
    }
}
