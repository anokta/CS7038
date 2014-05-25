using UnityEngine;
using System.Collections;

public class RectTrigger : Trigger
{

	public TriggerAction action;

	Transform playerTransform;
	
	// Update is called once per frame
	void Update()
	{
		if (playerTransform == null) {
			playerTransform = Object.FindObjectOfType<HandController>().transform;
		}
		if (action != null && action.area.Contains(playerTransform.position)) {
			action.OnRun();
			if (!action.repeat) {
				Destroy(this.gameObject);
			}
		}
	}
}

