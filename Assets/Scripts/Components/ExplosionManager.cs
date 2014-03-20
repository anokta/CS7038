using UnityEngine;
using Object = UnityEngine.Object;

public class ExplosionManager
{
    private static ExplosionManager instance;
    public static ExplosionManager Instance
    {
        get { return instance ?? (instance = new ExplosionManager()); }
    }

    public readonly GameObject ExplosionPrefab;

    public ExplosionManager()
    {
        ExplosionPrefab = Resources.Load<GameObject>("Explosion");
    }

    public void Add(GameObject gameObj, float delay = 0.2f)
    {
        var entity = gameObj.GetComponent<Entity>();
        if (entity != null && entity.Explosive)
        {
            var task = new ExplosionTask(gameObj);
            TaskScheduler.Instance.Add(delay, task);

            AudioManager.PlaySfxDelayed("Explosion Crate", delay - 0.02f);
        }
    }
}

public class ExplosionTask : Task
{
    public GameObject GameObj { get; private set; }

    public ExplosionTask(GameObject gameObj)
    {
        GameObj = gameObj;
    }

    public override void Run()
    {
        if (GameObj == null) return;

        Object.Instantiate(ExplosionManager.Instance.ExplosionPrefab, GameObj.transform.position, Quaternion.identity);
        Object.Destroy(GameObj.gameObject);

        foreach (var direction in DirectionExt.Values)
        {
            var directionVector = direction.ToVector2();
            var hit = Physics2D.Raycast(GameObj.transform.position, directionVector, 1);
            if (hit.collider != null)
            {
                ExplosionManager.Instance.Add(hit.collider.gameObject);
            }
        }
    }

    public override bool Equals(Task other)
    {
        var explosionTask = other as ExplosionTask;
        return explosionTask != null && GameObj == explosionTask.GameObj;
    }
}
