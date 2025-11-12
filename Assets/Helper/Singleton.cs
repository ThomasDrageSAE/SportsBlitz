using UnityEngine;

namespace Helper.Blake
{
    /// <summary>
    /// Creates a singleton in the scene when called to allow for public static access to 
    /// the attached functions
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        // INFO: Set the instance
        public static T Instance => CreateSingletonInstance();

        // private void OnDestroy() => instance = null;
        

        protected static T CreateSingletonInstance()
        {
            if (instance != null) return instance;
            instance = FindFirstObjectByType<T>();

            if (instance != null) return instance;
            GameObject singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
            instance = singletonObject.AddComponent<T>();

            // TODO: Fix errors
            GameObject singletonObjectParent = GameObject.Find("Singletons") ?? new GameObject("Singletons");
            singletonObject.transform.SetParent(singletonObjectParent.transform);

            return instance;
        }
    }
    
}
