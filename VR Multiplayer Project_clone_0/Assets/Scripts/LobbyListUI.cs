using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyListUI : MonoBehaviour
{

    public Transform contentParent;
    public LobbyListElement LobbyListElementPrefab;
    public float refreshTime = 2f;
    private float timer = 0;

    public async void UpdateLobbyList()
    {
        QueryResponse response = await Lobbies.Instance.QueryLobbiesAsync();

        for (int i = 0; i < contentParent.childCount; i++)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }

        foreach (var lobby in response.Results)
        {
            LobbyListElement spawnElement = Instantiate(LobbyListElementPrefab, contentParent);
            spawnElement.Initialize(lobby);
        }
    }

    private void OnEnable()
    {
        UpdateLobbyList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > refreshTime)
        {
            UpdateLobbyList();

            timer -= refreshTime;
        }

        timer += Time.deltaTime;
    }
}
