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

	float _time;

	void Start() {
		var delegator = new GroupDelegator(
			                () => {
				_time += Time.deltaTime * 4;
				float val = (Mathf.Sin(_time) * 0.5f + 0.5f) * 0.6f + 0.1f;
				HeatMaterial.color = new Color(val, val, val, 0.65f);
			},
			null, null);
		GroupManager.main.group["Running"].Add(this, delegator);
		GroupManager.main.group["To Level Over"].Add(this, delegator);
	}
}