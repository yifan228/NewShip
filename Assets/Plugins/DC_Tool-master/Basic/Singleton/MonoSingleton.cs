using UnityEngine;

namespace DC_Tool.Basic
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static readonly object lockObj = new object();

        private static T instance;
        public static T Instance
        {
            get
            {
                if (isQuitting)
                {
                    return null;
                }

                if (isDestroyed)
                {
                    DebugTool.Info($"Instance '{typeof(T)}' is destroyed");
                    return null;
                }

                lock (lockObj)
                {
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("Singleton - " + typeof(T).ToString());
                        instance = obj.AddComponent<T>();
                        instance.Initialize();
                        DontDestroyOnLoad(obj);
                    }
                }

                return instance;
            }
        }

        private static bool isDestroyed;
        private static bool isQuitting;

        void OnDestroy()
        {
            isDestroyed = true;
        }

        void OnApplicationQuit()
        {
            isQuitting = true;
        }

        protected virtual void Initialize()
        {
        }
    }
}
