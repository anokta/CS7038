using UnityEngine;
using System.Collections;
using Grouping;

public class Ashes : MonoBehaviour
{
	[SerializeField]
	GameObject _smoke;
	Timer timer;
	// Use this for initialization

	Transform _ashTransform;
	Transform _smokeTransform;
	
	void Awake()
	{
		var animator = GetComponent<Animator>();
		_smoke = Entity.Spawn(this.gameObject, _smoke);
		_ashTransform = transform;
		_smokeTransform = _smoke.transform;
		//t.position = t.position + new Vector3(0, 0.45f, 0);
		_smoke.renderer.sortingOrder += 2;
		_smoke.SetActive(false);
		_smokeTransform.parent = transform;
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

	void Start() {
		//_smoke.SetActive(true);
		Trigger(_ashTransform.position);
	}

	public void Trigger(Vector3 position) {
		gameObject.SetActive(true);
		_smoke.SetActive(true);
		_ashTransform.position = position;
		_smokeTransform.position = position + new Vector3(0, 0.45f, 0);
	}

	// Update is called once per frame
	void Update()
	{
		timer.Update();
	}
}

