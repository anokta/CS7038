using System;
using System.Collections.Generic;
using TiledMax;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelLoader
{
    public static LevelLoader Instance { get; private set; }
    // Entities
    private GameObject entityContainer;
	private GameObject wallContainer, shadeContainer, floorContainer, collectibleContainer, pushableContainer, accessibleContainer, switchableContainer;
    private readonly Dictionary<TileType, GameObject> prefabs;

	private GameObject shade;
    public GameObject ExplosionContainer { get; private set; }

	/// <summary>
	/// Sorting order for the floor
	/// </summary>
	public static readonly short FloorOrder = short.MinValue;
	/// <summary>
	/// Sorting order for any ashes
	/// </summary>
	public static readonly short AshesOrder = (short)(FloorOrder + 10);

	/// <summary>
	/// Distance of sorting order between two levels of walls.
	/// </summary>
	public static readonly short PlaceWallOffset = 10;
	/// <summary>
	/// Distance of sorting order between an object and a wall of the same level.
	/// </summary>
	public static readonly short PlaceDepthOffset = 5;
	/// <summary>
	/// Equal to: PlaceDepthOffset - 1
	/// </summary>
	public static readonly short UsableOffset = 4;


	public static short PlaceWall(float y) {
		return (short)-Mathf.RoundToInt(10 * y);
	}

	public static short PlaceDepth(float y) {
		return (short)(-Mathf.RoundToInt(10 * y) - 5);
	}


    public LevelLoader()
    {
        prefabs = new Dictionary<TileType, GameObject>();
        prefabs[TileType.Crate] = Resources.Load<GameObject>("Crate");
        prefabs[TileType.Explosive] = Resources.Load<GameObject>("ExplosiveCrate");
        prefabs[TileType.Trolley] = Resources.Load<GameObject>("Trolley");
        
        prefabs[TileType.Player] = Resources.Load<GameObject>("Dr Handrew");
        prefabs[TileType.Patient] = Resources.Load<GameObject>("Patient");
        prefabs[TileType.Plant] = Resources.Load<GameObject>("Plant");
        prefabs[TileType.Fountain] = Resources.Load<GameObject>("Fountain");
        prefabs[TileType.Door] = Resources.Load<GameObject>("Door");
		prefabs[TileType.GateClean] = Resources.Load<GameObject>("Gate Clean");

        prefabs[TileType.LaserDown] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.LaserUp] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.LaserRight] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.LaserLeft] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.Mirror] = Resources.Load<GameObject>("Mirror");
        prefabs[TileType.MirrorInverse] = Resources.Load<GameObject>("Mirror");
        
        prefabs[TileType.Wall] = Resources.Load<GameObject>("Wall");
        prefabs[TileType.Floor] = Resources.Load<GameObject>("Floor");
        prefabs[TileType.Sanitizer] = Resources.Load<GameObject>("Sanitizer");

        prefabs[TileType.Lever1] = Resources.Load<GameObject>("Lever");
        prefabs[TileType.Lever2] = Resources.Load<GameObject>("Lever");
        prefabs[TileType.Lever3] = Resources.Load<GameObject>("Lever");

        prefabs[TileType.Gate1] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.Gate2] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.Gate3] = Resources.Load<GameObject>("Gate");

        prefabs[TileType.Gate1Open] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.Gate2Open] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.Gate3Open] = Resources.Load<GameObject>("Gate");

        prefabs[TileType.Gate1Vertical] = Resources.Load<GameObject>("Gate Vertical");
        prefabs[TileType.Gate2Vertical] = Resources.Load<GameObject>("Gate Vertical");
        prefabs[TileType.Gate3Vertical] = Resources.Load<GameObject>("Gate Vertical");

        prefabs[TileType.Gate1VerticalOpen] = Resources.Load<GameObject>("Gate Vertical");
        prefabs[TileType.Gate2VerticalOpen] = Resources.Load<GameObject>("Gate Vertical");
        prefabs[TileType.Gate3VerticalOpen] = Resources.Load<GameObject>("Gate Vertical");

		//This shouldn't be a TileType object
		shade = Resources.Load<GameObject>("Shade");
    }

    public void Clear()
    {
        if (entityContainer != null) Object.Destroy(entityContainer);
    }

    public void Load(TmxMap map)
    {
        Instance = this;

        // Instantiate the containers
        entityContainer = new GameObject("31 Entities");

        wallContainer = new GameObject("Walls");
        wallContainer.transform.parent = entityContainer.transform;

        floorContainer = new GameObject("Floors");
        floorContainer.transform.parent = entityContainer.transform;

        collectibleContainer = new GameObject("Collectibles");
        collectibleContainer.transform.parent = entityContainer.transform;

        pushableContainer = new GameObject("Pushables");
        pushableContainer.transform.parent = entityContainer.transform;

        accessibleContainer = new GameObject("Accessibles");
        accessibleContainer.transform.parent = entityContainer.transform;

        switchableContainer = new GameObject("Switchables");
        switchableContainer.transform.parent = entityContainer.transform;

		shadeContainer = new GameObject("Shades");
		shadeContainer.transform.parent = entityContainer.transform;

        ExplosionContainer = new GameObject("Explosions");
        ExplosionContainer.transform.parent = entityContainer.transform;

        var leverGateManager = new LeverGateManager();

        foreach (var layer in map.Layers)
        {
            var tiles = layer.Tiles;

            foreach (var tile in tiles)
            {
                var tileType = (TileType)tile.Gid;

                if (tileType == TileType.Empty) continue;
                if (!prefabs.ContainsKey(tileType)) continue;

                var indexX = tile.X;
                var indexY = tile.Y;
                var position = new Vector2(indexX, map.Height - indexY - 1);

                var prefab = prefabs[tileType];
                var gameObj = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
                var transform = gameObj.transform;
                GameObject parent;

                switch (tileType)
                {
                    case TileType.Player:
                        parent = entityContainer;
                        break;
                    case TileType.Crate:
                    case TileType.Trolley:
                    case TileType.Explosive:
					case TileType.Plant:
                        parent = pushableContainer;
                        break;
                    case TileType.Sanitizer:
                        parent = collectibleContainer;
                        break;
                    case TileType.Door:
                    case TileType.Fountain:
                    case TileType.GateClean:
                        parent = accessibleContainer;
                        break;
					case TileType.Patient:
						parent = switchableContainer;
						transform.GetComponent<SpriteRenderer>().sortingOrder = PlaceDepth(transform.position.y);//-Mathf.RoundToInt(4 * transform.position.y) - 1;
                        break;

                    case TileType.Lever1:
                        parent = accessibleContainer;
                        var lever1 = gameObj.GetComponent<Lever>();
                        lever1.LeverGateType = LeverGateType.Type1;
                        leverGateManager.Add(lever1);
                        break;
                    case TileType.Lever2:
                        parent = accessibleContainer;
                        var lever2 = gameObj.GetComponent<Lever>();
                        lever2.LeverGateType = LeverGateType.Type2;
                        leverGateManager.Add(lever2);
                        break;
                    case TileType.Lever3:
                        parent = accessibleContainer;
                        var lever3 = gameObj.GetComponent<Lever>();
                        lever3.LeverGateType = LeverGateType.Type3;
                        leverGateManager.Add(lever3);
                        break;

                    case TileType.Gate1:
                    case TileType.Gate1Vertical:
                        parent = accessibleContainer;
                        var gate1 = gameObj.GetComponent<Gate>();
                        gate1.LeverGateType = LeverGateType.Type1;
                        leverGateManager.Add(gate1);
                        break;
                    case TileType.Gate2:
                    case TileType.Gate2Vertical:
                        parent = accessibleContainer;
                        var gate2 = gameObj.GetComponent<Gate>();
                        gate2.LeverGateType = LeverGateType.Type2;
                        leverGateManager.Add(gate2);
                        break;
                    case TileType.Gate3:
                    case TileType.Gate3Vertical:
                        parent = accessibleContainer;
                        var gate3 = gameObj.GetComponent<Gate>();
                        gate3.LeverGateType = LeverGateType.Type3;
                        leverGateManager.Add(gate3);
                        break;

                    case TileType.Gate1Open:
                    case TileType.Gate1VerticalOpen:
                        parent = accessibleContainer;
                        var gate1Open = gameObj.GetComponent<Gate>();
                        gate1Open.LeverGateType = LeverGateType.Type1;
                        gate1Open.Open = true;
                        leverGateManager.Add(gate1Open);
                        break;
                    case TileType.Gate2Open:
                    case TileType.Gate2VerticalOpen:
                        parent = accessibleContainer;
                        var gate2Open = gameObj.GetComponent<Gate>();
                        gate2Open.LeverGateType = LeverGateType.Type2;
                        gate2Open.Open = true;
                        leverGateManager.Add(gate2Open);
                        break;
                    case TileType.Gate3Open:
                    case TileType.Gate3VerticalOpen:
                        parent = accessibleContainer;
                        var gate3Open = gameObj.GetComponent<Gate>();
                        gate3Open.LeverGateType = LeverGateType.Type3;
                        gate3Open.Open = true;
                        leverGateManager.Add(gate3Open);
                        break;

                    case TileType.LaserDown:
                        parent = wallContainer;
                        var laserDown = gameObj.GetComponent<LaserEmitter>();
                        laserDown.Direction = Direction.Down;
                        break;
                    case TileType.LaserUp:
                        parent = wallContainer;
                        var laserUp = gameObj.GetComponent<LaserEmitter>();
                        laserUp.Direction = Direction.Up;
                        break;
                    case TileType.LaserRight:
                        parent = wallContainer;
                        var laserRight = gameObj.GetComponent<LaserEmitter>();
                        laserRight.Direction = Direction.Right;
                        break;
                    case TileType.LaserLeft:
                        parent = wallContainer;
                        var laserLeft = gameObj.GetComponent<LaserEmitter>();
                        laserLeft.Direction = Direction.Left;
                        break;

                    case TileType.Mirror:
                        parent = switchableContainer;
                        break;
                    case TileType.MirrorInverse:
                        parent = switchableContainer;
                        var mirror = gameObj.GetComponent<Mirror>();
                        mirror.Forward = false;
                        break;

                    case TileType.Wall:
                        parent = wallContainer;
						transform.GetComponent<SpriteRenderer>().sortingOrder = PlaceWall(transform.position.y);
                        break;
                    case TileType.Floor:
                        parent = floorContainer;
						transform.GetComponent<SpriteRenderer>().sortingOrder = FloorOrder;
                        break;
                    default:
                        throw new Exception("Impossible");
                }

                transform.parent = parent.transform;
            }
        }

		for (int y = 0; y < map.Height; ++y) {
			var shadeLeft = makeGradient(map, -1, y);
			shadeLeft.renderer.material = shadeLeft.left;
			var shadeRight = makeGradient(map, map.Width, y);
			shadeRight.renderer.material = shadeRight.right;
		}
		for (int x = 0; x < map.Width; ++x) {
			var shadeTop = makeGradient(map, x, -1);
			shadeTop.renderer.material = shadeTop.top;
			var shadeBot = makeGradient(map, x, map.Height);
			shadeBot.renderer.material = shadeBot.bottom;
		}
		var shadeTL = makeGradient(map, -1, -1);
		shadeTL.renderer.material = shadeTL.topLeft;
		var shadeTR = makeGradient(map, map.Width, -1);
		shadeTR.renderer.material = shadeTR.topRight;
		var shadeBL = makeGradient(map, -1, map.Height);
		shadeBL.renderer.material = shadeBL.bottomLeft;
		var shadeBR = makeGradient(map, map.Width, map.Height);
		shadeBR.renderer.material = shadeBR.bottomRight;
    }

	ShadeController makeGradient(TmxMap map, float x, float y) {
		var position = new Vector2(x, map.Height - y - 1);
		var gradient = Object.Instantiate(shade, position, Quaternion.identity) as GameObject;
		gradient.transform.GetComponent<SpriteRenderer>().sortingOrder = FloorOrder;
		gradient.transform.parent = shadeContainer.transform;
		return gradient.GetComponent<ShadeController>();
	}

}
