using UnityEngine;

public class Plant : Crate
{
	public GameObject fire;
	public GameObject ashes;
	private Timer clock;

	 void Start() {
		base.Start();
		clock = new Timer(1, () => {
			Break();
		});
		clock.Stop(); 
	}

	 void Update() {
		clock.Update();
	}

	public void Break() {
		var obj = Object.Instantiate(
			ashes, transform.position, transform.rotation) as GameObject;
		obj.GetComponent<SpriteRenderer>().sortingOrder = this.spriteRenderer.sortingOrder;
		obj.transform.parent = transform.parent;
		Destroy(this.gameObject);
		clock.Stop();
	}

	public void Burn()
	{
		clock.Resume();
		//Object.Instantiate(fire);
		//Destroy(this.gameObject, 1);
	}
}

