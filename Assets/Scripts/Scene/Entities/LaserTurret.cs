using System;
using UnityEngine;
//using System.IO.Ports;

//TODO Placeholder obect to replace LaserEmitter
using System.Collections.Generic;
using System.Collections.Specialized;

public class LaserTurret : Entity
{
	#region Fields
	[SerializeField]
	Sprite _up;
	[SerializeField]
	Sprite _right;
	[SerializeField]
	Sprite _down;
	[SerializeField]
	Sprite _left;
	[SerializeField]
	Material _laserMaterial;
	[SerializeField]
	private Direction _direction;

	public Direction direction {
		get { return _direction; }
		set {
			_direction = value;
			switch (_direction) {
				case Direction.Up:
					spriteRenderer.sprite = _up;
					break;
				case Direction.Right:
					spriteRenderer.sprite = _right;
					break;
				case Direction.Down:
					spriteRenderer.sprite = _down;
					break;
				case Direction.Left:
					spriteRenderer.sprite = _left;
					break;
				default:
					Debug.Log("Impossibru!");
					break;
			}
		}
	}
	#endregion

	public LaserTurret() {
		hits = new LaserHit[max];
	}

	struct LaserHit {
		public LaserHit(Vector2 position, int order) {
			//this.position = position;
			//this.order = order;
		}
		//Vector2 position;
		//int order;
	}

	readonly Vector2 height = new Vector2(0, 0.3f);
	readonly int max = 30;
	LaserHit[] hits;
	int hitSize;

	void AddHit(LaserHit hit) {
		hits[hitSize++] = hit;
	}

	protected override void Update() {
		var dirVec = direction.ToVector2();
		var lastPos = entity.position.xy();
		//var currentPoint = lastPos;
		hitSize = 0;

		AddHit(new LaserHit(lastPos, spriteRenderer.sortingOrder - 1));
		for (int i = 0; i < 20; ++i) {
			var hit = Physics2D.Raycast(lastPos + dirVec, dirVec, 100);  //TODO: change 100 to max level width

			if (hit.collider == null) {
				return;
			}

			AddHit(new LaserHit(hit.point, hit.collider.gameObject.renderer.sortingOrder - 1));

			if (dirVec.y > 0) { 
				AddHit(new LaserHit(hit.point + height, hit.collider.gameObject.renderer.sortingOrder + 1));
			}
		}
	}

}


