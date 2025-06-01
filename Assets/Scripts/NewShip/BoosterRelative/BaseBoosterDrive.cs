using Unity.VisualScripting;
using UnityEngine;

[System.Flags]
public enum BoosterDriveName
{
    RTrigger_Base = 1<<0,
    LTrigger_Base= 1<<1,
    RShoulder_Base = 1<<2,
    LShoulder_Base = 1<<3
}

public class BaseLeftTriggerBoosterDrive : InputSubScriber
{
    float onPressTimer;
    public BaseLeftTriggerBoosterDrive(NewShipController ship, BoosterData boosterData) : base(ship, boosterData)
    {
        onPressTimer = 0;
    }

    public override void OnPressingLeftTirgger(float strengh)
    {
        onPressTimer += GlobalTimeManager.Global_Deltatime;

        ship.externalForce += (Vector2)ship.transform.up.normalized * boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaSpeedPower;

        ship.externalTorque += -boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaAngularPower;
    }
    public override void OnReleaseLeftTrigger()
    {
        onPressTimer = 0;
    }
    
}

public class BaseRightTriggerBoosterDrive : InputSubScriber
{
    float onPressTimer;
    public BaseRightTriggerBoosterDrive(NewShipController ship, BoosterData boosterData) : base(ship, boosterData)
    {
        onPressTimer = 0;
    }

    public override void OnPressingRightTirgger(float strengh)
    {
        onPressTimer += GlobalTimeManager.Global_Deltatime;

        ship.externalForce += (Vector2)ship.transform.up.normalized * boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaSpeedPower;

        ship.externalTorque += boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaAngularPower ;
    }
    public override void OnReleaseRightTrigger()
    {
        onPressTimer = 0;
    }
    
}

public class BaseLeftShoulderBoosterDrive : InputSubScriber
{
    float onPressTimer;
    public BaseLeftShoulderBoosterDrive(NewShipController ship, BoosterData boosterData) : base(ship, boosterData)
    {
        onPressTimer = 0;
    }

    public override void OnPressLeftShouler()
    {
        onPressTimer += GlobalTimeManager.Global_Deltatime;

        ship.externalForce -= (Vector2)ship.transform.up.normalized * boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaSpeedPower ;

        ship.externalTorque += boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaAngularPower;
    }
    public override void OnReleaseLeftShouler()
    {
        onPressTimer = 0;
    }
}

public class BaseRightShoulderBoosterDrive : InputSubScriber
{
    float onPressTimer;
    public BaseRightShoulderBoosterDrive(NewShipController ship, BoosterData boosterData) : base(ship, boosterData)
    {
        onPressTimer = 0;
    }

    public override void OnPressRightShouler()
    {
        onPressTimer += GlobalTimeManager.Global_Deltatime;

        ship.externalForce -= (Vector2)ship.transform.up.normalized * boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaSpeedPower;

        ship.externalTorque += -boosterData.Accelerationcurve.Evaluate(onPressTimer) * boosterData.DeltaAngularPower;
    }
    public override void OnReleaseRightShouler()
    {
        onPressTimer = 0;
    }
}

