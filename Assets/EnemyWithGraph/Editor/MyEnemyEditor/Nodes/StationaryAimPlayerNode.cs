using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("移動/不動但會瞄準玩家")]
public class StationaryAimPlayerNode : EditorActionNode
{
    [Input(name = "進入", allowMultiple = true)] public float input;
    [Output(name = "完成")] public float output;
    public StationaryAimAction action;

    public override string name => "不動但會瞄準玩家";

    protected override void Process()
    {
        output = input;
    }
} 