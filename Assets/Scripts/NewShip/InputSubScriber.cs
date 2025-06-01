using UnityEngine;

public class InputSubScriber
{
    protected NewShipController ship;
    protected BoosterData boosterData;
    protected WeaponData weaponData;

    //protected WeaponSlot leftGun;
    //protected WeaponSlot rightGun;

    public InputSubScriber(NewShipController ship, BoosterData boosterData)
    {
        this.ship = ship;
        this.boosterData = boosterData;   
    }

    public InputSubScriber(Rigidbody2D ship, WeaponData weaponData, WeaponSlot left, WeaponSlot right)
    {

    }


    public virtual void OnPressRightTrigger() { }
    public virtual void OnPressLeftTrigger() { }
    public virtual void OnPressingLeftTirgger(float strengh) { }
    public virtual void OnReleaseLeftTrigger() { }
    public virtual void OnPressingRightTirgger(float strengh) { }
    public virtual void OnReleaseRightTrigger() { }
    public virtual void OnPressLeftShouler() { }
    public virtual void OnReleaseLeftShouler() { }
    public virtual void OnPressRightShouler() { }
    public virtual void OnReleaseRightShouler() { }
    public virtual void OnPushLeftStick(Vector2 v) { }
    public virtual void OnPushRightStick(Vector2 v) { }
}

