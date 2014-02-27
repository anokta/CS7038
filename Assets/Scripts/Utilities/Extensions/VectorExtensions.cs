using System;
using UnityEngine;

public static class VectorExtensions
{
	public static Vector2 xy(this Vector3 vec)
	{
		return new Vector2(vec.x, vec.y);
	}

	public static Vector2 xz(this Vector3 vec)
	{
		return new Vector2(vec.x, vec.z);
	}

	public static Vector2 yz(this Vector3 vec)
	{
		return new Vector2(vec.y, vec.z);
	}

	public static Vector3 xy_(this Vector2 vec, float z)
	{
		return new Vector3(vec.x, vec.y, z);
	}

    public static Vector3 xy0(this Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }

	public static Vector3 vec3(this Vector2 vec, float z)
	{
		return xy_(vec, z);
	}

	public static Vector3 x_z(this Vector2 vec, float y)
	{
		return new Vector3(vec.x, y, vec.y);
	}

	public static Vector3 _yz(this Vector2 vec, float x)
	{
		return new Vector3(x, vec.x, vec.y);
	}
	
	//UNDONE: Related Vector4 extension methods
}
