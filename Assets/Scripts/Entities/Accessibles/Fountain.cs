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
    Vector2 guiPosition;


	protected override void Start()
	{
		base.Start();

        timer = new Timer(2f, Exit);
		animator = GetComponent<Animator>();

        player = GameObject.FindObjectOfType<PlayerController>();
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
        GUIpie.SetFloat("Clockwise", 0);

        Graphics.DrawTexture(new Rect(guiPosition.x - Screen.width * 0.01f, guiPosition.y - Screen.width * 0.04f, Screen.width * 0.02f, Screen.width * 0.02f), progressTexture, GUIpie);
    }

	public override bool Enter()
	{
        if (!isHeld)
        {
            player.AnimState = PlayerController.PlayerAnimState.Wash;

            isHeld = true;

            timer.Reset();

            Vector2 p = Camera.main.WorldToScreenPoint(player.transform.position);
            p.y = Screen.height - p.y;

            guiPosition = p;
            
            lastPlayerDirection = player.NextDirection;

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
