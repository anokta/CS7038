using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Transform player;

    // Use this for initialization
    void Start()
    {
        player = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEventManager.CurrentState == GameEventManager.GameState.Running)
        {
            // Get Input
            int dx = Input.GetButtonDown("Horizontal") ? (int)Input.GetAxisRaw("Horizontal") : 0;
            int dy = Input.GetButtonDown("Vertical") ? (int)Input.GetAxisRaw("Vertical") : 0;

            // If input is valid
            if (dx != 0 || dy != 0)
            {
                // Get the next direction
                Vector3 direction = new Vector3(dx, dy, 0);

                // Check collisions
                RaycastHit2D hit = Physics2D.Raycast(player.position + direction, direction, 0.0f);
                if (hit != null && hit.collider != null)
                {
                    // Collision detected
                    Debug.Log("Collided with " + hit.collider.name + " [" + hit.collider.tag + "].");
                    switch (hit.collider.tag)
                    {
                        case "Wall":
                            break;

                        case "Pushable":
                            Pushable pushable = hit.collider.GetComponent<Pushable>();
                            if (pushable.Push(direction))
                                player.position += direction;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    // Translate if not collided
                    player.position += direction;
                }
            }
        }
    }
}
