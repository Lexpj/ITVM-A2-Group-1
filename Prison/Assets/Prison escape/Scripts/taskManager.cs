using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class taskManager : MonoBehaviour
{
    public int totalTasks = 0;
    private int prevTotalTasks = 0;
    private int completedTasks = 0;
    private int prevCompletedTasks = 0;
    public bool endTaskEnabled = false;
    public GameObject lastTask;
    public GameObject endPoint;
    public bool allTasksCompleted;
    [SerializeField] private TextMeshProUGUI TaskBar;

    void Start()
    {
        TaskBar.text = $"Tasks completed {completedTasks}/{totalTasks}";
    }

    // Update is called once per frame
    void Update()
    {
        AllTasksCompleted();
        IncrementTaskCounter();
        prevCompletedTasks = completedTasks;
        prevTotalTasks = totalTasks;
    }

    public void TaskCompleted()
    {
        completedTasks++;
    }

    private void AllTasksCompleted()
    {
        if(totalTasks == completedTasks && !endTaskEnabled)
        {
            totalTasks++;
            lastTask.SetActive(true);
            endTaskEnabled = true;
        }

        if(totalTasks == completedTasks && endTaskEnabled)
        {
            endPoint.SetActive(true);
            allTasksCompleted = true;
        }
    }

    private void IncrementTaskCounter()
    {
        if(completedTasks > prevCompletedTasks || totalTasks < prevTotalTasks)
        {
            TaskBar.text = $"Tasks completed {completedTasks}/{totalTasks}";
        }
    }
}
