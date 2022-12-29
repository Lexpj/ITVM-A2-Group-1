using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
//using Mirror.NetworkManager;
//using UnityEngine.Networking;



public class NetworkManagerCC : NetworkManager
{

[SerializeField] public int minPlayers = 2;
[SerializeField] private LobbyPlayer lobbyPlayerPrefab;
//[SerializeField] public gameObject test;
[SerializeField] private GamePlayer gamePlayerPrefab;
//public List<gameObject> LobbyPlayers { get; } = new List<LobbyPlayer>();
public List<LobbyPlayer> LobbyPlayers { get; } = new List<LobbyPlayer>();
public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();
//playerPrefab = test;

        public override void OnStartClient()
    {
        Debug.Log("Starting client...");
        //OnClientConnect();
    }

    public override void OnClientConnect()
    {
        Debug.Log("Client connected.");
        base.OnClientConnect();
        //bool isGameLeader = LobbyPlayers.Count == 0; // isLeader is true if the player count is 0, aka when you are the first player to be added to a server/room

        //LobbyPlayer lobbyPlayerInstance = Instantiate(lobbyPlayerPrefab);
        //NetworkConnectionToClient conn = NetworkServer.connections[0];
        //lobbyPlayerInstance.IsGameLeader = isGameLeader;
        //lobbyPlayerInstance.ConnectionId = conn.connectionId;

        //NetworkServer.Spawn(test);
    }

    public override void OnClientDisconnect()
    {
        Debug.Log("Client disconnected.");
        base.OnClientDisconnect();
    }

    public override void OnStartServer()
    {
                Debug.Log("Server started");
        base.OnStartServer();
    }


    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Connecting to server...");
        if (numPlayers >= maxConnections) // prevents players joining if the game is full
        {
            Debug.Log("Too many players. Disconnecting user.");
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != "CharacterSelect") // prevents players from joining a game that has already started. When the game starts, the scene will no longer be the "TitleScreen"
        {
            Debug.Log("Player did not load from correct scene. Disconnecting user. Player loaded from scene: " + SceneManager.GetActiveScene().name);
            conn.Disconnect();
            return;
        }
        Debug.Log("Server Connected");
        //OnServerAddPlayer(conn);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
{
    Debug.Log("Checking if player is in correct scene. Player's scene name is: " + SceneManager.GetActiveScene().name.ToString() + ". Correct scene name is: TitleScreen");
    if (SceneManager.GetActiveScene().name == "CharacterSelect")
    {
        bool isGameLeader = LobbyPlayers.Count == 0; // isLeader is true if the player count is 0, aka when you are the first player to be added to a server/room

        LobbyPlayer lobbyPlayerInstance = Instantiate(lobbyPlayerPrefab);

        lobbyPlayerInstance.IsGameLeader = isGameLeader;
        lobbyPlayerInstance.ConnectionId = conn.connectionId;

        NetworkServer.AddPlayerForConnection(conn, lobbyPlayerInstance.gameObject);
        Debug.Log("Player added. Player name: " + lobbyPlayerInstance.PlayerName + ". Player connection id: " + lobbyPlayerInstance.ConnectionId.ToString());
    }
}


private bool CanStartGame()
{
    Debug.Log("CanStartGame");
    if (numPlayers < minPlayers)
        return false;
    foreach (LobbyPlayer player in LobbyPlayers)
    {
        if (!player.IsReady)
            return false;
    }
    return true;
}

public void StartGame()
    {
        Debug.Log("StartGame");
        if (CanStartGame() && SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            ServerChangeScene("PrisonScene");
        }
    }

    public override void ServerChangeScene(string newSceneName)
{
    Debug.Log("ServerChangeScene");
    //Changing from the menu to the scene
    if (SceneManager.GetActiveScene().name == "PrisonScene" && newSceneName == "CharacterSelect")
    {
        for (int i = LobbyPlayers.Count - 1; i >= 0; i--)
        {
            var conn = LobbyPlayers[i].connectionToClient;
            var gamePlayerInstance = Instantiate(gamePlayerPrefab);

            NetworkServer.Destroy(conn.identity.gameObject);
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);
        }
    }
    base.ServerChangeScene(newSceneName);
}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
