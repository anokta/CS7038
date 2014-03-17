using UnityEngine;
using System.Collections;

public class TouchLogic : MonoBehaviour {

	public static int currTouch = 0;
	private Ray2D ray;
	private RaycastHit2D rayInfo = new RaycastHit2D();
    
	public void PollTouches(){

		if (Input.touches.Length <= 0) {
				
		} 
		else 
		{
			//We have found at least one touch
			for(int i = 0; i < Input.touchCount;i++)
			{
				if(this.guiTexture.HitTest(Input.GetTouch(i).position)){
					if(Input.GetTouch(i).phase == TouchPhase.Began)
					{
						OnTouchDown();
					}
					if(Input.GetTouch(i).phase == TouchPhase.Ended)
					{
						OnTouchDown();
					}
						
				}
			}
					
		}
	}

	public virtual void OnTouchDown(){}
	public virtual void OnTouchUp(){}
}
