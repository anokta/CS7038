using UnityEngine;

public class Crate : Pushable
{
    public Crate()
    {
        MovingWithPlayer = true;
        Sfx = "Push Crate";
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
