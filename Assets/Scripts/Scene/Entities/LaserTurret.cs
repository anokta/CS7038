using System;
using UnityEngine;
//using System.IO.Ports;

//TODO Placeholder obect to replace LaserEmitter

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


	void Update() {

	}

}


