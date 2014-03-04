using UnityEngine;
using System.Collections;

public class Lever : Switchable {

    public Gate gate;
    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override void Switch()
    {
        audioManager.PlaySFX("Lever");

        gate.ToggleLock();
    }
}
