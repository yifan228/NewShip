namespace DC_Tool.Basic
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static readonly object lockObj = new object();

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                            instance.Initialize();
                        }
                    }
                }
                return instance;
            }
        }

        protected Singleton()
        {
        }

        protected virtual void Initialize()
        {
        }
    }
}
