using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("移動/不動也不會瞄準玩家")]
public class StationaryNoAimNode : EditorActionNode
{
    [Input(name = "進入", allowMultiple = true)] public float input;
    [Output(name = "完成")] public float output;

    public override string name => "不動也不會瞄準玩家";
    public StationaryNoAimAction action;

    protected override void Process()
    {
        output = input;
    }
} 