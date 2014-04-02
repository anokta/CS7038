using UnityEngine;
using System.Collections;
using Grouping;

public class ToLevelOver : MonoBehaviour {

    Timer timer;

    void Start()
    {
        GroupManager.main.group["To Level Over"].Add(this);
        GroupManager.main.group["To Level Over"].Add(this, new GroupDelegator(null, StartCounter, null));

        timer = new Timer(1.2f, WaitComplete);
        timer.Stop();
	}
	
	void Update () {
        timer.Update();
	}

    void StartCounter()
    {
        timer.Reset();
    }

    void WaitComplete()
    {
        GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
    }
}
