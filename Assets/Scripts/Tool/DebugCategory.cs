[System.Serializable]
public enum DebugCategory
{
    Enemy = 1<<0,
    GameLogic = 1<<1,
    DamageLogic = 1<<2,
    UI = 1<<3,
    Projectile = 1<<4,
    EnemyProjectile = 1<<5,
    PlayerProjectile = 1<<6,
    Server = 1<<7,
    // 你可以根據需求再擴充
} 