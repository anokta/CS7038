using UnityEngine;
using System.Collections;

public class Lever : Switchable {

    public Gate gate;
    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Switch()
    {
        gate.ToggleLock();
    }
}
