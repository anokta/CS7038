using UnityEngine;

public abstract class Pushable : Entity
{
    public bool MovingWithPlayer { get; protected set; }

    protected string Sfx;
    protected bool SpoilHand = true;

    public virtual bool Push(Vector3 direction)
    {
        var canPush = CanPush(direction);

        if (canPush)
        {
            if (Sfx != null)
            {
                audioManager.PlaySFX(Sfx);
            }

            if (SpoilHand)
            {
                var controller = FindObjectOfType<PlayerController>();
                controller.spoilHand();
            }
        }

        return canPush;
    }

    /// <summary>
    /// Specify restrictions per each entity type. TODO: the method name doesn't seem to be intuitive enough
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    protected virtual bool Push(RaycastHit2D hit, Vector3 direction)
    {
        return false;
    }

    public bool CanPush(Vector3 direction)
    {
        // Check collision
        var hit = Physics2D.Raycast(entity.position + direction, direction, 0.0f);

        return hit.collider == null || Push(hit, direction);
    }
}
