using UnityEngine;


[CreateAssetMenu(fileName = "boosterParam", menuName = "Data/Booster")]
public class BoosterSO : ScriptableObject
{
    [SerializeField] public BoosterDriveName driveNames;
    [SerializeField] public BoosterData data;
    public BoosterData GetData(string installationPosition)
    {
        data.driveName = GetDriverWithPositionPrefix(installationPosition);
        return data;
    }

    private BoosterDriveName GetDriverWithPositionPrefix(string prefix)
    {

        foreach (BoosterDriveName option in System.Enum.GetValues(typeof(BoosterDriveName)))
        {
            string tmp = option.ToString().Split('_')[0];
            if (prefix == tmp)
            {
                return option;
            }
        }

        return default;
    }
}
