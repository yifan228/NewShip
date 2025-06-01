using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("條件/距離小於X")]
public class EnemyAICondition_DistanceToPlayerNode : BaseNode
{
    [Input(name = "進入")] public float input;
    [Output(name = "是")] public float yes;
    [Output(name = "否")] public float no;

    public float distance = 3f;
    public override string name => "距離小於X";

    protected override void Process()
    {
        yes = input;
        no = 0f;
    }
} 