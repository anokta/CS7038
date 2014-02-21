public class Door : Accessible
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool Enter()
    {
        audioManager.PlaySFX("Door");
        var controller = FindObjectOfType<PlayerController>();
        controller.spoilHand();
        return true;
    }
}
