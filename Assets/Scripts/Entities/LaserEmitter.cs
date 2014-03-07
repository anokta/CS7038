using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserEmitter : Entity
{
	public Direction Direction;
	private LineStripRenderer lineStrip;
	public Sprite laserUp;
	public Sprite laserDown;
	public Sprite laserLeft;
	public Sprite laserRight;
	private SpriteRenderer renderer;

	public LaserEmitter()
	{
		Direction = Direction.Down;
	}

	// Use this for initialization
	protected override void Start()
	{
		base.Start(); 

		lineStrip = new LineStripRenderer(this);
		renderer = GetComponent<SpriteRenderer>();
	}

	private Vector2 height = new Vector2(0, 0.3f);

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (Direction == Direction.Down && renderer.sprite != laserDown) {
			renderer.sprite = laserDown;
		} else if (Direction == Direction.Up && renderer.sprite != laserUp) {
			renderer.sprite = laserUp;
		} else if (Direction == Direction.Left && renderer.sprite != laserLeft) {
			renderer.sprite = laserLeft;
		} else if (Direction == Direction.Right && renderer.sprite != laserRight) {
			renderer.sprite = laserRight;
		} else if (Direction == null) {
			throw new Exception("Impossibru!");
		}

		var direction = Direction;
		var directionVector = direction.ToVector2();
		var origin = transform.position.xy();
		var points = new List<Vector3>();
		points.Add(origin + height);

		for (; ;) {
			var hit = Physics2D.Raycast(origin + directionVector, directionVector, 15);  //TODO: change 15 to max level width

			DebugUtils.Assert(hit.collider != null);
			if (hit.collider == null)
				break; // for robustness

			origin = hit.point;
			points.Add(origin.xy0() + new Vector3(0, 0.3f, 0));

			var mirror = hit.collider.GetComponent<Mirror>();
			if (mirror != null) {
				direction = mirror.Reflect(direction);
				directionVector = direction.ToVector2();
				continue;
			}

			if (hit.collider.name.StartsWith("Explosive")) {
				Destroy(hit.collider.gameObject);
				continue;
			}

			break;
		}

		lineStrip.Draw(points);
	}
}
