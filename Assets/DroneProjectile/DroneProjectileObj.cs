using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.VFX;

public class DroneProjectileObj : MonoBehaviour
{
    [SerializeField] Rigidbody2D rbody2D;
    [SerializeField] SpriteRenderer spriteRenderer;
    TagsEnum targetTag;
    GameObject owner;

    DroneProjectileData thisProjectileData;
    AProjectileMovement movingAction;
    float timer;
    bool isupdating = false;
    Vector2 initWorldSpeed;
    float circleCastTimer = 0f;
    int circleCastCount = 0;
    List<GameObject> canNotDamageList = null;

    public DamageDTO damageDTO;

    public void Update()
    {
        if (isupdating)
        {
            if (thisProjectileData.LifeTime>0&& timer>thisProjectileData.LifeTime)
            {
                //DroneProjectileObjPool.ObjReturn(this);
                Destroy(gameObject);
            }

            movingAction?.Moving();

            if(thisProjectileData.TriggerDmgType == TriggerDmgType.CircleCast){
                if (circleCastCount < thisProjectileData.DurationDamageMaxCount)
                {
                    circleCastTimer += Time.deltaTime;
                    if (circleCastTimer >= thisProjectileData.DurationDamageCD)
                    {
                        CircleCast2D(thisProjectileData.ObjSize.z*thisProjectileData.View.RealRangeDevidedSize/2);
                        circleCastTimer = 0f;
                        circleCastCount++;
                    }
                }
            }

            timer += GlobalTimeManager.Global_Deltatime;
        }
    }

    void DecideMovementType(ProjectileMovementType type)
    {
        ProjectileMovementData d= new ProjectileMovementData(rbody2D,initWorldSpeed,thisProjectileData.TargetWithPos,thisProjectileData.TargetWithTrans);
        switch (type)
        {
            case ProjectileMovementType.Stright:
                movingAction = new BulleteProjectile(d,thisProjectileData.MaxSpeed);
                break;
            case ProjectileMovementType.HomingMissle:
                movingAction = new HomingMissleProjectile(thisProjectileData.MaxAngularSpeed,thisProjectileData.Acceleration, d);
                break;
            case ProjectileMovementType.StraitHoming:
                movingAction = new StraitHomingBullete(d,thisProjectileData.MaxSpeed,thisProjectileData.MaxAngularSpeed,45f);
            break;
            default:
                break;
        }
    }

