using UnityEngine;
using System.Collections;

public class MenuBackground : MonoBehaviour {

	public Texture2D backTex;

	// Use this for initialization
	void Start () {
		//backTex.texture.width = Screen.width;
	}

	void OnGUI(){
		GUI.depth = -1;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backTex,ScaleMode.StretchToFill);
	}

}
