using Helper.Blake;
using UnityEngine;

public class SteamManager : Singleton<SteamManager>
{
    [field: SerializeField] public uint appID { get; private set; } = 4231540;

    private void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(appID);

        }
        catch (System.Exception e)
        {
            Debug.Log($"Couldn't initialize Steam Client: {e.Message}");
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

}