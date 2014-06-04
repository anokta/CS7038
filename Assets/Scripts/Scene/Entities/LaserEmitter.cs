using System.Collections.Generic;
using UnityEngine;
using Grouping;
using System.Security.Permissions;

public class LaserEmitter : Entity
{
    /// <summary>
    /// The direction that laser is going to (not coming from).
    /// For example: Right means the laser direction is left to right.
    /// </summary>
    private Direction direction;

    public Material LaserMaterial;
    public Color LaserColor;
    public float LaserWidth;

    public Direction Direction
    {
        get { return direction; }
        set
        {
            direction = value;
            switch (direction)
            {
                case Direction.Up:
                    spriteRenderer.sprite = EmitterUp;
                    break;
                case Direction.Down:
                    spriteRenderer.sprite = EmitterDown;
                    break;
                case Direction.Left:
                    spriteRenderer.sprite = EmitterLeft;
                    break;
                case Direction.Right:
                    spriteRenderer.sprite = EmitterRight;
                    break;
            }
        }
    }

    private LineStripRenderer lineStrip;
    public Sprite EmitterUp;
    public Sprite EmitterDown;
    public Sprite EmitterLeft;
    public Sprite EmitterRight;

    private int lastExplosiveID;

    private static readonly Vector2 LaserPositionOffset = new Vector2(0, 0.3f);
	public float LaserSpeed = 40;
    private List<Vector2> previousEndpoints;

    public LaserEmitter()
    {
        direction = Direction.Down;
    }

	//private struct LaserHit

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        lineStrip = new LineStripRenderer(this, LaserMaterial, LaserColor, LaserWidth);

        GroupManager.main.group["To Level Over"].Add(this);

        previousEndpoints = new List<Vector2> { transform.position.xy() };
    }

	/*private struct LaserHit
	{
		Vector2 hitPosition;
		int 
	}*/

	public float off1 = 3;
	public float off2 = 1.5f;


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        var currDirection = Direction;
        var directionVector = currDirection.ToVector2();
        var endpoint = transform.position.xy();

        var endpoints = new List<Vector2>();
        endpoints.Add(endpoint);

        var sortingOrderOffsets = new List<int>();
		sortingOrderOffsets.Add(-1);

        var movement = Time.deltaTime * LaserSpeed;

        // Set max iteration 20 to avoid infinite reflection
        for (var endpointIndex = 1; endpointIndex < 20; endpointIndex++)
        {
            var hit = Physics2D.Raycast(endpoint + directionVector, directionVector, 100);  //TODO: change 100 to max level width
			//Debug.Log(endpointIndex);
            DebugExt.Assert(hit.collider != null);
            if (hit.collider == null)
                break; // for robustness

            if (directionVector.x.IsZero())
            {
                endpoint.y = hit.collider.transform.position.y;
            }
            else
            {
                endpoint.x = hit.collider.transform.position.x;
            }

            Vector2? previousEndpoint = null;

            if (endpointIndex >= previousEndpoints.Count)
            {
                previousEndpoint = endpoints[endpointIndex - 1];
            }
            else if (!endpoint.Equals(previousEndpoints[endpointIndex]))
            {
                previousEndpoint = previousEndpoints[endpointIndex];
                var previousDirection = previousEndpoint.Value - previousEndpoints[endpointIndex - 1];
                previousDirection.Normalize();
                if (directionVector != previousDirection)
                {
                    previousEndpoint = endpoints[endpointIndex - 1];
                }
            }

            if (previousEndpoint.HasValue)
            {
                var newEndpoint = previousEndpoint.Value + movement * directionVector;

                var diff = Vector2.Dot(endpoint - newEndpoint, directionVector);

                if (diff > 0)
                {
					endpoints.Add(newEndpoint);
					//sortingOrderOffsets.Add(100);
                    break;
                }
                movement += diff;
            }

            endpoints.Add(endpoint);

			if (directionVector.y > 0) {
				//endpoints.Add(previousEndpoint.Value);
				//sortingOrderOffsets.Add(-1); 	 	
				//endpoints.Add(hit.point);
				//sortingOrderOffsets.Add(1);
				/*endpoints.Add(hit.transform.position.xy() - new Vector2(0, off1));
				sortingOrderOffsets.Add(-1);
				endpoints.Add(hit.transform.position.xy() - new Vector2(0, off2));
				sortingOrderOffsets.Add(1);*/

			}

            if (hit.transform.tag == "Player")
            {
                var player = hit.transform.GetComponent<PlayerController>();

                if (player.IsAlive)
                {
                    player.Die();

                    AudioManager.PlaySFX("Laser Hit");
                }

                break;
			}
			else if (hit.collider.name.StartsWith("Mirror"))
            {
                var mirror = hit.collider.GetComponent<Mirror>();
                if (mirror != null)
                {
                    var oldDirection = currDirection;
                    currDirection = mirror.Reflect(currDirection);

                    var sortingOrderOffset = oldDirection == Direction.Down || currDirection == Direction.Up ? -1 : 1;
                    sortingOrderOffsets.Add(sortingOrderOffset);

                    directionVector = currDirection.ToVector2();
                    continue;
                }
            }
            else if (hit.collider.name.StartsWith("Explosive"))
            {
                if (lastExplosiveID != hit.transform.GetInstanceID())
                {
                    lastExplosiveID = hit.transform.GetInstanceID();

                    ExplosionManager.Instance.Add(hit.collider.gameObject, hit.collider.transform.position);
                }
            }
            else if (hit.collider.name.StartsWith("Patient"))
            {
                var patient = hit.collider.GetComponent<Patient>();
                if (patient != null)
                {
                    patient.Kill(GameWorld.LevelOverReason.LaserKilledPatient);

                    AudioManager.PlaySFX("Laser Hit");
                }
            }

            var plant = hit.collider.GetComponent<Plant>();
            if (plant != null)
            {
                plant.Break();
            }

            break;
        }
			
		sortingOrderOffsets.Add(-1);
        lineStrip.Draw(endpoints, sortingOrderOffsets, LaserPositionOffset);

        previousEndpoints = endpoints;
    }
}
