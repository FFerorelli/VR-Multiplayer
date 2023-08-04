using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public Slider maxPlayerSlider;
    public Button createLobbyButton;
    public TMP_Dropdown gameModeDropdown;

    // Start is called before the first frame update
    void Start()
    {
        createLobbyButton.onClick.AddListener(CreateLobbyFromUI);
    }
   

    public void CreateLobbyFromUI()
    {
        LobbyManager.LobbyData lobbyData = new LobbyManager.LobbyData();
        lobbyData.maxPlayers = (int)maxPlayerSlider.value;
        lobbyData.lobbyName = nameInputField.text;
        lobbyData.gameMode = gameModeDropdown.options[gameModeDropdown.value].text;
        LobbyManager.Instance.CreateLobby(lobbyData);
    }

    // Update is called once per frame
    void Update()
    {
        createLobbyButton.gameObject.SetActive(nameInputField.text != "");
    }
}
