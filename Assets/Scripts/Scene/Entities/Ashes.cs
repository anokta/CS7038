using UnityEngine;
using System.Collections;
using Grouping;

public class Ashes : MonoBehaviour
{
	[SerializeField]
	GameObject _smoke;
	Timer timer;
	// Use this for initialization
	void Start()
	{
		var animator = GetComponent<Animator>();
		_smoke = Object.Instantiate(_smoke) as GameObject;
		_smoke.transform.parent = transform;
		_smoke.transform.position = new Vector3(
			transform.position.x,
			transform.position.y + 0.5f,
			transform.position.z);
		_smoke.renderer.sortingOrder = renderer.sortingOrder + 1;
		GroupManager.main.group["Running"].Add(animator);
		GroupManager.main.group["To Level Over"].Add(animator);


		timer = new Timer(
			0.2f, () => {
				GetComponent<SpriteRenderer>().sortingOrder = LevelLoader.AshesOrder;
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

