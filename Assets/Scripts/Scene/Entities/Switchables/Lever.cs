using UnityEngine;

public class Lever : Switchable
{
	LeverGateType _type;
	public LeverGateType LeverGateType {
		get { return _type; }
		set {
			_type = value; 
			spriteRenderer.color = GetColorOf(value);
		}
	}

	static Color GetColorOf(LeverGateType type) {
		switch (type) {
			case LeverGateType.Type1:
				return new Color(1, 0.85f, 0.85f);       
			case LeverGateType.Type2:
				return new Color(0.85f, 1, 0.85f);;
			case LeverGateType.Type3:
				return new Color(0.85f, 0.85f, 1);;
			default:
				return Color.white;
		}
	}

    public LeverGateManager Manager;

    private bool open;
    public bool Open
    {
        get { return open; }
        set
        {
            open = value;
            spriteRenderer.sprite = open ? LeverOpen : LeverClosed;
        }
    }

    public Sprite LeverOpen;
    public Sprite LeverClosed;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override void Switch(bool byPlayer)
    {
        Manager.Switch(LeverGateType);

        AudioManager.PlaySFX("Lever");

		if (byPlayer) {
			Execute(Trigger.ActionType.Handy);
        	playerHand.SpoilHand(GetInstanceID());
		}
		else {
			Execute(Trigger.ActionType.Other);
		}
    }

    public void SwitchState()
    {
        Open = !Open;
    }
}
