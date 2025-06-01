using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("攻擊/散射攻擊")]
public class SpreadShotNode : EditorActionNode
{
    [Input(name = "進入", allowMultiple = true)] public float input;
    [Output(name = "完成")] public float output;
    
    public override string name => "散射攻擊";
    public SpreadShotAction action;

    protected override void Process()
    {
        output = input;
    }
} 