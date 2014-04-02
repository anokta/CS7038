using System;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    public static TaskScheduler Instance { get; private set; }

    private SortedQueue<float, Task> tasks;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        tasks = new SortedQueue<float, Task>(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = tasks.Count - 1; i >= 0; i--)
        {
            var task = tasks[i];

            if (task.Key > Time.time) break;

            tasks.RemoveAt(i);

            task.Value.Run();
        }
    }

    public bool Add(Task newTask, bool unique = true)
    {
        if (unique)
        {
            foreach (var task in tasks)
            {
                if (task.Value.Equals(newTask)) return false;
            }
        }

        var time = Time.time + newTask.Delay;
        tasks.Add(time, newTask);

        return true;
    }
}

public abstract class Task : IEquatable<Task>
{
    public float Delay { get; protected set; }

    public abstract void Run();

    public abstract bool Equals(Task other);
}
