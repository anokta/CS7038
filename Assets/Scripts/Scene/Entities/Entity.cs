using UnityEngine;
using Grouping;

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
		spriteRenderer.sortingOrder =  LevelLoader.PlaceDepth(entity.position.y);
    }
}

public abstract class EntityExplosionTask : Task
{
    public Vector2 ExplosionSource;
}
