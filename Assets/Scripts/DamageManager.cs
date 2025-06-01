using EnemyNameSpace;
using UnityEngine;

public static class DamageManager
{
    //(玩家基礎數值)*(傷害倍率 等等)*(爆擊傷害)
    private static float randomRange = 10f;
    public static void Damage(DamageDTO damageDTO,GameObject target, GameObject owener)
    {
        if (owener.CompareTag(TagsEnum.Player.ToString())&&target.CompareTag(TagsEnum.Enemy.ToString()))
        {
            HpSystem enemy = target.GetComponentInParent<HpSystem>();
            DamageToEnemy(enemy,damageDTO);
        } else if (owener.CompareTag(TagsEnum.Enemy.ToString())&& target.CompareTag(TagsEnum.Player.ToString()))
        {
            HpSystem player = target.GetComponent<HpSystem>();
            DamageToPlayer(player,owener.GetComponent<EnemyController>().KeyString,damageDTO);
        }else if (target.CompareTag(TagsEnum.Wall.ToString()))
        {
            
        }
    }

    private static void DamageToEnemy(HpSystem enemy,DamageDTO damageDTO)
    {
        float dmg_no_critical = (RoundDataManager.Out_PlayerData.hpdmgData.Dmg + RoundDataManager.In_GameData.hpdmgData.Dmg);

        float superCrashDmg = (1f+RoundDataManager.Out_PlayerData.armorDmgData.SuperCrashDmg_Percentage
         + RoundDataManager.In_GameData.armorDmgData.SuperCrashDmg_Percentage
         +damageDTO.ArmorDmgDataWithAllRatio.SuperCrashDmg_Percentage);

        float dmg = dmg_no_critical;

        bool isCritical = false;

        float secondaryCritical =damageDTO.HpDmgDataWithAllRatio.Critical_Percentage;

        if (Random.Range(0f, 1f) < (RoundDataManager.Out_PlayerData.hpdmgData.Critical_Percentage + 
        RoundDataManager.In_GameData.hpdmgData.Critical_Percentage+
        secondaryCritical))
        {
            isCritical = true;  

            float secondaryCriticalDamage =damageDTO.HpDmgDataWithAllRatio.CriticalDamage_Percentage;

            // 爆擊傷害
            dmg *= 
            (1+RoundDataManager.Out_PlayerData.hpdmgData.CriticalDamage_Percentage + 
            RoundDataManager.In_GameData.hpdmgData.CriticalDamage_Percentage+secondaryCriticalDamage);
        }

        // 飛行道具類型的倍率
        switch(damageDTO.DroneProjectileType){
            case DroneProjectileType.Bullete:
                dmg *=GlobalDatabase.Instance.DroneProjectileDamageRatio+damageDTO.HpDmgDataWithAllRatio.Dmg;
                break;
            case DroneProjectileType.Field:
                dmg *=(GlobalDatabase.Instance.DroneProjectileFieldDamageRatio+damageDTO.HpDmgDataWithAllRatio.Dmg);
                break;
            case DroneProjectileType.Explode:
                dmg *=(GlobalDatabase.Instance.DroneProjectileExplodeDamageRatio+damageDTO.HpDmgDataWithAllRatio.Dmg);
                break;
        }

        if(dmg<=0){
            UnityEngine.Debug.LogError($"{damageDTO.DroneProjectileType}傷害為0");
            return;
        }

        if(enemy.CurrentArmor==0)
        {
            if (isCritical)
            {
                DamageTextManager.instance.ShowCriticalDamage(dmg*superCrashDmg, enemy.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
            }
            else
            {
                DamageTextManager.instance.ShowHpDamage(dmg*superCrashDmg, enemy.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
            }
            bool isKill= enemy.HpBeDamaged(dmg);
            if (isKill)
            {
                enemy.Die();
            }
        }
        else
        {
            //forHP 生命傷害*0.5 for armor 若護甲傷害大於護甲值，則擊破 擊破傷害:護甲傷害*擊破傷害倍率
            bool isKill = enemy.HpBeDamaged(dmg * 0.5f);
            if(isKill){
            enemy.Die();
            }
            
            if (isCritical)
            {
                DamageTextManager.instance.ShowCriticalDamage(dmg * 0.5f, enemy.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
            }
            else
            {
                DamageTextManager.instance.ShowHpDamage(dmg*0.5f, enemy.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
            }

            float armorDmg = RoundDataManager.Out_PlayerData.armorDmgData.Dmg + RoundDataManager.In_GameData.armorDmgData.Dmg;
            if(armorDmg>0){
                float remain = enemy.ArmorBeDamaged(armorDmg);
                DamageTextManager.instance.ShowArmorDamage(armorDmg, enemy.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
                if (remain <= 0)
                {
                    enemy.ShowCrash();
                    bool isCrashKill = enemy.HpBeDamaged(armorDmg * RoundDataManager.PlayerCrashPara+damageDTO.ArmorDmgDataWithAllRatio.OnCrash);
                    if (isCrashKill)
                    {
                        DamageTextManager.instance.ShowHpDamage(armorDmg * RoundDataManager.PlayerCrashPara+damageDTO.ArmorDmgDataWithAllRatio.OnCrash, enemy.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
                        enemy.Die();
                    }
                }
            }
        }
    }

    private static void DamageToPlayer(HpSystem player,string enemyKeyString,DamageDTO damageDTO)
    {
        if (player.CurrentArmor == 0)
        {
            float dmg = RoundDataManager.EnemyDamage[enemyKeyString + "_hpDamage"];
            bool isKill = player.HpBeDamaged(dmg);
            DamageTextManager.instance.ShowHpDamage(dmg, player.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
            if (isKill)
            {
                player.Die();
            }
        }
        else
        {
            float hpdmg = RoundDataManager.EnemyDamage[enemyKeyString + "_hpDamage"] * (1 - RoundDataManager.Out_PlayerData.hpdata.ArmorResistance);
            bool isKill = player.HpBeDamaged(hpdmg);
            DamageTextManager.instance.ShowPlayerBeDamage(hpdmg, player.ShowDmgNumPoint,new Vector2(randomRange,randomRange));

            float armordmg = RoundDataManager.EnemyDamage[enemyKeyString + "_armorDamage"];
            player.ArmorBeDamaged(armordmg);
            DamageTextManager.instance.ShowArmorDamage(armordmg, player.ShowDmgNumPoint,new Vector2(randomRange,randomRange));
            
        }
    }
}

public struct DamageDTO{
    public HpDmgData HpDmgDataWithAllRatio;
    public ArmorDmgData ArmorDmgDataWithAllRatio;
    public DroneProjectileType DroneProjectileType;
    public DamageDTO(HpDmgData hpDmgData,ArmorDmgData armorDmgData,DroneProjectileType droneProjectileType){
        HpDmgDataWithAllRatio = hpDmgData;
        ArmorDmgDataWithAllRatio = armorDmgData;
        DroneProjectileType = droneProjectileType;
    }
}
