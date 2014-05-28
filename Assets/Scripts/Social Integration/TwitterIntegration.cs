using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Grouping;
using Prime31;

public class TwitterIntegration : SocialIntegration {

	#if UNITY_IPHONE || UNITY_ANDROID

	private string status, url;
    
    protected override void Start () {
		TwitterCombo.init( "k6lpYZrWpvVVICQQHVQVkGCIT", "rCEaAZmgFEemPJ7s18cVkgRLQ5JegoR9vO97uya8F1vBl7rgaY" );

        base.Start();
	}

    protected override void OnEnable()
	{
		TwitterManager.loginSucceededEvent += LoginSucceeded;
		TwitterManager.loginFailedEvent += LoginFailed;
		TwitterManager.requestDidFinishEvent += PostTweetSucceeded;
		TwitterManager.requestDidFailEvent += PostTweetFailed;

        base.OnEnable();
	}

	void OnDisable()
	{
		TwitterManager.loginSucceededEvent -= LoginSucceeded;
		TwitterManager.loginFailedEvent -= LoginFailed;
		TwitterManager.requestDidFinishEvent -= PostTweetSucceeded;
		TwitterManager.requestDidFailEvent -= PostTweetFailed;
	}

	void LoginSucceeded(object result)
	{
		TwitterCombo.postStatusUpdate (status + " " + url);
	}

	void LoginFailed(string error)
	{
		popupMessage = "Login to Twitter failed.";
		popupActive = true;
	}

	void PostTweetSucceeded(object result)
	{
		if (TwitterCombo.isLoggedIn ()) {
			popupMessage = "Tweet is posted!";
			popupActive = true;
		}
	}

	void PostTweetFailed(string error)
	{		
		popupMessage = "Tweet could not be posted.";
		popupActive = true;
	}

    public override void Post(string status, string url)
	{
		this.status = status;
		this.url = url;

		TwitterCombo.showLoginDialog();		
	}

	#endif

}
