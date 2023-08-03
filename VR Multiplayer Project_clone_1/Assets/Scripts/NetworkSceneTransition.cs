using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class NetworkSceneTransition : MonoBehaviour
{
    public static NetworkSceneTransition Instance;

    private bool isLoading = false;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += ServerStarted;
    }

    private void ServerStarted()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
    }

    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        isLoading = false;
    }

    public void LoadSceneForEveryBody(string sceneName)
    {
        if (!isLoading)
        {
            isLoading = true;
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
