using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("UI 元件")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI tipText;

    [Header("載入提示設定")]
    [SerializeField] private string[] loadingTips;
    [SerializeField] private float tipChangeInterval = 3f;
    
    [Header("進度條動畫設定")]
    [SerializeField] private float smoothSpeed = 5f;
    private float targetProgress = 0f;
    private float currentProgress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化進度條
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }

        // 開始循環顯示提示
        if (loadingTips != null && loadingTips.Length > 0)
        {
            StartCoroutine(CycleTips());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 平滑更新進度條
        if (Mathf.Abs(currentProgress - targetProgress) > 0.01f)
        {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * smoothSpeed);
            UpdateUI(currentProgress);
        }
    }

    public void SetProgress(float progress)
    {
        targetProgress = progress / 100f;
    }

    private void UpdateUI(float progress)
    {
        if (progressBar != null)
        {
            progressBar.value = progress;
        }

        if (progressText != null)
        {
            progressText.text = $"載入中... {(progress * 100):F0}%";
        }
    }

    private IEnumerator CycleTips()
    {
        int currentTipIndex = 0;
        
        while (true)
        {
            if (tipText != null)
            {
                tipText.text = loadingTips[currentTipIndex];
                currentTipIndex = (currentTipIndex + 1) % loadingTips.Length;
            }
            
            yield return new WaitForSeconds(tipChangeInterval);
        }
    }
}
