using UnityEngine;
using System.Collections;

public class GameWorld : MonoBehaviour
{
    // Entity prefabs
    public GameObject playerPrefab, patientPrefab;
    public GameObject wallPrefab, cratePrefab, trolleyPrefab, sanitizerPrefab, doorPrefab, fountainPrefab;
    public GameObject laserEmitterPrefab, mirrorPrefab, mirrorInversePrefab, explosiveCratePrefab;
    public GameObject gatePrefab, leverPrefab;

    // Current level
    public static int level;
    // Level maps
    public static char[, ,] levels =
    {
        {   // Level 1
            { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },//  5.5
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },//  4.5
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 's', 'w' },//  3.5
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'b', ' ', ' ', ' ', 'w' },//  2.5
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'b', ' ', 's', 'w' },//  1.5
            { 'w', ' ', 'e', 'x', ' ', ' ', 'b', ' ', ' ', ' ', 't', ' ', ' ', 'b', ' ', 'w' },//  0.5
            { 'w', 'f', ' ', ' ', ' ', ' ', ' ', 'b', ' ', 't', ' ', ' ', ' ', ' ', 'b', 'w' },// -0.5
            { 'w', ' ', ' ', ' ', 't', ' ', ' ', ' ', 'b', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },// -1.5
            { 'w', ' ', 'm', ' ', ' ', 'i', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },// -2.5
            { 'w', ' ', 'P', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },// -3.5
            { 'w', 'p', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 's', ' ', 'w' },// -4.5
            { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' } // -5.5
            //-7.5 -6.5 -5.5 -4.5 -3.5 -2.5 -1.5 -0.5  0.5 1.5  2.5  3.5  4.5  5.5  6.5  7.5
        },
        {
            { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
            { 'w', 'b', ' ', ' ', ' ', ' ', 'w', ' ', 's', 'w', ' ', ' ', ' ', ' ', 'b', 'w' },
            { 'w', ' ', 'b', ' ', ' ', ' ', 'w', ' ', ' ', 'w', ' ', ' ', ' ', 'b', ' ', 'w' },
            { 'w', ' ', ' ', 'b', ' ', ' ', 'w', 'd', 'w', 'w', ' ', ' ', 'b', ' ', 'f', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', 'b', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 't', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', 't', ' ', ' ', ' ', ' ', ' ', 'w', 'w', 'd', 'w', 'w', 'w', 'w' },
            { 'w', ' ', 't', 'P', 't', ' ', ' ', 't', ' ', 'w', ' ', 'b', ' ', ' ', 's', 'w' },
            { 'w', ' ', ' ', 't', ' ', ' ', ' ', ' ', ' ', 'w', 's', ' ', ' ', ' ', 'b', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', 's', ' ', ' ', ' ', 'b', 'w' },
            { 'w', 'p', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', 'b', ' ', ' ', 's', 'w' },
            { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' }
        },
        {
            { 'w', 'w', 'e', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 's', 'w' },
            { 'w', ' ', 'm', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'b', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 's', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', 't', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', 'w', 'd', 'w', 'w', 'w', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', 'b', 'w' },
            { 'w', ' ', 'P', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', 'p', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', 'f', ' ', ' ', 's', ' ', 'w' },
            { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' }
        },
        {
            { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', 'p', ' ', 'p', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 's', 'w' },
            { 'w', 'w', 'w', 'g', 'w', 'w', 'w', 'w', 'w', 'w', ' ', 'b', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', ' ', 's', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', 't', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', 'f', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', 'w', 'd', 'w', 'w', 'w', 'w' },
            { 'w', ' ', ' ', ' ', 'b', ' ', 'l', ' ', 't', ' ', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'b', ' ', ' ', ' ', 'w' },
            { 'w', ' ', 'P', ' ', ' ', ' ', 't', ' ', ' ', 'w', ' ', ' ', ' ', ' ', ' ', 'w' },
            { 'w', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'w', ' ', ' ', ' ', 's', ' ', 'w' },
            { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' }
        }
    };

    // Entities
    private GameObject EntityContainer;
    private GameObject WallContainer, CollectibleContainer, PushableContainer, AccessibleContainer, SwitchableContainer;
    private Transform Player;
    private ArrayList Walls, Collectibles, Pushables, Accessibles, Switchables;

    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
 
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.LevelStart += LevelStart;
        GameEventManager.LevelOver += LevelOver;

        Walls = new ArrayList();
        Collectibles = new ArrayList();
        Pushables = new ArrayList();
        Accessibles = new ArrayList();
        Switchables = new ArrayList();

        GameEventManager.TriggerGameMenu();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the application
            GameEventManager.TriggerGameQuit();
        }

        switch (GameEventManager.CurrentState)
        {
            case GameEventManager.GameState.InMenu:
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    // Start the level
                    GameEventManager.TriggerLevelStart();
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

                #region TO_BE_DELETED

                // Testing purposes only //
                bool isOver = true;
                foreach(Transform patient in Switchables)
                {
                    if (patient.name.StartsWith("Patient"))
                    {
                        isOver &= patient.GetComponent<Patient>().IsTreated();
                    }
                }
                if (isOver)
                    GameEventManager.TriggerLevelOver();
                //if (CollectibleContainer.transform.childCount == 0)
                //{
                //    GameEventManager.TriggerLevelOver();
                //}

                break;

            case GameEventManager.GameState.Over:
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    // Start the level
                    GameEventManager.TriggerLevelStart();
                }
                #endregion

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

        AccessibleContainer = new GameObject("Accessibles");
        AccessibleContainer.transform.parent = EntityContainer.transform;

        SwitchableContainer = new GameObject("Switchables");
        SwitchableContainer.transform.parent = EntityContainer.transform;

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
                        Player = (Instantiate(playerPrefab, new Vector3(i - offsetX, -j + offsetY, playerPrefab.transform.position.z), Quaternion.identity) as GameObject).transform;
                        Player.parent = EntityContainer.transform;
                        break;
                    case 'w': // Walls
                        Walls.Add((Instantiate(wallPrefab, new Vector3(i - offsetX, -j + offsetY, wallPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Walls[Walls.Count - 1] as Transform).parent = WallContainer.transform;
                        break;
                    case 'b': // Pushable Crates
                        Pushables.Add((Instantiate(cratePrefab, new Vector3(i - offsetX, -j + offsetY, cratePrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Pushables[Pushables.Count - 1] as Transform).parent = PushableContainer.transform;
                        break;
                    case 't': // Pushable Trolleys
                        Pushables.Add((Instantiate(trolleyPrefab, new Vector3(i - offsetX, -j + offsetY, trolleyPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Pushables[Pushables.Count - 1] as Transform).parent = PushableContainer.transform;
                        break;
                    case 's': // Sanitizers
                        Collectibles.Add((Instantiate(sanitizerPrefab, new Vector3(i - offsetX, -j + offsetY, sanitizerPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Collectibles[Collectibles.Count - 1] as Transform).parent = CollectibleContainer.transform;
                        break;
                    case 'd': // Doors
                        Accessibles.Add((Instantiate(doorPrefab, new Vector3(i - offsetX, -j + offsetY, doorPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Accessibles[Accessibles.Count - 1] as Transform).parent = AccessibleContainer.transform;
                        break;
                    case 'f': // Fountain
                        Accessibles.Add((GameObject.Instantiate(fountainPrefab, new Vector3(i - offsetX, -j + offsetY, fountainPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Accessibles[Accessibles.Count - 1] as Transform).parent = AccessibleContainer.transform;
                        break;
                    case 'e': // Laser Emitters
                        Walls.Add((Instantiate(laserEmitterPrefab, new Vector3(i - offsetX, -j + offsetY, laserEmitterPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Walls[Walls.Count - 1] as Transform).parent = WallContainer.transform;
                        break;
                    case 'm': // Mirrors
                        Pushables.Add((Instantiate(mirrorPrefab, new Vector3(i - offsetX, -j + offsetY, mirrorPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Pushables[Pushables.Count - 1] as Transform).parent = PushableContainer.transform;
                        break;
                    case 'i': // Inverse Mirrors
                        Pushables.Add((Instantiate(mirrorInversePrefab, new Vector3(i - offsetX, -j + offsetY, mirrorInversePrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Pushables[Pushables.Count - 1] as Transform).parent = PushableContainer.transform;
                        break;
                    case 'x': // Explosive Crates
                        Pushables.Add((Instantiate(explosiveCratePrefab, new Vector3(i - offsetX, -j + offsetY, explosiveCratePrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Pushables[Pushables.Count - 1] as Transform).parent = PushableContainer.transform;
                        break;
                    case 'g': // Gates
                        Accessibles.Add((Instantiate(gatePrefab, new Vector3(i - offsetX, -j + offsetY, gatePrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Accessibles[Accessibles.Count - 1] as Transform).parent = AccessibleContainer.transform;
                        break;
                    case 'l': // Levers
                        Switchables.Add((Instantiate(leverPrefab, new Vector3(i - offsetX, -j + offsetY, leverPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Switchables[Switchables.Count - 1] as Transform).parent = SwitchableContainer.transform;
                        break;
                    case 'p': // Patients
                        Switchables.Add((Instantiate(patientPrefab, new Vector3(i - offsetX, -j + offsetY, patientPrefab.transform.position.z), Quaternion.identity) as GameObject).transform);
                        (Switchables[Switchables.Count - 1] as Transform).parent = SwitchableContainer.transform;
                        break;
                }
            }
        }

        BindLeverGate();
    }

    void LevelOver()
    {
        // Clear resources
        if (EntityContainer != null)
            Destroy(EntityContainer);

        Walls.Clear();
        Collectibles.Clear();
        Pushables.Clear();
        Accessibles.Clear();
        Switchables.Clear();
    }

    void BindLeverGate()
    {
        // TO BE CHANGED
        var leverObj = GameObject.Find("Lever(Clone)");
        if (leverObj == null) return;
        var lever = leverObj.GetComponent<Lever>();

        var gateObj = GameObject.Find("Gate(Clone)");
        if (gateObj == null) return;
        var gate = gateObj.GetComponent<Gate>();

        lever.gate = gate;
        //
    }
}
