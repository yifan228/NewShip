using UnityEngine;

public class TurnState:BoosterEngineState{
    AdvanceBoosterEngine advanceBoosterEngine;
    public TurnState(BoosterEngineStateMachine stateMachine,AdvanceBoosterEngine advanceBoosterEngine) : base(stateMachine)
    {
        this.advanceBoosterEngine = advanceBoosterEngine;
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        AddTorque(advanceBoosterEngine.ThisRB);
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
        return state != BoosterState.SingleBooster;;
    }
    public void AddTorque(Rigidbody2D rb){
        advanceBoosterEngine.AddTorque(true,rb.mass);
        advanceBoosterEngine.AddTorque(false,rb.mass);
    }
}
