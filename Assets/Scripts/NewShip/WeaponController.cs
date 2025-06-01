using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    WeaponData weaponData;
    [SerializeField] DroneProjectileDataBase droneProjectileDataBase;
    [SerializeField] DroneProjectileObj droneProjectilePrefab;
    [SerializeField] Transform muzzel;
    [SerializeField] Transform bulleteHeirarchyPlace;
    DroneProjectileData bullete;
    NewShipController ship;
    float timer;
    [ContextMenu("test 散射")]
    public void TestSpread()
    {
        RoundDataManager.DroneCount++;
    }
    [ContextMenu("test fusillade")]
    public void TestFusillade()
    {
        RoundDataManager.FusilladeCount++;
    }

    public void Init(NewShipController ship, WeaponData weaponData)
    {
        this.weaponData = weaponData;   
        this.ship = ship;
        timer = 0;
        //testing
        bullete = droneProjectileDataBase.GetData( weaponData.BulleteKeyString);
    }

    public void AddGun()
    {

    }

    public void AddBullete()
    {

    }

    public void AddShoot()
    {

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > weaponData.CD)
        {
            StartCoroutine(Fusillade(RoundDataManager.FusilladeCount));
            timer = 0;
        }
    }
    IEnumerator Fusillade(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Fire(RoundDataManager.DroneCount, RoundDataManager.BulleteSpreadAngle);
            yield return new WaitForSeconds(0.2f); 
        }
    }

    void Fire(int spreadCount, float spreadAngle)
    {
        if (spreadCount ==1)
        {
            Shoot(bullete, muzzel.position, ship.transform.up.normalized);
            return;
        }
        float angleStep = spreadAngle / spreadCount; // 計算每顆子彈的角度間隔
        float startAngle = -spreadAngle / 2f; // 設定最左側子彈的角度

        for (int i = 0; i < spreadCount; i++)
        {
            // 計算當前子彈的角度
            float angle = startAngle + (i * angleStep);
            Vector2 fireDirection = CalculateMethod.RotateVector(ship.transform.up.normalized, angle);

            Shoot(bullete, muzzel.position, fireDirection);
        }
    }
    private void Shoot(DroneProjectileData projectileData, Vector2 firePoint,Vector2 direction)
    {
        DroneProjectileObj obj = Instantiate(droneProjectilePrefab, bulleteHeirarchyPlace);

        projectileData.Direction = direction;


        switch (weaponData.pickTargetType)
        {
            case PickTargetType.NoOperate:
                projectileData.TargetWithPos = default;
                projectileData.TargetWithTrans = null;
                break;
            case PickTargetType.PickNearestTargetTrans:
                projectileData.TargetWithTrans = EnemyTargetPicker.Instance.PickNearestTargetTransformWithAngleAndRange(ship.transform.position,ship.transform.up,60f,100f);
                break;
            case PickTargetType.PickNearestTargetPos:
                projectileData.TargetWithPos = EnemyTargetPicker.PickNearestTargetPositionWithAngleAndRange(ship.transform.position,ship.transform.up,30f,10f,Vector2.one*3f);
            break;
            default:
                break;
        }

        obj.Init(ship.gameObject, TagsEnum.Enemy, ship.velocity, firePoint, direction, projectileData);
    }

}
[Serializable]
public struct WeaponData
{

    public string WeaponName;
    public string WeaponKeyString;
    public PickTargetType pickTargetType;
    public string BulleteKeyString;
    public float ShootAngleFrom;
    public float ShootAngleTo;
    public GameObject shootFX;
    public float CD;
    public float MoveRecoil;
    public float RotateRecoil;
    public int Magzine;
}
[Serializable]
public struct WeaponXlsxData
{
    public string KeyString;
    public string Name_TW;
    public string Name_EN;
    public string Description_TW;
    public string Description_EN;
    public string ImageName;
    public HpDmgData HpDmgData;
    public ArmorDmgData ArmorData;
}
public enum PickTargetType
{
    NoOperate=0,
    PickNearestTargetTrans=1,
    PickNearestTargetPos=2,
}
// public class MachineGun_Left : InputSubScriber
// {
//     private float lastAttackTime;
//     public MachineGun_Left(Rigidbody2D ship, WeaponData weaponData, WeaponSlot left, WeaponSlot right) : base(ship, weaponData,left,right)
//     {
//     }

//     public override void OnPushLeftStick(Vector2 v)
//     {
//         //Debug.Log($"vector : {v}");
//         //float angle = Vector2.SignedAngle(Vector2.up, v);
//         //Debug.Log(angle);
//         //bool isBetween = CalculateMethod.IsAngleBetween(angle, weaponData.ShootAngleFrom, weaponData.ShootAngleTo);
//         //if (isBetween)
//         //{
//         //    leftGun.RotateGun(angle);
//         //}
//         //else
//         //{
//         //    leftGun.RotateGun(CalculateMethod.GetClosestWithSign(angle, weaponData.ShootAngleFrom, weaponData.ShootAngleTo));
//         //}
//         //if (Time.time - lastAttackTime >= weaponData.CD)
//         //{
//         //    leftGun.PlayShoot();
//         //    ship.velocity += -(Vector2)ship.transform.up * weaponData.MoveRecoil;
//         //    ship.angularVelocity += weaponData.MoveRecoil;
//         //    lastAttackTime = Time.time;
//         //}
//     }
// }

// public class MachineGun_Right : InputSubScriber
// {
//     private float lastAttackTime;
//     public MachineGun_Right(Rigidbody2D ship, WeaponData weaponData, WeaponSlot left, WeaponSlot right) : base(ship, weaponData, left, right)
//     {
//     }

//     public override void OnPushRightStick(Vector2 v)
//     {
//         //float angle = Vector2.SignedAngle(Vector2.up, v);
//         //bool isBetween = CalculateMethod.IsAngleBetween(angle, weaponData.ShootAngleFrom, weaponData.ShootAngleTo);
//         //if (isBetween)
//         //{
//         //    rightGun.RotateGun(angle);
//         //}
//         //else
//         //{
//         //    rightGun.RotateGun(CalculateMethod.GetClosestWithSign(angle, weaponData.ShootAngleFrom, weaponData.ShootAngleTo));
//         //}

//         //if (Time.time - lastAttackTime >= weaponData.CD)
//         //{
//         //    rightGun.PlayShoot();
//         //    ship.velocity += -(Vector2)ship.transform.up * weaponData.MoveRecoil;
//         //    ship.angularVelocity += -weaponData.MoveRecoil;
//         //    lastAttackTime = Time.time;
//         //}
//     }
//}