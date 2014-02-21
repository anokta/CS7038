using UnityEngine;

public abstract class Pushable : Entity
{
    public bool MovingWithPlayer { get; protected set; }

    protected string Sfx;
    protected bool SpoilHand = true;

    public virtual bool Push(Vector3 direction)
    {
        // Check collisions
        var hit = Physics2D.Raycast(entity.position + direction, direction, 0.0f);

        var canPush = hit.collider == null || Push(hit, direction);

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
    /// Specify restrictions per each entity type.
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual bool Push(RaycastHit2D hit, Vector3 direction)
    {
        return false;
    }
}
