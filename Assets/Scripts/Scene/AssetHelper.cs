using System;
using UnityEngine;

public class AssetHelper : MonoBehaviour
{
	public Material HeatMaterial;
	public Sprite SurewashIcon;
	public Sprite SurewashText;
	public Sprite SurewashLogo;
	public Sprite SureWall;
	public Sprite SureFloor;

	public static AssetHelper instance { get; private set; }

	void Awake()
	{
		instance = this;
		HeatMaterial = GameObject.Instantiate(HeatMaterial) as Material;
	}
}