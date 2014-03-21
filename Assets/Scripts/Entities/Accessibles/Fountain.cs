using UnityEngine;
using System.Collections;
using HandyGestures;

public class Fountain : Accessible
{
    private bool isHeld;

    private Timer timer;

    PlayerController player;
    Vector2 lastPlayerDirection;

    private Animator animator;

    public Material GUIpie;
    public Texture progressTexture;

    public float pieSize;
    Vector2 guiPosition;


	protected override void Start()
	{
		base.Start();

        timer = new Timer(1.6f, Exit);
		animator = GetComponent<Animator>();

        player = GameObject.FindObjectOfType<PlayerController>();

        pieSize *= Screen.height;
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
               
			animator.SetBool("Water", true);
		}
		else {
			animator.SetBool("Water", false);
		}
    }

    void OnGUI()
    {
        GUIpie.SetFloat("Value", timer.progress);
        GUIpie.SetFloat("Clockwise", 1);

        Graphics.DrawTexture(new Rect(guiPosition.x - pieSize * 0.5f, guiPosition.y - pieSize * 0.5f, pieSize, pieSize), progressTexture, GUIpie);
        //Graphics.DrawTexture(new Rect(Screen.width * 0.5f - pieSize * 0.5f, Screen.height * 0.5f - pieSize * 0.5f, pieSize, pieSize), progressTexture, GUIpie);
    }

	public override bool Enter()
	{
        if (!isHeld)
        {
            player.AnimState = PlayerController.PlayerAnimState.Wash;

            isHeld = true;

            timer.Reset();

            lastPlayerDirection = player.NextDirection;

            Vector2 p = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(lastPlayerDirection.x * 0.25f, 0.5f + lastPlayerDirection.y * 0.25f, 0.0f));
            p.y = Screen.height - p.y;
            guiPosition = p;

            audioManager.PlaySFX("Loop Fountain");
        }

		return false;
	}

    void Exit()
    {
        Interrupted();

        audioManager.PlaySFX("Fountain");

        playerHand.SpoilHand(HandController.MaxValue, GetInstanceID());
    }

    void Interrupted()
    {
        audioManager.StopSFX("Loop Fountain");

        isHeld = false;

        timer.Stop();

        if (player.AnimState == PlayerController.PlayerAnimState.Wash)
        {
            player.AnimState = PlayerController.PlayerAnimState.Idle;
        }
    }
}
