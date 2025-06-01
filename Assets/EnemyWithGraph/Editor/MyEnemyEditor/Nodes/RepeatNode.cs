using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("流程/重複次數判斷")]
public class RepeatNode : BaseNode
{
    [Input(name = "進入", allowMultiple = true)] public float input;
    [Output(name = "Y")] public float yes;
    [Output(name = "N")] public float no;

    public int repeatTimes = 3;

    public override string name => "Repeat";

    protected override void Process()
    {

    }


} 