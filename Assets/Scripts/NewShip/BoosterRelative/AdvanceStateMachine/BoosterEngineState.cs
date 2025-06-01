using DC_Tool.Basic;

public class BoosterEngineState:State<BoosterState>{
    public BoosterEngineState(BoosterEngineStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
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