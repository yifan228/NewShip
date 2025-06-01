using DG.Tweening;
using UnityEngine;

public class SimpleTweenAnimation : MonoBehaviour
{
    public bool useStartToStart;
    [SerializeField] SimpleTweenType type;
    [SerializeField] Vector3 shakeEndValue;
    [SerializeField] float duration;
    private void Start()
    {
        if (useStartToStart)
        {
            switch (type)
            {
                case SimpleTweenType.Shake:
                    transform.DOLocalRotate(shakeEndValue, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    break;
                case SimpleTweenType.Rotate:
                    transform.DORotate(new Vector3(0,0,-360), duration,RotateMode.FastBeyond360).SetSpeedBased(true).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                    break;
                case SimpleTweenType.Pump:
                    transform.DOScale(Vector2.one * 1.25f, duration)
                    .SetLoops(-1, LoopType.Yoyo) // Loops infinitely, making it pump back and forth.
                    .SetEase(Ease.InOutSine);
                    break;
                default:
                    break;
            }
        }
    }

    public void StartAnim()
    {
        switch (type)
        {
            case SimpleTweenType.Shake:
                transform.DOLocalRotate(shakeEndValue, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                break;
            case SimpleTweenType.Rotate:
                transform.DORotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360).SetSpeedBased(true).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                break;
            case SimpleTweenType.Pump:
                transform.DOScale(Vector2.one * 1.25f, duration)
                .SetLoops(-1, LoopType.Yoyo) // Loops infinitely, making it pump back and forth.
                .SetEase(Ease.InOutSine);
                break;
            default:
                break;
        }
    }
}
public enum SimpleTweenType
{
    Rotate,
    Shake,
    Pump
}
