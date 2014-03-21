using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour {

	GameObject tex;

	void Init() {
		tex = GameObject.Find("");
	}

	void OnMouseEnter(){
		renderer.material.color = Color.blue;
	}
	void OnMouseExit(){
		renderer.material.color = Color.red;
		renderer.guiText.fontSize++;
	}


}
