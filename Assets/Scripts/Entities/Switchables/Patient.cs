using UnityEngine;
using System.Collections;

public class Patient : Switchable
{
    private bool treated;
	public Sprite treatedSprite;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        treated = false;
    }

    public bool IsTreated()
    {
        return treated;
    }

    public override void Switch()
    {
        if (!treated)
        {
            audioManager.PlaySFX("Treated");

            var controller = FindObjectOfType<HandController>();
			controller.value = HandController.MinValue;
			var renderer = GetComponent<SpriteRenderer>();
			renderer.sprite = treatedSprite;
        }

        treated = true;
    }
}
