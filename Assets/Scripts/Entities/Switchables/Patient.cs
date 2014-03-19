using UnityEngine;
using System.Collections;

public class Patient : Switchable
{
    private bool treated;
	public Sprite treatedSprite;

    private bool isHeld;

    private Timer timer;

    PlayerController player;
    Vector2 lastPlayerDirection;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        treated = false;

        player = GameObject.FindObjectOfType<PlayerController>();

        timer = new Timer(0.6f, Success);
    }

    protected override void Update()
    {
        base.Update();

        timer.Update();

        if (isHeld)
        {
            if (!player.IsHeld || player.NextDirection != lastPlayerDirection)
            {
                Interrupted();
            }
        }
    }

    public bool IsTreated()
    {
        return treated;
    }

    public override void Switch()
    {
        if (!treated)
        {
            isHeld = true;

            timer.Reset();

            lastPlayerDirection = player.NextDirection;
        }
    }

    void Success()
    {
        spriteRenderer.sprite = treatedSprite;

        Interrupted();

        audioManager.PlaySFX("Treated");

        player.GetComponent<HandController>().value = HandController.MinValue;

        treated = true;
    }

    void Interrupted()
    {
        isHeld = false;

        timer.Stop();

        if (player.AnimState == PlayerController.PlayerAnimState.Wash)
        {
            player.AnimState = PlayerController.PlayerAnimState.Idle;
        }
    }
}
