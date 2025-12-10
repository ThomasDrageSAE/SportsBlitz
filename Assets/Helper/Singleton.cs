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

        private void OnDestroy()
        {
            instance = null;
        }

        protected static T CreateSingletonInstance()
        {
            if (instance != null) return instance;
            instance = FindFirstObjectByType<T>();

            if (instance != null) return instance;
            GameObject singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
            instance = singletonObject.AddComponent<T>();

            GameObject singletonObjectParent = GameObject.Find("Singletons") ?? new GameObject("Singletons");
            singletonObject.transform.SetParent(singletonObjectParent.transform);

            return instance;
        }
        protected virtual void OnInstanceCreated() { }

    }

    /// <summary>
    /// Creates a singleton in the scene when called but is set to not destroy on load allowing it to stay in the scene 
    /// when transitioning between scenes
    /// </summary>
    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void OnInstanceCreated()
        {
            DontDestroyOnLoad(gameObject);

        }
    }


}
