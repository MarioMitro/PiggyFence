using UnityEngine;

namespace PiggyFence
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T innerInstance;

        public static T instance
        {
            get
            {
                CreateSingletonInstance();
                return innerInstance;
            }
        }

        public static void CreateSingletonInstance()
        {
            if (!innerInstance)
                innerInstance = FindObjectOfType<T>();
        }
    }
}