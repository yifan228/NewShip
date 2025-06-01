using EnemyNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] OutRoundData testPlayerData;
    [SerializeField] EnemiesDatas enemiesDatas;
    [SerializeField] EnemiesBornManager enemiesBornManager;
    [SerializeField] TextAsset outData;
    [SerializeField] Transform player;
    [SerializeField] HpSystem playerHp;
    private void Start()
    {
        OutRoundData data = JsonUtility.FromJson<OutRoundData>(outData.text);
        RoundDataManager.InitPlayerPartData(data);
        RoundDataManager.InitDatabase(enemiesDatas.GetDataBase());
        playerHp.Init(data.hpdata,GameOver);
        enemiesBornManager.Init(player);
    }

    private void GameOver()
    {
        Debug.LogWarning("game over!!!");
    }
}
