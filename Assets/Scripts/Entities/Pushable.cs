using UnityEngine;
using System.Collections;

public abstract class Pushable : Entity
{
    public abstract bool Push(Vector3 direction);
}
