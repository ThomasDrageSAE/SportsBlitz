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
        private static bool applicationIsQuitting = false;

        // INFO: Set the instance
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting) return null;
                return CreateSingletonInstance();
            }
        }

        private void OnDestroy() => instance = null;

        private void OnApplicationQuit() => applicationIsQuitting = true;

        protected static T CreateSingletonInstance()
        {
            if (instance != null) return instance;

            // INFO: Try to find an existing instance in the loaded scene(s)
            instance = FindFirstObjectByType<T>();
            if (instance != null) return instance;

            // INFO: If the application is quitting or we're not playing, don't create new objects
            if (applicationIsQuitting || !Application.isPlaying) return null;

            // INFO: Create the singleton GameObject
            GameObject singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
            instance = singletonObject.AddComponent<T>();

            // INFO: Ensure a parent exists
            GameObject singletonObjectParent = GameObject.Find("Singletons") ?? new GameObject("Singletons");
            singletonObject.transform.SetParent(singletonObjectParent.transform);

            // INFO: Keep singletons across scenes (optional, but usually desired)
            DontDestroyOnLoad(singletonObjectParent);
            DontDestroyOnLoad(singletonObject);

            return instance;
        }
    }
    
}
