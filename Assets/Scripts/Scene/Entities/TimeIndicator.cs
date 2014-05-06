using UnityEngine;
using System.Collections;

public class TimeIndicator : MonoBehaviour
{
	SpriteRenderer spriteRenderer;

	void Awake()
	{
		spriteRenderer = renderer as SpriteRenderer;
	}

	public delegate float ValueReceiver();

	private static float defaultReceiver() {
		return 0;
	}

	private ValueReceiver _receiver = defaultReceiver;

	public ValueReceiver Receiver {
		get { return _receiver; }
		set {
			if (value == null) {
				_receiver = defaultReceiver;
			} else {
				_receiver = value;
			}
		}
	}

	public Color color {
		get { return spriteRenderer.color; }
		set { spriteRenderer.color = value; }
	}


	// Update is called once per frame
	void Update()
	{
		float value;
		if ((value = _receiver()) <= 0) {
			if (spriteRenderer.enabled == true) {
				spriteRenderer.enabled = false;
			}
		} else {
			if (!spriteRenderer.enabled) {
				spriteRenderer.enabled = true;
			}
			//	Debug.Log(_receiver());
			spriteRenderer.material.SetFloat("Value", _receiver());
		}
	}
}

