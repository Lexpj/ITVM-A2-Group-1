using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private GameObject[] prisonerAIs;
    private GameObject[] tasks;
    public taskManager taskManager;
    public GameObject endPoint;
    private bool fleeing = false;
    private float knockedCounter = 0f;
    private string state = "";
    public int currentTask = -1;
    public bool onWayToTask = false;
    [SerializeField] private GameObject particles;
    [SerializeField] private Health health;


    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        tasks = GameObject.FindGameObjectsWithTag("Task");
        prisonerAIs = GameObject.FindGameObjectsWithTag("PrisonerAI");
    }

    // Update is called once per frame
    void Update()
    {
        setState();
        if (!onWayToTask && state == "Tasks")
        {
            ChooseTask();
        }
        CheckIfCurrentTaskCompleted();
        if(taskManager.endTaskEnabled)
        {
            tasks = GameObject.FindGameObjectsWithTag("Task");
        }
    }

    void setState()
    {
        if(health.GetHealthState() == "Knocked")
        {
            state = "Knocked";
            agent.SetDestination(this.transform.position);
            particles.SetActive(true);
        }
        else if (taskManager.allTasksCompleted)
        {
            agent.SetDestination(endPoint.transform.position);
            state = "Escaping";
            particles.SetActive(false);
        }
        else
        {
            state = "Tasks";
            particles.SetActive(false);
        }
    }
    
    void ChooseTask()
    {
        float distance = 1000;
        float taskDistance = 0;
        int bestTask = 0;
        int tasksToBeCompleted = 0;
        bool taskAlreadyExecuted = false;

        for (int i = 0; i < tasks.Length; i++)
        {
            if (!tasks[i].GetComponent<Task>().taskCompleted)
            {
                foreach(GameObject prisoner in prisonerAIs)
                {
                    if(prisoner.GetComponent<AIMovement>().currentTask == i && prisoner.GetComponent<AIMovement>().onWayToTask)
                    {
                        taskAlreadyExecuted = true;
                    }
                }

                if (!taskAlreadyExecuted)
                {
                    taskDistance = Vector3.Distance(transform.position, tasks[i].transform.position);
                    if (taskDistance < distance)
                    {
                        distance = taskDistance;
                        bestTask = i;
                    }
                }
            }
            taskAlreadyExecuted = false;
        }

        currentTask = bestTask;

        agent.SetDestination(tasks[currentTask].transform.position);

        onWayToTask = true;
    }

    void CheckIfCurrentTaskCompleted()
    {
        if(currentTask != -1)
        {
            if (tasks[currentTask].GetComponent<Task>().taskCompleted)
            {
                onWayToTask = false;
            }
        }
    }
}
