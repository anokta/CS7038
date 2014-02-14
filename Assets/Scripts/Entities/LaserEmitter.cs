using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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

        // TODO: parameterize
        Direction = Direction.Down;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Direction;
        var directionVector = direction.ToVector3();
        var origin = transform.position;
        var points = new List<Vector3>();
        points.Add(origin);

        for (; ; )
        {
            var hit = Physics2D.Raycast(origin + 0.9f * directionVector, directionVector, 100); //TODO: change 100 to max level width

            if (hit.collider == null) break;  // Should not happen

            points.Add(hit.point);

            var mirror = hit.collider.GetComponent<Mirror>();
            if (mirror != null)
            {
                direction = mirror.Reflect(direction);
                directionVector = direction.ToVector3();
                origin = hit.point.xy0();
                continue;
            }

            if (hit.collider.name.StartsWith("Explosive"))
            {
                Destroy(hit.collider.gameObject);
                continue;
            }
            else if (hit.collider.tag == "Pushable")
                break;
            break;
        }

        _line.SetVertexCount(points.Count);

        for (var i = 0; i < points.Count; i++)
        {
            _line.SetPosition(i, points[i]);
        }
    }
}
