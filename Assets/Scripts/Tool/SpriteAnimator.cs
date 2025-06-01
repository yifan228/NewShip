using UnityEngine;
using UnityEngine.U2D;
using System;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteAtlas spriteAtlas;
    [SerializeField] private string spritePrefix;
    [SerializeField] private int spriteCount;
    [SerializeField] private int spriteAnimFPS;
    [SerializeField]public bool IsLoop = false;

    private bool isPlaying = false;
    private int currentSpriteIndex = 0;
    private float spriteAnimTimer = 0f;
    private Action onEndCallback;
    private float playDuration = 0f;
    private float playTimer = 0f;
    private bool useDuration = false;
    public float OneSpriteLifeTime{
        get{
            return spriteCount / spriteAnimFPS;
        }
    }
    public void Play(float duration, Action onEnd)
    {
        playDuration = duration;
        playTimer = 0f;
        onEndCallback = onEnd;
        isPlaying = true;
        currentSpriteIndex = 0;
        spriteAnimTimer = 0f;

        useDuration = !this.IsLoop && duration > 0f;
        spriteRenderer.enabled = true;
        SetSpriteByIndex(0);
    }

    public void OnUpdate()
    {
        if (!isPlaying) return;

        playTimer += Time.deltaTime;
        float frameTime = 1f / Mathf.Max(1, spriteAnimFPS);
        spriteAnimTimer += Time.deltaTime;

        while (spriteAnimTimer >= frameTime)
        {
            spriteAnimTimer -= frameTime;
            currentSpriteIndex++;
            if (currentSpriteIndex >= spriteCount)
            {
                if (this.IsLoop)
                {
                    currentSpriteIndex = 0;
                }
                else if (useDuration)
                {
                    currentSpriteIndex = spriteCount - 1;
                }
                else
                {
                    EndAnimation();
                    return;
                }
            }
            SetSpriteByIndex(currentSpriteIndex);
        }

        if (useDuration && playTimer >= playDuration)
        {
            EndAnimation();
        }
    }

    private void SetSpriteByIndex(int idx)
    {
        if (spriteRenderer == null || spriteAtlas == null || spriteCount <= 0) return;
        string spriteName = spritePrefix + idx;
        Sprite sp = spriteAtlas.GetSprite(spriteName);
        if (sp != null)
            spriteRenderer.sprite = sp;
    }

    private void EndAnimation()
    {
        isPlaying = false;
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        onEndCallback?.Invoke();
    }

    public void Reset()
    {
        isPlaying = false;
        playTimer = 0f;
        playDuration = 0f;
        onEndCallback = null;
        currentSpriteIndex = 0;
        spriteAnimTimer = 0f;
        useDuration = false;
        IsLoop = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            spriteRenderer.sprite = null;
        }
    }
} 