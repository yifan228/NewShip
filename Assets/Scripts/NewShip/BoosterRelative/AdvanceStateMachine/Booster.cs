using UnityEngine;

public class Booster{
    bool isActive;
    float progress;
    float coldDownTime;
    float lastActivationTime;
    BoosterData boosterData;
    public bool IsActive => isActive;
    bool addImplus = false;
    bool isBackward = false;
    public float CurrStrength {get;private set;}
    public bool IsBackword => isBackward;
    public Booster(float coldDownTime){
        this.coldDownTime = coldDownTime;
        lastActivationTime = Time.time - coldDownTime;
        progress = 0;
    }
    public void SetBackward(bool isBackward){
        this.isBackward = isBackward;
    }
    public void ChangeData(BoosterData boosterData){
        this.boosterData = boosterData;
    }
    public void Activate(){
        if(!isActive) addImplus = true;
        isActive = true;
    }
    
    public void Deactivate(){
        isActive = false;
    }
    public void AddProgress(float progress){
        CurrStrength = progress * boosterData.DeltaAngularPower;
        this.progress += progress;
    }
    public void ResetProgress(){
        progress = 0;
    }
    public float GetForce(float mass){
        float acceleration = boosterData.Accelerationcurve.Evaluate(progress)*boosterData.DeltaSpeedPower;
        return mass * acceleration ;
    }
    
    public float GetForceImplus(float mass){
        if(addImplus ){
            addImplus = false;
            if(Time.time - lastActivationTime > coldDownTime){ 
                lastActivationTime = Time.time;
                return boosterData.ImplusForce * mass;
            }
            else return 0;
        }
        return 0;
    }
    public float GetTorque(float mass){
        return mass * boosterData.DeltaAngularPower ;
    }
    public float GetTorqueImplus(float mass){
         if(addImplus){
            addImplus = false; 
            return boosterData.ImplusTourque * mass;
         }
        return 0;
    }
}
