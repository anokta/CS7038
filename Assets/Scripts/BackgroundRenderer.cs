using UnityEngine;
using System.Collections;

public class BackgroundRenderer : MonoBehaviour
{

	SpriteRenderer _renderer;

	public float squareSize = 10;
		
	// Use this for initialization
	void Start()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_renderer.sortingOrder = short.MinValue;
	}

	// Update is called once per frame
	void Update()
	{
		transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
		transform.localScale = Camera.main.ScreenToWorldDelta(new Vector3(Screen.width, Screen.height, 1));

		_renderer.material.SetFloat("RepeatX", squareSize);
		_renderer.material.SetFloat("RepeatY", transform.localScale.y / transform.localScale.x * squareSize);
		//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
	}
}

