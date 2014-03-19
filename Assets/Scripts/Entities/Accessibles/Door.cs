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

        playerHand.value -= 0.5f;

        return true;
    }
}
