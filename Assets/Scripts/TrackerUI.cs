using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class TrackerUI : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text text;
        [SerializeField] Image icon;
        [SerializeField] RectTransform iconRectTransform; // icon 的 RectTransform
        public void SetUp(Sprite icon)
        {
            this.icon.sprite = icon;
        }
        public void UpdateDistence(Transform obj,Vector2 dir,float distance)
        {
            if (!IsObjectInCameraView(obj))
            {
                text.gameObject.SetActive(true);
                icon.gameObject.SetActive(true);
                text.text = $"{distance.ToString("F1")} m";
                MoveIconToScreenEdge(obj);
            }
            else
            {
                text.gameObject.SetActive(false);
                icon.gameObject.SetActive(false);
            }
        }
        void MoveIconToScreenEdge(Transform obj)
        {
            Vector3 objectPosition = obj.position;
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(objectPosition);

            // 計算物件的方向向量 (相對於視口中心)
            Vector3 direction = viewportPosition - new Vector3(0.5f, 0.5f, 0f);
            direction.z = 0; // 我們只關心 X 和 Y
            direction.Normalize();

            // 計算 Icon 位置，將 Icon 限制在螢幕邊界
            Vector2 screenPosition = new Vector2(
                Mathf.Clamp(viewportPosition.x, 0.05f, 0.95f), // 限制在 5% 到 95% 之間
                Mathf.Clamp(viewportPosition.y, 0.05f, 0.95f)
            );

            // 將視口坐標轉換為螢幕坐標
            Vector3 iconScreenPosition = Camera.main.ViewportToScreenPoint(screenPosition);

            // 設定 Icon 的新位置
            iconRectTransform.position = iconScreenPosition;
        }
        bool IsObjectInCameraView(Transform obj)
        {
            // 取得物件的世界座標
            Vector3 objectPosition = obj.position;

            // 將物件的世界座標轉換為視口座標
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(objectPosition);

            // 檢查視口座標是否在範圍內
            bool isInView = viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                            viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                            viewportPosition.z > 0; // z 大於 0 表示在攝影機前方

            return isInView;
        }
        
    }
}