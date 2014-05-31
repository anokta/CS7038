using UnityEngine;
using Grouping;

public class Explosion : Entity
{
    private Animator animator;

	[SerializeField]
    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
		animator.speed = 4f;
	}

	//SpriteRenderer spriteRenderer;

	protected override void Start()
	{
		base.Start();
		//Grouping.GroupManager.main.group["
		GroupManager.main.group["Running"].Add(animator);
		GroupManager.main.group["To Level Over"].Add(animator);
		//if (Random.Range(0.0f, 1.0f) > 0.5f) {
			//Debug.Log("Zing!");
			//transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		//}
		//	spriteRenderer = renderer as SpriteRenderer;
		//transform.rotation = Quaternion.Euler(0, 0, Random.Range(-45, 45));
		//transform.
		//	localEulerAngles = new Vector3(0, 0, Random.Range(0.0f, 45) - 22.5f);
		transform.localEulerAngles = new Vector3(0, 0, Random.Range(-45, 45));
	}

    // Update is called once per frame
    protected override void Update()
    {
        var playing = animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion");
		/*if (playing) {
			spriteRenderer.sprite = null;
		} else {*/
		if (!playing) {
			//spriteRenderer.sprite = null;
			//DestroyImmediate(spriteRenderer);
			DestroyImmediate(gameObject);
        }
    }
}
