using UnityEngine;
using System.Collections;
using HandyGestures;
using System;

public class PlayerController : MonoBehaviour, IPan
{    
	private Transform player;
	public enum HandState
	{
		Clean,
		Dirty,
		Filthy
	}

	public HandState handState { get; set; }

	public HandState improveHand()
	{
		if (handState == HandState.Dirty) {
			handState = HandState.Clean;
		} else if (handState == HandState.Filthy) {
			handState = HandState.Dirty;
		}
		
		return handState;
	}

	public HandState spoilHand()
	{
		if (handState == HandState.Clean) {
			handState = HandState.Dirty;
		} else if (handState == HandState.Dirty) {
			handState = HandState.Filthy;
		}
		
		return handState;
	}

	// Use this for initialization
	void Start()
	{
		player = transform;
		movement = new Clock();
		movement.duration = 0.45f;
		movement.Run += () => moving = true;

		var detector = GameObject.FindObjectOfType<HandyDetector>();
		if (detector != null) {
			detector.defaultObject = this.gameObject;
		}
	}

	int moveX;
	int moveY;
	bool moving;
	Clock movement;

    #region Gestures

	public void OnGesturePan(PanArgs args)
	{
		if (args.state == PanArgs.State.Move) {
			moveX = 0;
			moveY = 0;
			if (Math.Abs(args.delta.x - args.delta.y) >= 1f) {
				if (Math.Abs(args.delta.x) > Math.Abs(args.delta.y)) {
					moveX = args.delta.x < 0 ? 1 : -1;
				} else {
					moveY = args.delta.y < 0 ? 1 : -1;
				}
			}
			if (!movement.running && !moving) {
				moving = true;
				movement.Reset();
			}
		}
		if (args.state == PanArgs.State.Up || args.state == PanArgs.State.Interrupt) {
			movement.Stop();
			moving = false;
		}
	}

    #endregion

	// Update is called once per frame
	void Update()
	{
		if (GameEventManager.CurrentState == GameEventManager.GameState.Running) {
			// Get Input
			//int dx = Input.GetButtonDown("Horizontal") ? (int)Input.GetAxisRaw("Horizontal") : 0;
			//int dy = Input.GetButtonDown("Vertical") ? (int)Input.GetAxisRaw("Vertical") : 0;
			movement.Update();
			int dx = (moving) ? moveX : 0;
			int dy = (moving) ? moveY : 0;
			moving = false;

			// If input is valid
			if (dx != 0 || dy != 0) {
				// Get the next direction
				Vector3 direction = new Vector3(dx, dy, 0);

				// Check collisions
				RaycastHit2D hit = Physics2D.Raycast(player.position + direction, direction, 0.0f);
				if (hit != null && hit.collider != null) {
					// Collision detected
					Debug.Log("Collided with " + hit.collider.name + " [" + hit.collider.tag + "].");
					switch (hit.collider.tag) {
						case "Wall":
							break;

						case "Pushable":
							Pushable pushable = hit.collider.GetComponent<Pushable>();
							if (pushable.Push(direction)) {
								player.position += direction;
							}

							break;

						case "Collectible":
							Collectible collectible = hit.collider.GetComponent<Collectible>();
                            
							collectible.Collect();

							player.position += direction;

							break;

						case "Accessible":
							Accessible accessible = hit.collider.GetComponent<Accessible>();

							if (accessible.Enter()) {
								player.position += direction;
							}

							break;

						default:
							break;
					}
				} else {
					// Translate if not collided
					player.position += direction;
				}
			}
		}
	}
}
