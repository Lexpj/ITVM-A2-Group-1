using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PrisonerManager : MonoBehaviour
{
    public int totalPrisoners = 0;
    private int reachedExit = 0;
    private int prevReachedExit = 0;
    bool AllPrisonersReachedExit = false;
    private Label prisonLabel;
    private Label clockLabel;
    private float currentTime = 0f;
    private float startingTime = 301f;

    void OnEnable()
    {
        var rootVisualElement = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;
        prisonLabel = rootVisualElement.Q<Label>("PrisonCounter");
        prisonLabel.text = $"Prisoners escaped {reachedExit}/{totalPrisoners}";
        clockLabel = rootVisualElement.Q<Label>("clock");
    }

    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        clock();
        AllPrisonersEscaped();
        updateText();
        prevReachedExit = reachedExit;
    }

    public void PrisonerEscaped()
    {
        reachedExit++;
    }

    private void updateText()
    {
        if(!AllPrisonersReachedExit && prevReachedExit < reachedExit)
        {
            prisonLabel.text = $"Prisoners escaped {reachedExit}/{totalPrisoners}";
        }
    }

    private void AllPrisonersEscaped()
    {
        if(currentTime <= 0)
        {
            if(reachedExit < (totalPrisoners % 2))
            {
                LoadEndScene("The guard has won!");
            }
            if(totalPrisoners % 2 == 0)
            {
                if(reachedExit == (totalPrisoners / 2))
                {
                    LoadEndScene("It's a Tie!");
                }
            }
        }
        if(reachedExit == totalPrisoners)
        {
            //TO DO; go to win screen
            LoadEndScene("The prisoners have won!");
        }
    }

    public void clock()
    {
        currentTime -= 1 * Time.deltaTime;

        if(currentTime <= 0)
        {
            currentTime = 0;
        }

        updateUIClock();
    }

    public void updateUIClock()
    {
        int displayMinute = (int)currentTime / 60;
        int displaySecond = (int)currentTime % 60;
        string seconds;
        if (displaySecond < 10)
        {
            seconds = "0" + displaySecond.ToString();
        }
        else
        {
            seconds = displaySecond.ToString();
        }
        clockLabel.text = $"{displayMinute}:{seconds}";
    }

    public void LoadEndScene(string input)
    {
        StateNameController.endGameText = input;
        SceneManager.LoadScene("EndScene");
    }
    
}
