using System.Collections.Generic;
using UnityEngine;
using Grouping;
using System.Security.Permissions;
using UnityEditor;
using System;

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
	//private List<Vector2> previousEndpoints;

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

		//previousEndpoints = new List<Vector2> { transform.position.xy() };
		AddPrev(transform.position.xy());
    }
		
	//Hardcoded values for vertical laser facing upwards
	private readonly static float wallOffset = -0.17f;
	private readonly static float gateOffset = 0.16f;
	private readonly static float doorOffset = 0.23f;
	private readonly static float leverOffset = 0.13f;
	private readonly static float trolleyOffset = -0.07f;
	private readonly static float otherOffset = 0;
	private readonly static float turretOffset = 0.24f;

	private const int MaxEndpoints = 20;
	int endpointSize = 0;
	int offsetSize = 0;
	int prevSize = 0;

	private void AddPoint(Vector2 point) {
		endpoints[endpointSize++] = point;
	}

	private void AddOffset(int offset) {
		offsets[offsetSize++] = offset;
	}

	private void AddPrev(Vector2 point) {
		prevPoints[prevSize++] = point;
	}

	private void SwapPoints() {
		var temp = endpoints;
		var tempSize = endpointSize;
		endpoints = prevPoints;
		endpointSize = prevSize;
		prevPoints = temp;
		prevSize = tempSize;
	}

	/// <summary>
	/// The endpoints in the current update
	/// </summary>
	Vector2[] endpoints = new Vector2[MaxEndpoints];
	int[] offsets = new int[MaxEndpoints];
	/// <summary>
	/// The endpoints from the previous update
	/// </summary>
	Vector2[] prevPoints = new Vector2[MaxEndpoints];

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        var currDirection = Direction;
        var directionVector = currDirection.ToVector2();
        var endpoint = transform.position.xy();

		//Swap prev points with current endpoints
		SwapPoints();
		//Clear the endpoint and offset list
		endpointSize = 0;
		offsetSize = 0;
		//Init
		AddPoint(endpoint);
		AddOffset(-1);

        var movement = Time.deltaTime * LaserSpeed;

		bool gorillaTape = true;

		bool isMirror = false;

        // Set max iteration 20 to avoid infinite reflection
		for (var endpointIndex = 1; endpointIndex < MaxEndpoints; endpointIndex++)
        {
			var hit = Physics2D.Raycast(endpoint + directionVector, directionVector, 100);  //TODO: change 100 to max level width
			//Debug.Log(endpointIndex);
            DebugExt.Assert(hit.collider != null);
            if (hit.collider == null)
                break; // for robustness

			string hitName = hit.collider.name;
			isMirror = hitName.StartsWith("Mirror");

			var tPos = hit.collider.transform.position;

            if (directionVector.x.IsZero())
            {
				//Adjust the hit point when hitting from below
				if (directionVector.y > 0 && !isMirror) {

					float offset = 0;
					float dist;

					if (hitName.StartsWith("Wall")) {
						offset = wallOffset;
					} else if (hitName.StartsWith("Gate")) {
						offset = gateOffset;
					} else if (hitName.StartsWith("Door")) {
						offset = doorOffset;
					} else if (hitName.StartsWith("Lever")) {
						offset = leverOffset;
					} else if (hitName.StartsWith("Trolley")) {
						offset = trolleyOffset;
					} else if (hitName.StartsWith("LaserEmitter")) {
						if (hit.collider.gameObject.GetComponent<LaserEmitter>().direction != Direction.Down) {
							offset = turretOffset;
						}
					} else {
						offset = otherOffset;
					}

					//Fixes issue for when object is too close to the laser
					//This assumes the object's collider is 1 unit high
					if ((dist = tPos.y - endpoint.y) < 1.5f) {
						offset -= 1.5f - dist;
					}

					endpoint.y = hit.point.y + offset;

					//gorillaTape = hit.point;
				} else {
					endpoint.y = hit.collider.transform.position.y;
				//Debug.Log("whoop-di-doo");
				}
            }
            else
            {
                endpoint.x = hit.collider.transform.position.x;
            }

            Vector2? previousEndpoint = null;

			if (endpointIndex >= prevSize)
            {
                previousEndpoint = endpoints[endpointIndex - 1];
            }
			else if (!endpoint.Equals(prevPoints[endpointIndex]))
            {
				previousEndpoint = prevPoints[endpointIndex];
				var previousDirection = previousEndpoint.Value - prevPoints[endpointIndex - 1];
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
					AddPoint(newEndpoint);
                    break;
                }
                movement += diff;
            }

			
			AddPoint(endpoint);
			

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
			else if (isMirror)
            {
                var mirror = hit.collider.GetComponent<Mirror>();
                if (mirror != null)
                {
                    var oldDirection = currDirection;
                    currDirection = mirror.Reflect(currDirection);

                    var sortingOrderOffset = oldDirection == Direction.Down || currDirection == Direction.Up ? -1 : 1;
					AddOffset(sortingOrderOffset);

                    directionVector = currDirection.ToVector2();
                    continue;
                }
            }
			else if (hitName.StartsWith("Explosive"))
            {
                if (lastExplosiveID != hit.transform.GetInstanceID())
                {
                    lastExplosiveID = hit.transform.GetInstanceID();

                    ExplosionManager.Instance.Add(hit.collider.gameObject, hit.collider.transform.position);
                }
            }
			else if (hitName.StartsWith("Patient"))
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

		//Laser should be in front of the object when hitting from below
		if (directionVector.y > 0 && !isMirror) {
			AddOffset(5);
		} else {
			AddOffset(-5);
		}
		lineStrip.Draw(endpoints, offsets, endpointSize, LaserPositionOffset);

		//previousEndpoints = endpoints;
    }
}
