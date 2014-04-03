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
		GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null, InstantiateBubbles, null));
		GroupManager.main.group["Level Select"].Add(this, new GroupDelegator(null, InstantiateBubbles, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, DestroyBubbles, null));

		ResetParticles();
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
	}

    void InstantiateBubbles()
    {
        if (_bubbles == null)
        {
            _bubbles = GameObject.Instantiate(bubblePrefab) as ParticleSystem;
            _bubbles.transform.parent = gameObject.transform;

            _bubbles.renderer.sortingOrder = short.MaxValue;

            ResetParticles();
        }
    }

    void DestroyBubbles()
    {
        if (_bubbles != null)
        {
            Destroy(_bubbles.gameObject);
        }
    }
}

