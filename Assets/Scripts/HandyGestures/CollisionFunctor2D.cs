using UnityEngine;
using System.Collections;

public abstract class CollisionFunctor2D : MonoBehaviour
{
	public abstract Collider2D[]
		GetResults(Vector2 position, int mask, float minDepth, float maxDepth);
}
