using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MySceneManager : MonoBehaviour
{

    private static MySceneManager instance;
    public static MySceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithLoading(string targetScene)
    {
        StartCoroutine(LoadSceneAsync(targetScene));
    }

    private IEnumerator LoadSceneAsync(string targetScene)
    {
        // 先載入 Loading 場景
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("LoadingScene");
        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        // 確保 Loading 場景至少顯示 3 秒
        float startTime = Time.time;
        float minimumLoadingTime = 3f;

        // 開始載入目標場景
        AsyncOperation targetSceneOperation = SceneManager.LoadSceneAsync(targetScene);
        targetSceneOperation.allowSceneActivation = false;

        while (!targetSceneOperation.isDone)
        {
            float elapsedTime = Time.time - startTime;
            float progress = targetSceneOperation.progress;

            if (elapsedTime >= minimumLoadingTime && progress >= 0.9f)
            {
                targetSceneOperation.allowSceneActivation = true;
            }

            // 可以在這裡更新載入進度UI
            float currentProgress = Mathf.Min((progress * 100f), (elapsedTime / minimumLoadingTime) * 100f);
            UpdateLoadingProgress(currentProgress);

            yield return null;
        }
    }

    private void UpdateLoadingProgress(float progress)
    {
        // 尋找並更新載入進度UI
        LoadingSceneManager loadingUI = GameObject.FindFirstObjectByType<LoadingSceneManager>();
        if (loadingUI != null)
        {
            loadingUI.SetProgress(progress);
        }
    }


}
