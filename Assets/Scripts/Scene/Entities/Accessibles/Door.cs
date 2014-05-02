using UnityEngine;

public class Door : Accessible
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
		spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + LevelLoader.UsableOffset;
    }

    protected override void Update()
    {
        base.Update();

        spriteRenderer.enabled = (Vector2.Distance(playerHand.transform.position, entity.position) >= 0.5f);
    }

    public override bool Enter()
    {
        AudioManager.PlaySFX("Door");
        playerHand.SpoilHand(-0.5f, GetInstanceID());

        return true;
    }
}
