#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class EnemyAIGraphContextMenu
{
    [MenuItem("Assets/導出選取的 EnemyAIGraph 為 AI 資料", true)]
    private static bool ValidateExport()
    {
        return Selection.activeObject is EnemyAIGraph;
    }

    [MenuItem("Assets/導出選取的 EnemyAIGraph 為 AI 資料")]
    private static void ExportSelectedGraph()
    {
        var graph = Selection.activeObject as EnemyAIGraph;
        if (graph == null)
        {
            EditorUtility.DisplayDialog("錯誤", "請選取一個 EnemyAIGraph 資料", "OK");
            return;
        }

        // 確保目錄存在
        string defaultPath = "Assets/Datas/EnemyAi";

        string path = EditorUtility.SaveFilePanel(
            "儲存 AI 資料",
            defaultPath,
            graph.name + "_AIData",
            "asset"
        );

        if (!string.IsNullOrEmpty(path))
        {
            // 將完整路徑轉換為相對於專案的路徑
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            EnemyAIGraphExporter.Export(graph, relativePath);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("導出完成", "AI 資料已成功導出！", "OK");
        }
    }
}
#endif
