using UnityEngine;

public class Plant : Crate
{
	public GameObject fire;
	public void Burn()
	{
		//Object.Instantiate(fire);
		Destroy(this.gameObject, 1);
	}
}

