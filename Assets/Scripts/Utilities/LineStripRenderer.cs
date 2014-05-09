using System.Collections.Generic;
using UnityEngine;

public class LineStripRenderer : Component
{
    private readonly List<LineRenderer> rendererPool;
    private int numEnabledRenderers;
    private readonly Component parent;
    private readonly Material material;
    private readonly Color color;
    private readonly float width;

    public LineStripRenderer(Component parent, Material material, Color color, float width)
    {
        rendererPool = new List<LineRenderer>();
        numEnabledRenderers = 0;
        this.parent = parent;
        this.material = material;
        this.color = color;
        this.width = width;
    }

    public void Draw(List<Vector2> points, List<int> sortingOrderOffsets, Vector2 laserPositionOffset)
    {
        var numLines = points.Count - 1;
        var numRenderers = numLines * 3;

        EnableRenderers(numRenderers);

        for (var i = 0; i < numLines; i++)
        {
            var startRenderer = rendererPool[3 * i];
            var middleRenderer = rendererPool[3 * i + 1];
            var endRenderer = rendererPool[3 * i + 2];

            var point1 = points[i] + laserPositionOffset;
            var point4 = points[i + 1] + laserPositionOffset;

            var direction = point4 - point1;

            if (direction.magnitude > 0.5f)
            {
                direction.Normalize();
                var point2 = point1 + direction * 0.5f;
                var point3 = point4 - direction * 0.5f;

                startRenderer.SetPosition(0, point1);
                startRenderer.SetPosition(1, point2);
                startRenderer.sortingOrder = LevelLoader.PlaceDepth(point1.y) + sortingOrderOffsets[i];

                middleRenderer.SetPosition(0, point2);
                middleRenderer.SetPosition(1, point3);
                middleRenderer.sortingOrder = LevelLoader.FloorOrder + 100;

                endRenderer.SetPosition(0, point3);
                endRenderer.SetPosition(1, point4);
                endRenderer.sortingOrder = LevelLoader.PlaceDepth(point4.y) + sortingOrderOffsets[i + 1];
            }
            else
            {
                startRenderer.SetPosition(0, point1);
                startRenderer.SetPosition(1, point4);
                startRenderer.sortingOrder = LevelLoader.PlaceDepth(point1.y) + sortingOrderOffsets[i];

                middleRenderer.enabled = false;
                endRenderer.enabled = false;
                numEnabledRenderers -= 2;
            }
        }
    }

    private void EnableRenderers(int numRenderers)
    {
        for (var i = numRenderers; i < numEnabledRenderers; i++)
        {
            rendererPool[i].enabled = false;
        }

        for (var i = numEnabledRenderers; i < numRenderers; i++)
        {
            if (i < rendererPool.Count)
            {
                rendererPool[i].enabled = true;
            }
            else
            {
                var laser = new GameObject("Laser");
                laser.transform.parent = parent.transform;

                var line = laser.AddComponent<LineRenderer>();
                line.material = material;
                line.SetColors(color, color);
                line.SetWidth(width, width);
                line.SetVertexCount(2);

                rendererPool.Add(line);
            }
        }

        numEnabledRenderers = numRenderers;
    }
}
