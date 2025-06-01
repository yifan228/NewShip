using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("攻擊/朝玩家射擊")]
public class ShootPlayerNode : EditorActionNode
{
    [Input(name = "進入", allowMultiple = true)] public float input;
    [Output(name = "完成")] public float output;
    public ShootPlayerAction action;
    public override string name => "朝玩家射擊";

    protected override void Process()
    {
        output = input;
    }
} 