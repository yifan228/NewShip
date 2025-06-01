using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("移動/追擊玩家到附近")]
public class ChasePlayerNode : EditorActionNode
{
    [Input(name = "進入", allowMultiple = true)] public float input;
    [Output(name = "完成")] public float output;

    public MoveToPlayerAction action;
    public override string name => "追擊玩家到附近";

    protected override void Process()
    {
        output = input;
    }
} 