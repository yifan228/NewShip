public class IdleState:BoosterEngineState{
    BoosterEngineStateMachine boosterEngineStateMachine;
    AdvanceBoosterEngine advanceBoosterEngine;
    public IdleState(BoosterEngineStateMachine stateMachine,AdvanceBoosterEngine advanceBoosterEngine) : base(stateMachine)
    {
        boosterEngineStateMachine = stateMachine;
        this.advanceBoosterEngine = advanceBoosterEngine;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        advanceBoosterEngine.ResetProgress();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
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
        return base.CanChangeState(state);
    }
}
