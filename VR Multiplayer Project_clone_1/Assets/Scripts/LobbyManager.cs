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
    private float heartBeatTimer = 0;
    private float updateLobbyTimer = 0;

    private bool hasPlayerDataToUpdate = false;
    private Dictionary<string, PlayerDataObject> newPlayerData;

    public Lobby CurrentLobby { get => currentLobby; }

    private void Awake()
    {
        Instance = this;
    }

    public struct LobbyData
    {
        public string lobbyName;
        public int maxPlayers;
        public string gameMode;
    }

    public async void UpdatePlayer(Dictionary<string, PlayerDataObject> data)
    {
        UpdatePlayerOptions updateOptions = new UpdatePlayerOptions();
        updateOptions.Data = data;
        currentLobby = await LobbyService.Instance.UpdatePlayerAsync(currentLobby.Id, AuthenticationService.Instance.PlayerId, updateOptions);
    }

    public async void LockLobby()
    {
        currentLobby = await Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions { IsLocked = true });
    }

    public void UpdatePlayerData(Dictionary<string, PlayerDataObject> data)
    {
        newPlayerData = data;
        hasPlayerDataToUpdate = true;
    }

    public async void CreateLobby(LobbyData lobbyData)
    {
        CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
        lobbyOptions.IsPrivate = false;
        lobbyOptions.Data = new Dictionary<string, DataObject>();

        string joinCode = await RelayManager.Instance.CreateRelayGame(lobbyData.maxPlayers);

        DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, joinCode);
        lobbyOptions.Data.Add("Join Code Key", dataObject);
        
        DataObject gameDataObject = new DataObject(DataObject.VisibilityOptions.Public, lobbyData.gameMode);
        lobbyOptions.Data.Add("Game Mode", gameDataObject);

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
    async void Update()
    {
        if (heartBeatTimer > 15)
        {
            heartBeatTimer -= 15;

            if (currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);  
            }
        }

        heartBeatTimer += Time.deltaTime;

        if (updateLobbyTimer > 1.5f)
        {
            updateLobbyTimer -= 1.5f;

            if (currentLobby != null)
            {
                if (hasPlayerDataToUpdate)
                {
                    UpdatePlayer(newPlayerData);
                    hasPlayerDataToUpdate = false;
                }
                else
                {
                    currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
                }
            }

            updateLobbyTimer += Time.deltaTime;
        }
    }
}
