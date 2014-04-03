using UnityEngine;
using System.Collections;

public class Ashes : MonoBehaviour
{
	Timer timer;
	// Use this for initialization
	void Start()
	{
		timer = new Timer(
			0.2f, () => {
			GetComponent<SpriteRenderer>().sortingOrder = LevelLoader.FloorOrder + 50;
			Destroy(this);
			timer.Stop();
		});
		timer.Resume();
	}
	// Update is called once per frame
	void Update()
	{
		timer.Update();
	}
}

