using UnityEngine;

public class Gate : Accessible
{

	LeverGateType _type;

	public LeverGateType LeverGateType {
		get { return _type; }
		set {
			_type = value; 
			spriteRenderer.color = GetColorOf(value);
		}
	}

	static Color GetColorOf(LeverGateType type)
	{
		switch (type) {
			case LeverGateType.Type1:
				return new Color(1, 0.91f, 0.91f);       
			case LeverGateType.Type2:
				return new Color(0.91f, 1, 0.91f);
				;
			case LeverGateType.Type3:
				return new Color(0.91f, 0.91f, 1);
				;
			default:
				return Color.white;
		}
	}

	public LeverGateManager Manager;

	PlayerController drHandrew;

	private bool open;

	public virtual bool Open {
		get { return open; }
		set {
			if (open != value) {
				if (value) {
					Execute(Trigger.ActionType.On);
				} else {
					Execute(Trigger.ActionType.Off);
				}
			}
			open = value;
			collider2D.enabled = !open;
			spriteRenderer.sprite = open ? GateOpen : GateClosed;
		}
	}

	// TO BE CHANGED
	public Sprite GateOpen;
	public Sprite GateClosed;
	//


	// Use this for initialization
	protected override void Start()
	{
		base.Start();
	}

	/// <summary>
	/// Check if a gate can switch it's state, i.e not blocked by an object
	/// </summary>
	/// <returns><c>true</c> if this instance can switch; otherwise, <c>false</c>.</returns>
	public bool CanSwitch()
	{
		//There should only be an issue if the gate is already open
		if (!Open) {
			return true;
		}
			
		//Cast two rays in a cross shape to make up for both vertical and horizontal gates
		var cast1 = Physics2D.Raycast(
			            entity.position.xy() - new Vector2(0, 0.49f), -Vector2.up, 0.98f);
		var cast2 = Physics2D.Raycast(
			            entity.position.xy() - new Vector2(0.49f, 0), Vector2.right, 0.98f);
		return cast1.collider == null && cast2.collider == null;
	}

	protected override void Update()
	{
		base.Update();
		spriteRenderer.sortingOrder = LevelLoader.PlaceDepth(transform.position.y) + LevelLoader.UsableOffset;
		if (drHandrew == null) {
			drHandrew = GameObject.FindObjectOfType<PlayerController>();
		} else {
			if (!Open && Vector3.Distance(transform.position, drHandrew.transform.position) < 0.1f) {
				Debug.Log("Squashed!");
				AudioManager.PlaySFX("Squashed");
				drHandrew.Die(GameWorld.LevelOverReason.Squashed);

			}
		}
	}

	public override bool Enter()
	{
		return Open;
	}

	public void SwitchState()
	{
		Open = !Open;
	}
}
