using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewShipController : MonoBehaviour
{
    [SerializeField] Rigidbody2D thisRB;
    [SerializeField] WeaponController weaponController;
    private List<InputSubScriber> inputs = new List<InputSubScriber>();
    [SerializeField] BoosterSO upRight;
    [SerializeField] BoosterSO upLeft;
    [SerializeField] BoosterSO downRight;
    [SerializeField] BoosterSO downLeft;
    [SerializeField] bool IsUsedStartToTest;
    [SerializeField] RocketsHandller rocketsHandller;
    [SerializeField] WeaponDataSOs weaponDataSOs;
    IBoosterEngine boosterEngine;

    public Vector2 externalForce { get; set; } 
    public float externalTorque { get; set; }
    [SerializeField]float dragCoefficient = 200f;

    public float momentOfInertia = 1f; 
    [SerializeField]float rotationalDragCoefficient = 0.5f; 

    public Vector2 velocity{ get; private set; } = Vector2.up*0.1f;
    public float angularVelocity { get; private set; } = 0;

    [SerializeField] float testmaxSpd;
    [SerializeField] float testmaxW;

    #region player input attribute
    public bool IsPressingRightTrigger {  get; private set; }
    private float pressingRightTriggerStrength;

    public bool IsPressingLefttTrigger { get; private set; }
    private float pressingLeftTriggerStrength;

    public bool IsPressingRightShoulder {  get; private set; }

    public bool IsPressingLeftShoulder { get; private set; }

    #endregion
    
    private void Start()
    {
        //if(boosterEngine == null){
        //    if(!TryGetComponent<IBoosterEngine>(out boosterEngine)) Debug.LogError("need booster engine");
        //}
        if (IsUsedStartToTest)
        {
            Init(upRight.GetData("RShoulder"), upLeft.GetData("LShoulder"), downRight.GetData("RTrigger"), downLeft.GetData("LTrigger"));
            weaponController.Init(this, weaponDataSOs.GetData("weapon_basic"));
        }
        
    }

    void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        // 計算加速度 a = (F - bv) / m
        Vector2 acceleration =Vector2.zero;
        float angularAcceleration = 0;

        acceleration = (externalForce - dragCoefficient * velocity) / 1;
        angularAcceleration = (externalTorque - rotationalDragCoefficient * angularVelocity) / momentOfInertia;
        // 更新速度
        velocity += acceleration * deltaTime;
        if (velocity.magnitude>testmaxSpd)
        {
            velocity = velocity*testmaxSpd/velocity.magnitude;
        }

        angularVelocity += angularAcceleration * deltaTime;
        if (Mathf.Abs( angularVelocity)>testmaxW)
        {
            angularVelocity = Mathf.Sign( angularVelocity )*testmaxW;
        }

        // 更新位置與旋轉
        transform.position += (Vector3)(velocity * deltaTime);

        float newRotation = transform.eulerAngles.z + angularVelocity * deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, newRotation);

        if (Mathf.Approximately(velocity.magnitude,0))
        {
            velocity = Vector2.zero;
        }
        if (Mathf.Approximately( angularVelocity,0))
        {
            angularVelocity = 0;
        }
    }

    private void Update()
    {
        externalForce = Vector2.zero;
        externalTorque = 0;

        if (IsPressingRightTrigger)
        {
            foreach (var b in inputs)
            {
                b.OnPressingRightTirgger(pressingRightTriggerStrength);
            }
        }

        if (IsPressingRightShoulder)
        {
            foreach (var b in inputs)
            {
                b.OnPressRightShouler();
            }
        }

        if (IsPressingLefttTrigger)
        {
            foreach (var b in inputs)
            {
                b.OnPressingLeftTirgger(pressingLeftTriggerStrength);
            }
        }

        if (IsPressingLeftShoulder)
        {
            foreach (var b in inputs)
            {
                b.OnPressLeftShouler();
            }
        }
    }

    public void Init(BoosterData upRightBooster,BoosterData upLeftBooster,BoosterData downRightBooster, BoosterData downLeftBooster)
    {
        inputs.Add(GetBooster(upRightBooster));
        inputs.Add(GetBooster(upLeftBooster));
        inputs.Add(GetBooster(downRightBooster));
        inputs.Add(GetBooster(downLeftBooster));
    }

    #region input action map
    public void OnInputRightTrigger(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed) 
        {
            rocketsHandller?.OnRightForce(callbackContext.ReadValue<float>());
            IsPressingRightTrigger = true;
            pressingRightTriggerStrength = callbackContext.ReadValue<float>();
        }
        else if (callbackContext.canceled) 
        {
            rocketsHandller?.OnRightForce(0);
            foreach (var b in inputs)
            {
                b.OnReleaseRightTrigger(); 
            }
            IsPressingRightTrigger = false;
        }
    }

    // UI專用多型
    public void OnInputRightTrigger(float value)
    {
        if (value > 0.01f)
        {
            rocketsHandller?.OnRightForce(value);
            IsPressingRightTrigger = true;
            pressingRightTriggerStrength = value;
        }
        else
        {
            rocketsHandller?.OnRightForce(0);
            foreach (var b in inputs)
            {
                b.OnReleaseRightTrigger();
            }
            IsPressingRightTrigger = false;
        }
    }

    public void OnInputLeftTrigger(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rocketsHandller?.OnLeftForce(callbackContext.ReadValue<float>());
            IsPressingLefttTrigger = true;
            pressingLeftTriggerStrength = callbackContext.ReadValue<float>();
        }
        else if (callbackContext.canceled)
        {
            rocketsHandller?.OnLeftForce(0);
            foreach (var b in inputs)
            {
                b.OnReleaseLeftTrigger();
            }
            IsPressingLefttTrigger = false;
        }
    }

    // UI專用多型
    public void OnInputLeftTrigger(float value)
    {
        if (value > 0.01f)
        {
            rocketsHandller?.OnLeftForce(value);
            IsPressingLefttTrigger = true;
            pressingLeftTriggerStrength = value;
        }
        else
        {
            rocketsHandller?.OnLeftForce(0);
            foreach (var b in inputs)
            {
                b.OnReleaseLeftTrigger();
            }
            IsPressingLefttTrigger = false;
        }
    }

    public void OnInputRightShoulder(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rocketsHandller?.OnRightBackForce(callbackContext.ReadValue<float>());  
            IsPressingRightShoulder = true;
        }
        else if (callbackContext.canceled)
        {
            rocketsHandller?.OnRightBackForce(0);  
            foreach (var b in inputs)
            {
                b.OnReleaseRightShouler();
            }
            IsPressingRightShoulder = false;
        }
    }

    // UI專用多型
    public void OnInputRightShoulder(float value)
    {
        if (value > 0.01f)
        {
            rocketsHandller?.OnRightBackForce(value);
            IsPressingRightShoulder = true;
        }
        else
        {
            rocketsHandller?.OnRightBackForce(0);
            foreach (var b in inputs)
            {
                b.OnReleaseRightShouler();
            }
            IsPressingRightShoulder = false;
        }
    }

    public void OnInputLeftShoulder(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rocketsHandller?.OnLeftBackForce(callbackContext.ReadValue<float>());   
            IsPressingLeftShoulder = true;
        }
        else if (callbackContext.canceled)
        {
            rocketsHandller?.OnLeftBackForce(0);
            foreach (var b in inputs)
            {
                b.OnReleaseLeftShouler();
            }
            IsPressingLeftShoulder = false;
        }
    }

    // UI專用多型
    public void OnInputLeftShoulder(float value)
    {
        if (value > 0.01f)
        {
            rocketsHandller?.OnLeftBackForce(value);
            IsPressingLeftShoulder = true;
        }
        else
        {
            rocketsHandller?.OnLeftBackForce(0);
            foreach (var b in inputs)
            {
                b.OnReleaseLeftShouler();
            }
            IsPressingLeftShoulder = false;
        }
    }

    public void OnPushLeftStick(InputAction.CallbackContext callbackContext)
    {
        Vector2 v = callbackContext.ReadValue<Vector2>();
        foreach(var b in inputs)
        {
            b.OnPushLeftStick(v);
        }
    }

    public void OnPushRightStick(InputAction.CallbackContext callbackContext)
    {
        Vector2 v = callbackContext.ReadValue<Vector2>();
        foreach (var b in inputs)
        {
            b.OnPushRightStick(v);
        }
    }

    #endregion
    private InputSubScriber GetBooster(BoosterData data)
    {
        switch (data.driveName)
        {
            case BoosterDriveName.RTrigger_Base:
                return new BaseRightTriggerBoosterDrive(this, data);
            case BoosterDriveName.LTrigger_Base:
                return new BaseLeftTriggerBoosterDrive(this, data);
            case BoosterDriveName.RShoulder_Base:
                return new BaseRightShoulderBoosterDrive(this, data);
            case BoosterDriveName.LShoulder_Base:
                return new BaseLeftShoulderBoosterDrive(this, data);
            default:
                return null;
        }
    }
}
 public enum TagsEnum
{
    Player,
    Enemy,
    Drone,
    Boss,
    FrontCollision,
    Wall
}
