using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimeManager :MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (!isUseCustomTime)
        {
            Global_Deltatime = Time.deltaTime;
        }

        if (!isUseCustomEnemyTime)
        {
            Global_Enemy_Deltatime = Time.deltaTime;
        }
    }

    /// <summary>
    /// for player
    /// </summary>
    public static float Global_Deltatime { get; private set; }
    private bool isUseCustomTime;

    public static float Global_Enemy_Deltatime { get; private set; }
    private bool isUseCustomEnemyTime;

}
/// <summary>
/// 時間分類
/// </summary>
public enum GlobalTimerEnum
{
    Player,
    Enemy,
    UI
}
