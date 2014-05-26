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

		if (Vector2.Distance(playerHand.transform.position, entity.position) >= 0.5f) {
			if (!spriteRenderer.enabled) {
				spriteRenderer.enabled = true;
				Execute(Trigger.ActionType.Off);
				//OnDeactivate();
			}
		}
		else {
			spriteRenderer.enabled = false;
		}
        //spriteRenderer.enabled = (Vector2.Distance(playerHand.transform.position, entity.position) >= 0.5f);
    }

    public override bool Enter()
    {
        AudioManager.PlaySFX("Door");
        playerHand.SpoilHand(GetInstanceID());
		//OnActivate();
		Execute(Trigger.ActionType.On | Trigger.ActionType.Handy);
        return true;
    }
}
