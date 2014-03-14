using UnityEngine;
using System.Collections;
using HandyGestures;

public class Fountain : Accessible
{
    private bool isHeld;

    private Timer timer;

    //GameObject testObject;
    //private GUIText seconds;

    PlayerController player;
    Vector3 lastPlayerPosition;

    public Material GUIpie;
    public Texture progressTexture;
    Vector2 guiPosition;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();

        timer = new Timer(0.6f, Exit);

        //testObject = GameObject.Find("TEST");

        //if(seconds == null)
        //    seconds = (Instantiate(testObject) as GameObject).GetComponent<GUIText>();

        player = GameObject.FindObjectOfType<PlayerController>();
	}

    protected override void Update()
    {
       base.Update();

       timer.Update();

       if (isHeld)
       {
           //seconds.text = Mathf.RoundToInt(20 * timer.progress).ToString();

           if (!player.IsHeld || player.transform.position != lastPlayerPosition)
           {
               Interrupted();
           }
               
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
            isHeld = true;

            timer.Reset();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector2 p = Camera.main.WorldToScreenPoint(player.transform.position);
            p.y = Screen.height - p.y;

            guiPosition = p;
            
            //p = new Vector2(p.x/Screen.width, p.y/Screen.height + 0.1f);
            //seconds.transform.position = p;

            lastPlayerPosition = player.transform.position;
        }

		return false;
	}

    void Exit()
    {
        audioManager.PlaySFX("Fountain");

        player.GetComponent<HandController>().value = HandController.MaxValue;

        Interrupted();
    }

    void Interrupted()
    {
        isHeld = false;

        //seconds.text = "";

        timer.Stop();
    }
}
