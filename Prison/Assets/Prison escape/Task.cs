using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    private float secondsToCompleteTask = 2f;
    private float counter = 0f;
    private bool taskCompleted = false;
    private bool atTask = false;
    private taskManager targetObj;

    private void Start()
    {
        targetObj = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<taskManager>();
    }

    private void Update()
    {
        if (!taskCompleted)
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
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        atTask = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        atTask = false;
    }
}
