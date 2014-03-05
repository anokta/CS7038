using UnityEngine;
using System.Collections;

public static class GameEventManager
{

    public delegate void GameEvent();

    // Game Events
    public static event GameEvent LevelStart, LevelOver, GameMenu, GameQuit;

    // Game State
    public enum GameState { InMenu, Running, Over };
    public static GameState CurrentState = GameState.InMenu;


    public static void TriggerLevelStart()
    {
        if (LevelStart != null)
        {
            LevelStart();
            CurrentState = GameState.Running;
        }

        Debug.Log("TriggerGameStart - State: " + CurrentState);
    }

    public static void TriggerLevelOver()
    {
        if (LevelOver != null)
        {
            LevelOver();
            CurrentState = GameState.Over;
        }

        Debug.Log("TriggerGameOver - State: " + CurrentState);
    }

    public static void TriggerGameMenu()
    {
        if (GameMenu != null)
        {
            GameMenu();
            CurrentState = GameState.InMenu;
        }

        Debug.Log("TriggerGameMenu - State: " + CurrentState);
    }

    public static void TriggerGameQuit()
    {
        if (GameQuit != null)
        {
            GameQuit();
        }

        Debug.Log("Quiting . . .");

        Application.Quit();
    }
}