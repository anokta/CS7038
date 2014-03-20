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

        playerHand.SpoilHand(-0.5f, GetInstanceID());

        return true;
    }
}
