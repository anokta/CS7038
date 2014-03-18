using UnityEngine;

public abstract class Pushable : Entity
{
    public bool MovingWithPlayer { get; protected set; }

    public bool SpoilHand = true;

    protected Pushable()
    {
        Explosive = true;
    }

    public virtual bool Push(Vector3 direction)
    {
        var canPush = CanPush(direction);

        if (canPush)
        {
        }

        return canPush;
    }

    public bool CanPush(Vector3 direction)
    {
        // Check collision
        var hit = Physics2D.Raycast(entity.position + direction, direction, 0.0f);

        return hit.collider == null || CanPush(hit, direction);
    }

    /// <summary>
    /// Specify restrictions for each entity type in derived classes.
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    protected virtual bool CanPush(RaycastHit2D hit, Vector3 direction)
    {
        return false;
    }
}
