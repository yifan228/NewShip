using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using EnemyNameSpace;
using System;
using System.Linq;

public class AiGraphRunner
{
    private EnemyController enemyController;
    private List<EnemyAIAction> allActions;
    private List<EnemyAIAction> activeActions;
    private int tickperframe = 30;
    private float tickTimer = 0f;
    private float tickInterval;
    private AiGraphRunnerBlackboard blackboard = new AiGraphRunnerBlackboard();

    public void Init(EnemyAIData data, EnemyController controller)
    {
        allActions = data.actions;
        enemyController = controller;
        activeActions = new List<EnemyAIAction>();
        tickInterval = 1f / tickperframe;
        blackboard.Clear();

        // 添加初始行為
        if (data.EntryAction != null)
        {
            activeActions.Add(data.EntryAction);
            blackboard.GetOrCreateTimer(data.EntryAction);
        }
        else
        {
            Debug.LogError("no entry action");
        }
    }

    public void Tick(float deltaTime)
    {
        //if (activeActions.Count == 0) return;

        tickTimer += deltaTime;

        while (tickTimer >= tickInterval)
        {
            // foreach (var action in activeActions)
            // {
            //     Debug.Log($"action: {action.type}");
            // }
            tickTimer -= tickInterval;

            int count = activeActions.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var action = activeActions[i];
                var board = blackboard.GetOrCreateTimer(action);

                // LifeTime 與 Delay 一律累加
                board.lifeTimer += tickInterval;
                board.delayTimer += tickInterval;

                // 行為結束
                if (board.lifeTimer >= action.LifeTime)
                {
                    // 觸發下一個行為
                    if (action.branches != null)
                    {
                        // 條件節點特別處理
                        if (action is ConditionDistanceToPlayerAction cond)
                        {
                            Debug.Log("Executing ConditionDistanceToPlayer");
                            float playerDist = enemyController.GetPlayerDistance();
                            if (playerDist < cond.distance)   
                            {
                                Debug.Log("yes");
                                List<EnemyAIAction> yesActions = action.branches.Where(branch => branch.key == "yes").Select(branch => branch.action).ToList();
                                activeActions.AddRange(yesActions);
                            }
                            else
                            {
                                Debug.Log("no");
                                List<EnemyAIAction> noActions = action.branches.Where(branch => branch.key == "no").Select(branch => branch.action).ToList();
                                activeActions.AddRange(noActions);
                            }
                            blackboard.ResetStatus(action);
                            activeActions.Remove(action);
                        }
                        else if (action is RepeatAction repeat)
                        {
                            Debug.Log("Executing Repeat");
                            Debug.Log($"repeat.currentCount: {repeat.repeatTimes}");

                            if (board.currentRepeatCount < repeat.repeatTimes)
                            {
                                Debug.Log($"Executing Repeat {board.currentRepeatCount}");
                                List<EnemyAIAction> repeatActions = repeat.branches.Where(branch => branch.key == "no").Select(branch => branch.action).ToList();
                                Debug.Log($"repeatActions: {repeatActions.Count}");
                                activeActions.AddRange(repeatActions);
                                foreach (var a in repeatActions)
                                {
                                    blackboard.GetOrCreateTimer(a);
                                }
                                board.currentRepeatCount++;
                            }
                            else
                            {
                                Debug.Log("Executing Repeat End");
                                List<EnemyAIAction> repeatActions = repeat.branches.Where(branch => branch.key == "yes").Select(branch => branch.action).ToList();
                                activeActions.AddRange(repeatActions);
                                foreach (var a in repeatActions)
                                {
                                    blackboard.GetOrCreateTimer(a);
                                }
                            }
                            board.lifeTimer = 0f;
                            activeActions.Remove(action);
                        }
                        else
                        {
                            // 一般行為只走所有 branches
                            foreach (var branch in action.branches)
                            {
                                if (branch.action != null && !activeActions.Contains(branch.action))
                                {
                                    activeActions.Add(branch.action);
                                    blackboard.GetOrCreateTimer(branch.action);
                                }
                            }
                            blackboard.ResetStatus(action);
                            activeActions.Remove(action);
                        }
                    }
                    continue;
                }

                // 還在 delay 期間，不執行
                if (board.delayTimer < action.Delay)
                    continue;

                // CD 控制 ExecuteAction
                board.cdTimer += tickInterval;
                if (action.CD <= 0 || board.cdTimer > action.CD)
                {
                    ExecuteAction(action);
                    board.cdTimer = 0f;
                }
            }
        }
    }

    private void ExecuteAction(EnemyAIAction action)
    {
        // 根據行為型別執行
        switch (action.type)
        {
            case EnemyAIActionType.MoveToPlayer:
                enemyController.MoveToPlayer(action.Get<float>("nearDistance"),action.Get<float>("speed"));
                break;
            case EnemyAIActionType.StationaryAim:
                enemyController.StationaryAim();
                break;
            case EnemyAIActionType.StationaryNoAim:
                enemyController.StationaryNoAim();
                break;
            case EnemyAIActionType.ShootPlayer:
                enemyController.ShootForAI((action as ShootPlayerAction).bulleteKeyString);
                break;
            case EnemyAIActionType.SpreadShot:
                enemyController.SpreadShot(
                    (action as SpreadShotAction).spreadAngle,
                    (action as SpreadShotAction).bulleteCount,
                    (action as SpreadShotAction).bulleteKeyString
                );
                break;
            case EnemyAIActionType.Entry:
                //Debug.Log("Executing Entry");
                break;
            case EnemyAIActionType.ConditionDistanceToPlayer:
                //Debug.Log("Executing ConditionDistanceToPlayer");
                break;
            case EnemyAIActionType.Repeat:
                break;
        }
    }

    public bool IsRunning()
    {
        return activeActions.Count > 0;
    }
}

public class AiGraphRunnerBlackboard
{
    public class boardAttributes
    {
        public float delayTimer = 0f;
        public float lifeTimer = 0f;
        public float cdTimer = 0f;
        public int currentRepeatCount = 0;//just for repeat action
    }

    private Dictionary<EnemyAIAction, boardAttributes> actionStatusDict = new();



    public boardAttributes GetOrCreateTimer(EnemyAIAction action)
    {
        if (!actionStatusDict.TryGetValue(action, out var status))
        {
            status = new boardAttributes();
            actionStatusDict[action] = status;
        }
        return status;
    }

    public void ResetStatus(EnemyAIAction action)
    {
        actionStatusDict[action] = new boardAttributes();
    }

    public void Clear()
    {
        actionStatusDict.Clear();
    }
}
