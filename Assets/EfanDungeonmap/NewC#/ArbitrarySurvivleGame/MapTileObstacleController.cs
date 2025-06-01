using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileObstacleController : MonoBehaviour
{
    [SerializeField]List<ObstacleItem> obstacleItems = new List<ObstacleItem>();
    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        foreach (var item in obstacleItems)
        {
            item.Initialize(GameObject.FindFirstObjectByType<NewShipController>().transform);
        }
    }
}
