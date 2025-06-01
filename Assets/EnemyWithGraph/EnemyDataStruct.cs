using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnemyNameSpace{
    [Serializable]
    public struct EnemyDataStruct
    {
        public string Name;
        public string KeyString;
        public Sprite Sprite;

        public EnemyMotionPattern MotionPattern;
        public EnemyAttackPattern AttackPattern;

        public EnemyAIData AIData;
        public PickTargetType PickTargetType;

        public int FusilladeCount;
        public int DroneCount;
        [Tooltip("angle between bulletes")]
        public float DroneAngle;

        public string DroneKeyString;

        public MoveAndRotateData MoveAndRotateData;

        public HpData HPdata;

        public float HpDamage;
        public float ArmorDamage;
        public float Crash;
        public Vector2 FirePosition;


        //public ChasePatternData ChasePatternData;
    }
    public enum EnemyMotionPattern
    {
        Stationary,
        OnlyStationary,
        StartionaryNoRotate,
        ChasePlayer,
    }
    public enum EnemyAttackPattern
    {
        Bullete,
        Homing
    }

    [Serializable]
    public struct MoveAndRotateData
    {
        public float Range;
        public float AngularRange;
        public float MaxMoveSpeed;
        public float MinMoveSpeed;
        public float Acceleration;
        public float Deceleration;
        public float MaxAngularSpeed;
        public float MinAngularSpeed;
        public float RotateAcceleration;
        public float RotateDeceleration;
    }
}
