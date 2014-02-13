using UnityEngine;

public abstract class CollisionFunctor : MonoBehaviour
{
	public abstract RaycastHit[]
	GetResults(Vector3 position, float distance, int mask);
}