using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LaserEmitter : Entity
{
    public Direction Direction { get; set; }

    RaycastHit _hit;
    LineRenderer _line;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        _line = GetComponent<LineRenderer>();
        _line.enabled = true;

        //TODO: parameterize
        Direction = Direction.Down;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Direction;
        var directionVector = direction.ToVector2();
        var origin = transform.position.xy();

        // drawing
        var points = new List<Vector2>();
        points.Add(origin);

        for (; ; )
        {
            var hit = Physics2D.Raycast(origin + directionVector, directionVector, 15);  //TODO: change 15 to max level width

            DebugUtils.Assert(hit.collider != null);
            if (hit.collider == null) break; // for robustness

            points.Add(hit.point);

            var mirror = hit.collider.GetComponent<Mirror>();
            if (mirror != null)
            {
                direction = mirror.Reflect(direction);
                directionVector = direction.ToVector2();
                origin = hit.point;
                continue;
            }

            if (hit.collider.name.StartsWith("Explosive"))
            {
                Destroy(hit.collider.gameObject);
                continue;
            }

            if (hit.collider.tag == "Pushable")
            {
                break;
            }

            break;
        }

        // drawing
        _line.SetVertexCount(points.Count);

        for (var i = 0; i < points.Count; i++)
        {
            _line.SetPosition(i, points[i]);
        }
    }
}
