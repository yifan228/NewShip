using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RestorePack : MonoBehaviour
{
    [SerializeField] UpgradeMaptileType type;
    public float amount;
    //[SerializeField] SpriteAnimation spAnim;
    Action<float> TriggerCallback;

    //public void StartTirgger(Mayzi charactor)
    //{
    //    TriggerCallback?.Invoke(amount);
    //    transform.DOScale(Vector2.zero, 0.25f).OnComplete(AnimEnd);
    //}

    public void SetTriggerDelegate(Action<float> triggerCallback)
    {
        TriggerCallback = triggerCallback;
    }
    private void AnimEnd()
    {
        gameObject.SetActive(false);
    }
}
public enum UpgradeMaptileType
{
    Time,
    StationedEnemyBornInterval,
    Ammo
}


