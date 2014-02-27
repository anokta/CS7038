using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
	public Texture[] textures;
	// Use this for initialization
	void Start()
	{

	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void OnGUI()
	{
		float size = Screen.height / 5;
		var player = GameObject.FindObjectOfType<PlayerController>();
		if (player != null) {
			var level = player.cleanLevel;
			GUI.DrawTexture(new Rect(0, 0, size, size), textures[level]);
		}
	}
}
