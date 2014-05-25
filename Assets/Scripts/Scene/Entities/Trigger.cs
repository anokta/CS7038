using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
	[SerializeField]
	TmxObject _object;

	public TmxObject TmxObject {
		get { return _object; }
		set {
			_object = value; 
			if (_object != null) {
				PrepareTrigger();
			}
		}
	}

	public event System.Action Activate;

	protected virtual void OnActivate()
	{
		if (Activate != null) {
			Activate();
		}
	}

	protected bool once = true;

	void PrepareTrigger()
	{
		int repeat;
		if (_object.properties.GetInt("Repeat", 0) != 0) {
			once = false;
		}
		else {
			once = true;
		}

		string targetDialogue;
		if (_object.properties.GetTag("Dialogue", out targetDialogue)) {

		}
	}

	// Use this for initialization
	void Awake()
	{
		if (_object != null) {
			PrepareTrigger();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}

