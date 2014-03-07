using UnityEngine;
using System.Collections;

public class ExitGameButton : TouchLogic {

	public GUITexture StartGameButton;

	void Start(){

	}
	
	// Update is called once per frame
	void Update () {
		PollTouches ();
	}

	public override void OnTouchDown(){

	}
	public override void OnTouchUp(){
		Application.Quit ();
	}

	void OnMouseDown(){
		Debug.Log ("Over " + this.name);
	}
	void OnMouseUp(){
		//Application.LoadLevel("MainScene");
		//Application.OpenURL("http://www.surewash.com/");
	}
}
