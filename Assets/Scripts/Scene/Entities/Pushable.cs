using UnityEngine;

public abstract class Pushable : Entity
{
    public bool MovingWithPlayer { get; protected set; }

    public bool SpoilHand = true;

    protected Pushable()
    {
        ExplosionHandler = new ExplosionTask(this);
    }

    protected override void Start()
    {
        base.Start();
    }

    public virtual bool Push(Vector3 direction, bool byPlayer = true)
    {
        var canPush = CanPush(direction);

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

    public class ExplosionTask : EntityExplosionTask
    {
        public Pushable Pushable { get; private set; }

        public ExplosionTask(Pushable pushable)
        {
            Pushable = pushable;
            Delay = 0;
        }

        public override void Run()
        {
            var direction = Pushable.Position - ExplosionSource;
            direction.Normalize();

            HandController hand = FindObjectOfType<HandController>();
            float value = hand.value;
            Pushable.Push(direction, false);
            hand.value = value;
        }

        public override bool Equals(Task other)
        {
            var explosionTask = other as ExplosionTask;
            return explosionTask != null && Pushable == explosionTask.Pushable;
        }
    }
}
