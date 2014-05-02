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
		//	_smoke = Entity.Spawn(this.gameObject, _smoke);

		//_smoke.transform.position = _smoke.transform.position + new Vector3(0, 0.45f, 0);
		//_smoke.renderer.sortingOrder += 2;
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

