using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class ResetCollider
    {
        static void AdjustBoxCollider(BoxCollider2D collider, SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer.sprite != null)
            {
                collider.size = spriteRenderer.bounds.size; // 讓 BoxCollider2D 自動匹配 Sprite 大小
                collider.offset = Vector2.zero; // 讓 Collider 居中
            }
        }

        static void AdjustCircleCollider(CircleCollider2D collider, SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer.sprite != null)
            {
                collider.radius = spriteRenderer.bounds.extents.magnitude / 2f; // 設定半徑
            }
        }

        static void AdjustPolygonCollider(PolygonCollider2D collider, SpriteRenderer spriteRenderer)
        {
            collider.pathCount = 0; // 清空舊的碰撞數據
            collider.autoTiling = true; // 啟用自動計算（如果適用）
            collider.SetPath(0, spriteRenderer.sprite.vertices); // 設置新 Sprite 的碰撞區
        }

        public static void ResetCollider2D(Collider2D collider, SpriteRenderer spriteRenderer)
        {

            if (collider is BoxCollider2D)
            {
                AdjustBoxCollider(collider as BoxCollider2D, spriteRenderer);
            }
            else if (collider is CircleCollider2D)
            {
                AdjustCircleCollider(collider as CircleCollider2D, spriteRenderer);
            }
            else if (collider is PolygonCollider2D)
            {
                AdjustPolygonCollider(collider as PolygonCollider2D, spriteRenderer);
            }

            collider.enabled = false;
            collider.enabled = true; // 重新啟用 Collider
        }
    }
}
