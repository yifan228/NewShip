using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlueDepressionBOss : ABoss
{
    List<BossAttackPattern> attackPatterns;
    [SerializeField]CollideTriggerTool leftScythe;
    [SerializeField]CollideTriggerTool rightScythe;
    [SerializeField] Transform leftScythOriginTransform;
    [SerializeField] Transform rightScythOriginTransform;
    [SerializeField] ParticleSystem attackRange;
    private bool isTrashTalking;

    bool alreadyAddRotatePattern = false;

    public override void ActivateBoss()
    {
        gameObject.SetActive(true);
        currentHp = totalHp;
        leftScythe.Initialize(OnScytheHitPlayer, TagsEnum.Player);
        rightScythe.Initialize(OnScytheHitPlayer, TagsEnum.Player);

        ChangeState(BossBattleState.Battle);
    }
    [ContextMenu("test")]
    public void secondStep()
    {
        currentHp = (totalHp / 2) - 0.1f;
    }
    void OnScytheHitPlayer()
    {
        
    }
    public override void DrawAttackRange(Vector2 origin, Vector2 direction)
    {
        attackRange.transform.position = direction;
        attackRange.Play();
    }
    private void Start()
    {
        attackPatterns = new List<BossAttackPattern>() { new ThrowScythePattern(leftScythe,rightScythe,leftScythOriginTransform,rightScythOriginTransform),new SwingScythePattern(leftScythe, rightScythe) };

        SetPattern(attackPatterns[0]);
        ActivateBoss();
    }
    private void Update()
    {
        if (isDefeated)
        {
            return;
        }
        if (state == BossBattleState.Battle)
        {
            if (currentPattern.RunningPattern(Time.deltaTime, this))
            {
                if (currentHp>totalHp/2)
                {
                    int rngnum = UnityEngine.Random.Range(0, 3);
                    if (rngnum ==2)
                    {
                        SetPattern(attackPatterns[1]);
                    }
                    else
                    {
                        SetPattern(attackPatterns[0]);
                    }
                }
                else
                {
                    if (!alreadyAddRotatePattern)
                    {
                        attackPatterns.Add(new BossRotatingAttack(leftScythe.transform, rightScythe.transform));
                        alreadyAddRotatePattern = true;
                    }
                    int rngnum = UnityEngine.Random.Range(0, 3);
                    if (rngnum == 0)
                    {
                        SetPattern(attackPatterns[2]);
                    }
                    else if(rngnum == 1)
                    {
                        SetPattern(attackPatterns[0]);
                    }else
                    {
                        SetPattern(attackPatterns[1]);
                    }
                }
            }
        }
        else if (state == BossBattleState.TrashTalk)
        {
            if (isTrashTalking)
            {
                return;
            }
            isTrashTalking = true;
        }
        else if (state == BossBattleState.Idle)
        {

        }
    }

    public override void GetHurt(float damage)
    {
        if (state == BossBattleState.Battle)
        {
            currentHp -= damage;
            SetHpUI();
            if (currentHp < 0)
            {
                //¥ý³o¼Ë
                Die();
            }
        }
    }
    private void Die()
    {
        isDefeated = true;
        OnDie();
    }
}

public class ThrowScythePattern : BossAttackPattern
{
    float timer;
    Rigidbody2D leftScythe;
    Rigidbody2D rightScythe;
    Vector2 targetPosleft;
    Vector2 targetPosright;
    float speed =20;
    throwtype leftScytheType;
    throwtype rightScytheType;
    Transform leftScythOriginTransform;
    Transform rightScythOriginTransform;
    float maxW = 720;
    public ThrowScythePattern(CollideTriggerTool leftScythe, CollideTriggerTool rightScythe, Transform leftScythOriginTransform,Transform rightScythOriginTransform)
    {
        this.leftScythe = leftScythe.GetComponent<Rigidbody2D>();
        this.rightScythe = rightScythe.GetComponent<Rigidbody2D>();
        this.leftScythOriginTransform = leftScythOriginTransform;
        this.rightScythOriginTransform = rightScythOriginTransform;
    }
    public override void Reset()
    {
        timer = 0;
        leftScytheType = throwtype.GotoTarget;
        rightScytheType = throwtype.GotoTarget;
        isDrawnLeftRange = false;
        isDrawnRightRange = false;
    }
    float startAttackTimeleft = 1;//test
    float startAttackTimeright = 2f;//test

