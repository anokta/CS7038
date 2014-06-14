using System;
using UnityEngine;

public class HeatPad : MonoBehaviour
{
	private Material _cached;

	void Start() {
		renderer.material = _cached = AssetHelper.instance.HeatMaterial;
	}

	void LateUpdate() {
		//	float val = (Mathf.Sin(Time.time * 4) * 0.5f + 0.5f) * 0.6f + 0.1f;
		//	_cached.color = new Color(val, val, val, 0.65f);
	}
}
