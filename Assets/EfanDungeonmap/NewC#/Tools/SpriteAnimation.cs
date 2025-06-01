using UnityEngine.U2D;
using UnityEngine;
using System;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField]SpriteRenderer spRendere;
    [SerializeField] SpriteAtlas atlas;
    [SerializeField] string spritepref;
    [SerializeField] int frameRate;
    [SerializeField] SimpleTweenAnimation simpleTweenAnimation;
    AnimationType type;
    private bool loop;
    Action singleLoopEndCallback;
    int currentIndex= 0;
    int tick =1;
    bool noUseUpdate;
    bool isPlaying;
    void Update()
    {
        if (noUseUpdate)
        {
            return;
        }

        if (!isPlaying)
        {
            return;
        }
        if (tick == frameRate)
        {
            PlayAnim();
            if (loop)
            {
                isPlaying = true;
            }
            else
            {
                isPlaying = false;
                singleLoopEndCallback?.Invoke();
            }
        }
        tick++;
    }

    private void PlayAnim()
    {
        spRendere.sprite = atlas.GetSprite($"{spritepref}" + currentIndex);
        currentIndex++;
        if (currentIndex >= atlas.spriteCount)
        {
            currentIndex = 0;
        }
        tick = 0;
    }

    public void Play(bool isloop,Action onSingleLoopEnd = null)
    {
        switch (type)
        {
            case AnimationType.SpriteAnim:
                if (atlas == null)
                {
                    Debug.LogWarning("no atlas");
                    return;
                }
                singleLoopEndCallback = onSingleLoopEnd;
                loop = isloop;
                isPlaying = true;
                break;
            case AnimationType.DotweenAnim:
                noUseUpdate = true;
                simpleTweenAnimation.StartAnim();
                break;
            default:
                break;
        }
    }

    [ContextMenu("testPlause")]
    public void Puase()
    {
        isPlaying = false;
    }

    public void ResetSatus()
    {
        isPlaying = false;
        currentIndex = 0;
        tick = 1;
    }

    public void InitializeAnimationData(SpriteAnimatinData data,SpriteRenderer sprendere)
    {
        atlas = data.atlas;
        spritepref = data.spritepref;
        frameRate = data.frameRate;
        spRendere = sprendere;
        type = data.type;
    }
}

public enum AnimationType
{
    SpriteAnim,
    DotweenAnim
}

[Serializable]
public class SpriteAnimatinData
{
    public AnimationType type;
    public SpriteAtlas atlas;
    public string spritepref;
    public int frameRate;
}
