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
		//FIXME: The dimensions are wrong
		transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
		transform.localScale = Camera.main.ScreenToWorldDelta(new Vector3(Screen.width, Screen.height, 0));

		_renderer.sharedMaterial.SetFloat("RepeatX", squareSize);
		_renderer.sharedMaterial.SetFloat("RepeatY", transform.localScale.y / transform.localScale.x * squareSize);
		//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
	}
}

