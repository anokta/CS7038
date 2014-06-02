using System;
using UnityEngine;

public class AssetHelper : MonoBehaviour
{
	public Sprite SurewashIcon;
	public Sprite SurewashText;
	public Sprite SurewashLogo;
	public Sprite SureWall;
	public Sprite SureFloor;

	public static AssetHelper instance { get; private set; }

	void Start()
	{
		instance = this;
	}
}