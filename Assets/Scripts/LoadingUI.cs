using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private string[] loadingTips;

    void Start()
    {
        if (loadingTips != null && loadingTips.Length > 0)
        {
            tipText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }
    }

    public void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            progressBar.value = progress / 100f;
        }

        if (progressText != null)
        {
            progressText.text = $"載入中... {progress:F0}%";
        }
    }
} 