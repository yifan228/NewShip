using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiTriggerNewShipMove : MonoBehaviour
{
    [SerializeField] Slider rightSlider;
    [SerializeField] Slider leftSlider;
    [SerializeField] NewShipController shipController;

    private bool isRightDragging = false;
    private bool isLeftDragging = false;
    private float sliderCenter = 0.5f;
    private float sliderSpeed = 3f; // 回彈速度

    void Start()
    {
        if (rightSlider != null)
        {
            rightSlider.onValueChanged.AddListener(OnRightSliderChanged);
            AddEvent(rightSlider, EventTriggerType.PointerDown, (e) => isRightDragging = true);
            AddEvent(rightSlider, EventTriggerType.PointerUp, (e) => isRightDragging = false);
            rightSlider.value = sliderCenter;
        }
        if (leftSlider != null)
        {
            leftSlider.onValueChanged.AddListener(OnLeftSliderChanged);
            AddEvent(leftSlider, EventTriggerType.PointerDown, (e) => isLeftDragging = true);
            AddEvent(leftSlider, EventTriggerType.PointerUp, (e) => isLeftDragging = false);
            leftSlider.value = sliderCenter;
        }
    }

    void Update()
    {
        // 回彈右Slider
        if (!isRightDragging && rightSlider != null && Mathf.Abs(rightSlider.value - sliderCenter) > 0.001f)
        {
            rightSlider.value = Mathf.Lerp(rightSlider.value, sliderCenter, Time.deltaTime * sliderSpeed);
            if (Mathf.Abs(rightSlider.value - sliderCenter) < 0.01f) rightSlider.value = sliderCenter;
            OnRightSliderChanged(rightSlider.value);
        }
        // 回彈左Slider
        if (!isLeftDragging && leftSlider != null && Mathf.Abs(leftSlider.value - sliderCenter) > 0.001f)
        {
            leftSlider.value = Mathf.Lerp(leftSlider.value, sliderCenter, Time.deltaTime * sliderSpeed);
            if (Mathf.Abs(leftSlider.value - sliderCenter) < 0.01f) leftSlider.value = sliderCenter;
            OnLeftSliderChanged(leftSlider.value);
        }
    }

    void OnRightSliderChanged(float value)
    {
        if (value > 0.5f)
        {
            float strength = (value - 0.5f) * 2f; // 0~1
            shipController?.OnInputRightShoulder(strength);
            shipController?.OnInputRightTrigger(0);
        }
        else if (value < 0.5f)
        {
            float strength = (0.5f - value) * 2f; // 0~1
            shipController?.OnInputRightShoulder(0);
            shipController?.OnInputRightTrigger(strength);
        }
        else
        {
            shipController?.OnInputRightTrigger(0);
            shipController?.OnInputRightShoulder(0);
        }
    }

    void OnLeftSliderChanged(float value)
    {
        if (value > 0.5f)
        {
            float strength = (value - 0.5f) * 2f;
            shipController?.OnInputLeftShoulder(strength);
            shipController?.OnInputLeftTrigger(0);
        }
        else if (value < 0.5f)
        {
            float strength = (0.5f - value) * 2f;
            shipController?.OnInputLeftShoulder(0);
            shipController?.OnInputLeftTrigger(strength);
        }
        else
        {
            shipController?.OnInputLeftTrigger(0);
            shipController?.OnInputLeftShoulder(0);
        }
    }

    // 工具：建立假的 CallbackContext
    UnityEngine.InputSystem.InputAction.CallbackContext CreateCallbackContext(float value, bool performed)
    {
        var ctx = new UnityEngine.InputSystem.InputAction.CallbackContext();
        // 這裡僅作為示意，實際專案可用自訂 struct 或直接傳值
        return ctx;
    }

    // 工具：註冊事件
    void AddEvent(Slider slider, EventTriggerType type, System.Action<BaseEventData> action)
    {
        EventTrigger trigger = slider.GetComponent<EventTrigger>();
        if (trigger == null) trigger = slider.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((e) => action(e));
        trigger.triggers.Add(entry);
    }
}
