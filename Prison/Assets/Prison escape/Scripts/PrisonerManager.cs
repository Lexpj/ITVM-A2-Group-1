using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PrisonerManager : MonoBehaviour
{
    public int totalPrisoners = 0;
    private int reachedExit = 0;
    private int prevReachedExit = 0;
    bool AllPrisonersReachedExit = false;
    private float currentTime = 0f;
    private float startingTime = 301f;
    [SerializeField] private TextMeshProUGUI prisonLabel;
    [SerializeField] private TextMeshProUGUI clocklabel;

    void Start()
    {
        currentTime = startingTime;
        prisonLabel.text = $"Prisoners escaped {reachedExit}/{totalPrisoners}";
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
        if (!AllPrisonersReachedExit && prevReachedExit < reachedExit)
        {
            prisonLabel.text = $"Prisoners escaped {reachedExit}/{totalPrisoners}";
        }
    }

    private void AllPrisonersEscaped()
    {
        if (currentTime <= 0)
        {
            if (reachedExit < (totalPrisoners / 2))
            {
                LoadEndScene("The guard has won!");
            }
            if (totalPrisoners % 2 == 0)
            {
                if (reachedExit == (totalPrisoners / 2))
                {
                    LoadEndScene("It's a Tie!");
                }
            }
            if (reachedExit > (totalPrisoners / 2))
            {
                LoadEndScene("The prisoners have won!");
            }
        }
        if (reachedExit == totalPrisoners)
        {
            LoadEndScene("The prisoners have won!");
        }
    }

    public void clock()
    {
        currentTime -= 1 * Time.deltaTime;

        if (currentTime <= 0)
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
        clocklabel.text = $"{displayMinute}:{seconds}";
    }

    public void LoadEndScene(string input)
    {
        StateNameController.endGameText = input;
        SceneManager.LoadScene("EndScene");
    }

}