    private void SpawnHitFX(Vector3 size, VFXBase hitfx)
    {
        if (hitfx != null)
        {
            VFXBase vfx = Instantiate(hitfx, transform.position, Quaternion.identity);
            vfx.transform.localScale = size;
            vfx.Play(0, (x) => {
                Destroy(x.gameObject);
            }, (progress) => {
                // 可在這裡更新特效進度
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (thisProjectileData.TriggerDmgType != TriggerDmgType.OnTriggerEnter) return;

        if (collision.CompareTag(TagsEnum.Wall.ToString()))
        {
            SpawnHitFX(Vector3.one, thisProjectileData.HitFX);
            Destroy(gameObject);
        }



        if (collision.CompareTag(targetTag.ToString()) )
        {
            if(thisProjectileData.PermissionCount == 0){
                return;
            }
            if( canNotDamageList != null && canNotDamageList.Contains(collision.gameObject)){
                //UnityEngine.Debug.Log($"canNotDamageList: {collision.name}");
                return;
            }
            if(targetTag == TagsEnum.Enemy){
                Debugger.Log(DebugCategory.PlayerProjectile, $"permission count: {thisProjectileData.PermissionCount}");
            }else if(targetTag == TagsEnum.Player){
                Debugger.Log(DebugCategory.EnemyProjectile, $"permission count: {thisProjectileData.PermissionCount}");
            }
            DroneProjectileEffectManager.Hit(collision.gameObject,thisProjectileData,owner,targetTag,transform.position,transform.up);
            DamageManager.Damage(damageDTO,collision.gameObject, owner);

            Vector2 pos = transform.position;
            SpawnHitFX(Vector3.one, thisProjectileData.HitFX);

            if(thisProjectileData.PermissionCount>0&&thisProjectileData.PermissionCount<999){
                thisProjectileData.PermissionCount--;
            }
            if (thisProjectileData.PermissionCount<=0)
            {
                Destroy(gameObject);
            }
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(thisProjectileData.TriggerDmgType == TriggerDmgType.OnTriggerStay){
            DamageManager.Damage(damageDTO,other.gameObject, owner);
        }
    }   

    public void Init(GameObject owner,TagsEnum targetTag,Vector2 initWorldSpeed,Vector2 initPosition,Vector2 initToward,DroneProjectileData data,List<GameObject> canNotDamageList = null)
    {
        this.canNotDamageList = canNotDamageList;
        this.targetTag = targetTag;
        this.owner = owner;
        this.initWorldSpeed = initWorldSpeed;
        this.thisProjectileData = data;
        gameObject.SetActive(true);
        gameObject.transform.position = initPosition;
        gameObject.transform.up = initToward.normalized;

        if(targetTag == TagsEnum.Player){
            Debugger.Log(DebugCategory.Projectile,$"DroneProjectileData: \n" +
            $"MovementType: {data.MovementType}\n" +
            $"DroneProjectileType: {data.DroneProjectileType}\n" + 
            $"TriggerDmgType: {data.TriggerDmgType}\n" +
            $"SecondaryHpDmg: {data.SecondaryHpDmg}\n" +
            $"SecondaryArmorDmg: {data.SecondaryArmorDmg}\n" +
            $"LifeTime: {data.LifeTime}\n" +
            $"ObjSize: {data.ObjSize}\n" +
            $"PermissionCount: {data.PermissionCount}\n" +
            $"Direction: {data.Direction}\n" +
            $"TargetWithPos: {data.TargetWithPos}\n" +
            $"TargetWithTrans: {data.TargetWithTrans}\n" +
            $"View: {data.View}\n" +
            $"HitFX: {data.HitFX}");
        }

        if(targetTag == TagsEnum.Enemy){
            // Debugger.Log(DebugCategory.Projectile,$"DroneProjectileData: \n" +
            //     $"MovementType: {data.MovementType}\n" +
            //     $"DroneProjectileType: {data.DroneProjectileType}\n" + 
            //     $"TriggerDmgType: {data.TriggerDmgType}\n" +
            //     $"SecondaryHpDmg: {data.SecondaryHpDmg}\n" +
            //     $"SecondaryArmorDmg: {data.SecondaryArmorDmg}\n" +
            //     $"LifeTime: {data.LifeTime}\n" +
            //     $"ObjSize: {data.ObjSize}\n" +
            //     $"PermissionCount: {data.PermissionCount}\n" +
            //     $"Direction: {data.Direction}\n" +
            //     $"TargetWithPos: {data.TargetWithPos}\n" +
            //     $"TargetWithTrans: {data.TargetWithTrans}\n" +
            //     $"View: {data.View}\n" +
            //     $"HitFX: {data.HitFX}");
        }   

        if (data.View != null)
        {
            VFXBase vfx = Instantiate(data.View, transform);
            gameObject.name = vfx.name;
            vfx.Play(data.LifeTime, null, null);
        }else{
            UnityEngine.Debug.LogWarning("view is null");
        }
        transform.localScale = data.ObjSize;
        if(data.View.DamageRangeCollider != null){
            var src = data.View.DamageRangeCollider;
            if (src is CircleCollider2D circle) {
                var c = gameObject.AddComponent<CircleCollider2D>();
                c.isTrigger = circle.isTrigger;
                c.radius = circle.radius;
                c.offset = circle.offset;
            } else if (src is BoxCollider2D box) {
                var b = gameObject.AddComponent<BoxCollider2D>();
                b.isTrigger = box.isTrigger;
                b.size = box.size;
                b.offset = box.offset;
            } else if (src is CapsuleCollider2D capsule) {
                var cap = gameObject.AddComponent<CapsuleCollider2D>();
                cap.isTrigger = capsule.isTrigger;
                cap.size = capsule.size;
                cap.offset = capsule.offset;
                cap.direction = capsule.direction;
            }
            // 你可以根據需要繼續擴充其他 Collider2D 型別

            // 關閉原本的 collider
            src.enabled = false;
        }

        damageDTO = new DamageDTO(data.SecondaryHpDmg,data.SecondaryArmorDmg,data.DroneProjectileType);
        DecideMovementType(data.MovementType);
        //Tool.ResetCollider.ResetCollider2D(triggerCollider, spriteRenderer);
        timer = 0;

        // explode summoned con't skip first trigger
        if(thisProjectileData.DroneProjectileType == DroneProjectileType.Explode)
        {
            isupdating = false;
            CircleCast2D(thisProjectileData.ObjSize.z*thisProjectileData.View.RealRangeDevidedSize/2);
            //SpawnHitFX(thisProjectileData.ObjSize, thisProjectileData.HitFX);
            Destroy(gameObject,thisProjectileData.View.OneVFXLifeTime);
        }else{
            isupdating = true;
        }
    }

    /// <summary>
    /// 如果是二次傷害，則使用 thisProjectileData.ObjSize.z*thisProjectileData.View.RealRangeDevidedSize/2
    /// </summary>
    /// <param name="radius"></param>
    private void CircleCast2D(float radius)
    {
        UnityEngine.Debug.Log($"CircleCast2D: {radius}");
        Vector2 origin = transform.position; // 以當前物件為圓心
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(targetTag.ToString())) // 用 Tag 來篩選目標
            {
                DamageManager.Damage(damageDTO,hit.collider.gameObject, owner);
            }
        }
    }

    public void CloseDisPlay()
    {
        gameObject.SetActive(false);
        timer = 0;
        rbody2D.velocity = Vector2.zero;
        rbody2D.angularVelocity = 0;
    }
}
[Serializable]
public struct DroneProjectileData
{
    public string ProjectileKeyString;
    public DroneProjectileType DroneProjectileType;
    public float LifeTime;
    public Template Template;
    public TriggerDmgType TriggerDmgType;
    public float DurationDamageCD;
    public float DurationDamageMaxCount;
    public VFXBase View;
    /// <summary>
    /// x:width,y:height,z:diameter
    /// </summary>
    public Vector3 ObjSize;

    public ProjectileMovementType MovementType;

    public Transform TargetWithTrans { get; set; }
    public Vector2 TargetWithPos { get; set; }
    public Vector2 Direction { get; set; }

    // When you select 'ProjectileMovementType.Stright' and 'ProjectileMovementType.HomingMissle', the corresponding attribute will be displayed.
    public float MaxSpeed;
    public float Acceleration;

    // When you select 'ProjectileMovementType.HomingMissle', the corresponding attribute will be displayed.
    public float MaxAngularSpeed;
    public float AngularAcceleration;

    public int PermissionCount ;

    public VFXBase HitFX;
    //----------------- When you select 'OnHitSkill' in the editor, the corresponding attribute will be displayed.
    public OnHitSkill onHitSkill;
    // explode
    public string ExplodeProjectileKeyString;
    public float ExplodeRange;// 直徑
    public HpDmgData Out_EplodeHpDmg;
    public ArmorDmgData Out_EplodeArmorDmg;
    //

    //split
    public string SplitProjectileKeyString;
    public int SplitAcount;
    public float AngleFrom;
    public float AngleTo;
    public HpDmgData Out_SplitHpDmg;
    public ArmorDmgData Out_SplitArmorDmg;
    //

    //Field
    public string FieldProjectileKeyString;
    public float FieldRange;//直徑
    public float FieldTime;
    public HpDmgData Out_FieldHpDmg;
    public ArmorDmgData Out_FieldArmorDmg;
    //

    public HpDmgData SecondaryHpDmg;
    public ArmorDmgData SecondaryArmorDmg;
}

public enum ProjectileMovementType
{
    Stationary,
    Stright,
    StraitHoming,
    HomingMissle
}

public enum OnHitSkill 
{
    None,
    Explode,
    Split,
    Field
}

public enum TriggerDmgType
{
    OnTriggerEnter,
    CircleCast,
    OnTriggerStay
}

public enum Template
{
    Basic
}

public enum DroneProjectileType
{
    Bullete,
    Field,
    Explode,
}
