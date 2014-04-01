using UnityEngine;
using System.Collections;
using Grouping;

public class BubbleController : MonoBehaviour
{

	public ParticleSystem bubblePrefab;
	private ParticleSystem _bubbles;


	// Use this for initialization
	void Start()
	{
		_bubbles = GameObject.Instantiate(bubblePrefab) as ParticleSystem;

		_bubbles.renderer.sortingOrder = short.MaxValue;

		GroupDelegator bubbleAction = new GroupDelegator(
			null,
			() => {
				if (_bubbles != null) {
					Camera.main.orthographicSize = 1;
					ResetParticles();
				}
			},
			() => {
				if (_bubbles != null) {
					_bubbles.Stop();
				}
			}
		);
		GroupManager.main.group["Main Menu"].Add(this, bubbleAction);
		GroupManager.main.group["Level Select"].Add(this, bubbleAction);
		GroupManager.main.group["Intro"].Add(this, bubbleAction);

		GroupDelegator concealer = new GroupDelegator(
			null,
			() => { if (_bubbles != null) { _bubbles.renderer.enabled = true; }},
			() => { if (_bubbles != null) { _bubbles.renderer.enabled = false; }}
			);

		GroupManager.main.group["Main Menu"].Add(this, concealer);
		GroupManager.main.group["Level Select"].Add(this, concealer);
		GroupManager.main.group["Fading"].Add(this, concealer);


		ResetParticles();
	}

	void Update() {
		// Hack
		if (Camera.main.orthographicSize != 1 && GroupManager.main.activeGroup.name == "Fading") {
			_bubbles.renderer.enabled =  false;
		}
	}

	void ResetParticles() {
		var cam = Camera.main;
		cam.orthographicSize = 1;
		var point = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
		point.y -= 0.5f;
		point.z = 1;
		_bubbles.transform.position = point;
		_bubbles.transform.localScale = new Vector3(
			cam.ScreenToWorldLength(new Vector3(Screen.width, 0, 0)).x
			, 1f, 1f);
		_bubbles.Play();
	}
}

