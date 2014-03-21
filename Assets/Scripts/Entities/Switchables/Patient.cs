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

    public Material GUIpie;
    public Texture progressTexture;

    public float pieSize;
    Vector2 guiPosition;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        treated = false;

        player = GameObject.FindObjectOfType<PlayerController>();

        pieSize *= Screen.height;

        timer = new Timer(0.8f, Finish);
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

    void OnGUI()
    {
        GUIpie.SetFloat("Value", timer.progress);
        GUIpie.SetFloat("Clockwise", 1);

        Graphics.DrawTexture(new Rect(guiPosition.x - pieSize * 0.5f, guiPosition.y - pieSize * 0.5f, pieSize, pieSize), progressTexture, GUIpie);
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

            lastPlayerDirection = player.NextDirection;

            Vector2 p = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(lastPlayerDirection.x * 0.25f, 0.5f + lastPlayerDirection.y * 0.25f, 0.0f));
            p.y = Screen.height - p.y;
            guiPosition = p;
        }
    }

    void Finish()
    {
        spriteRenderer.sprite = treatedSprite;

        Interrupted();

        if (player.GetComponent<HandController>().state != HandController.HandState.Clean)
        {
            GameWorld.levelOverReason = GameWorld.LevelOverReason.PatientInfected;
        }
        else
        {
            //GameWorld.success &= true;TODO: see if it works after commented

            audioManager.PlaySFX("Treated");
        }

        playerHand.value = HandController.MinValue;

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
