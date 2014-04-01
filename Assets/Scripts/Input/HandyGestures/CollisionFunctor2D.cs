using UnityEngine;
using System.Collections;

public abstract class CollisionFunctor2D : MonoBehaviour
{
	public abstract Collider2D[]
		GetResults(Vector3 screenPoint, int mask, float minDepth, float maxDepth);
}
