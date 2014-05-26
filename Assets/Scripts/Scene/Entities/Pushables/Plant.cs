using UnityEngine;

public class Plant : Crate
{
    public GameObject fire;
    public GameObject ashes;
	private Ashes _ashController;
//    private Timer clock;

    public Plant()
    {
        ExplosionHandler = new ExplosionTask(this);
    }

    protected override void Start()
    {
        base.Start();
     //   clock = new Timer(1, Break);
       // clock.Stop();
		ashes = Entity.Spawn(gameObject, ashes);
		_ashController = ashes.GetComponent<Ashes>();
		ashes.SetActive(false);
    }

    public void Break()
    {
        AudioManager.PlaySFX("Burn");
		Execute(Trigger.ActionType.Break);
		_ashController.Trigger(entity.position);
		Destroy(this.gameObject);
    }

    public new class ExplosionTask : EntityExplosionTask
    {
        public Plant Plant { get; private set; }

        public ExplosionTask(Plant plant)
        {
            Plant = plant;
            Delay = 0;
        }

        public override void Run()
        {
            Plant.Break();
        }

        public override bool Equals(Task other)
        {
            var explosionTask = other as ExplosionTask;
            return explosionTask != null && Plant == explosionTask.Plant;
        }
    }
}
