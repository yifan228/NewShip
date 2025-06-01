using UnityEngine;
using UnityEngine.InputSystem;

public class RocketsHandller : MonoBehaviour
{
    [SerializeField] RocketControl LRocket,LBRocket, RBRocket,RRocket;
    public void OnLeftForce(float val)
    {
        LRocket.OnForce(0, val > 0.1f ? Mathf.Abs(val) : 0);
    }
    public void OnRightForce(float val)
    {
        RRocket.OnForce(0, val > 0.1f ? Mathf.Abs(val) : 0);
    }
    public void OnLeftBackForce(float val)
    {
        LBRocket.OnForce(0, val > 0.1f ? Mathf.Abs(val) : 0);
    }
    public void OnRightBackForce(float val)
    {
        RBRocket.OnForce(0, val > 0.1f ? Mathf.Abs(val) : 0);
    }
}
