using UnityEngine;

public class MoveState:BoosterEngineState{
    AdvanceBoosterEngine advanceBoosterEngine;
    public MoveState(BoosterEngineStateMachine stateMachine,AdvanceBoosterEngine advanceBoosterEngine) : base(stateMachine)
    {
        this.advanceBoosterEngine = advanceBoosterEngine;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        StopRotation(advanceBoosterEngine.ThisRB);
        
    }
    void StopRotation(Rigidbody2D rb)
    {
        // 獲取當前的角速度
        float angularVelocity = rb.angularVelocity;

        // 使用 Lerp 慢慢修正當前的轉動量
        float targetAngularVelocity = 0f;
        float lerpFactor = 0.1f; // 調整這個值來控制修正速度
        float newAngularVelocity = Mathf.Lerp(angularVelocity, targetAngularVelocity, lerpFactor);

        // 計算反向力矩所需的角加速度
        float angularAcceleration = (newAngularVelocity - angularVelocity) / Time.fixedDeltaTime;

        // 獲取剛體的轉動慣量
        float inertia = rb.inertia;

        // 計算反向力矩
        float torque = angularAcceleration * inertia;
        torque = Mathf.Clamp(torque, -1, 1);

        // 施加反向力矩
        rb.AddTorque(torque, ForceMode2D.Force);

        // Debug 輸出
        //Debug.Log($"AngularVelocity: {angularVelocity}, NewAngularVelocity: {newAngularVelocity}, Inertia: {inertia}, Torque: {torque}");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        addForce(advanceBoosterEngine.transform);
        advanceBoosterEngine.AddBothTorque();
        //advanceBoosterEngine.AdjustRotation();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    public override void OnExit()
    {
        
        base.OnExit();
    }
    public override bool CanChangeState(BoosterState state)
    {
        return state != BoosterState.DoubleBooster;
    }
    public void addForce(Transform transform){
        advanceBoosterEngine.AddForce(true,transform.up);
        advanceBoosterEngine.AddForce(false,transform.up);
    }
}
