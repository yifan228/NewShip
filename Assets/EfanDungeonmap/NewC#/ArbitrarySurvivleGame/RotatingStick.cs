using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStick : ObstacleItem
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float detectedDistance;
    [SerializeField] float staticAngularSpeed;
    [SerializeField] float maxAngularSpeed;
    [SerializeField] float swingingAcceleration;
    [SerializeField] float staticAcceleration;
    protected override void Updating()
    {
        if (Vector2.Distance(rb2d.transform.position,target.position)< detectedDistance)
        {
            //swing
            float f = 0;
            if (rb2d.angularVelocity>=0)
            {
                f= (swingingAcceleration) * Time.deltaTime;
            }
            else
            {
                f= -(swingingAcceleration) * Time.deltaTime;
            }
            if (Mathf.Abs(rb2d.angularVelocity) < maxAngularSpeed)
            {
                rb2d.angularVelocity += f;
            }
        }
        else
        {
            if (Mathf.Abs(rb2d.angularVelocity)< staticAngularSpeed)
            {
                if (rb2d.angularVelocity>=0)
                {
                    rb2d.angularVelocity += (staticAcceleration) * Time.deltaTime;
                }
                else
                {
                    rb2d.angularVelocity += -(staticAcceleration) * Time.deltaTime;
                }
            }
            else
            {
                // after swing slow down
                if (rb2d.angularVelocity >= 0)
                {
                    rb2d.angularVelocity -= (staticAcceleration) * Time.deltaTime;
                }
                else
                {
                    rb2d.angularVelocity -= -(staticAcceleration) * Time.deltaTime;
                }
            }
        }
                
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce(collision.GetContact(0).normal*-1000);
            //Debug.Log("force : "+ collision.GetContact(0).normal * 1000);
            rb2d.angularVelocity *= 0.25f;
        }
    }
}
