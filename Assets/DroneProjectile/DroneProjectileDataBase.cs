using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DroneProjectileDatabase", menuName = "Data/DroneProjectileDatabase")]
public class DroneProjectileDataBase : ScriptableObject
{
    [SerializeField] List<DroneProjectileData> Data;

    public DroneProjectileData GetData(string projectileName)
    {
        DroneProjectileData drone = Data.Find(x => x.ProjectileKeyString == projectileName);
        if ( !drone.Equals(default))
        {
            return drone;
        }
        else
        {
            Debug.LogWarning($"can't find projectileName : {projectileName}");
            return default;
        }
    }
}
