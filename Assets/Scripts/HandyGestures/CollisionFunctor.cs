using UnityEngine;

public abstract class CollisionFunctor : MonoBehaviour
{
	public abstract RaycastHit[]
	GetResults(Vector3 screenPoint, float distance, int mask);
}