    bool isDrawnLeftRange;
    bool isDrawnRightRange;
    public override bool RunningPattern(float deltatime, ABoss thisBoss)
    {
        if (timer> startAttackTimeleft)
        {
            leftScytheType = ScytheFlying(leftScytheType, leftScythe, targetPosleft, thisBoss,"L");
        }
        else
        {
            if (!isDrawnLeftRange)
            {
                targetPosleft = target.position;
                Debug.Log("left" + targetPosleft);
                thisBoss.DrawAttackRange(Vector2.zero, targetPosleft);
                isDrawnLeftRange = true;
            }
        }

        if (timer>startAttackTimeright)
        {
            rightScytheType = ScytheFlying(rightScytheType, rightScythe, targetPosright, thisBoss,"R");
        }
        else if(timer>startAttackTimeleft)
        {
            if (!isDrawnRightRange)
            {
                targetPosright = target.position;
                thisBoss.DrawAttackRange(Vector2.zero, targetPosright);
                isDrawnRightRange = true;
            }
        }

        timer+=Time.deltaTime;

        if (leftScytheType == throwtype.IsDone && rightScytheType == throwtype.IsDone)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private throwtype ScytheFlying(throwtype currentScytheType,Rigidbody2D scythe,Vector3 targetPos,ABoss thisBoss,string LorR)
    {
        switch (currentScytheType)
        {
            case throwtype.GotoTarget:
                
                if (Mathf.Abs(scythe.angularVelocity)< maxW)
                {
                    if (LorR == "L")
                    {
                        scythe.angularVelocity += maxW * Time.deltaTime ;
                    }
                    else
                    {
                        scythe.angularVelocity += -maxW * Time.deltaTime;
                    }
                }

                Vector2 R =  targetPos - scythe.transform.position;
                if (R.magnitude<0.2f)
                {
                    return throwtype.BackToBoss;
                }
                else
                {
                    scythe.transform.position = (Vector2)scythe.transform.position+ R.normalized*speed*Time.deltaTime;
                }
                break;
            case throwtype.BackToBoss:
                Vector2 R2;
                if (LorR == "L")
                {
                   R2 = leftScythOriginTransform.position - scythe.transform.position;
                }
                else
                {
                    R2 = rightScythOriginTransform.position - scythe.transform.position;
                }
                if (R2.magnitude < 0.15f)
                {
                    if (LorR == "L")
                    {
                        scythe.transform.position = leftScythOriginTransform.position;
                    }
                    else
                    {
                        scythe.transform.position = rightScythOriginTransform.position;
                    }
                    return throwtype.IsBackToBoss;
                }
                else
                {
                    scythe.transform.position = (Vector2)scythe.transform.position+ R2.normalized * speed * Time.deltaTime;
                }
                break;
            case throwtype.IsBackToBoss:
                if (Mathf.Abs(scythe.angularVelocity)>480)
                {
                    scythe.angularVelocity -= 720 * Time.deltaTime * scythe.angularVelocity / Mathf.Abs(scythe.angularVelocity);
                }
                else
                {
                    if (Mathf.Abs(scythe.transform.localRotation.eulerAngles.z-180f)>170f )
                    {
                        scythe.transform.localRotation = Quaternion.identity;
                        scythe.angularVelocity = 0;
                        return throwtype.IsDone;
                    }
                }
                break;
        }
        return currentScytheType;

    }
    enum throwtype
    {
        GotoTarget,
        BackToBoss,
        IsBackToBoss,
        IsDone
    }
}
public class SwingScythePattern : BossAttackPattern
{
    Rigidbody2D leftScythe;
    Rigidbody2D rightScythe;
    Vector2 targetPos;
    float lookAtAngle;
    bool isTriggered = false;
    bool isEnd;
    public SwingScythePattern(CollideTriggerTool leftscythe, CollideTriggerTool rightscythe)
    {
        this.leftScythe = leftscythe.GetComponent<Rigidbody2D>();
        this.rightScythe = rightscythe.GetComponent<Rigidbody2D>();

    }
    public override void Reset()
    {
        isTriggered = false;
        isEnd = false;
    }
    float minBossAndTargetDistance = 3.5f;
    public override bool RunningPattern(float deltatime, ABoss thisBoss)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            targetPos = target.position + minBossAndTargetDistance * (target.position - thisBoss.transform.position).normalized;
            Sequence s = DOTween.Sequence();
            s.Append(thisBoss.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.75f)).AppendCallback(() => LookAt2D(thisBoss.transform.position))
                .Append(thisBoss.GetComponent<Transform>().DORotate(new Vector3(0, 0, lookAtAngle), 0.25f))
                .Append(thisBoss.GetComponent<Transform>().DOMove(targetPos, 0.5f)).Join(leftScythe.transform.DOLocalRotate(new Vector3(0, 0, 90), 0.5f)).Join(rightScythe.transform.DOLocalRotate(new Vector3(0, 0, -90), 0.5f)).
                Append(leftScythe.transform.DOLocalRotate(new Vector3(0, 0, 180), 0.15f)).Join(rightScythe.transform.DOLocalRotate(new Vector3(0, 0, -180), 0.15f)).
                AppendInterval(0.5f).
                Append(leftScythe.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.25f)).Join(rightScythe.transform.DOLocalRotate(new Vector3(0, 0, -360), 0.25f)).
                AppendCallback(()=>thisBoss.GetComponent<SpriteRenderer>().color = Color.white).AppendCallback(()=>isEnd = true);
        }

        return isEnd;
    }
    void LookAt2D(Vector2 bossPos)
    {
        Vector2 direction = -(targetPos - bossPos);

        lookAtAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}

