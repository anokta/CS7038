using UnityEngine;
using System.Collections;
using Grouping;

public class BackgroundRenderer : MonoBehaviour
{

	SpriteRenderer _renderer;

	public float squareSize = 10;

	public Material Tile;
	public Material FunkySun;
		
	// Use this for initialization
	void Start()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_renderer.sortingLayerName = "Background";
		//_renderer.material = FunkySun;
		//var sun = new GroupDelegator(null, SetSunBackground, SetTileBackground);
	//	var tiled = new GroupDelegator(null, SetTileBackground, null);
		//GroupManager.main.group["Main Menu"].Add(this, sun);
	//	GroupManager.main.group["Level Select"].Add(this, sun);


	}
	
	void SetSunBackground() {
		_renderer.material = FunkySun;
	}
	void SetTileBackground() {
		_renderer.material = Tile;
	}

	// Update is called once per frame
	void Update()
	{
		//FIXME: Optimize
		transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
		transform.localScale = Camera.main.ScreenToWorldLength(new Vector3(Screen.width, Screen.height, 0));
		float repY = transform.localScale.y / transform.localScale.x * squareSize;
		_renderer.material.SetFloat("RepeatX", squareSize);
		_renderer.material.SetFloat("RepeatY", repY);
		//_renderer.material.SetFloat("Radius", 0.01f * (repY / squareSize));
		//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
	}
}

