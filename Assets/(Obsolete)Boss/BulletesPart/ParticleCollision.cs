using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject explosionPrefab;
    public Action<GameObject,Vector2> ParticleOncollision;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {

        if (other.CompareTag(TagsEnum.Enemy.ToString()) || other.CompareTag(TagsEnum.FrontCollision.ToString()))
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            ParticleOncollision?.Invoke(other, collisionEvents[0].intersection);
            Debug.LogWarning("laser bullete collision");
            GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);
        }

        //if (other.GetComponent<Rigidbody2D>() != null)
        //    other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 10 - transform.position, collisionEvents[0].intersection + Vector3.up);
        
    }
}
