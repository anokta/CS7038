using UnityEngine;
using System.Collections;

public class Sanitizer : Collectible
{

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
	}

	public override void Collect()
	{
		audioManager.PlaySFX("Collect");
		var state = GameObject.FindObjectOfType<HandController>().state;
		if (state == HandController.HandState.Clean) {
			++GlobalState.score;
		}
		else if (state == HandController.HandState.Filthy) {
			--GlobalState.score;
		}

        playerHand.SpoilHand(1.0f, GetInstanceID());

		base.Collect();
	}
}
