using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    private Lobby currentLobby;
    private float timer = 0;

    private void Awake()
    {
        Instance = this;
    }

    public struct LobbyData
    {
        public string lobbyName;
        public int maxPlayers;
    }

    public async void CreateLobby(LobbyData lobbyData)
    {
        CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
        lobbyOptions.IsPrivate = false;
        lobbyOptions.Data = new Dictionary<string, DataObject>();

        string joinCode = await RelayManager.Instance.CreateRelayGame(lobbyData.maxPlayers);

        DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, joinCode);
        lobbyOptions.Data.Add("Join Code Key", dataObject);

        currentLobby = await Lobbies.Instance.CreateLobbyAsync(lobbyData.lobbyName, lobbyData.maxPlayers, lobbyOptions);

    }

    public async void QuickJoinLobby()
    {
        currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
        string relayJoinCode = currentLobby.Data["Join Code Key"].Value;

        RelayManager.Instance.JoinRelayGame(relayJoinCode);
    }

    public async void JoinLobby(string lobbyId)
    {
        currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);
        string relayJoinCode = currentLobby.Data["Join Code Key"].Value;

        RelayManager.Instance.JoinRelayGame(relayJoinCode);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 15)
        {
            timer -= 15;

            if (currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                 LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);  
            }
        }

        timer += Time.deltaTime;
    }
}
