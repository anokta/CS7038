public class Door : Accessible
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override bool Enter()
    {
        audioManager.PlaySFX("Door");
        var controller = FindObjectOfType<PlayerController>();
        controller.spoilHand();
        return true;
    }
}
