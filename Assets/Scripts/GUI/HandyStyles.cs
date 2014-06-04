
//------------------------------------------------------------------------------
//
//	Fact 1: OnGUI gets called MULTIPLE times per frame
//	Fact 2: GetStyle(string) performs a lookup that is unnecessary
//	Fact 3: This lookup happens a lot, and especially in the level selector
//
//  This class caches custom styles and creates references to styles that can be used
//
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class HandyStyles
{
	public HandyStyles(GUISkin skin)
	{
		/*
		 * This is in case the ordering changes
		 * We only need to have the same ordering here as well
		*/
		int count = 0;
		author = skin.customStyles[count++];
		content = skin.customStyles[count++];
		narratorContent = skin.customStyles[count++];
		contentBorder = skin.customStyles[count++];
		next = skin.customStyles[count++];
		skip = skin.customStyles[count++];
		restartInGame = skin.customStyles[count++];
		restartOver = skin.customStyles[count++];
		continueButton = skin.customStyles[count++];
		play = skin.customStyles[count++];
		pause = skin.customStyles[count++];
		menu = skin.customStyles[count++];
		volumeOn = skin.customStyles[count++];
		volumeOff = skin.customStyles[count++];
		credits = skin.customStyles[count++];
		info = skin.customStyles[count++];
		back = skin.customStyles[count++];
		xbutton = skin.customStyles[count++];
		title = skin.customStyles[count++];
		overTitle = skin.customStyles[count++];
		overSuccess = skin.customStyles[count++];
		overMessage = skin.customStyles[count++];
		levelLabel = skin.customStyles[count++];
		inGameWindow = skin.customStyles[count++];
		overWindow = skin.customStyles[count++];
		rectButton = skin.customStyles[count++];
		buttonYes = skin.customStyles[count++];
		buttonNo = skin.customStyles[count++];
		sureButton = skin.customStyles[count++];
		debugText = skin.customStyles[count++];
		creditsName = skin.customStyles[count++];
		creditsDescription = skin.customStyles[count++];
		creditsMessage = skin.customStyles[count++];
		facebook = skin.customStyles[count++];
		twitter = skin.customStyles[count++];
		starFull = skin.customStyles[count++];
		starEmpty = skin.customStyles[count++];
		scores = skin.customStyles[count++];
		loading = skin.customStyles[count++];
		log = skin.customStyles[count++];
	}

	public readonly GUIStyle author;
	public readonly GUIStyle content;
	public readonly GUIStyle narratorContent;
	public readonly GUIStyle contentBorder;
	public readonly GUIStyle next;
	public readonly GUIStyle skip;
	public readonly GUIStyle restartInGame;
	public readonly GUIStyle restartOver;
	public readonly GUIStyle continueButton;
	public readonly GUIStyle play;
	public readonly GUIStyle pause;
	public readonly GUIStyle menu;
	public readonly GUIStyle volumeOn;
	public readonly GUIStyle volumeOff;
	public readonly GUIStyle credits;
	public readonly GUIStyle info;
	public readonly GUIStyle back;
	public readonly GUIStyle xbutton;
	public readonly GUIStyle title;
	public readonly GUIStyle overTitle;
	public readonly GUIStyle overSuccess;
	public readonly GUIStyle overMessage;
	public readonly GUIStyle levelLabel;
	public readonly GUIStyle inGameWindow;
	public readonly GUIStyle overWindow;
	public readonly GUIStyle rectButton;
	public readonly GUIStyle buttonYes;
	public readonly GUIStyle buttonNo;
	public readonly GUIStyle sureButton;
	public readonly GUIStyle debugText;
	public readonly GUIStyle creditsName;
	public readonly GUIStyle creditsDescription;
	public readonly GUIStyle creditsMessage;
	public readonly GUIStyle facebook;
	public readonly GUIStyle twitter;
	public readonly GUIStyle starFull;
	public readonly GUIStyle starEmpty;
	public readonly GUIStyle scores;
	public readonly GUIStyle loading;
	public readonly GUIStyle log;
}

