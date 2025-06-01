#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using GraphProcessor;
using System.Collections.Generic;

public static class EnemyAIGraphExporter
{
    public static EnemyAIData Export(EnemyAIGraph graph, string assetPath)
    {
        var data = ScriptableObject.CreateInstance<EnemyAIData>();
        var actionList = new List<EnemyAIAction>();
        var nodeToAction = new Dictionary<BaseNode, EnemyAIAction>();

        // 1. 找到 Entry Node
        BaseNode entryNode = null;
        foreach (var node in graph.nodes)
        {
            if (node is EnemyAIEntryNode)
            {
                entryNode = node;
                break;
            }
        }
        if (entryNode == null)
        {
            Debug.LogError("找不到 Entry Node！");
            return data;
        }

        // 2. 生成 EntryAction
        EnemyAIAction entryAction = CreateActionFromNode(entryNode);
        data.EntryAction = entryAction;
        actionList.Add(data.EntryAction);
        nodeToAction[entryNode] = entryAction;
        // 3. 遞迴建立圖狀結構
        BuildTree(data, entryNode, entryAction, graph, actionList, nodeToAction);

        // 4. 收集所有 action
        data.actions.AddRange(actionList);

        AssetDatabase.CreateAsset(data, assetPath);
        AssetDatabase.SaveAssets();
        return data;
    }

    // 工廠方法：根據 node 產生對應的 action
    private static EnemyAIAction CreateActionFromNode(BaseNode node)
    {
        if (node is EnemyAIEntryNode entry)
            return new EntryAction(entry.action);
        if (node is ChasePlayerNode chase)
            return new MoveToPlayerAction(chase.action);
        if (node is StationaryAimPlayerNode station)
            return new StationaryAimAction(station.action);
        if (node is StationaryNoAimNode stationNoAim)
            return new StationaryNoAimAction(stationNoAim.action);
        if (node is ShootPlayerNode shoot)
            return new ShootPlayerAction(shoot.action);
        if (node is SpreadShotNode spread)
            return new SpreadShotAction(spread.action);
        // 條件節點
        if (node is EnemyAICondition_DistanceToPlayerNode cond)
        {
            // 你可以根據需要自訂 ConditionAction
            var condAction = new ConditionDistanceToPlayerAction();
            condAction.distance = cond.distance;
            return condAction;
        }
        if (node is RepeatNode repeat)
        {
            return new RepeatAction(repeat.repeatTimes);
        }
        return null;
    }

    // 遞迴建立圖狀結構，支援 branches 與循環
    private static void BuildTree(EnemyAIData data, BaseNode parentNode, EnemyAIAction parentAction, EnemyAIGraph graph, List<EnemyAIAction> actionList, Dictionary<BaseNode, EnemyAIAction> nodeToAction)
    {
        foreach (var port in parentNode.outputPorts)
        {
            foreach (var edge in graph.edges)
            {
                if (edge.outputNode == parentNode && edge.outputFieldName == port.fieldName)
                {
                    // 如果已經產生過這個 node 的 action，直接引用
                    if (nodeToAction.TryGetValue(edge.inputNode, out var existedAction))
                    {
                        parentAction.branches.Add(new Branch { key = port.fieldName, action = existedAction });
                    }
                    else
                    {
                        EnemyAIAction childAction = CreateActionFromNode(edge.inputNode);
                        actionList.Add(childAction);
                        nodeToAction[edge.inputNode] = childAction;
                        parentAction.branches.Add(new Branch { key = port.fieldName, action = childAction });
                        BuildTree(data, edge.inputNode, childAction, graph, actionList, nodeToAction);
                    }
                }
            }
        }
    }
}
#endif
