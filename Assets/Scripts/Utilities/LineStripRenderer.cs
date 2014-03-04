using System.Collections.Generic;
using UnityEngine;

public class LineStripRenderer : Component
{
    private readonly List<LineRenderer> lines;
    private readonly Component parent;

    public LineStripRenderer(Component parent)
    {
        lines = new List<LineRenderer>();
        this.parent = parent;
    }

    public void Draw(List<Vector2> points)
    {
        var newLinesCount = points.Count - 1;

        for (var i = newLinesCount; i < lines.Count; i++)
        {
            lines[i].enabled = false;
        }

        for (var i = lines.Count; i < newLinesCount; i++)
        {
            var laser = new GameObject("Laser");
            laser.transform.parent = parent.transform;

            var line = laser.AddComponent<LineRenderer>();
            line.SetWidth(0.1f, 0.1f);
            line.SetVertexCount(2);

            lines.Add(line);
        }

        for (var i = 0; i < newLinesCount; i++)
        {
            lines[i].SetPosition(0, points[i]);
            lines[i].SetPosition(1, points[i + 1]);
        }
    }
}
