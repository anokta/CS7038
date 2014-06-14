using System;
using UnityEngine;
using Grouping;

/// <summary>
/// This class contains any global assets that can be used by scripts
/// </summary>
public class AssetHelper : MonoBehaviour
{
	public Material HeatMaterial;
	public Sprite SureWall;
	public Sprite SureFloor;

	public static AssetHelper instance { get; private set; }

	void Awake()
	{
		instance = this;
		HeatMaterial = GameObject.Instantiate(HeatMaterial) as Material;
	}

	void Start() {
		var delegator = new GroupDelegator(
			                () => {
				float val = (Mathf.Sin(Time.time * 4) * 0.5f + 0.5f) * 0.6f + 0.1f;
				HeatMaterial.color = new Color(val, val, val, 0.65f);
			},
			null, null);
		GroupManager.main.group["Running"].Add(this, delegator);
		GroupManager.main.group["To Level Over"].Add(this, delegator);
	}
}