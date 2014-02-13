using UnityEngine;
using System.Collections;

public class GameWorld : MonoBehaviour
{
    // Entity prefabs
    public GameObject playerPrefab, wallPrefab, boxPrefab, trolleyPrefab, sanitizerPrefab;
    // Current level
    public static int level;
    // Level maps
    public static char[, ,] levels = new char[,,] { {		  // Level 1
			   { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
			   { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
			   { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 's', 'w' },
			   { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', ' ', 'b', ' ', ' ', ' ', 'w' },
			   { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', 's', 'w' },
			   { 'w', ' ', ' ', ' ', ' ', ' ', 't', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
			   { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
			   { 'w', ' ', ' ', ' ', 'b', ' ', 's', ' ', 't', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
			   { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'b', ' ', ' ', ' ', 'w' },
			   { 'w', ' ', 'P', ' ', ' ', ' ', 't', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
			   { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', 's', ' ', 'w' },
			   { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' }
		  }
	 };
    // Entities
    private GameObject EntityContainer;
    private GameObject WallContainer, CollectibleContainer, PushableContainer;
    private Transform Player;
    private ArrayList Walls, Collectibles, Pushables;

    // Use this for initialization
    void Start()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.LevelStart += LevelStart;
        GameEventManager.LevelOver += LevelOver;

        Walls = new ArrayList();
        Collectibles = new ArrayList();
        Pushables = new ArrayList();

        GameEventManager.TriggerGameMenu();
    }
    // Update is called once per frame
    void Update()
    {
        switch (GameEventManager.CurrentState)
        {
            case GameEventManager.GameState.InMenu:
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
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
        if (level > levels.GetLength(0))
            level = 1;

        // Instantiate the containers
        EntityContainer = new GameObject("21 Entities");

        WallContainer = new GameObject("Walls");
        WallContainer.transform.parent = EntityContainer.transform;

        CollectibleContainer = new GameObject("Collectibles");
        CollectibleContainer.transform.parent = EntityContainer.transform;

        PushableContainer = new GameObject("Pushables");
        PushableContainer.transform.parent = EntityContainer.transform;

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
                        var detectorObject = GameObject.Find("HandyDetector");
                        if (detectorObject != null)
                        {
                            var detector = detectorObject.gameObject.GetComponent<HandyDetector>();
                            if (detector != null)
                            {
                                detector.defaultObject = Player.gameObject;
                            }
                        }
                        break;
                    case 'w': // Walls
                        Walls.Add((GameObject.Instantiate(wallPrefab, new Vector3(i - offsetX, -j + offsetY, wallPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Walls[Walls.Count - 1] as Transform).parent = WallContainer.transform;
                        break;
                    case 'b': // Pushable Boxes
                        Pushables.Add((GameObject.Instantiate(boxPrefab, new Vector3(i - offsetX, -j + offsetY, boxPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Pushables[Pushables.Count - 1] as Transform).parent = PushableContainer.transform;
                        break;
                    case 't': // Pushable Trolleys
                        Pushables.Add((GameObject.Instantiate(trolleyPrefab, new Vector3(i - offsetX, -j + offsetY, trolleyPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Pushables[Pushables.Count - 1] as Transform).parent = PushableContainer.transform;
                        break;
                    case 's': // Sanitizer
                        Collectibles.Add((GameObject.Instantiate(sanitizerPrefab, new Vector3(i - offsetX, -j + offsetY, sanitizerPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Collectibles[Collectibles.Count - 1] as Transform).parent = CollectibleContainer.transform;
                        break;

                }
            }
        }
    }

    void LevelOver()
    {
        // Clear resources
        if (EntityContainer != null)
            Destroy(EntityContainer);
    }
}

