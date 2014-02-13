using UnityEngine;
using System.Collections;

public abstract class Pushable : MonoBehaviour
{
    protected Transform pushable;

    protected virtual void Start()
    {
        pushable = transform;
    }

    public abstract bool Push(Vector3 direction);
}
