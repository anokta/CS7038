using System.Collections.Generic;
using UnityEngine;
using Grouping;

public class LaserEmitter : Entity
{
    /// <summary>
    /// The direction that laser is going (not coming from)
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

    public LaserEmitter()
    {
        direction = Direction.Down;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        lineStrip = new LineStripRenderer(this, LaserMaterial, LaserColor, LaserWidth);

		GroupManager.main.group["To Level Over"].Add(this);
    }

    private readonly Vector2 offset = new Vector2(0, 0.3f);

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        var currDirection = Direction;
        var directionVector = currDirection.ToVector2();
        var origin = transform.position.xy();
        var points = new List<Vector3>();
        points.Add(origin + offset);

        // Set max iteration 20 to avoid infinite reflection
        for (var iteration = 0; iteration < 20; iteration++)
        {
            var hit = Physics2D.Raycast(origin + directionVector, directionVector, 100);  //TODO: change 100 to max level width

            DebugExt.Assert(hit.collider != null);
            if (hit.collider == null)
                break; // for robustness

            origin = hit.point;
            points.Add(origin.xy0() + new Vector3(0, 0.3f, 0));

            if (hit.transform.tag == "Player")
            {
                PlayerController player = hit.transform.GetComponent<PlayerController>();

                if (player.IsAlive)
                {
                    player.Die();

                    audioManager.PlaySFX("Laser Hit");
                }

                break;
            }

            if (hit.collider.name.StartsWith("Mirror"))
            {
                var mirror = hit.collider.GetComponent<Mirror>();
                if (mirror != null)
                {
                    currDirection = mirror.Reflect(currDirection);
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
                }
            }

			var plant = hit.collider.GetComponent<Plant>();
			if (plant != null) {
				plant.Burn();
			}

            break;
        }

        lineStrip.Draw(points);
    }
}
