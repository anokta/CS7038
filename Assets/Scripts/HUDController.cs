using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
	public Texture clean;
	public Texture dirty;
	public Texture filthy;
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
		Texture tex = null;
		var player = GameObject.FindObjectOfType<PlayerController>();
		if (player != null) {
			var state = player.handState;
			switch (state) {
				case PlayerController.HandState.Clean:
					tex = clean;
					break;
				case PlayerController.HandState.Dirty:
					tex = dirty;
					break;
				case PlayerController.HandState.Filthy:
					tex = filthy;
					break;
			}
			GUI.DrawTexture(new Rect(0, 0, size, size), tex);
		}
	}
}
