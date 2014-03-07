public class Door : Accessible
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override bool Enter()
    {
        var controller = FindObjectOfType<PlayerController>();
        controller.spoilHand(0.5f);

        audioManager.PlaySFX("Door");

        return true;
    }
}
