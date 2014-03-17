using UnityEngine;
using System.Collections;

public class SurewashLogo : TouchLogic {

	public GUITexture surewashLogo;

	void Start(){
		//surewashLogo.transform.localScale.Set(0.01f,0.01f,0.01f);
        surewashLogo.transform.localPosition = new Vector3(Screen.width/2, 0, 0);
	}

	void Update () {
		PollTouches ();
	}

	public override void OnTouchDown(){
		Application.OpenURL ("http://www.surewash.com/");
	}

	//Testing purposes only
	void OnMouseDown(){

		Application.OpenURL ("http://www.surewash.com/");
	}
}
