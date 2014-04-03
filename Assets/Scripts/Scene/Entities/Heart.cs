using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
	Timer rotator;
	Timer fader;
	float _swap = 1;
	SpriteRenderer renderer;
	// Use this for initialization
	void Start()
	{
		rotator = new Timer(0.5f, Swap);
		rotator.Resume();
		rotator.time = 0.25f;
		rotator.repeating = true;
		fader = new Timer(3.0f, Dispose);
		fader.Resume();
		renderer = GetComponent<SpriteRenderer>();
	}

	void Dispose() {
		Destroy(gameObject);
	}

	void Swap() {
		_swap = -_swap;
	}
	// Update is called once per frame
	void Update()
	{
		rotator.Update();
		fader.Update();
		float boost = fader.progress * 0.75f + 0.25f;
		transform.position = new Vector3(transform.position.x, transform.position.y + 1f * Time.deltaTime * boost, transform.position.z);
		float angle;
		float ratio = rotator.progress * rotator.progress * 2 - 1;
		angle = _swap * ratio * 30;

		transform.localEulerAngles = new Vector3(0, 0, angle);
		renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1 - fader.progress);
		//renderer.s
	}
}

