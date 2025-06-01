using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform spaceShip;
    //[SerializeField] ShipControl shipControl;
    [SerializeField] AdvanceBoosterEngine  shipControl;
    [SerializeField] float followVerticalSpeed, followRSpeed, followHorizontalSpeed;
    [SerializeField] Vector2 offset;
    [SerializeField] float minViewSize = 25,maxViewSize = 40;
    [SerializeField] bool lockRotation = false; 
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }
    public void LockRotation(bool lockRotate)
    {
        lockRotation = lockRotate;
    }
    // Update is called once per frame
    void Update()
    {
        // 获取目标在局部坐标中的位置
        Vector3 targetLocalPosition = spaceShip.TransformPoint(offset);

        // 计算相对目标的垂直和水平方向（局部坐标）
        Vector3 relativePosition = spaceShip.InverseTransformPoint(transform.position);

        // 垂直方向使用目标的局部Y轴进行平滑跟随
        float newY = Mathf.Lerp(relativePosition.y, offset.y, Time.deltaTime * followVerticalSpeed);

        // 水平方向使用目标的局部X轴进行平滑跟随
        float newX = Mathf.Lerp(relativePosition.x, offset.x, Time.deltaTime * followHorizontalSpeed);

        // 将新的相对位置转换回世界坐标并更新相机的位置
        Vector3 newPosition = spaceShip.TransformPoint(new Vector3(newX, newY, transform.position.z));
        transform.position = newPosition;
        if(!lockRotation)
            transform.rotation = Quaternion.Lerp(transform.rotation, spaceShip.rotation, Time.deltaTime * followRSpeed);

        //change view size
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, shipControl.GetForwardSpeed / 100 * (maxViewSize - minViewSize) + minViewSize, Time.deltaTime);
        
    }
}
