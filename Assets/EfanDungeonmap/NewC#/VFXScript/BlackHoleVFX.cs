using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class BlackHoleVFX : VFXBase
{
    [SerializeField] ParticleSystem blackHoleParticle;
    // public override void SetEndDelegate(Action onEnd)
    // {
    //     Debug.Log("�ثe black hole vfx �S�� onEnd");
    // }

    // public override void Play()
    // {
    //     blackHoleParticle.Play();
    // }

    // public override void SetDuration(float duration)
    // {
    //     if (duration > 0)
    //     {
    //         StartCoroutine(Timer(duration,End));
    //     }
    // }

    // void End()
    // {
    //     blackHoleParticle.Stop();
    // }

    // IEnumerator Timer(float timer,Action EndTimeCallback)
    // {
    //     yield return new WaitForSeconds(timer);
    //     EndTimeCallback();
    // }

    // public override void SetPlayingDelegate(Func<float> onPlaying)
    // {
        
    // }
}
