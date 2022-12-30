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
    private bool guardInRange = false;
    private Transform guard;
    [SerializeField] GuardAI guardscript;
    private float knockedCounter = 0f;
    private string state = "";
    private bool captured = false;
    public int currentTask = -1;
    public bool onWayToTask = false;
    private bool rescueing = false;
    private bool setCel = false;
    private GameObject rescuePoint;
    [SerializeField] private GameObject particles;
    [SerializeField] private Health health;
    [SerializeField] private GameObject cross;


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
        reset();
        setState();
        if (state == "Captured")
        {
            if (guardscript.getCelLocation() != null && !setCel)
            {
                agent.SetDestination(guardscript.getCelLocation().position);
                setCel = true;
            }
        }
        if (state == "Knocked")
        {
            agent.SetDestination(this.transform.position);
            transform.position = health.lastPos().position;
        }
        else if (state == "Fleeing")
        {
            RunAway();
        }
        else if (state == "Rescueing")
        {
            agent.SetDestination(rescuePoint.transform.position);
        }
        else if (state == "Escaping")
        {
            agent.SetDestination(endPoint.transform.position);
        }
        if (!onWayToTask && state == "Tasks")
        {
            ChooseTask();
        }
        else if (state == "Tasks")
        {
            agent.SetDestination(tasks[currentTask].transform.position);
        }
        CheckIfCurrentTaskCompleted();
        if (taskManager.endTaskEnabled)
        {
            tasks = GameObject.FindGameObjectsWithTag("Task");
        }
    }

    void reset()
    {
        if (!captured)
        {
            setCel = false;
        }
        if (health.GetHealthState() != "Knocked")
        {
            particles.SetActive(false);
        }
        if (captured && rescueing)
        {
            if (rescuePoint != null)
            {
                rescuePoint.GetComponent<rescue>().unnotify();
            }
            setRescueing(false, null);
        }
    }

    void setState()
    {
        if (captured)
        {
            state = "Captured";
            cross.SetActive(true);
        }
        else if (health.GetHealthState() == "Knocked")
        {
            state = "Knocked";
            particles.SetActive(true);
        }
        else if (guardInRange)
        {
            state = "Fleeing";
        }
        else if (rescueing)
        {
            state = "Rescueing";
        }
        else if (taskManager.allTasksCompleted)
        {
            state = "Escaping";
        }
        else
        {
            state = "Tasks";
        }
    }

    void RunAway()
    {
        Vector3 dirToGuard = transform.position - guard.position;
        Vector3 newPos = transform.position + dirToGuard;
        agent.SetDestination(newPos);
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
                foreach (GameObject prisoner in prisonerAIs)
                {
                    if (prisoner.GetComponent<AIMovement>().currentTask == i && prisoner.GetComponent<AIMovement>().onWayToTask)
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
        if (currentTask != -1)
        {
            if (tasks[currentTask].GetComponent<Task>().taskCompleted)
            {
                onWayToTask = false;
            }
        }
    }

    public void SetGuardInRange(bool guard)
    {
        guardInRange = guard;
    }

    public void SetGuard(Transform guardPos)
    {
        guard = guardPos;
    }

    public void capture(bool capture)
    {
        captured = capture;
        cross.SetActive(capture);
    }

    public bool isCaptured()
    {
        return captured;
    }

    public void setRescueing(bool rescue, GameObject rescueLoc)
    {
        rescueing = rescue;
        rescuePoint = rescueLoc;
    }

    public bool isRescueing()
    {
        return rescueing;
    }
}
