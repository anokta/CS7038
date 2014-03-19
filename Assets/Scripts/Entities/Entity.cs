using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Transform entity;
    protected SpriteRenderer spriteRenderer;

    protected AudioManager audioManager;

    public Vector2 Position
    {
        get { return entity.position; }
        set { entity.position = value; }
    }

    public bool Explosive { get; protected set; }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioManager = FindObjectOfType<AudioManager>();
    }

    protected virtual void Start()
    {
        entity = transform;
    }

    protected virtual void Update()
    {
        spriteRenderer.sortingOrder =  - Mathf.RoundToInt(4 * entity.position.y);
    }
}
