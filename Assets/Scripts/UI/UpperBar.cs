using UnityEngine;

public class UpperBar : MonoBehaviour
{
    public float height = 180f; // 上方欄高度（可依設計調整）

    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.offsetMin = new Vector2(0, -height);
        rt.offsetMax = new Vector2(0, 0);
    }
} 