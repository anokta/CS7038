using UnityEngine;
using Grouping;

public class Entity : MonoBehaviour
{
    protected Transform entity;
    protected SpriteRenderer spriteRenderer;

    protected AudioManager audioManager;

    protected HandController playerHand;

    public EntityExplosionTask ExplosionHandler { get; protected set; }

    public Vector2 Position
    {
        get { return entity.position; }
        set { entity.position = value; }
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioManager = FindObjectOfType<AudioManager>();
    }

    protected virtual void Start()
    {
        entity = transform;

        playerHand = FindObjectOfType<HandController>();

        spriteRenderer.sortingOrder = -Mathf.RoundToInt(4 * entity.position.y);
        
        GroupManager.main.group["Running"].Add(this);
    }

    protected virtual void Update()
    {
        spriteRenderer.sortingOrder =  - Mathf.RoundToInt(4 * entity.position.y);
    }
}

public abstract class EntityExplosionTask : Task
{
    public Vector2 ExplosionSource;
}
