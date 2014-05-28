using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class FacebookIntegration : MonoBehaviour
{
	#if UNITY_IPHONE || UNITY_ANDROID

	void Start()
	{
		FacebookCombo.init ();
	}

	public void Post(string caption, string link)
	{			
		var parameters = new Dictionary<string,object>
		{
			{ "link", link },
			{ "name", "Handy MD" },
			{ "picture", "https://pbs.twimg.com/profile_images/443709766687150080/6EZbY-5c_400x400.png" },
			{ "caption", caption },
			{ "description", "A 2D puzzle/platform game for mobile devices to promote the importance of hand hygiene, by Surewash" }
		};
		FacebookCombo.showFacebookShareDialog( parameters );
	}

	#endif
}