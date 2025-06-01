using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private MainModule main;

    private void Awake()
    {
        main = _particleSystem.main;
    }

    public void SetScaleAndDirection(Vector3 scale, Vector2 angleV2)
    {
        gameObject.SetActive(true);

        float deg = Vector2.Angle(Vector2.up, angleV2);
        deg = 0 > angleV2.x ? -deg : deg;
        main.startRotation = deg * Mathf.Deg2Rad;

        main.startSizeX = scale.x;
        main.startSizeY = scale.y;
        main.startSizeZ = scale.z;
        _particleSystem.Play();
    }

    public void Recycle()
    {
        gameObject.SetActive(false);
        main.startSizeY = 1;
        main.startRotation = 0;
    }
}
