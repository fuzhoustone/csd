using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamScript : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        Debug.LogWarning("this is not call steam run");
#else
        
        if (SteamManager.Initialized)
        {
            Debug.Log("steam sdk init success");

            /*
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
            */
        }
#endif
    }
}
