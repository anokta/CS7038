using Grouping;
using UnityEngine;

public class CheatManager
{
    public static CheatManager Instance { get; private set; }

    public bool IsCheating { get; set; }

    public static void Initialize()
    {
        Instance = new CheatManager();
    }

    public CheatManager()
    {
        var detector = new KeySequenceDetector("T C D", EnableCheating);
        KeyboardController.Instance.Add(detector);
    }

    public void EnableCheating()
    {
        IsCheating = true;
        AudioManager.PlaySFX("Collect");
    }

    public void Update()
    {
        if (IsCheating && Input.GetKeyDown(KeyCode.Q))
        {
            LevelManager.instance.Level -= 2;
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        }

        if (IsCheating && Input.GetKeyDown(KeyCode.E))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        }

        if (IsCheating && Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.instance.Level--;
            GameWorld.success = false;
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        }
    }
}
