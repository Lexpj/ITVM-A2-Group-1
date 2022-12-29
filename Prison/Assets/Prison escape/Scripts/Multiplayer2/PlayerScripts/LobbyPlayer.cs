using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
using Mirror;
using System.Linq;



public class LobbyPlayer : NetworkBehaviour
{
	[SyncVar(hook = nameof(HandlePlayerNameUpdate))] public string PlayerName;
	[SyncVar] public int ConnectionId;

	[Header("Game Info")]
	public bool IsGameLeader = false;
    [SyncVar(hook = nameof(HandlePlayerReadyStatusUpdate))] public bool IsReady = false;

    [Header("UI")]
    [SerializeField] private GameObject PlayerLobyUI;
    [SerializeField] private GameObject Player1ReadyPanel;
    [SerializeField] private GameObject Player2ReadyPanel;
    [SerializeField] private GameObject Player3ReadyPanel;
    [SerializeField] private GameObject Player4ReadyPanel;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private Button readyButton;

	private const string PlayerPrefsNameKey = "PlayerName";

    private NetworkManagerCC game;
    private NetworkManagerCC Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = NetworkManagerCC.singleton as NetworkManagerCC;
        }
    }

    public override void OnStartAuthority()
    {
        Debug.Log("aTest1");
        CmdSetPlayerName(PlayerPrefs.GetString(PlayerPrefsNameKey));
        if (!PlayerLobyUI.activeInHierarchy)
        {
            PlayerLobyUI.SetActive(true);
        }
        gameObject.name = "LocalLobbyPlayer";
    }
    [Command]

    private void CmdSetPlayerName(string playerName)
    {
        Debug.Log("aTest2");
        PlayerName = playerName;
        Debug.Log("Player display name set to: " + playerName);
    }

    public override void OnStartClient()
{
    Debug.Log("aTest3");
    Game.LobbyPlayers.Add(this);
    Debug.Log("Added to GamePlayer list: " + this.PlayerName);
}

public override void OnStopClient()
{
    Debug.Log("aTest4");
    Debug.Log(PlayerName + " is quiting the game.");
    Game.LobbyPlayers.Remove(this);
    Debug.Log("Removed player from the GamePlayer list: " + this.PlayerName);
}

public void UpdateLobbyUI()
{
    Debug.Log("aTest5");
    Debug.Log("Updating UI for: " + this.PlayerName);
    GameObject localPlayer = GameObject.Find("LocalLobbyPlayer");
    if (localPlayer != null)
    {
        localPlayer.GetComponent<LobbyPlayer>().ActivateLobbyUI();
    }
}

