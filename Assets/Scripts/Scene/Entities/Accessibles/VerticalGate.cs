using UnityEngine;

public class VerticalGate : Gate
{

    public override bool Open {
        get { return base.Open; }
        set
        {
            base.Open = value;
            CreateBotPart();
            _botRenderer.enabled = value;
        }
    }

    private SpriteRenderer _botRenderer;
    private GameObject _bottom;

    // TO BE CHANGED
    public Sprite GateOpenBot;
    //

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        
    }

    private void CreateBotPart()
    {
        if (_bottom == null)
        {
            _bottom = new GameObject();
            _bottom.transform.position = transform.position;
            _bottom.transform.parent = transform;
            _botRenderer = _bottom.AddComponent<SpriteRenderer>();
            _botRenderer.sharedMaterial = renderer.sharedMaterial;
            _botRenderer.sprite = GateOpenBot;
            _botRenderer.enabled = false;
        }
    }

    protected override void Update() {
        base.Update();
        CreateBotPart();
        _botRenderer.sortingOrder = Entity.Place(transform.position.y - 1) - Entity.PlaceOffset + 1;
    }
}
