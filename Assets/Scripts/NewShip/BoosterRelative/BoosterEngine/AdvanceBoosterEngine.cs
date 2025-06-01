using UnityEngine;

public enum BoosterState{None,SingleBooster,DoubleBooster}
public class AdvanceBoosterEngine : MonoBehaviour,IBoosterEngine
{
    [SerializeField] Rigidbody2D thisRB;
    [SerializeField] float strengthCurveMoveTime = 0.1f;
    [SerializeField] float breakStrength = 1f;
    [SerializeField] float boosterColdDownTime = 3f;
    [SerializeField] float rigiLinerDrag = 1f;
    [SerializeField] float rigiAngularDrag = 1f;
    public Rigidbody2D ThisRB => thisRB;
    BoosterEngineStateMachine boosterEngineStateMachine;
    Booster leftBooster,rightBooster;
    public float GetForwardSpeed => thisRB.velocity.magnitude;
    
    private void Start() {
        thisRB.drag = rigiLinerDrag;
        thisRB.angularDrag = rigiAngularDrag;
        leftBooster = new Booster(boosterColdDownTime);
        rightBooster = new Booster(boosterColdDownTime);
        boosterEngineStateMachine = new BoosterEngineStateMachine();
        boosterEngineStateMachine.AddState(BoosterState.None,new IdleState(boosterEngineStateMachine,this));
        boosterEngineStateMachine.AddState(BoosterState.SingleBooster,new TurnState(boosterEngineStateMachine,this));
        boosterEngineStateMachine.AddState(BoosterState.DoubleBooster,new MoveState(boosterEngineStateMachine,this));
        Debug.Log($"is start?{boosterEngineStateMachine.TryChangeState(BoosterState.None)}");
    }
    private void Update() {
        boosterEngineStateMachine.OnUpdate();
        
    }
    private void FixedUpdate() {
        boosterEngineStateMachine.OnFixedUpdate();
    }
#region IBoosterEngine
    public void BoostLeft(float strength,BoosterData boosterData)
    {
        Debug.Log("BoostLeft");
        ExecuteBoosterActivation(strength,leftBooster,boosterData,false);
        CheckStateChange();
    }

    public void BoostRight(float strength,BoosterData boosterData)
    {
        ExecuteBoosterActivation(strength,rightBooster,boosterData,false);
        CheckStateChange();
    }

    public void BrakeLeft(BoosterData boosterData)
    {
        ExecuteBoosterActivation(breakStrength,leftBooster,boosterData,true);
        CheckStateChange();
    }

    public void BrakeRight(BoosterData boosterData)
    {
        ExecuteBoosterActivation(breakStrength,rightBooster,boosterData,true);
        CheckStateChange();
    }
    private void ExecuteBoosterActivation(float strength,Booster booster,BoosterData boosterData,bool isBackward)
    {
        booster.ChangeData(boosterData);
        booster.Activate();
        booster.SetBackward(isBackward);
        booster.AddProgress(strength * Time.deltaTime * strengthCurveMoveTime);
    }
    public void ReleaseBoostLeft()
    {
        leftBooster.Deactivate();
        leftBooster.ResetProgress();
        CheckStateChange();
    }

    public void ReleaseBoostRight()
    {
        rightBooster.Deactivate();
        rightBooster.ResetProgress();
        CheckStateChange();

    }
#endregion
    void CheckStateChange(){
        
        if(leftBooster.IsActive && rightBooster.IsActive && (rightBooster.IsBackword == leftBooster.IsBackword)){
            boosterEngineStateMachine.TryChangeState(BoosterState.DoubleBooster);
        }else if(leftBooster.IsActive || rightBooster.IsActive){
            boosterEngineStateMachine.TryChangeState(BoosterState.SingleBooster);
        }else{
            boosterEngineStateMachine.TryChangeState(BoosterState.None);
        }
    }
    
    public void AddForce(bool boosterSide,Vector2 toward)
    {
        var booster = boosterSide ? rightBooster : leftBooster;
        if(booster.IsActive){
            thisRB.AddForce(booster.GetForceImplus(thisRB.mass) * toward,ForceMode2D.Impulse);
            thisRB.AddForce(booster.GetForce(thisRB.mass) * toward,ForceMode2D.Force);
        }
        
    }
    public void AddBothTorque()
    {
        
        //if(booster.IsActive){
        //thisRB.AddTorque( * torqueSide);
        thisRB.AddTorque((rightBooster.CurrStrength - leftBooster.CurrStrength)*5,ForceMode2D.Impulse);
        //thisRB.AddForce(booster.GetForce(mass) * transform.up,ForceMode2D.Force);
        //}
    }
    public void AddTorque(bool boosterSide,float mass)
    {
        var booster = boosterSide ? rightBooster : leftBooster;   
        var torqueSide = boosterSide ? 1 : -1;
        if(booster.IsActive){
            thisRB.AddTorque(booster.GetTorque(mass) * torqueSide);
            thisRB.AddTorque(booster.GetTorqueImplus(mass) * torqueSide,ForceMode2D.Impulse);
            thisRB.AddForce(booster.GetForce(mass) * transform.up,ForceMode2D.Force);
        }
    }
    public void AdjustRotation(){
        var leftForce = (int)(leftBooster.GetForce(thisRB.mass)/2);
        var rightForce = (int)(rightBooster.GetForce(thisRB.mass)/2);
        Debug.Log($"leftForce:{leftForce},rightForce:{rightForce}");
        if(leftForce-rightForce>=3){
            thisRB.AddTorque(leftBooster.GetTorque(thisRB.mass));
        }
        else if(leftForce-rightForce<=-3){
            thisRB.AddTorque(rightBooster.GetTorque(thisRB.mass));
        }
    }
    public void ResetProgress(){
        leftBooster.ResetProgress();
        rightBooster.ResetProgress();
    }
    
}
