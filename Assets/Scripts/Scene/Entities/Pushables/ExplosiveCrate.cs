using UnityEngine;

public class ExplosiveCrate : Crate
{
    public ExplosiveCrate()
    {
        ExplosionHandler = new ExplosionTask(this);
    }

    public new class ExplosionTask : EntityExplosionTask
    {
        public ExplosiveCrate ExplosiveCrate { get; private set; }

        public ExplosionTask(ExplosiveCrate explosiveCrate)
        {
            ExplosiveCrate = explosiveCrate;
            Delay = 0.2f;
        }

        public override void Run()
        {
            if (ExplosiveCrate == null) return;

            var explosionSource = ExplosiveCrate.transform.position;
            Destroy(ExplosiveCrate.gameObject);

            var explosion = Instantiate(ExplosionManager.Instance.ExplosionPrefab, explosionSource, Quaternion.identity) as GameObject;
            explosion.transform.parent = LevelLoader.Instance.ExplosionContainer.transform;

            foreach (var direction in DirectionExt.Values)
            {
                var directionVector = direction.ToVector2();
                var hit = Physics2D.Raycast(explosionSource.xy() + directionVector, directionVector, 0);
                if (hit.collider != null)
                {
                    ExplosionManager.Instance.Add(hit.collider.gameObject, explosionSource);
                }
            }

			ExplosiveCrate.Execute(Trigger.ActionType.Break);
        }

        public override bool Equals(Task other)
        {
            var explosionTask = other as ExplosionTask;
            return explosionTask != null && ExplosiveCrate == explosionTask.ExplosiveCrate;
        }
    }
}
