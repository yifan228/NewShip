using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDatabase : MonoBehaviour
{
    public static GlobalDatabase Instance { get; private set; }
    [SerializeField] DroneProjectileDataBase droneProjectileDataBase;
    [SerializeField] VFXBase hitFX;
    [SerializeField] DroneProjectileObj droneProjectilePrefab;
    [SerializeField] Transform droneProjectileParent;
    public DroneProjectileDataBase DroneProjectileDataBase => droneProjectileDataBase;
    public VFXBase HitFX => hitFX;
    public DroneProjectileObj DroneProjectilePrefab => droneProjectilePrefab;

    public float DroneProjectileDamageRatio => 1;
    public float DroneProjectileFieldDamageRatio => 0.1f;
    public float DroneProjectileExplodeDamageRatio => 0.25f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
