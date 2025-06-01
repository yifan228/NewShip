using EnemyNameSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EnemyNameSpace
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
    [System.Serializable]
    public class EnemyData : ScriptableObject
    {
        public EnemyDataStruct Data;
    }


}
