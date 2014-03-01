﻿using UnityEngine;
using System.Collections;

public class Gate : Accessible {

    // TO BE CHANGED
    public Sprite gateOpen, gateClosed;
    SpriteRenderer spriteRenderer;
    //

    private bool locked;

	// Use this for initialization
	protected override void Start () 
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        locked = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override bool Enter()
    {
        if (locked)
        {
            return false;
        }
        else
        {
            audioManager.PlaySFX("Door");
            return true;
        }
    }

    public void ToggleLock()
    {
        locked = !locked;

        spriteRenderer.sprite = locked ? gateClosed : gateOpen;
    }
}