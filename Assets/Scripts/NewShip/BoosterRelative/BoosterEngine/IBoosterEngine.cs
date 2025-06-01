public interface IBoosterEngine
{
    void BoostLeft(float strength,BoosterData boosterData);
    void BoostRight(float strength,BoosterData boosterData);
    void BrakeLeft(BoosterData boosterData);
    void BrakeRight(BoosterData boosterData);
    void ReleaseBoostLeft();
    void ReleaseBoostRight();
    
}

