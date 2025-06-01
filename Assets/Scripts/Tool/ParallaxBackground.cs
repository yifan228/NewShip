using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float parallaxEffectMultiplier = 0.5f;
    private Vector3 lastTargetPosition; 

    void Start()
    {
        if (target == null)
        {
            target = Camera.main.transform;
        }
        lastTargetPosition = target.position;
    }

    void Update()
    {
        Vector3 deltaMovement = target.position - lastTargetPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);
        lastTargetPosition = target.position;
    }
}
