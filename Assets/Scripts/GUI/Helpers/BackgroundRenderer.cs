using UnityEngine;
using System.Collections;
using Grouping;

public class BackgroundRenderer : MonoBehaviour
{

	SpriteRenderer _renderer;

	public float squareSize = 10;
	[SerializeField]
	private Material _tile;
	public Material FunkySun;

	public static BackgroundRenderer instance { get; private set; }

	void Awake() {
		instance = this;
		_tile = Material.Instantiate(_tile) as Material;
		FunkySun = Material.Instantiate(FunkySun) as Material;
	}

	bool sun = false;
		
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
		var sunDelegator = new GroupDelegator(null, SetSunBackground, SetTileBackground);
		//GroupManager.main.group["Main Menu"].Add(this, sunDelegator);
		//	GroupManager.main.group["Level Select"].Add(this, sunDelegator);

		SetSunBackground();

		ResetSize();
	}
	
	public void SetSunBackground() {
		if (this == null || !this.enabled) {
			return;
		}
		_renderer.material = FunkySun;
		sun = true;
		ResetSize();
	}

	public void SetTileBackground() {
		if (this == null || !this.enabled) {
			return;
		}
		_renderer.material = _tile;
		sun = false;
		ResetSize();
	}

	public void ResetSize() {
		transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
		transform.localScale = Camera.main.ScreenToWorldLength(new Vector3(Screen.width, Screen.height, 0));
		float repY = transform.localScale.y / transform.localScale.x * squareSize;
		_renderer.material.SetFloat("RepeatX", squareSize);
		_renderer.material.SetFloat("RepeatY", repY);
		_renderer.material.SetFloat("Radius", ( transform.localScale.y / transform.localScale.x) * SunRadius);
	}

	public float SunRadius = 0.15f;

	float t;

	// Update is called once per frame
	void Update()
	{
		if (GUIManager.CameraChanged || GUIManager.ScreenResized) {
			ResetSize();
		}
		if (sun) {
			t += Time.deltaTime / 3;
			t = t % 360f;
			_renderer.material.SetFloat("AxisX", Mathf.Cos(t));
			_renderer.material.SetFloat("AxisY", Mathf.Sin(t));
		}
	}
}

