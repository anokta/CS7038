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
	private Ashes _ashController;

	//  public Material GUIpie;
    public Texture progressTexture;

    public float pieSize;

    public Patient()
    {
        ExplosionHandler = new ExplosionTask(this);
    }


	public GameObject indicatorObject;
	private TimeIndicator indicator;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        treated = false;
		animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();

        pieSize *= Screen.height;

        timer = new Timer(0.8f, Finish);

		indicatorObject = Entity.Spawn(this.gameObject, indicatorObject);
		indicator = indicatorObject.GetComponent<TimeIndicator>();
		indicator.transform.position = indicator.transform.position + new Vector3(0, 0.95f, 0); 
		indicator.Receiver = timer.GetProgress;
		//indicator.color = new Color(113f / 255f, 238f / 255f, 244f / 255f);
		indicator.color = new Color(1, 1, 1, 0.8f);

		heart = Object.Instantiate(heart,
		                           new Vector3(transform.position.x + 0.25f, transform.position.y + 0.5f, transform.position.z),
		                           new Quaternion()) as GameObject;
		heart.transform.parent = transform.parent;
		heart.renderer.sortingOrder = short.MaxValue;
		heart.SetActive(false);

		ashes = Entity.Spawn(gameObject, ashes);
		_ashController = ashes.GetComponent<Ashes>();
		ashes.SetActive(false);
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

	/* void OnGUI()
    {
		if (timer.progress > 0) {
			GUIManager.GUIPie.color = Color.white;
			GUIManager.GUIPie.SetFloat("Value", timer.progress);
			GUIManager.GUIPie.SetFloat("Clockwise", 1);
			 
			Graphics.DrawTexture(new Rect(guiPosition.x - pieSize * 0.5f, guiPosition.y - pieSize * 0.5f, pieSize, pieSize), progressTexture, GUIManager.GUIPie);
		}
    }*/


    public bool IsTreated()
    {
        return treated;
    }

    public override void Switch(bool byPlayer)
    {
        if (byPlayer && !treated && !isHeld)
        {
            player.AnimState = PlayerController.PlayerAnimState.Treat;

            isHeld = true;

            timer.Reset();
            AudioManager.PlaySFX("Loop Patient");

            lastPlayerDirection = player.NextDirection;
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
			heart.SetActive(true);

            AudioManager.PlaySFX("Treated");
        }

       // playerHand.value = HandController.MinValue;
		playerHand.ResetHand(GetInstanceID());

        treated = true;
    }

    public void Kill(GameWorld.LevelOverReason reason)
    {
        Interrupted();
        GameWorld.levelOverReason = reason;
		if (reason == GameWorld.LevelOverReason.PatientInfected) {
			animator.SetTrigger("Kill");
		} else {
			_ashController.Trigger(entity.position);
			Destroy(this.gameObject);
			//Entity.Replace(this.gameObject, ashes);
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
        
        if (player.AnimState == PlayerController.PlayerAnimState.Treat)
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
