using UnityEngine;
using System.Collections;
using HandyGestures;
using Grouping;

public class Fountain : Accessible
{
    private static bool isWashing;

    private bool isHeld;

    private Timer timer;

    PlayerController player;
    Vector2 lastPlayerDirection;

    private Animator animator;

	// public Material GUIpie;
    public Texture progressTexture;

    public float pieSize;
	ParticleSystem bubbles;

	public GameObject indicatorObject;
	private TimeIndicator indicator;


	protected override void Start()
	{
		base.Start();

        timer = new Timer(1.6f, Exit);
		animator = GetComponent<Animator>();

		GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));

        player = GameObject.FindObjectOfType<PlayerController>();
		bubbles = GetComponentInChildren<ParticleSystem>();
		bubbles.enableEmission = false;
		bubbles.Play();
        pieSize *= Screen.height;
		indicatorObject = Entity.Spawn(this.gameObject, indicatorObject);
		indicator = indicatorObject.GetComponent<TimeIndicator>();
		indicator.transform.position = indicator.transform.position + new Vector3(0, 0.95f, 0); 
		indicator.Receiver = timer.GetProgress;
		indicator.color = new Color(113f / 255f, 238f / 255f, 244f / 255f, 0.8f);
	}

	void LevelOver() {
		isWashing = false;
	}


    protected override void Update()
    {
       base.Update();

       timer.Update();

		if (isHeld) {
			if (player.NextDirection != lastPlayerDirection) 
            {
				Interrupted();
			}
			//if (!bubbles.isPlaying) {
			//		bubbles.Play();
			//}
			bubbles.enableEmission = true;
			animator.SetBool("Water", true);
		}
		else {
			//if (bubbles.isPlaying) {
			//		bubbles.Stop();
			//}
			bubbles.enableEmission = false;
			animator.SetBool("Water", false);
		}
    }

  //  void OnGUI()
  //  {
		//if (timer.progress > 0) {
		//	var GUIpie = GUIManager.GUIPie;
		//	GUIpie.color = new Color(113f / 255f, 238f / 255f, 244f / 255f);
		//	GUIpie.SetFloat("Value", timer.progress);
		//	GUIpie.SetFloat("Clockwise", 1);
		//
		//	Graphics.DrawTexture(new Rect(guiPosition.x - pieSize * 0.5f, guiPosition.y - pieSize * 0.5f, pieSize, pieSize), progressTexture, GUIpie);
			//Graphics.DrawTexture(new Rect(Screen.width * 0.5f - pieSize * 0.5f, Screen.height * 0.5f - pieSize * 0.5f, pieSize, pieSize), progressTexture, GUIpie);
		//}
  //  }

	public override bool Enter()
	{
        if (!isWashing && !isHeld)
        {
            player.AnimState = PlayerController.PlayerAnimState.Wash;

            isHeld = true;

            timer.Reset();

            lastPlayerDirection = player.NextDirection;

            AudioManager.PlaySFX("Loop Fountain");
			OnActivate();

            isWashing = true;
        }

		return false;
	}

    void Exit()
    {
        Interrupted();

        AudioManager.PlaySFX("Fountain");

		playerHand.RestoreHand(GetInstanceID());

		OnDeactivate();
        //playerHand.SpoilHand(HandController.MaxValue - HandController.InfectionThreshold, GetInstanceID());
    }

    void Interrupted()
    {
            AudioManager.StopSFX("Loop Fountain");

            isHeld = false;
            //if (bubbles.isPlaying) {
            //	bubbles.Stop();
            //}
            bubbles.enableEmission = false;
            timer.Stop();

            if (player.AnimState == PlayerController.PlayerAnimState.Wash)
            {
                player.AnimState = PlayerController.PlayerAnimState.Idle;
            }

            isWashing = false;
    }
}
