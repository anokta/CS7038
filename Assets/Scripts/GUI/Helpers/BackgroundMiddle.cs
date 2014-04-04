using UnityEngine;
using System.Collections;
using Grouping;

public class BackgroundMiddle : MonoBehaviour {

	public Sprite Left;
	public Sprite Right;
	public Material TileMat;
	public Material ColorMat;

	// Use this for initialization
	void Start () {
		renderer.sortingOrder = -1;
		GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null,
			() => {if (renderer != null) { renderer.enabled = true; }},
			() => {if (renderer != null) { renderer.enabled = false; }}));

	}

	private SpriteRenderer _rendererLeft;
	private SpriteRenderer _rendererRight;
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.2f, Screen.height, 0));
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
		transform.localScale = Camera.main.ScreenToWorldLength(new Vector3(Screen.width * 0.6f, Screen.height, 0));
		//float repY = transform.localScale.y / transform.localScale.x * squareSize;
	}
}
