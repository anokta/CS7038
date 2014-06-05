using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using Grouping;

public class FacebookIntegration : SocialIntegration
{
	#if UNITY_IPHONE || UNITY_ANDROID

	protected override void Start()
	{
        FacebookCombo.init();

        base.Start();
	}

    protected override void OnEnable()
    {
        FacebookManager.loginFailedEvent += LoginFailed;
		FacebookManager.dialogCompletedWithUrlEvent += ShareCompleted;
       	FacebookManager.dialogFailedEvent += ShareFailed;

        base.OnEnable();
    }

    void OnDisable()
    {
        FacebookManager.loginFailedEvent -= LoginFailed;
		FacebookManager.dialogCompletedWithUrlEvent -= ShareCompleted;
		FacebookManager.dialogFailedEvent -= ShareFailed;
    }


    void LoginFailed(P31Error error)
    {
        popupMessage = "Login to Facebook failed.";
        popupActive = true;
    }

	void ShareCompleted(string result)
    {
		popupMessage = (result.Length > 0 && result.Contains("post_id")) ? "Post published succesfully!"  : "Post could not be published.";
        popupActive = true;
    }

    void ShareFailed(P31Error error)
    {
        popupMessage = "Post could not be published.";
        popupActive = true;
    }

	public override void Post(string caption, string link)
	{
		#if UNITY_IOS
			var parameters = new Dictionary<string, object>
			{
				{ "link", link },
				{ "name", "Handy MD" },
				{ "picture", "https://pbs.twimg.com/profile_images/443709766687150080/6EZbY-5c_400x400.png" },
				{ "caption", caption },
				{ "description", "A 2D puzzle/platform game for mobile devices to promote the importance of hand hygiene, by Surewash" }
			};
			FacebookCombo.showFacebookShareDialog( parameters );
		#elif UNITY_ANDROID
			var parameters = new Dictionary<string, string>
			{
				{ "link", link },
				{ "name", "Handy MD" },
				{ "caption", caption },
				{ "description", "A 2D puzzle/platform game for mobile devices to promote the importance of hand hygiene, by Surewash" }
			};
			FacebookCombo.showDialog( "stream.publish", parameters );
		#endif
	}

#endif
}