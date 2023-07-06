using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;

public class LobbyListElement : MonoBehaviour
{

    public Button joinButton;
    public TextMeshProUGUI lobbyName;
    public TextMeshProUGUI playersIn;
    private string lobbyId;

    public void Initialize(Lobby lobby)
    {
        lobbyName.text = lobby.Name;
        playersIn.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

        lobbyId = lobby.Id;
    }

    // Start is called before the first frame update
    void Start()
    {
        joinButton.onClick.AddListener(() => LobbyManager.Instance.JoinLobby(lobbyId));
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
