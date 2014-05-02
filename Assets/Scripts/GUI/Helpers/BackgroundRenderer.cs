using UnityEngine;
using System.Collections;
using Grouping;

public class BackgroundRenderer : MonoBehaviour
{

	SpriteRenderer _renderer;

	public float squareSize = 10;
	[SerializeField]
	private Material _tile;
	[SerializeField]
	private Material _funkySun;

	private Material _current;

	public static BackgroundRenderer instance { get; private set; }

	float targetSunValue;
	float sunValue;

	GroupManager.Group _menu;
	GroupManager.Group _selector;


	void OnNextBeat(int beatCount) {
		//If the center (pivot) is moving, then don't change radius to beat
		//This check makes transitioning look a lot better
		if (Mathf.Abs(_currentPivotY - _targetPivotY) > 0.1f) {
			if (GroupManager.main.activeGroup == _selector) {
				targetSunValue = maxSunValueLS;
			} else {
				targetSunValue = maxSunValue;
			}
		} // The pivot is not moving, change radius to beat
		else if (GroupManager.main.activeGroup == _selector) {
			targetSunValue = (beatCount % 2 == 0) ? minSunValueLS : maxSunValueLS;
		} else {
			targetSunValue = (beatCount % 2 == 0) ? minSunValue : maxSunValue;
		}
	}

	public float minSunValue= 0.95f;
	public float maxSunValue= 1.1f;
	public float minSunValueLS = 1.6f;
	public float maxSunValueLS = 1.8f;
	public float pivotY = 0.395f;
	public float pivotYLS = 0.5f;

	float _currentPivotY;
	float _targetPivotY;

	void Awake() {
		instance = this;
		_tile = Material.Instantiate(_tile) as Material;
		_funkySun = Material.Instantiate(_funkySun) as Material;
		targetSunValue = sunValue = minSunValue;
		renderer.material.SetFloat("Value", 0.8f);
		AudioMenu.OnNextBeat += OnNextBeat;
		_currentPivotY = _targetPivotY = pivotY;
	}

	bool sun = false;
		
	// Use this for initialization
	void Start()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_menu = GroupManager.main.group["Main Menu"];
		_selector = GroupManager.main.group["Level Select"];
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

		//ResetSize();
	}
	
	public void SetSunBackground() {
		if (this == null || !this.enabled) {
			return;
		}
		_current = _renderer.material = _funkySun;
		/*_matAxisX  = _current.shader.;
		_matAxisY  = _current.Ge;
     	_matRadius = _current.Ge;
     	_matPivotY = _current.;*/
		sun = true;
		ResetSize();
	}

	public void SetTileBackground() {
		if (this == null || !this.enabled) {
			return;
		}
		_current = _renderer.material = _tile;
		sun = false;
		ResetSize();
	}

	public void ResetSize() {
		Printer.Print("Width: " + Screen.width + ", Height: " + Screen.height);
		transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
		transform.localScale = Camera.main.ScreenToWorldLength(new Vector3(Screen.width, Screen.height, 0));
		float repY = transform.localScale.y / transform.localScale.x * squareSize;
		_renderer.material.SetFloat("RepeatX", squareSize);
		_renderer.material.SetFloat("RepeatY", repY);
		_renderer.material.SetFloat("Radius", ( transform.localScale.y / transform.localScale.x) * SunRadius);
		Printer.Print("Position: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
		Printer.Print("Scale: " + transform.localScale.x + ", " + transform.localScale.y + ", " + transform.localScale.z);
		Printer.Print(repY);
		Printer.Print(squareSize);
	}

	public float SunRadius = 0.15f;

	float t;

	//Cache for performance
	int _matAxisX;
	int _matAxisY;
	int _matRadius;
	int _matPivotY;

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

			float repY = transform.localScale.y / transform.localScale.x * squareSize;
			_renderer.material.SetFloat("Radius", ( transform.localScale.y / transform.localScale.x) * SunRadius * sunValue);
			//if (targetSunValue != sunValue)
			//{
				sunValue = Mathf.Lerp(sunValue, targetSunValue, 4.0f * Time.deltaTime);
			//}
			//renderer.material.SetFloat("Value", sunValue);
			if (GroupManager.main.activeGroup == _selector) {
				_targetPivotY = pivotYLS;
			} else {
				_targetPivotY = pivotY;
			}
			if (_currentPivotY != _targetPivotY) {
				_currentPivotY = Mathf.Lerp(_currentPivotY, _targetPivotY, 2.0f * Time.deltaTime);
				_renderer.material.SetFloat("PivotY", _currentPivotY);
			}
		}
	}
}

