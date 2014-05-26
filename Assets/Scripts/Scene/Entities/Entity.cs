using UnityEngine;
using Grouping;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    protected Transform entity;
    protected SpriteRenderer spriteRenderer;

    protected HandController playerHand;

    public EntityExplosionTask ExplosionHandler { get; protected set; }

	public static GameObject Spawn(GameObject original, GameObject newObject) {
		Transform t = original.transform;
		var obj = Object.Instantiate(
			newObject, t.position, t.rotation) as GameObject;
		obj.GetComponent<SpriteRenderer>().sortingOrder = original.renderer.sortingOrder;
		obj.transform.parent = t.parent;
		return obj;
	}

	public static GameObject Replace(GameObject original, GameObject newObject) {
		var obj = Spawn(original, newObject);
		Destroy(original);
		return obj;
	}

	public void AddTriggerAction(Trigger action) {
		_actions.Add(action);
	}
	
	private List<Trigger> _actions = new List<Trigger>();

	public void Execute(Trigger.ActionType type) {
		type |= Trigger.ActionType.Any;
		var none = (Trigger.ActionType)0;
	
		foreach (var a in _actions) {
			if ((a.type & type) != none) {
				a.OnRun();
			}
		}

		_actions.RemoveAll(_=> (_.type & type) != none && _.repeat == false);
	}

    public Vector2 Position
    {
        get { return entity.position; }
        set { entity.position = value; }
    }
		
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
		
    protected virtual void Start()
    {
        entity = transform;

        playerHand = FindObjectOfType<HandController>();

		spriteRenderer.sortingOrder = LevelLoader.PlaceDepth(entity.position.y);
        
        GroupManager.main.group["Running"].Add(this);
    }

    protected virtual void Update()
    {
        spriteRenderer.sortingOrder = LevelLoader.PlaceDepth(entity.position.y);
    }

	public static GameObject Create<T>() where T : Component {
		GameObject ret = new GameObject();
		ret.AddComponent<T>();
		return ret;
	}

	public static GameObject Create<T>(string name) where T : Component {
		GameObject ret = new GameObject(name);
		ret.AddComponent<T>();
		return ret;
	}
}

public abstract class EntityExplosionTask : Task
{
    public Vector2 ExplosionSource;
}
