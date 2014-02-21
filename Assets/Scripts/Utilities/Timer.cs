using UnityEngine;
using System;

/// <summary>
/// Executes an event after a specific time.
/// </summary>
public class Timer
{
    public float time { get; set; }

    float _duration;
    public float duration
    {
        get { return _duration; }
        set { _duration = Math.Max(0, value); }
    }

    public float progress { get { return time / duration; } }

    public bool running { get; private set; }
    public bool repeating { get; set; }

    public Action Complete;

    public Timer()
    {
    }

    public Timer(float duration)
    {
        _duration = duration;
    }

    public Timer(float duration, Action complete)
        : this()
    {
        _duration = duration;
        Complete = complete;
    }

    public void Update()
    {
        if (!running) {
            return;
        }
        time += Time.deltaTime;
        while (time >= _duration) {
            time -= _duration;
            Complete();
            if (!repeating) {
                Stop();
            }
        }
    }

    public void ForceInvoke()
    {
        Complete();
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

    public void Stop()
    {
        time = 0;
        running = false;
    }
}
