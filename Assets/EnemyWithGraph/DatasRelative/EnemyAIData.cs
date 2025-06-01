using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/EnemyAIData")]
[System.Serializable]
public class EnemyAIData : ScriptableObject
{
    [SerializeReference]
    public List<EnemyAIAction> actions = new List<EnemyAIAction>();
    [SerializeReference]
    public EnemyAIAction EntryAction; // 入口節點 index
}
