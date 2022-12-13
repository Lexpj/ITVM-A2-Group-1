using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitsMinigame : MonoBehaviour
{
    public int solutionNr1;
    public int solutionNr2;
    public int solutionNr3;
    public GameObject nr1;
    public GameObject nr2;
    public GameObject nr3;
    public Task task;

    // Ticking sound
    public AudioSource audioSource;
    public AudioClip ticker;
    
    public bool firstClick;
    private float startTime;
    public float maxTime = 30.0f;
    private float tickTimePeriod = 0.5f;
    private float tickTime = 0.0f;
    private bool firstIncrease = false;
    private float firstIncreaseTime;
    private bool secondIncrease = false;
    private float secondIncreaseTime;

    public bool hardMode = false;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        init();  
    }

    void init() {
        // Randomize input, such that nothing matches:
        int firstRandom = Random.Range(0,9);
        nr1.GetComponent<DigitScript>().currentDigit = firstRandom;
        solutionNr1 = Random.Range(0,9);
        while (solutionNr1 == firstRandom) {
            solutionNr1 = Random.Range(0,9);
        }
        int secondRandom = Random.Range(0,9);
        nr2.GetComponent<DigitScript>().currentDigit = secondRandom;
        solutionNr2 = Random.Range(0,9);
        while (solutionNr2 == secondRandom) {
            solutionNr2 = Random.Range(0,9);
        }
        int thirdRandom = Random.Range(0,9);
        nr3.GetComponent<DigitScript>().currentDigit = thirdRandom;
        solutionNr3 = Random.Range(0,9);
        while (solutionNr3 == thirdRandom) {
            solutionNr3 = Random.Range(0,9);
        }

        // Time tick
        firstClick = false;
        startTime = Time.time;
        tickTimePeriod = 0.5f;
        tickTime = 0.0f;
        firstIncrease = false;
        firstIncreaseTime = maxTime/3 * 2;
        secondIncrease = false;
        secondIncreaseTime = maxTime / 3;
    }

    bool checkWin() {
        bool win = true;

        if (nr1.GetComponent<DigitScript>().currentDigit != solutionNr1) {
            win = false;
        } else if (nr2.GetComponent<DigitScript>().currentDigit != solutionNr2) {
            win = false;
        } else if (nr3.GetComponent<DigitScript>().currentDigit != solutionNr3) {
            win = false;
        }
        return win;
    }


    void Failed() {
        task.Failed();
        init();
    }
    void Completed() {
        task.Completed();
    }

    // Update is called once per frame
    void Update()
    {


        // Tick
        if (firstClick) {
            if (Time.time-startTime > maxTime) {
                Failed();
            }

            if (Time.time-startTime > tickTime) {
                audioSource.PlayOneShot(ticker, 1);
                tickTime += tickTimePeriod;
                if (Time.time-startTime > firstIncreaseTime && !firstIncrease) {
                    firstIncrease = true;
                    tickTimePeriod = tickTimePeriod * 0.5f;
                }
                else if (Time.time-startTime > secondIncreaseTime && !secondIncrease) {
                    secondIncrease = true;
                    tickTimePeriod = tickTimePeriod * 0.5f;
                }
            }
        }



        bool won = checkWin();
        if (won) {
            Completed();
        }
    }
}
