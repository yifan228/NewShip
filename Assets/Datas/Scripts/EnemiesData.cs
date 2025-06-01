using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyNameSpace
{

    [CreateAssetMenu(fileName = "EnemiesDatabase", menuName = "Data/EnemyDatabase")]
    [System.Serializable]
    public class EnemiesDatas : ScriptableObject
    {
        [SerializeField] List<EnemyData> Data;
        public EnemyDataStruct GetData(string keystring)
        {
            return Data.Find(x => x.Data.KeyString == keystring).Data;
        }

        public List<EnemyDataStruct> GetDataBase()
        {
            return Data.Select(x => x.Data).ToList();
        }
    }
    
}
