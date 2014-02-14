using UnityEngine;
using System.Collections;

public class Trolley : Pushable
{

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public override bool Push(Vector3 direction)
	{
		bool spoiled = false;
		// Check collisions
		while (true) {
			RaycastHit2D hit = Physics2D.Raycast(entity.position + direction, direction, 0.0f);
			if (hit != null && hit.collider != null) {
				// Collision detected
				switch (hit.collider.tag) {
				/* TODO: Specify restrictions per each entity type if needed later. */
					case "Pushable":
						if (hit.collider.name.StartsWith("Trolley")) {
							hit.collider.GetComponent<Pushable>().Push(direction);

							// OPTIONAL: Should it stay still or replace the original position? (needs an additional check on the next push value if used)
							//entity.position += direction;
						}
						break;
				}

				return false;
			} else {
				audioManager.PlaySFX("Push Trolley");
				if (!spoiled) {
					var controller = GameObject.FindObjectOfType<PlayerController>();
					controller.spoilHand();
					spoiled = true;
				}
				// Translate if not collided
				entity.position += direction;
			}
		}
	}
}
