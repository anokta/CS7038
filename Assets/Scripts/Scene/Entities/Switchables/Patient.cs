using UnityEngine;
using System.Collections;

public class Patient : Switchable
{
    private bool treated;

    private bool isHeld;

    private Timer timer;

    PlayerController player;
    Vector2 lastPlayerDirection;
	Animator animator;

	[SerializeField]
	public GameObject heart;

	[SerializeField]
	private GameObject ashes;

    public Material GUIpie;
    public Texture progressTexture;

    public float pieSize;
    Vector2 guiPosition;

    public Patient()
    {
        ExplosionHandler = new ExplosionTask(this);
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        treated = false;
		animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();

        pieSize *= Screen.height;

        timer = new Timer(0.8f, Finish);
    }

    protected override void Update()
    {
        base.Update();

		spriteRenderer.sortingOrder = LevelLoader.PlaceDepth(entity.position.y) - LevelLoader.UsableOffset;//- Mathf.RoundToInt(4 * entity.position.y) - 1;

        timer.Update();

        if (isHeld)
        {
            if(player.NextDirection != lastPlayerDirection)
            {
                Interrupted();
            }
        }
    }

    void OnGUI()
    {
		if (timer.progress > 0) {
			GUIpie.SetFloat("Value", timer.progress);
			GUIpie.SetFloat("Clockwise", 1);

			Graphics.DrawTexture(new Rect(guiPosition.x - pieSize * 0.5f, guiPosition.y - pieSize * 0.5f, pieSize, pieSize), progressTexture, GUIpie);
		}
    }


    public bool IsTreated()
    {
        return treated;
    }

    public override void Switch()
    {
        if (!treated && !isHeld)
        {
            player.AnimState = PlayerController.PlayerAnimState.Wash;

            isHeld = true;

            timer.Reset();
            AudioManager.PlaySFX("Loop Patient");

            lastPlayerDirection = player.NextDirection;

            Vector2 p = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(lastPlayerDirection.x * 0.25f, 0.5f + lastPlayerDirection.y * 0.25f, 0.0f));
            p.y = Screen.height - p.y;
            guiPosition = p;
        }
    }

    void Finish()
    {
        Interrupted();

        if (player.GetComponent<HandController>().state != HandController.HandState.Clean)
        {
            GameWorld.levelOverReason = GameWorld.LevelOverReason.PatientInfected;
			animator.SetTrigger("Kill");

            AudioManager.PlaySFX("Died");
        }
        else
        {
            int direction = 0;
            if (-lastPlayerDirection == new Vector2(0, -1))
            {
                direction = 0;
            }
            else if (-lastPlayerDirection == new Vector2(0, 1))
            {
                direction = 2;
            }
            else if (-lastPlayerDirection == new Vector2(1, 0))
            {
                direction = 1;
            }
            else if (-lastPlayerDirection == new Vector2(-1, 0))
            {
                direction = 3;
            }
            animator.SetInteger("Direction", direction);
			animator.SetTrigger("Treat");
			var h = Object.Instantiate(heart,
				new Vector3(transform.position.x + 0.25f, transform.position.y + 0.5f, transform.position.z),
				new Quaternion()) as GameObject;
			h.transform.parent = transform.parent;
			h.renderer.sortingOrder = short.MaxValue;

            AudioManager.PlaySFX("Treated");
        }

        playerHand.value = HandController.MinValue;

        treated = true;
    }

    public void Kill(GameWorld.LevelOverReason reason)
    {
        Interrupted();
        GameWorld.levelOverReason = reason;
		if (reason == GameWorld.LevelOverReason.PatientInfected) {
			animator.SetTrigger("Kill");
		} else {
			Entity.Replace(this.gameObject, ashes);
		}
			//animator.SetTrigger(reason == GameWorld.LevelOverReason.PatientInfected ? "Kill" : "Die");
		collider2D.enabled = false;
        treated = true;
    }

    void Interrupted()
    {
        AudioManager.StopSFX("Loop Patient");

        isHeld = false;

        timer.Stop(); 
        
        if (player.AnimState == PlayerController.PlayerAnimState.Wash)
        {
            player.AnimState = PlayerController.PlayerAnimState.Idle;
        }
    }

    public new class ExplosionTask : EntityExplosionTask
    {
        public Patient Patient { get; private set; }

        public ExplosionTask(Patient patient)
        {
            Patient = patient;
            Delay = 0;
        }

        public override void Run()
        {
            Patient.Kill(GameWorld.LevelOverReason.ExplosionKilledPatient);

            AudioManager.PlaySFX("Burn");
        }

        public override bool Equals(Task other)
        {
            var explosionTask = other as ExplosionTask;
            return explosionTask != null && Patient == explosionTask.Patient;
        }
    }
}
