using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed class FacebookIntegration : MonoBehaviour
{
    #region FB.Init() example

    private bool isInit = false;

    private void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private void OnInitComplete()
    {
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
        isInit = true;
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }

    #endregion

    #region FB.Login() example

    public void CallFBLogin()
    {
        FB.Login("email,publish_actions", LoginCallback);
    }

    void LoginCallback(FBResult result)
    {
        if (result.Error != null)
            lastResponse = "Error Response:\n" + result.Error;
        else if (!FB.IsLoggedIn)
        {
            lastResponse = "Login cancelled by Player";
        }
        else
        {
            lastResponse = "Login was successful!";

            CallFBFeed();
        }
    }

    private void CallFBLogout()
    {
        FB.Logout();
    }
    #endregion

    #region FB.Feed() example

    public string FeedToId = "";
    public string FeedLink = "http://handymd-game.appspot.com/";
    public string FeedLinkName = "Handy MD";
    public string FeedLinkCaption = "Play on the web!";
    public string FeedLinkDescription = "";
    public string FeedPicture = "";
    public string FeedMediaSource = "";
    public string FeedActionName = "";
    public string FeedActionLink = "";
    public string FeedReference = "";

    private void CallFBFeed()
    {
        FB.Feed(
            toId: FeedToId,
            link: FeedLink,
            linkName: FeedLinkName,
            linkCaption: FeedLinkCaption,
            linkDescription: FeedLinkDescription,
            picture: FeedPicture,
            mediaSource: FeedMediaSource,
            actionName: FeedActionName,
            actionLink: FeedActionLink,
            reference: FeedReference,
            properties: null,
            callback: null
        );
    }

    #endregion

    private string lastResponse = "";

    private int TextWindowHeight
    {
        get
        {
            return Screen.height;
        }
    }

    void Awake()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    public bool PostFeed()
    {
        if (isInit)
        {
            if (!FB.IsLoggedIn)
            {
                CallFBLogin();
            }
            else
            {
                CallFBFeed();
            }

            return FB.IsLoggedIn;
        }

        return false;
    }
}