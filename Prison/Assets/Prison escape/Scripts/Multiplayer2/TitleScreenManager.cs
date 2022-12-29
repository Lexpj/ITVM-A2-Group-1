using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
public static TitleScreenManager instance;
[SerializeField] private NetworkManagerCC networkManager;

[Header("UI Panels")]
[SerializeField] private string mainMenuPanel;
[SerializeField] private GameObject PlayerNamePanel;
[SerializeField] private GameObject HostOrJoinPanel;
[SerializeField] private GameObject EnterIPAddressPanel;

[Header("PlayerName UI")]
[SerializeField] private TMP_InputField playerNameInputField;

[Header("Enter IP UI")]
[SerializeField] private TMP_InputField IpAddressField;

//[Header("Misc. UI")]
//[SerializeField] private Button returnToMainMenu;

private const string PlayerPrefsNameKey = "PlayerName";

	//SceneManager.LoadScene(firstlevel);


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("theTest5");
        PlayerNamePanel.SetActive(true);
        GetSavedPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnToMainMenu()
    {
        Debug.Log("theTest4");
    	SceneManager.LoadScene(mainMenuPanel);
    }

    void MakeInstance()
    {
        Debug.Log("theTest3");
        if (instance == null)
            instance = this;
    }

    public void StartGame()
    {
        Debug.Log("theTest2");
        //SceneManager.LoadScene("Gameplay");
        //mainMenuPanel.SetActive(false);
        PlayerNamePanel.SetActive(true);
        GetSavedPlayerName();
        //returnToMainMenu.gameObject.SetActive(true);
    }

    private void GetSavedPlayerName()
    {
        Debug.Log("theTest1");
        if (PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            playerNameInputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);
        }
    }

    public void SavePlayerName()
    {
        Debug.Log("SavePlayer");
        string playerName = null;
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            playerName = playerNameInputField.text;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
            PlayerNamePanel.SetActive(false);
            HostOrJoinPanel.SetActive(true);
        }
    }

    public void HostGame()
    {
        Debug.Log("Hosting a game...");
        networkManager.StartHost();
        HostOrJoinPanel.SetActive(false);
        //returnToMainMenu.gameObject.SetActive(false);
    }

    public void JoinGame()
    {
        Debug.Log("IpPanel");
        HostOrJoinPanel.SetActive(false);
        EnterIPAddressPanel.SetActive(true);
    }

    public void ConnectToGame()
    {

        Debug.Log("Client will connect to: " + IpAddressField.text);
        if (!string.IsNullOrEmpty(IpAddressField.text))
        {
            Debug.Log("Client will connect to: " + IpAddressField.text);
            networkManager.networkAddress = IpAddressField.text;
            //networkManager.StartServer(); //hmm
            networkManager.StartClient();
            //networkManager.StartServer(); //hmm
        }
        EnterIPAddressPanel.SetActive(false);
        //returnToMainMenu.gameObject.SetActive(false);
    }
}
