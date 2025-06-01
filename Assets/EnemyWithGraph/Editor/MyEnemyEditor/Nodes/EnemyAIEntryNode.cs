using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("流程/入口")]
public class EnemyAIEntryNode : BaseNode
{
    [Output(name = "開始")] public float output;
    [Input(name = "進入", allowMultiple = true)] public float input;
    public override string name => "AI入口";
    public EntryAction action;

    protected override void Process()
    {
        output = 1f;
    }
} 