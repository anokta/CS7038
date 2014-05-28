using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Grouping;
using Prime31;

public class TwitterIntegration : MonoBehaviour {

	#if UNITY_IPHONE || UNITY_ANDROID

	private string status, url;

	private bool popupActive = false;
	private string popupMessage = "";
	private float popupY;

	void Start () {
		TwitterCombo.init( "k6lpYZrWpvVVICQQHVQVkGCIT", "rCEaAZmgFEemPJ7s18cVkgRLQ5JegoR9vO97uya8F1vBl7rgaY" );

		GroupManager.main.group["Level Over"].Add(this);

	}

	void OnEnable()
	{
		TwitterManager.loginSucceededEvent += LoginSucceeded;
		TwitterManager.loginFailedEvent += LoginFailed;
		TwitterManager.requestDidFinishEvent += PostTweetSucceeded;
		TwitterManager.requestDidFailEvent += PostTweetFailed;
		
		popupY = 1.0f;
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

	void OnGUI()
	{
		if (popupActive || popupY < 1.0f) {
			if (Input.GetMouseButtonDown (0)) {
				popupActive = false;		
			}

			popupY = Mathf.Lerp (popupY, popupActive ? 0.9f : 1.1f, Time.deltaTime * 4);

			GUI.Window(5, new Rect(Screen.width * 0.5f - Screen.height * 0.15f, Screen.height * popupY, Screen.height * 0.3f, Screen.height * 0.1f), DoPopup, "", GUIManager.Style.inGameWindow);
		}
	}

	void DoPopup(int id)
	{
		GUILayout.FlexibleSpace ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label (popupMessage, GUIManager.Style.log);
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace ();
	}

	public void Post(string status, string url)
	{
		this.status = status;
		this.url = url;

		TwitterCombo.showLoginDialog();		
	}

	#endif

}
