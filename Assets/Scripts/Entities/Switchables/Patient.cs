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

        timer = new Timer(0.5f, Finish);
    }

    protected override void Update()
    {
        base.Update();

        timer.Update();

        if (isHeld)
        {
            if(player.NextDirection != lastPlayerDirection)
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

    void Finish()
    {
        spriteRenderer.sprite = treatedSprite;

        Interrupted();

        if (player.GetComponent<HandController>().state != HandController.HandState.Clean)
        {
            GameWorld.success = false;
        }
        else
        {
            GameWorld.success &= true;

            audioManager.PlaySFX("Treated");
        }

        playerHand.value = HandController.MinValue;

        treated = true;
    }

    void Interrupted()
    {
        isHeld = false;

        timer.Stop();
    }
}
