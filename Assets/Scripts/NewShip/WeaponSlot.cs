using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class WeaponSlot : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Action<string, Vector2, Vector2, Transform> shootAction;
    WeaponData weaponData;
    public void Init(WeaponData weaponData,Action<string, Vector2, Vector2, Transform> shoot)
    {
        this.weaponData = weaponData;
        this.shootAction = shoot;
    }

    public void PlayShoot()
    {
        //Debug.Log($"gun rotate {localrotation}");
        //transform.localRotation = Quaternion.Euler(0, 0, localrotation);
        shootAction(weaponData.BulleteKeyString, transform.position, transform.position + transform.up, null);
    }
    public void RotateGun(float localrotation)
    {
        transform.localRotation = Quaternion.Euler(0, 0, localrotation);
    }
}
