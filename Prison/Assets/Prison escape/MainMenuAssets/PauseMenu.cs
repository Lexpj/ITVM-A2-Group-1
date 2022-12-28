using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	public static bool isGamePaused = false;
    public string qGame;

	[SerializeField] GameObject Pause;

    // Update is called once per frame
    void Update()
    {
    	if (Input.GetKeyDown(KeyCode.Escape))
    	{
    		if (isGamePaused)
    		{
    			ResumeGame();
    		}
    		else
    		{
    			PauseGame();
    		}
    	}
    }

    public void ResumeGame()
    {
    	Pause.SetActive(false);
    	Time.timeScale = 1f;
    	isGamePaused = false;

    }

    void PauseGame()
    {
    	Pause.SetActive(true);
    	Time.timeScale = 0f;
    	isGamePaused = true;
    }

    public void QuitGame()
    {
        Pause.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(qGame);
    }
}
