using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    private float secondsToCompleteTask = 10f;
    private float counter = 0f;
    public bool taskCompleted = false;
    private bool atTask = false;
    private bool disableIcon = false;
    private taskManager targetObj;
    public GameObject T_key;
    public GameObject exclamation;
    public GameObject taskIcon;
    private GuardAI guardScript;
    private bool player = false;
    private bool prisonerAI = false;

    AudioSource audioSource;
    public AudioClip win;
    public AudioClip lose;
    public bool isClickable = false;
    public static bool isMinigameOpen = false;
    private GameObject playerPos;

    public float completionRate = 70f;
    [SerializeField] GameObject minigame;

    private void Start()
    {
        targetObj = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<taskManager>();
        playerPos = GameObject.FindGameObjectWithTag("Player");
        guardScript = GameObject.FindGameObjectWithTag("Guard").GetComponent<GuardAI>();
        minigame.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!taskCompleted)
        {
            if (prisonerAI)
            {
                if (atTask)
                {
                     counter += Time.deltaTime;
                }

                if (!atTask)
                {
                    counter = 0f;
                }

                if (counter >= secondsToCompleteTask)
                {
                    float randomNumber = Random.Range(0f, 100f);

                    if(randomNumber <= completionRate)
                    {
                        targetObj.TaskCompleted();
                        taskCompleted = true;
                        prisonerAI = false;
                    }
                    else
                    {
                        guardScript.setNotified(this.transform);
                        audioSource = GetComponent<AudioSource>();
                        audioSource.clip = lose;
                        audioSource.Play();
                        counter = 0f;
                    }
                }
            }
            if (player)
            {
                if (Input.GetKey(KeyCode.T) && atTask)
                {
                    player = false;

                    T_key.SetActive(false);

                    minigame.SetActive(true);
                    isMinigameOpen = true;

                    if (!isClickable) {
                        minigame.transform.position = playerPos.transform.position;
                    } else {
                        minigame.transform.position = new Vector3(playerPos.transform.position.x,playerPos.transform.position.y,-2);
                    }
                }
            }
        }
        if (taskCompleted && !disableIcon)
        {
            exclamation.SetActive(false);
            taskIcon.SetActive(false);
            disableIcon = true;
        }
    }
    
    public void Completed() {
        minigame.SetActive(false);
        isMinigameOpen = false;
        taskCompleted = true;
        targetObj.TaskCompleted();
        exclamation.SetActive(false);
        taskIcon.SetActive(false);
        disableIcon = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = win;
        audioSource.Play();

    }
    public void Failed() {
        minigame.SetActive(false);
        isMinigameOpen = false;
        guardScript.setNotified(this.transform);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = lose;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        atTask = true;

        if (other.CompareTag("Player"))
        {
            player = true;
            prisonerAI = false;
        }
        else if (other.CompareTag("PrisonerAI"))
        {
            player = false;
            prisonerAI = true;
        }

        if (!taskCompleted && player)
        {
            T_key.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        atTask = false;
        T_key.SetActive(false);
        minigame.SetActive(false);
    }
}
