using UnityEngine;

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

    public void Add(GameObject gameObj, Vector3 explosionSource)
    {
        var entity = gameObj.GetComponent<Entity>();
        if (entity != null)
        {
            if (entity.ExplosionHandler != null)
            {
                var handler = entity.ExplosionHandler;
                handler.ExplosionSource = explosionSource;
                TaskScheduler.Instance.Add(handler);

                const float sfxDelay = -0.02f;

                if(entity.name.StartsWith("ExplosiveCrate"))
                    AudioManager.PlaySFXDelayed("Explosion Crate", handler.Delay + sfxDelay);
            }
        }
        else if (gameObj.tag == "Player")
        {
            var player = gameObj.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.IsAlive)
                {
                    player.Die(GameWorld.LevelOverReason.ExplosionKilledPlayer);
                    AudioManager.PlaySFX("Burn");
                }
            }
        }
    }
}
