using UnityEngine;
using System.Collections;

public class StartGameButton : TouchLogic {
		
	// Update is called once per frame
	void Update () {
		PollTouches ();
	}

	public override void OnTouchDown(){

	}

	public override void OnTouchUp(){
		Application.LoadLevel("MainScene");
	}

	//TestingPurposes
	void OnMouseDown(){
		Application.LoadLevel("MainScene");

	}
}