public class BossRotatingAttack : BossAttackPattern
{
    List<Vector2> points;
    float speed=12;
    float angularSpeed =720;
    Transform leftScythe;
    Transform rightScythe;
    public BossRotatingAttack(Transform leftScythe,Transform rightScythe)
    {
        this.leftScythe = leftScythe;
        this.rightScythe = rightScythe;
    }
    state updatingState = state.opening ;
    enum state
    {
        opening,
        moving,
        ending,
        onend,
    }
    public override void Reset()
    {
        points = new List<Vector2> { new Vector2(10, 19), new Vector2(-10, 19), new Vector2(10, -12), new Vector2(-10, -12) };
        updatingState = state.opening;
        isTriggeredEnding = false;
        isTriggeredMove = false;
        isTriggeredPrepatetoratate = false;
    }

    public override bool RunningPattern(float deltatime, ABoss thisBoss)
    {
        switch (updatingState)
        {
            case state.opening:
                if (!isTriggeredPrepatetoratate)
                {
                    prepateToRotate();
                }
                return false;
            case state.moving:
                if (!isTriggeredMove)
                {
                    int rngnum = UnityEngine.Random.Range(0, points.Count);
                    float movetime = Vector2.Distance(points[rngnum], boss.transform.position) / speed;
                    Move(rngnum, movetime);
                }
                return false;
            case state.ending:
                if (!isTriggeredEnding)
                {
                    Ending();
                }
                return false;
            case state.onend:
                return true;
        }
        return false;
    }

    bool isTriggeredPrepatetoratate;
    private void prepateToRotate()
    {
        isTriggeredPrepatetoratate = true;
        leftScythe.transform.DOLocalRotate(new Vector3(0, 0, 90), 0.5f);
        rightScythe.transform.DOLocalRotate(new Vector3(0, 0, -90), 0.5f).OnComplete(()=>updatingState = state.moving);
    }
    bool isTriggeredMove;
    private void Move(int index,float movetime)
    {
        isTriggeredMove = true;
        boss.GetComponent<Rigidbody2D>().angularVelocity = angularSpeed;
        boss.transform.DOMove(points[index], movetime).OnComplete(()=>CompleteOneMove(index));
    }
    private void CompleteOneMove(int completeindex)
    {
        points.RemoveAt(completeindex);
        if (points.Count > 0)
        {
            int rngnum = UnityEngine.Random.Range(0, points.Count);
            float movetime = Vector2.Distance(points[rngnum], boss.transform.position) / speed;
            Move(rngnum, movetime);
        }
        else
        {
            updatingState = state.ending;
        }
    }

    bool isTriggeredEnding;
    private void Ending()
    {
        isTriggeredEnding = true;
        boss.GetComponent<Rigidbody2D>().angularVelocity = 0;
        Sequence s = DOTween.Sequence();
        s.Append(boss.transform.DORotate(new Vector3(0, 0, 360f), boss.transform.rotation.eulerAngles.z / angularSpeed)).
            Append(leftScythe.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.25f)).Join(rightScythe.transform.DOLocalRotate(new Vector3(0, 0, -360), 0.25f)).
            AppendCallback(() => updatingState = state.onend);
    }
}
