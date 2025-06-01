using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[Obsolete]
public class BulleteView : MonoBehaviour
{
    private TagsEnum owner;

    [SerializeField] Rigidbody2D rbody2D;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] CircleCollider2D detectCollider;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] ParticleSystem laserGun;
    [SerializeField] ParticleCollision laserParticleCollision;
    [SerializeField] SpriteAtlas bulleteSpriteAtlas;
    [SerializeField] List<ParticleSystem> displayFXs;
    ParticleSystem currentDisplayFx;
    //[SerializeField] Light2D light2d;
    private ABulleteAction bulleteActions;
    private ContactFilter2D contactFilter2D;
    private BulleteBehaviourData moveDatas;
    float ATKBeforeCriticalCalculate;
    private bool canMove;
    private int maxCollideEnemyAmount;
    private string currentExplodefxName;

    public void Initialize(TagsEnum owner,CalculateDamageStrategy strategy, DTOBulleteData bullete,float damage, Vector3 gunPoint, Vector2 toward)
    {
        this.owner = owner;
        gameObject.SetActive(true);
        gameObject.transform.position = gunPoint;
        gameObject.transform.right = toward;
        ATKBeforeCriticalCalculate = damage;
        maxCollideEnemyAmount = bullete.DTOMaxDamagedEnemyAmount;
        detectCollider.radius = bullete.Radius;
        contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(layerMask);
        currentExplodefxName = bullete.ExplodeFxName;
        moveDatas = new BulleteBehaviourData()
        {
            Type = bullete.BehaviourType,
            InitialSpeed = bullete.DTOSpeed,
            Filter = contactFilter2D,
            DetectCollider = detectCollider,
            Toward = toward,
            Owner = this.owner,
        };
        bulleteActions = BuildBulleteActions(bullete.BehaviourType, strategy);
        if (bulleteActions is null)
        {
            Debug.LogError("bullete action is null");
        }
        SetSettingsByBulleteType(bullete);
    }

    private void SetSettingsByBulleteType(DTOBulleteData bullete)
    {
        switch (bullete.DisplayType)
        {
            case BulleteType.Laser:
                canMove = false;
                spriteRenderer.enabled = false;
                laserParticleCollision.ParticleOncollision = OnLaserBulleteParticleCollision;
                laserGun.Play();
                Debug.Log("laser gun is emmit"+laserGun.isEmitting);
                break;
            case BulleteType.ParticleSystemBullete:
                currentDisplayFx = displayFXs.Find(x => x.name == bullete.BulleteDisplayFXName);
                if (currentDisplayFx is null)
                {
                    Debug.Log("bullete.BulleteDisplayFXName is no :" + bullete.BulleteDisplayFXName);
                }
                currentDisplayFx.gameObject.SetActive(true);
                currentDisplayFx.Play();
                canMove = true;
                break;
            default:
                canMove = true;
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = bulleteSpriteAtlas.GetSprite(bullete.BulleteDisplayFXName);
                //light2d.lightCookieSprite = bullete.sprite;
                break;
        }
    }

    private ABulleteAction BuildBulleteActions(BulleteBehaviourType behaviourType, CalculateDamageStrategy strategy)
    {
        ABulleteAction bulleteActions = SimpleMoveActionFactory(behaviourType);
        bulleteActions.SetCalculateDamageStrategy(strategy);
        return bulleteActions;
    }


    private ABulleteAction SimpleMoveActionFactory(BulleteBehaviourType type)
    {
        switch (type)
        {
            case BulleteBehaviourType.BossDefault:
                return new BossDefaultBulltetAction(moveDatas);
            default:
                return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner == TagsEnum.Player)
        {
            bulleteActions.Trigger(collision, rbody2D, PlayExplodeFXAndReturnToPool, ref maxCollideEnemyAmount, DoDamageToEnemyOrBoss,CloseDisPlay);
        }
        else if(owner == TagsEnum.Boss)
        {
            bulleteActions.Trigger(collision, rbody2D, PlayExplodeFXAndReturnToPool, ref maxCollideEnemyAmount, BossDoDamageToPlayer, CloseDisPlay);
        }
        //else if (collision.transform.CompareTag(TagsEnum.BulleteCanInteractObstacle.ToString()))
        //{
        //    collision.transform.GetComponent<BulleteInteractObstacle>().Interact();
        //    CloseDisPlay();
        //    BulletePool.BulleteReturn(this);
        //}
    }

    private void DoDamageToEnemyOrBoss(TagsEnum targetTag,GameObject damageTarget, Vector2 triggerPoint)
    {
        if (targetTag == TagsEnum.Enemy)
        {
            EnemyView enemy = damageTarget.GetComponent<EnemyView>();

            float realdamage = bulleteActions.DamageStrategy.Calculate(ATKBeforeCriticalCalculate);
            bulleteActions.DamageStrategy.ShowDamage(realdamage, triggerPoint +UnityEngine.Random.insideUnitCircle * 0.8f);
            enemy.BeAttacked(new DamageStruct() { Damage = realdamage },TagsEnum.Player.ToString(),AttackedType.Hit);
        }
        else if (targetTag == TagsEnum.Boss)
        {
            ABoss boss = damageTarget.GetComponent<ABoss>();
            float realdamage = bulleteActions.DamageStrategy.Calculate(ATKBeforeCriticalCalculate);
            bulleteActions.DamageStrategy.ShowDamage(realdamage, triggerPoint + UnityEngine.Random.insideUnitCircle * 0.8f);
            boss.GetHurt(realdamage);
        }
    }

    private void BossDoDamageToPlayer(TagsEnum notused,GameObject damageTarget, Vector2 triggerPoint)
    {
        //Mayzi mayzi = damageTarget.GetComponent<Mayzi>();
        //mayzi.Hurt(ATKBeforeCriticalCalculate);
    }

    public void CloseDisPlay()
    {
        canMove = false;
        rbody2D.velocity = Vector2.zero;
        rbody2D.angularVelocity = 0;
        spriteRenderer.enabled = false;
    }

    private void OnLaserBulleteParticleCollision(GameObject collisionObject,Vector2 intersecPoint)
    {
        if (collisionObject.CompareTag(TagsEnum.Enemy.ToString()))
        {
            DoDamageToEnemyOrBoss(TagsEnum.Enemy,collisionObject, intersecPoint) ;
        }
        CloseDisPlay();
        BulletePool.BulleteReturn(this);
    }

    float tickTime;
    private void Update()
    {
        if (canMove)
        {
            bulleteActions.Move(rbody2D);
        }
    }

    private void PlayExplodeFXAndReturnToPool(Collider2D target)
    {
        if (currentDisplayFx !=null)
        {
            currentDisplayFx.gameObject.SetActive(false);
        }
        CrashEffectManager.Instance.PlayEffect(transform.position, currentExplodefxName);
        BulletePool.BulleteReturn(this);
    }

    //private IEnumerator ExplodeFX(string fxName)
    //{
    //    CrashEffectManager.Instance.PlayEffect(transform.position, fxName);
    //    ParticleSystem fx = explosionFXs.Find(x => x.name == fxName);
    //    fx.Play();
    //    float lifeTime =fx.main.startLifetime.constant;
    //    yield return new WaitForSeconds(lifeTime);
    //    BulletePool.BulleteReturn(this);
    //    yield break;
    //}
}
