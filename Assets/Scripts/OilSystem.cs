using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OilSystem : MonoBehaviour
{
    
    [SerializeField] float MaxOil = 100;
    [SerializeField] float current_oil = 0;
    [SerializeField] float minuesSpeed = 1;
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] Image progressBar;
    Vector2 stickL, stickR;
    // Start is called before the first frame update
    void Start()
    {
        current_oil = MaxOil;
    }
    private void Update()
    {
        if (stickL.magnitude > 0 && Mathf.Abs(stickL.y) > 0.1f)
        {
            float force = Mathf.Abs(stickL.y);
            DecreaseOil(force);
        }
        if (stickR.magnitude > 0 && Mathf.Abs(stickR.y) > 0.1f)
        {
            float force = Mathf.Abs(stickR.y);
            DecreaseOil(force);
        }
        progressBar.fillAmount = current_oil/ MaxOil;
        text.text = ((int)current_oil).ToString();
    }
    public void OnLeftForce(InputAction.CallbackContext callbackContext)
    {
         stickL = callbackContext.ReadValue<Vector2>();
        
    }

    public void DecreaseOil(float force)
    {
        current_oil -= minuesSpeed * Time.deltaTime * force;
        if (current_oil <= 0) current_oil = 0;
    }
    public void AddOil(float amount)
    {
        current_oil = Mathf.Clamp(current_oil+amount,0,MaxOil);
    }
    public void OnRightForce(InputAction.CallbackContext callbackContext)
    {
         stickR = callbackContext.ReadValue<Vector2>();
        
    }
    public bool HasOil() {
        return current_oil > 0;
    }
}
