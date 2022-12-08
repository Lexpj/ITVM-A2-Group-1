using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Task : MonoBehaviour
{
    private float secondsToCompleteTask = 2f;
    private float counter = 0f;
    public bool taskCompleted = false;
    private bool atTask = false;
    private bool disableIcon = false;
    private taskManager targetObj;
    public GameObject T_key;
    public GameObject exclamation;
    
    AudioSource audioSource;
    public AudioClip win;
    public AudioClip lose;

    private GameObject player;

    public static bool isMinigameOpen = false;
    [SerializeField] GameObject minigame;

    private void Start()
    {
        targetObj = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<taskManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        minigame.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!taskCompleted)
        {
           // if (atTask)
            //{
           //     counter += Time.deltaTime;
           // }

           // if (!atTask)
           // {
           //     counter = 0f;
           // }

           // if (counter >= secondsToCompleteTask)
           // {
           //     targetObj.TaskCompleted();              
           //     taskCompleted = true;
           //     T_key.SetActive(false);
           // }
            if (Input.GetKey(KeyCode.T) && atTask)
            {
                T_key.SetActive(false);
               
                minigame.SetActive(true);
                isMinigameOpen = true;
                minigame.transform.position = player.transform.position;
                
            }
        }
        if (taskCompleted && !disableIcon)
        {
            exclamation.SetActive(false);
            disableIcon = true;
        }
    }

    public void Completed() {
        minigame.SetActive(false);
        isMinigameOpen = false;
        taskCompleted = true;
        targetObj.TaskCompleted();
        exclamation.SetActive(false);
        disableIcon = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = win;
        audioSource.Play();

    }
    public void Failed() {
        minigame.SetActive(false);
        isMinigameOpen = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = lose;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        atTask = true;
        if (!taskCompleted)
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
