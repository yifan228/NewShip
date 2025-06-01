using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;  // 需要跟踪的相机
    public float parallaxFactor;       // 视差因子，用于控制背景图层的移动速度
    private Vector3 previousCameraPosition;

    void Start()
    {
        // 记录相机初始位置
        previousCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // 计算相机的移动距离
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        // 根据视差因子移动背景图层
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);

        // 更新相机位置
        previousCameraPosition = cameraTransform.position;
    }
}
