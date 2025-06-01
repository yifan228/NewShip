using Unity.VisualScripting;
using UnityEngine;
public class BaseBoosterEngine : MonoBehaviour,IBoosterEngine
{
    [SerializeField] Rigidbody2D thisRB;

    public void BoostLeft(float strength,BoosterData boosterData)
    {
        thisRB.velocity += Time.deltaTime*strength*boosterData.DeltaSpeedPower * (Vector2)thisRB.transform.up;
        thisRB.angularVelocity += -Time.deltaTime*strength * boosterData.DeltaAngularPower;
    }

    public void BoostRight(float strength,BoosterData boosterData)
    {
        thisRB.velocity += Time.deltaTime * strength * boosterData.DeltaSpeedPower * (Vector2)thisRB.transform.up;
        thisRB.angularVelocity += Time.deltaTime * strength * boosterData.DeltaAngularPower;
    }

    public void BrakeLeft(BoosterData boosterData)
    {
        thisRB.velocity += -Time.deltaTime * boosterData.DeltaSpeedPower * (Vector2)thisRB.transform.up;
        thisRB.angularVelocity += Time.deltaTime * boosterData.DeltaAngularPower;
    }

    public void BrakeRight(BoosterData boosterData)
    {
        thisRB.velocity += -Time.deltaTime * boosterData.DeltaSpeedPower * (Vector2)thisRB.transform.up;
        thisRB.angularVelocity += -Time.deltaTime * boosterData.DeltaAngularPower;
    }

    public void ReleaseBoostLeft()
    {
        //throw new System.NotImplementedException();
    }

    public void ReleaseBoostRight()
    {
        //throw new System.NotImplementedException();
    }
}
