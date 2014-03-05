using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : Entity
{
    public Direction Direction;
    private LineStripRenderer lineStrip;

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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        var direction = Direction;
        var directionVector = direction.ToVector2();
        var origin = transform.position.xy();
        var points = new List<Vector2>();
        points.Add(origin);

        for (; ; )
        {
            var hit = Physics2D.Raycast(origin + directionVector, directionVector, 15);  //TODO: change 15 to max level width

            DebugUtils.Assert(hit.collider != null);
            if (hit.collider == null) break; // for robustness

            origin = hit.point;
            points.Add(origin);

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
