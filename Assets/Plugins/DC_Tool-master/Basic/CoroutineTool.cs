using System;
using System.Collections;
using UnityEngine;

namespace DC_Tool.Basic
{
    public class CoroutineTool : MonoSingleton<CoroutineTool>
    {
        public void KillCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        public Coroutine CallInFrames(int frames, Action callback)
        {
            return StartCoroutine(DelayFrames(frames, callback));
        }

        private static IEnumerator DelayFrames(int frames, Action callback)
        {
            while (frames > 0)
            {
                frames--;
                yield return null;
            }
            callback?.Invoke();
        }

        public Coroutine CallInSeconds(float seconds, Action callback)
        {
            return StartCoroutine(DelaySeconds(seconds, callback));
        }

        private static IEnumerator DelaySeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }

        public Coroutine CallInEndOfFrame(Action callback)
        {
            return StartCoroutine(WaitForEndOfFrame(callback));
        }

        private static IEnumerator WaitForEndOfFrame(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }
    }
}
