using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour
{
    Transform pushable;

    // Use this for initialization
    void Start()
    {
        pushable = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Push(Vector3 direction)
    {
        // Check collisions
        RaycastHit2D hit = Physics2D.Raycast(pushable.position + direction, direction, 0.0f);
        if (hit != null && hit.collider != null)
        {
            // Collision detected
            switch (hit.collider.tag)
            {
                /* TODO: Specify restrictions per each entity type if needed later. */
                default:
                    return false;
            }
        }
        else
        {
            // Translate if not collided
            pushable.position += direction;

            return true;
        }
    }
}
