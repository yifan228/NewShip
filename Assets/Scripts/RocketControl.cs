using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class RocketControl : MonoBehaviour
{
    //[SerializeField] ShipControl shipControl;
    [SerializeField] ParticleSystem fire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*public void OnForce(InputAction.CallbackContext callbackContext)
    {
        Vector2 stick = callbackContext.ReadValue<Vector2>();
        float input = -stick.x; // 你的输入值，从 -1 到 1
        float targetAngle = Mathf.Lerp(-shipControl.forceAngleOffset, shipControl.forceAngleOffset, (input + 1f) / 2f);
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle);

        //add fire
        float val =  Mathf.Abs(shipControl.translateForce(stick));
        Color backColor = stick.y > 0 ? Color.white : Color.blue;
        fire.color = new Color(backColor.r, backColor.g, backColor.b, val);
    }*/
    public void OnForce(float angleInput,float force)
    {
        
        float input = angleInput; // 你的输入值，从 -1 到 1
        //float targetAngle = Mathf.Lerp(-shipControl.forceAngleOffset, shipControl.forceAngleOffset, (input + 1f) / 2f);
        //transform.localRotation = Quaternion.Euler(0, 0, targetAngle);

        //add fire

        Color backColor =  Color.white;
        //fire.color = new Color(backColor.r, backColor.g, backColor.b, force);

        if (force ==0)
        {
            fire.Stop();
        }
        else
        {
            if (fire.isPlaying) 
            {
                fire.transform.localScale = Vector3.one*(1+force);
            }
            else 
            {
                fire.Play();
                fire.transform.localScale = Vector3.one * (1 + force);
            }
        }
    }
    [Obsolete]
    public void OnForce(float force)
    {
        //add fire

        Color backColor = Color.white;
        //fire.color = new Color(backColor.r, backColor.g, backColor.b, force);
    }
}
