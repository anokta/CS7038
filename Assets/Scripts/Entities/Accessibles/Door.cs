﻿using UnityEngine;
using System.Collections;

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
		var controller = GameObject.FindObjectOfType<PlayerController>();
		controller.spoilHand();
		return true;
	}
}
