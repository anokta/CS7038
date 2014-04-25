using UnityEngine;
using Grouping;

public class Smoke : Entity
{
	private Animator animator;

	// Use this for initialization
	protected override void Awake()
	{
		base.Awake();

		animator = GetComponent<Animator>();
		animator.speed = 5f;
	}

	//SpriteRenderer spriteRenderer;

	protected override void Start()
	{
		base.Start();
		//Grouping.GroupManager.main.group["
		GroupManager.main.group["Running"].Add(animator);
		GroupManager.main.group["To Level Over"].Add(animator);
		/*if (Random.Range(0.0f, 1.0f) > 0.5f) {
			Debug.Log("Zing!");
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}*/
		//	spriteRenderer = renderer as SpriteRenderer;
	}

	// Update is called once per frame
	protected override void Update()
	{
		var playing = animator.GetCurrentAnimatorStateInfo(0).IsName("Smoke");
		if (playing) {
			spriteRenderer.sprite = null;
		} else {
			spriteRenderer.sprite = null;
			DestroyImmediate(gameObject);
		}
	}
}
