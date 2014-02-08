using UnityEngine;
using System.Collections;

public class GameWorld : MonoBehaviour
{
    // Entity prefabs
    public GameObject playerPrefab, wallPrefab, boxPrefab;
  
    // Current level
    public static int level;

    // Level maps
    public static char[, ,] levels = new char[,,]
    {
        // Level 1
     {
		{ 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
        { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
        { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
        { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', ' ', 'b', ' ', ' ', ' ', 'w' },
        { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
        { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
        { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
        { 'w', ' ', ' ', ' ', 'b', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
        { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'b', ' ', ' ', ' ', 'w' },
        { 'w', ' ', 'P', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
        { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
		{ 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' }
    }
    };

    // Entities  
    private GameObject EntityContainer, WallContainer, BoxContainer;

    private Transform Player;
    private ArrayList Walls;
    private ArrayList Boxes;


    // Use this for initialization
    void Start()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.LevelStart += LevelStart;
        GameEventManager.LevelOver += LevelOver;

        Walls = new ArrayList();
        Boxes = new ArrayList();

        GameEventManager.TriggerGameMenu();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameEventManager.CurrentState)
        {
            case GameEventManager.GameState.InMenu:
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    // Start the level
                    GameEventManager.TriggerLevelStart();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    // Exit the application
                    GameEventManager.TriggerGameQuit();
                }

                break;

            case GameEventManager.GameState.Running:

                if (Input.GetKeyDown(KeyCode.R))
                {
                    // Restart the level (for testing purposes)
                    GameEventManager.TriggerLevelOver();
                    level--;
                    GameEventManager.TriggerLevelStart();
                }

                break;
        }
    }

    void GameMenu()
    {
        level = 0;
    }

    void LevelStart()
    {
        // Next level
        level++;
        if (level > levels.GetLength(0)) level = 1;

        // Instantiate the containers
        EntityContainer = new GameObject("21 Entities");

        WallContainer = new GameObject("Walls");
        WallContainer.transform.parent = EntityContainer.transform;

        BoxContainer = new GameObject("Boxes");
        BoxContainer.transform.parent = EntityContainer.transform;
        
        // Generate the level
        int mapWidth = levels.GetLength(2);
        int mapHeight = levels.GetLength(1);

        float offsetX = (mapWidth - 1) / 2.0f;
        float offsetY = (mapHeight - 1) / 2.0f;

        for (int i = 0; i < mapWidth; ++i)
        {
            for (int j = 0; j < mapHeight; ++j)
            {

                switch (levels[level - 1, j, i])
                {
                    case 'P': // Dr Handrew
                        Player = (GameObject.Instantiate(playerPrefab, new Vector3(i - offsetX, -j + offsetY, playerPrefab.transform.position.z), Quaternion.identity) as GameObject).transform;
                        Player.parent = EntityContainer.transform;
                        break;
                    case 'w': // Walls
                        Walls.Add((GameObject.Instantiate(wallPrefab, new Vector3(i - offsetX, -j + offsetY, wallPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Walls[Walls.Count - 1] as Transform).parent = WallContainer.transform;
                        break;
                    case 'b': // Pushable Boxes
                        Boxes.Add((GameObject.Instantiate(boxPrefab, new Vector3(i - offsetX, -j + offsetY, boxPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Boxes[Boxes.Count - 1] as Transform).parent = BoxContainer.transform;
                        break;

                }
            }
        }
    }

    void LevelOver()
    {
        // Clear resources
        if (EntityContainer != null) Destroy(EntityContainer);
    }
}

