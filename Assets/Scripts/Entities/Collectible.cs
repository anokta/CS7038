using UnityEngine;
using System.Collections;

public abstract class Collectible : Entity
{
    public virtual void Collect()
    {
        Destroy(gameObject);
    }
}
