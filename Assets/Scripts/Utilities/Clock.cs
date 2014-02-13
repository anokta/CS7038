using UnityEngine;
using System;

/// <summary>
/// Executes an event after a specific time.
/// </summary>
public class Clock
{
    float _duration;

    public Clock()
    {
        repeat = true;
        running = true;
    }

    public Clock(float duration)
    {
        _duration = duration;
    }

    public Clock(float duration, Callback run)
        : this()
    {
        _duration = duration;
        Run = run;
    }

    public event Callback Run;

    public void Update()
    {
        if (!running) {
            return;
        }
        time += Time.deltaTime;
        while (time >= _duration) {
            time -= _duration;
            Run();
            if (!repeat) {
                Stop();
            }
        }
    }

    public void ForceInvoke()
    {
        Run();
    }

    public void Reset()
    { 
        running = true;
        time = 0;
    }

    public void Pause()
    {
        running = false;
    }

    public void Resume()
    {
        running = true;
    }

    public bool running { get; private set; }

    public void Stop()
    {
        time = 0;
        running = false;
    }

    public bool repeat { get; set; }

    public float duration {
        get { return _duration; } 
        set { _duration = Math.Max(0, value); } 
    }

    float progress { get { return time / duration; } }

    public float time { get; set; }

    public delegate void Callback();
}
