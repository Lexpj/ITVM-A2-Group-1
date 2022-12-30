using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	public string firstlevel;
    public AudioSource click;
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

    public void StartGame()
    {
        StartCoroutine(playSound());
    }

    public void OpenOptions()
    {
        click.Play();
    }

    public void CloseOptions()
    {

    }

    public void QuitGame()
    {
    	Application.Quit();
    }

    IEnumerator playSound()
    {
        click.Play();
        yield return new WaitForSeconds(click.clip.length);
        SceneManager.LoadScene(firstlevel);
    }

}
