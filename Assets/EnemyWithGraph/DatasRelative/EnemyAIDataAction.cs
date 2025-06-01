using UnityEngine;
using System.Collections.Generic;

public enum EnemyAIActionType
{
    Entry,
    MoveToPlayer,
    StationaryAim,
    StationaryNoAim,
    ShootPlayer,
    SpreadShot,
    ConditionDistanceToPlayer,
    Repeat,
    // ...可擴充
}

[System.Serializable]
public class Branch
{
    /// <summary>
    /// no yes
    /// </summary>
    public string key;
    [SerializeReference]
    public EnemyAIAction action;
}

[System.Serializable]
public abstract class EnemyAIAction
{
    public abstract EnemyAIActionType type { get; }

    [SerializeReference]
    public List<Branch> branches = new List<Branch>();
    public float LifeTime;
    public float Delay;
    /// <summary>
    /// -1 表示無 CD
    /// </summary>
    public float CD;

    // 靜態快取：型別 -> (欄位名稱 -> 欄位資訊)
    private static Dictionary<System.Type, Dictionary<string, System.Reflection.FieldInfo>> fieldCache = new();

    protected EnemyAIAction() { }

    public T Get<T>(string paramName)
    {
        var t = this.GetType();
        if (!fieldCache.TryGetValue(t, out var fields))
        {
            fields = new Dictionary<string, System.Reflection.FieldInfo>();
            foreach (var f in t.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                fields[f.Name] = f;
            }
            fieldCache[t] = fields;
        }
        if (fields.TryGetValue(paramName, out var field) && field.FieldType == typeof(T))
        {
            return (T)field.GetValue(this);
        }
        throw new System.Exception($"參數 {paramName} 不存在或型別不符於 {typeof(T)}");
    }

    public bool TryGet<T>(string paramName, out T value)
    {
        value = default(T);
        var t = this.GetType();
        
        if (!fieldCache.TryGetValue(t, out var fields))
        {
            fields = new Dictionary<string, System.Reflection.FieldInfo>();
            foreach (var f in t.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                fields[f.Name] = f;
            }
            fieldCache[t] = fields;
        }

        if (fields.TryGetValue(paramName, out var field) && field.FieldType == typeof(T))
        {
            value = (T)field.GetValue(this);
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class EntryAction : EnemyAIAction
{
    public override EnemyAIActionType type => EnemyAIActionType.Entry;

    public EntryAction() : base() { }
    public EntryAction(EntryAction target) : base()
    {
        this.LifeTime = target.LifeTime;
        this.branches = new List<Branch>();
        this.CD = target.CD;
        this.Delay = target.Delay;
        // 不做 branches 複製（只複製欄位）
    }
}

[System.Serializable]
public class ShootPlayerAction : EnemyAIAction
{
    public override EnemyAIActionType type => EnemyAIActionType.ShootPlayer;
    public int bulletCount;
    public string bulleteKeyString;

    public ShootPlayerAction() : base() { }
    public ShootPlayerAction(ShootPlayerAction target) : base()
    {
        this.LifeTime = target.LifeTime;
        this.bulletCount = target.bulletCount;
        this.bulleteKeyString = target.bulleteKeyString;
        this.branches = new List<Branch>();
        this.CD = target.CD;
        this.Delay = target.Delay;
    }
}

[System.Serializable]
public class MoveToPlayerAction : EnemyAIAction
{
    public override EnemyAIActionType type => EnemyAIActionType.MoveToPlayer;
    public float nearDistance;
    public float speed;

    public MoveToPlayerAction() : base() { }
    public MoveToPlayerAction(MoveToPlayerAction target) : base()
    {
        this.LifeTime = target.LifeTime;
        this.nearDistance = target.nearDistance;
        this.speed = target.speed;
        this.branches = new List<Branch>();
        this.CD = target.CD;
        this.Delay = target.Delay;
    }
}

[System.Serializable]
public class StationaryAimAction : EnemyAIAction
{
    public override EnemyAIActionType type => EnemyAIActionType.StationaryAim;

    public StationaryAimAction() : base() { }
    public StationaryAimAction(StationaryAimAction target) : base()
    {
        this.LifeTime = target.LifeTime;
        this.branches = new List<Branch>();
        this.CD = target.CD;
        this.Delay = target.Delay;
    }
}

[System.Serializable]
public class StationaryNoAimAction : EnemyAIAction
{
    public override EnemyAIActionType type => EnemyAIActionType.StationaryNoAim;

    public StationaryNoAimAction() : base() { }
    public StationaryNoAimAction(StationaryNoAimAction target) : base()
    {
        this.LifeTime = target.LifeTime;
        this.branches = new List<Branch>();
        this.CD = target.CD;
        this.Delay = target.Delay;
    }
}

[System.Serializable]
public class SpreadShotAction : EnemyAIAction
{
    public override EnemyAIActionType type => EnemyAIActionType.SpreadShot;
    public string bulleteKeyString;
    public float shootInterval;
    public float spreadAngle;
    public int bulleteCount;

    public SpreadShotAction() : base() { }
    public SpreadShotAction(SpreadShotAction target) : base()
    {
        this.LifeTime = target.LifeTime;
        this.bulleteKeyString = target.bulleteKeyString;
        this.shootInterval = target.shootInterval;
        this.spreadAngle = target.spreadAngle;
        this.bulleteCount = target.bulleteCount;
        this.branches = new List<Branch>();
        this.CD = target.CD;
        this.Delay = target.Delay;
    }
}

[System.Serializable]
public class ConditionDistanceToPlayerAction : EnemyAIAction
{
    public float distance;
    public override EnemyAIActionType type => EnemyAIActionType.ConditionDistanceToPlayer; // TODO: 建議你擴充 enum
    public ConditionDistanceToPlayerAction() : base() { }
    public ConditionDistanceToPlayerAction(ConditionDistanceToPlayerAction target) : base()
    {
        this.distance = target.distance;
        this.LifeTime = target.LifeTime;
        this.branches = new List<Branch>();
        this.CD = target.CD;
        this.Delay = target.Delay;

    }
}

[System.Serializable]
public class RepeatAction : EnemyAIAction
{
    public int repeatTimes = 0;
    public override EnemyAIActionType type => EnemyAIActionType.Repeat;
    public RepeatAction() : base() { }
    public RepeatAction(int repeatTimes) : base()
    {
        this.repeatTimes = repeatTimes;
        this.LifeTime =0f;
    }
} 