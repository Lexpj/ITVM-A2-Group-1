using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private GameObject[] tasks;
    public taskManager taskManager;
    public GameObject endPoint;
    private bool completeTasks = false;
    private bool fleeing = false;
    private bool escaping = false;
    private int currentTask = 0;
    private bool onWayToTask = false;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        tasks = GameObject.FindGameObjectsWithTag("Task");
    }

    // Update is called once per frame
    void Update()
    {
        if (taskManager.allTasksCompleted)
        {
            agent.SetDestination(endPoint.transform.position);
            escaping = true;
        }
        if (!onWayToTask && !escaping)
        {
            ChooseTask();
        }
        CheckIfCurrentTaskCompleted();
        if(taskManager.endTaskEnabled)
        {
            tasks = GameObject.FindGameObjectsWithTag("Task");
        }
        if (taskManager.allTasksCompleted)
        {
            agent.SetDestination(endPoint.transform.position);
        }
    }

    void setState()
    {
        completeTasks = true;
    }
    
    void ChooseTask()
    {
        float distance = 1000;
        float taskDistance = 0;
        int bestTask = 0;
        int tasksToBeCompleted = 0;

        for (int i = 0; i < tasks.Length; i++)
        {
            if (!tasks[i].GetComponent<Task>().taskCompleted)
            {
                taskDistance = Vector3.Distance(transform.position, tasks[i].transform.position);
                if (taskDistance < distance)
                {
                    distance = taskDistance;
                    bestTask = i;
                }
            }
        }

        currentTask = bestTask;

        agent.SetDestination(tasks[currentTask].transform.position);

        onWayToTask = true;
    }

    void CheckIfCurrentTaskCompleted()
    {
        if (tasks[currentTask].GetComponent<Task>().taskCompleted)
        {
            onWayToTask = false;
        }
    }
}
