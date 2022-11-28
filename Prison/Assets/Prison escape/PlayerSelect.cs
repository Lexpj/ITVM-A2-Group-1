using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
	[SerializeField]
	private GameObject[] characters;
	public string firstlevel;
    public string closePlayerSelect;

	private int characterIndex;
    public GameObject myAudio;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(myAudio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPrisoner()
    {
    	SceneManager.LoadScene(firstlevel);
    	PlayerPrefs.SetInt("CharacterIndex", 0);
    }

    public void StartGuard()
    {
        SceneManager.LoadScene(firstlevel);
        PlayerPrefs.SetInt("CharacterIndex", 1);
    }

    public void  ChangeCharacter(int index)
    {
    	for (int i=0; i < characters.Length; i++)
    	{
    		characters[i].SetActive(false);
    	}
        this.characterIndex = index;
    	characters[index].SetActive(true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(closePlayerSelect);
    }

}
