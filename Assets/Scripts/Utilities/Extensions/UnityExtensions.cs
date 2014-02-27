using System;
using UnityEngine;

public static class UnityExtensions
{
	public static Vector3 ScreenToWorldDelta(this Camera camera, Vector3 position)
	{
		return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(Vector3.zero);
	}
}

