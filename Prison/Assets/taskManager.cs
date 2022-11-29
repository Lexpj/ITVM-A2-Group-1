using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class taskManager : MonoBehaviour
{
    public int totalTasks = 0;
    public int prevTotalTasks = 0;
    public int completedTasks = 0;
    private int prevCompletedTasks = 0;
    private bool endTaskEnabled = false;
    private Label taskLabel;
    public GameObject lastTask;

    void OnEnable()
    {
        var rootVisualElement = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;
        taskLabel = rootVisualElement.Q<Label>("TaskCounter");
        taskLabel.text = $"Tasks completed {completedTasks}/{totalTasks}";
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
    }

    private void IncrementTaskCounter()
    {
        if(completedTasks > prevCompletedTasks || totalTasks < prevTotalTasks)
        {
            taskLabel.text = $"Tasks completed {completedTasks}/{totalTasks}";
        }
    }
}
