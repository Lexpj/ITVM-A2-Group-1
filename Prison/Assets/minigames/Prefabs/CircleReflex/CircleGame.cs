using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGame : MonoBehaviour
{
    public GameObject circle1;
    public GameObject circle2;
    public GameObject circle3;
    public GameObject circle4;
    public GameObject circle5;
    public GameObject circle6;
    public GameObject circle7;
    public GameObject circle8;
    public GameObject circle9;
    public GameObject circle10;
    public GameObject circle11;
    public GameObject circle12;
    public GameObject circle13;
    public GameObject circle14;
    public GameObject circle15;
    public GameObject circle16;
    public GameObject circle17;
    public GameObject circle18;
    public GameObject circle19;
    public GameObject circle20;
    public GameObject circle21;
    public GameObject circle22;
    public GameObject circle23;
    public GameObject circle24;
    public GameObject circle25;

    AudioSource audioSource;
    public AudioClip pop;
    
    public Task task;

    private int[] order;
    public int[] clicked;
    private GameObject[] objects;

    // Time
    private float nextActionTime;
    public float minPeriod = 0.4f;
    public float maxPeriod = 1f;
    private float increment;
    private float period;

    // game vars
    private int cur = 0;
    public int curpointer = 0;
    public bool clickDetected = false;
    private bool firstClick = false;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        init();
    }

    void init() {
        order = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24};
        clicked = new int[] {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
        cur = 0;
        curpointer = 0;
        increment = ((maxPeriod-minPeriod)/25);
        period = maxPeriod;

        clickDetected = false;
        firstClick = false;
        startTime = Time.time;
        nextActionTime = 0.0f;
        
        objects = new GameObject[] {circle1,circle2,circle3,circle4,circle5,circle6,circle7,circle8,circle9,circle10,circle11,circle12,circle13,circle14,circle15,circle16,circle17,circle18,circle19,circle20,circle21,circle22,circle23,circle24,circle25};
        for (int i = 0; i < 25; i++) {
            objects[i].GetComponent<DetectClick>().setPos(i);
            objects[i].GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,1f);
        }
        Shuffle();
        changeNext();
    }

    void Shuffle() {
        int randomint1;
        int randomint2;
        int temp;
        for (int i = 0; i < 200; i++) {
            randomint1 = Random.Range(0,24);
            randomint2 = Random.Range(0,24);
            temp = order[randomint1];
            order[randomint1] = order[randomint2];
            order[randomint2] = temp;
        }
    }

    void changeNext() {
        if (cur < 25) {
            objects[order[cur]].GetComponent<SpriteRenderer>().color = new Color(1f,1f,0f,1f);
            cur += 1;
            audioSource.PlayOneShot(pop,1);
        }
    }

    void checkIfCorrectClick() {
        if (order[curpointer] == clicked[curpointer]) { // GOOD
            objects[order[curpointer]].GetComponent<SpriteRenderer>().color = new Color(0f,1f,0f,1f);
            curpointer += 1;
            if (curpointer == 25) {
                Completed();
            }
        }
        else { // GAME OVER
            objects[order[curpointer]].GetComponent<SpriteRenderer>().color = new Color(1f,0f,0f,1f);
            Failed();
        }

    } 

    public void Completed() {
        task.Completed();
    }

    public void Failed() {
        init();
        task.Failed();
    }

    
    // Update is called once per frame
    void Update()
    {   
        if (firstClick) {
            if (Time.time-startTime > nextActionTime) {
                nextActionTime += period;
                period -= increment;
                changeNext();
            }
        }
        if (clickDetected) {
            if (!firstClick) {
                startTime = Time.time;
                firstClick = true;
            }
            checkIfCorrectClick();
            clickDetected = false;
        }
    }
}
