using UnityEngine;
using System.Collections;

public class Lever : Switchable {

    public Gate gate;

	public Sprite leverOn;
	public Sprite leverOff;
    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override void Switch()
    {
        audioManager.PlaySFX("Lever");

        gate.ToggleLock();
		var renderer = this.GetComponent<SpriteRenderer>();

		if (gate.isLocked) {
			renderer.sprite = leverOff;
		}
		else {
			renderer.sprite = leverOn;
		}

        var controller = FindObjectOfType<PlayerController>();
        controller.spoilHand(0.5f);
    }
}
