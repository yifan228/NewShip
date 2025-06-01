using System;
using UnityEngine;
using UnityEngine.U2D;
using System.Collections;

/// <summary>
/// vfx size1是10
/// </summary>
public class VFXBase : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] SpriteAnimator spriteAnimator;
    [SerializeField] VFXType vfxType;
    [SerializeField] float realRangeDevidedSize=10f;
    [SerializeField][Tooltip("傷害範圍")] public Collider2D DamageRangeCollider;

    public float RealRangeDevidedSize{
        get{
            return realRangeDevidedSize;
        }
    }

    private bool isPlaying = false;
    private float playTimer = 0f;
    private float playDuration = 0f;
    private Action<VFXBase> onEndCallback;
    private Action<float> onPlayingCallback;
    private bool isLoop = false;

/// <summary>
/// 撥放一次的時間，通常爆炸特校用
/// </summary>
    public float OneVFXLifeTime{
        get{
            switch (vfxType)
            {
                case VFXType.Particle:
                    return particle.main.duration;
                case VFXType.Sprite:
                    return spriteAnimator != null ? spriteAnimator.OneSpriteLifeTime : 0f;
                default:
                    return 0f;
            }
        }
    }
    /// <summary>
    /// 撥放特效
    /// </summary>
    /// <param name="duration">特效持續時間 if <=0 則使用prefav預設時間</param>
    /// <param name="onEnd">特效結束時的回調</param>
    /// <param name="onPlaying">特效播放過程中的回調</param>
    /// <param name="isLoop">是否循環播放</param>
    public void Play(float duration, Action<VFXBase> onEnd, Action<float> onPlaying, int isLoop = 0)
    {
        if(duration <= 0f)
        {
            switch (vfxType)
            {
                case VFXType.Particle:
                    playDuration = particle.main.duration;
                    //Debug.Log($"playDuration:{playDuration}");
                    break;
                case VFXType.Sprite:
                    playDuration = spriteAnimator != null ? spriteAnimator.OneSpriteLifeTime : 0f;
                    break;
            }
        }else{
            playDuration = duration;
        }
        playTimer = 0f;
        onEndCallback = onEnd;
        onPlayingCallback = onPlaying;
        isPlaying = true;

        if (isLoop == 1)
        {
            this.isLoop = true;
        }
        else if (isLoop == -1)
        {
            this.isLoop = false;
        }
        else if (isLoop == 0)
        {
            switch (vfxType)
            {
                case VFXType.Particle:
                    this.isLoop = particle.main.loop;
                    break;
                case VFXType.Sprite:
                    this.isLoop = spriteAnimator.IsLoop; // 預設不循環
                    break;
            }
        }

        switch (vfxType)
        {
            case VFXType.Particle:
                PlayParticle();
                break;
            case VFXType.Sprite:
                PlaySpriteAnimation();
                break;
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        playTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(playTimer / playDuration);
        onPlayingCallback?.Invoke(progress);

        switch (vfxType)
        {
            case VFXType.Particle:
                UpdateParticle();
                break;
            case VFXType.Sprite:
                spriteAnimator.OnUpdate();  
                break;
        }
    }

    private void PlayParticle()
    {
        if (particle == null) return;
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = particle.main;
        main.loop = isLoop;
        particle.Play();
    }

    private void UpdateParticle()
    {
        if (playTimer >= playDuration)
        {
            particle.Stop();
            EndVFX();
        }
    }

    private void PlaySpriteAnimation()
    {
        if (spriteAnimator == null) return;
        spriteAnimator.Play(playDuration, () => EndVFX());
    }

    private void EndVFX()
    {
        isPlaying = false;
        onEndCallback?.Invoke(this);
    }

    public void Reset()
    {
        isPlaying = false;
        playTimer = 0f;
        playDuration = 0f;
        onEndCallback = null;
        onPlayingCallback = null;
        isLoop = false;

        if (particle != null)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (spriteAnimator != null)
        {
            spriteAnimator.Reset();
        }
    }
}

public enum VFXType
{
    Particle,
    Sprite
}
