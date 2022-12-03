using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject taskIcon;
    private bool player = false;
    private bool prisonerAI = false;

    private void Start()
    {
        targetObj = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<taskManager>();
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
                    targetObj.TaskCompleted();              
                    taskCompleted = true;
                    T_key.SetActive(false);
                }
            }
            if (player)
            {
                if (Input.GetKey(KeyCode.T) && atTask)
                {
                    taskCompleted = true;
                    targetObj.TaskCompleted();
                    T_key.SetActive(false);
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
    }
}
