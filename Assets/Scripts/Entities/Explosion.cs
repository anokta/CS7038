using UnityEngine;
using System.Collections;

public class Explosion : Entity
{
    private Animator animator;

    // Use this for initialization
    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        var playing = animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion");
        if (!playing)
        {
            Destroy(gameObject);
        }
    }
}
