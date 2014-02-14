using UnityEngine;
using System.Collections;

public class Fountain : Accessible
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
		var player = GameObject.FindObjectOfType<PlayerController>();
		player.handState = PlayerController.HandState.Clean;
		return true;
	}
}
