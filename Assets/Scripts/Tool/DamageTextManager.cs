using TMPro;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using System.Threading.Tasks;
using DG.Tweening;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;
    [SerializeField] DamageNumber hpDmg;
    [SerializeField] DamageNumber armorDmg;
    [SerializeField] DamageNumber playerDmg;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowCriticalDamage(float damage,Vector2 position,Vector2 randomRange=default)
    {
        if(randomRange==default){
            hpDmg.Spawn(position, damage);
        }
        else{
            Vector2 randomPos = new Vector2(Random.Range(-randomRange.x, randomRange.x), Random.Range(-randomRange.y, randomRange.y));
            hpDmg.Spawn(position+randomPos, damage);
        }
    }

    public void ShowHpDamage(float damage,Vector2 position,Vector2 randomRange=default)
    {
        if(randomRange==default){
            hpDmg.Spawn(position, damage);
        }
        else{
            Vector2 randomPos = new Vector2(Random.Range(-randomRange.x, randomRange.x), Random.Range(-randomRange.y, randomRange.y));
            hpDmg.Spawn(position+randomPos, damage);
        }
    }

    public void ShowArmorDamage(float damage, Vector2 position,Vector2 randomRange=default)
    {
        if(randomRange==default){
            armorDmg.Spawn(position, damage);
        }
        else{
            Vector2 randomPos = new Vector2(Random.Range(-randomRange.x, randomRange.x), Random.Range(-randomRange.y, randomRange.y));
            armorDmg.Spawn(position+randomPos, damage);
        }
    }

    public void ShowPlayerBeDamage(float damage, Vector2 position,Vector2 randomRange=default)
    {
        if(randomRange==default){
            playerDmg.Spawn(position, damage);
        }
        else{
            Vector2 randomPos = new Vector2(Random.Range(-randomRange.x, randomRange.x), Random.Range(-randomRange.y, randomRange.y));
            playerDmg.Spawn(position+randomPos, damage);
        }
    }
}
