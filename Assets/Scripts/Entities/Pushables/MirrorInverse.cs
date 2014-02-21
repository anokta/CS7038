public class MirrorInverse : Mirror
{
    protected override void Start()
    {
        base.Start();

        Forward = false;
    }
}
