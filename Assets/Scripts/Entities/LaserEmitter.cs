using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : Entity
{
    /// <summary>
    /// The direction that laser is going (not coming from)
    /// </summary>
    private Direction direction;

    public Direction Direction
    {
        get { return direction; }
        set
        {
            direction = value;
            switch (direction)
            {
                case Direction.Up:
                    spriteRenderer.sprite = LaserUp;
                    break;
                case Direction.Down:
                    spriteRenderer.sprite = LaserDown;
                    break;
                case Direction.Left:
                    spriteRenderer.sprite = LaserLeft;
                    break;
                case Direction.Right:
                    spriteRenderer.sprite = LaserRight;
                    break;
            }
        }
    }

    private LineStripRenderer lineStrip;
    public Sprite LaserUp;
    public Sprite LaserDown;
    public Sprite LaserLeft;
    public Sprite LaserRight;

    private int lastExplosiveID;

    public LaserEmitter()
    {
        direction = Direction.Down;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        lineStrip = new LineStripRenderer(this);
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

        for (; ; )
        {
            var hit = Physics2D.Raycast(origin + directionVector, directionVector, 100);  //TODO: change 100 to max level width

            DebugExt.Assert(hit.collider != null);
            if (hit.collider == null)
                break; // for robustness

            origin = hit.point;
            points.Add(origin.xy0() + new Vector3(0, 0.3f, 0));

            if (hit.transform.tag == "Player")
            {
                audioManager.PlaySFX("Laser Hit");

                GameWorld.success = false;

                Grouping.GroupManager.main.activeGroup = Grouping.GroupManager.main.group["Level Over"];

                return;
            }

            var mirror = hit.collider.GetComponent<Mirror>();
            if (mirror != null)
            {
                currDirection = mirror.Reflect(currDirection);
                directionVector = currDirection.ToVector2();
                continue;
            }

            if (hit.collider.name.StartsWith("Explosive"))
            {
                if (lastExplosiveID != hit.transform.GetInstanceID())
                {
                    lastExplosiveID = hit.transform.GetInstanceID();

                    ExplosionManager.Instance.Add(hit.collider.gameObject);
                }
            }

            break;
        }

        lineStrip.Draw(points);
    }
}
