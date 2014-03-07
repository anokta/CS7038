using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : Entity
{
    /// <summary>
    /// The direction that laser is going (not coming from)
    /// </summary>
    public Direction Direction;
    private LineStripRenderer lineStrip;
    public Sprite LaserUp;
    public Sprite LaserDown;
    public Sprite LaserLeft;
    public Sprite LaserRight;

    public LaserEmitter()
    {
        Direction = Direction.Down;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        lineStrip = new LineStripRenderer(this);
    }

    private Vector2 height = new Vector2(0, 0.3f);

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Direction == Direction.Down && spriteRenderer.sprite != LaserDown)
        {
            spriteRenderer.sprite = LaserDown;
        }
        else if (Direction == Direction.Up && spriteRenderer.sprite != LaserUp)
        {
            spriteRenderer.sprite = LaserUp;
        }
        else if (Direction == Direction.Left && spriteRenderer.sprite != LaserLeft)
        {
            spriteRenderer.sprite = LaserLeft;
        }
        else if (Direction == Direction.Right && spriteRenderer.sprite != LaserRight)
        {
            spriteRenderer.sprite = LaserRight;
        }

        var direction = Direction;
        var directionVector = direction.ToVector2();
        var origin = transform.position.xy();
        var points = new List<Vector3>();
        points.Add(origin + height);

        for (; ; )
        {
            var hit = Physics2D.Raycast(origin + directionVector, directionVector, 15);  //TODO: change 15 to max level width

            DebugUtils.Assert(hit.collider != null);
            if (hit.collider == null)
                break; // for robustness

            origin = hit.point;
            points.Add(origin.xy0() + new Vector3(0, 0.3f, 0));

            var mirror = hit.collider.GetComponent<Mirror>();
            if (mirror != null)
            {
                direction = mirror.Reflect(direction);
                directionVector = direction.ToVector2();
                continue;
            }

            if (hit.collider.name.StartsWith("Explosive"))
            {
                Destroy(hit.collider.gameObject);
                continue;
            }

            break;
        }

        lineStrip.Draw(points);
    }
}