public void ActivateLobbyUI()
{
    Debug.Log("aTest6");
    Debug.Log("Activating lobby UI");
    if (!PlayerLobyUI.activeInHierarchy)
        PlayerLobyUI.SetActive(true);
    if (Game.LobbyPlayers.Count() > 0)
    {
        Player1ReadyPanel.SetActive(true);
        Debug.Log("Player1 Ready Panel activated");
        Player2ReadyPanel.SetActive(false);
        Player3ReadyPanel.SetActive(false);
        Player4ReadyPanel.SetActive(false);

    }
    else
    {
        Debug.Log("Player1 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
    }
    if (Game.LobbyPlayers.Count() > 1)
    {
        Player2ReadyPanel.SetActive(true);
        Player3ReadyPanel.SetActive(false);
        Player4ReadyPanel.SetActive(false);
        Debug.Log("Player2 Ready Panel activated");
    }
    else
    {
        Debug.Log("Player2 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
    }
    if (Game.LobbyPlayers.Count() > 2)
    {
        Player3ReadyPanel.SetActive(true);
        Player4ReadyPanel.SetActive(false);
        Debug.Log("Player3 Ready Panel activated");
    }
    else
    {
        Debug.Log("Player3 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
    }
        if (Game.LobbyPlayers.Count() > 3)
    {
        Player4ReadyPanel.SetActive(true);
        Debug.Log("Player4 Ready Panel activated");
    }
    else
    {
        Debug.Log("Player4 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
    }
    UpdatePlayerReadyText();
}

public void UpdatePlayerReadyText()
{
    Debug.Log("aTest7");
    if (Player1ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 0)
    {
        foreach (Transform childText in Player1ReadyPanel.transform)
        {
            if (childText.name == "Player1Name")
                childText.GetComponent<TMPro.TextMeshProUGUI>().text = Game.LobbyPlayers[0].PlayerName;
            if (childText.name == "Player1ReadyText")
            {
                bool isPlayerReady = Game.LobbyPlayers[0].IsReady;
                if (isPlayerReady)
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Not Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                }
            }
        }
    }
    if (Player2ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 1)
    {
        foreach (Transform childText in Player2ReadyPanel.transform)
        {
            if (childText.name == "Player2Name")
                childText.GetComponent<TMPro.TextMeshProUGUI>().text = Game.LobbyPlayers[1].PlayerName;
            if (childText.name == "Player2ReadyText")
            {
                bool isPlayerReady = Game.LobbyPlayers[1].IsReady;
                if (isPlayerReady)
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Not Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                }
            }
            Debug.Log("Updated Player2 Ready panel with player name: " + Game.LobbyPlayers[1].PlayerName + " and ready status: " + Game.LobbyPlayers[1].IsReady);
        }
    }
    if (Player3ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 2)
    {
        foreach (Transform childText in Player3ReadyPanel.transform)
        {
            if (childText.name == "Player3Name")
                childText.GetComponent<TMPro.TextMeshProUGUI>().text = Game.LobbyPlayers[2].PlayerName;
            if (childText.name == "Player3ReadyText")
            {
                bool isPlayerReady = Game.LobbyPlayers[2].IsReady;
                if (isPlayerReady)
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Not Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                }
            }
            Debug.Log("Updated Player3 Ready panel with player name: " + Game.LobbyPlayers[2].PlayerName + " and ready status: " + Game.LobbyPlayers[2].IsReady);
        }
    }
    if (Player4ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 3)
    {
        foreach (Transform childText in Player4ReadyPanel.transform)
        {
            if (childText.name == "Player4Name")
                childText.GetComponent<TMPro.TextMeshProUGUI>().text = Game.LobbyPlayers[3].PlayerName;
            if (childText.name == "Player4ReadyText")
            {
                bool isPlayerReady = Game.LobbyPlayers[3].IsReady;
                if (isPlayerReady)
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
                }
                else
                {
                    childText.GetComponent<TMPro.TextMeshProUGUI>().text = "Not Ready";
                    childText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                }
            }
            Debug.Log("Updated Player2 Ready panel with player name: " + Game.LobbyPlayers[3].PlayerName + " and ready status: " + Game.LobbyPlayers[3].IsReady);
        }
    }
    if (IsReady)
{
    readyButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Unready";
}
else
{
    readyButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Ready Up";
}
}

[Command]
public void CmdReadyUp()
{
    Debug.Log("aTest8");
    IsReady = !IsReady;
    Debug.Log("Ready status changed for: " + PlayerName);
}

public void HandlePlayerNameUpdate(string oldValue, string newValue)
{
    Debug.Log("aTest9");
    Debug.Log("Player name has been updated for: " + oldValue + " to new value: " + newValue);
    UpdateLobbyUI();
}
public void HandlePlayerReadyStatusUpdate(bool oldValue, bool newValue)
{
    Debug.Log("aTest10");
    Debug.Log("Player ready status has been has been updated for " + this.PlayerName + ": " + oldValue + " to new value: " + newValue);
    UpdateLobbyUI();
}

public void CheckIfAllPlayersAreReady()
{
    Debug.Log("aTest11");
    Debug.Log("Checking if all players are ready.");
    bool arePlayersReady = false;
    foreach (LobbyPlayer player in Game.LobbyPlayers)
    {
        if (!player.IsReady)
        {
            Debug.Log(player.PlayerName + "is not ready.");
            arePlayersReady = false;
            startGameButton.SetActive(false);
            break;
        }
        else
        {
            arePlayersReady = true;
        }

    }
    if (arePlayersReady)
        Debug.Log("All players are ready");

    if (arePlayersReady && IsGameLeader && Game.LobbyPlayers.Count() >= Game.minPlayers)
    {
        Debug.Log("All players are ready and minimum number of players in game. Activating the StartGame button on Game leader's UI.");
        startGameButton.SetActive(true);
    }
    else
    {
        startGameButton.SetActive(false);
    }

}

[Command]
public void CmdStartGame()
{
    Debug.Log("aTest12");
    Game.StartGame();
}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfAllPlayersAreReady();
    }
}

