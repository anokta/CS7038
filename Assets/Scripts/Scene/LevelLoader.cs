﻿using System;
using System.Collections.Generic;
using TiledMax;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelLoader
{
	public static LevelLoader Instance { get; private set; }
	// Entities
	private GameObject entityContainer;
	private GameObject wallContainer, shadeContainer, floorContainer,
	collectibleContainer, pushableContainer, accessibleContainer, 
	switchableContainer, triggerContainer, overlayContainer;
	private readonly Dictionary<TileType, GameObject> prefabs;

	private GameObject shade;
	private GameObject overlayFloor;
	private GameObject overlayWall;

	float wallOverlayOffset = -0.34f;

	public GameObject ExplosionContainer { get; private set; }

	Entity[,] entities;
	bool[,] walls;

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

	public static short PlaceWall(float y)
	{
		return (short)(-10 * Mathf.RoundToInt(y));
	}

	public static short PlaceDepth(float y)
	{
		return (short)(-10 * Mathf.RoundToInt(y) - 5);
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
		prefabs[TileType.DoorVertical] = Resources.Load<GameObject>("Door Vertical");
		prefabs[TileType.GateClean] = Resources.Load<GameObject>("Gate Clean");

		prefabs[TileType.LaserDown] = Resources.Load<GameObject>("LaserEmitter");
		prefabs[TileType.LaserUp] = Resources.Load<GameObject>("LaserEmitter");
		prefabs[TileType.LaserRight] = Resources.Load<GameObject>("LaserEmitter");
		prefabs[TileType.LaserLeft] = Resources.Load<GameObject>("LaserEmitter");
		prefabs[TileType.Mirror] = Resources.Load<GameObject>("Mirror");
		prefabs[TileType.MirrorInverse] = Resources.Load<GameObject>("Mirror");
        
		prefabs[TileType.Wall] = Resources.Load<GameObject>("Wall");
		prefabs[TileType.Floor] = Resources.Load<GameObject>("Floor");
		//prefabs[TileType.Trigger] = Resources.Load<GameObject>("Trigger");
//        prefabs[TileType.Sanitizer] = Resources.Load<GameObject>("Sanitizer");

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

		prefabs[TileType.Terminal] = Resources.Load<GameObject>("Terminal");

		overlayFloor = Resources.Load<GameObject>("Overlays/Floor Overlay");
		overlayWall = Resources.Load<GameObject>("Overlays/Wall Overlay");

		//This shouldn't be a TileType object
		shade = Resources.Load<GameObject>("Shade");
	}

	public void Clear()
	{
		if (entityContainer != null)
			Object.Destroy(entityContainer);
	}

	public void Load(TmxMap map, LevelSettings settings)	
	{
		Instance = this;
	
		// Instantiate the containers
		entityContainer = new GameObject("31 Entities");

		Transform ecTransform = entityContainer.transform;

		wallContainer = new GameObject("Walls");
		wallContainer.transform.parent = ecTransform;
			
		floorContainer = new GameObject("Floors");
		floorContainer.transform.parent = ecTransform;
		
		collectibleContainer = new GameObject("Collectibles");
		collectibleContainer.transform.parent = ecTransform;
		
		pushableContainer = new GameObject("Pushables");
		pushableContainer.transform.parent = ecTransform;
		
		accessibleContainer = new GameObject("Accessibles");
		accessibleContainer.transform.parent = ecTransform;
		
		switchableContainer = new GameObject("Switchables");
		switchableContainer.transform.parent = ecTransform;
		
		shadeContainer = new GameObject("Shades");
		shadeContainer.transform.parent = ecTransform;

		ExplosionContainer = new GameObject("Explosions");
		ExplosionContainer.transform.parent = ecTransform;
		
		triggerContainer = new GameObject("Triggers");
		triggerContainer.transform.parent = ecTransform;

		overlayContainer = new GameObject("Overlays");
		overlayContainer.transform.parent = ecTransform;

		var leverGateManager = new LeverGateManager();

		int type1 = 0;
		int type2 = 0;
		int type3 = 0;

		List<Lever> levers = new List<Lever>();
		List<Gate> gates = new List<Gate>();

		entities = new Entity[map.Width, map.Height];
		walls = new bool[map.Width, map.Height];

		//Initialized with a seed, so that every time the randomizer produces the same level
		var random = new System.Random(0);

		foreach (var layer in map.Layers) {
			var tiles = layer.Tiles;

			foreach (var tile in tiles) {
				var tileType = (TileType)tile.Gid;

				if (tileType == TileType.Empty)
					continue;
				if (!prefabs.ContainsKey(tileType))
					continue;

				var indexX = tile.X;
				var indexY = tile.Y;
				var position = new Vector2(indexX, map.Height - indexY - 1);

				var prefab = prefabs[tileType];
				var gameObj = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
				var transform = gameObj.transform;
				GameObject parent;

				var entity = gameObj.GetComponent<Entity>();
				if (entity != null) {
					entities[tile.X, map.Height - tile.Y - 1] = entity;
				}

				//Calling this adds some variety between levels, but ensures the same level will always look the same
				random.NextDouble();
				#region Read tile types
				switch (tileType) {
					case TileType.Player:
						parent = entityContainer;
						GameObject ovPrefab = Object.Instantiate(overlayFloor, position, Quaternion.identity) as GameObject;
						ovPrefab.renderer.sortingOrder = FloorOrder + 1;
						ovPrefab.transform.parent = overlayContainer.transform;
						/*GameObject obj = new GameObject();
						obj.transform.position = position;
						obj.transform.parent = overlayContainer.transform;
						var ovRen = obj.AddComponent<SpriteRenderer>();
						ovRen.sprite = AssetHelper.instance.SurewashIcon;
						ovRen.sortingOrder = FloorOrder + 1;
						ovRen.material = prefab.renderer.material;*/
						break;
					case TileType.Crate:
					case TileType.Trolley:
					case TileType.Explosive:
					case TileType.Plant:
						parent = pushableContainer;
						break;
				//case TileType.Sanitizer:
				//    parent = collectibleContainer;
				//    break;
					case TileType.Door:
					case TileType.DoorVertical:
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
						levers.Add(lever1);
						type1 = 1;
						leverGateManager.Add(lever1);
						break;
					case TileType.Lever2:
						parent = accessibleContainer;
						var lever2 = gameObj.GetComponent<Lever>();
						lever2.LeverGateType = LeverGateType.Type2;
						levers.Add(lever2);
						type2 = 1;
						leverGateManager.Add(lever2);
						break;
					case TileType.Lever3:
						parent = accessibleContainer;
						var lever3 = gameObj.GetComponent<Lever>();
						levers.Add(lever3);
						type3 = 1;
						lever3.LeverGateType = LeverGateType.Type3;
						leverGateManager.Add(lever3);
						break;

					case TileType.Gate1:
					case TileType.Gate1Vertical:
						parent = accessibleContainer;
						var gate1 = gameObj.GetComponent<Gate>();
						gates.Add(gate1);
						type1 = 1;
						gate1.LeverGateType = LeverGateType.Type1;
						leverGateManager.Add(gate1);
						break;
					case TileType.Gate2:
					case TileType.Gate2Vertical:
						parent = accessibleContainer;
						var gate2 = gameObj.GetComponent<Gate>();
						gates.Add(gate2);
						type2 = 1;
						gate2.LeverGateType = LeverGateType.Type2;
						leverGateManager.Add(gate2);
						break;
					case TileType.Gate3:
					case TileType.Gate3Vertical:
						parent = accessibleContainer;
						var gate3 = gameObj.GetComponent<Gate>();
						gate3.LeverGateType = LeverGateType.Type3;
						gates.Add(gate3);
						type1 = 3;
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
						walls[tile.X, tile.Y] = true;
						break;
					case TileType.Terminal:
						parent = accessibleContainer;
						transform.renderer.sortingOrder = PlaceWall(transform.position.y) + 1;
						break;
					case TileType.Floor:
						parent = floorContainer;
						transform.GetComponent<SpriteRenderer>().sortingOrder = FloorOrder;
						var blenderer = gameObj.renderer as SpriteRenderer;
						float brighter = ((float)random.NextDouble() - 0.5f) * 0.02f;
						blenderer.color = new Color(
							blenderer.color.r + (float)(random.NextDouble() * 0) + brighter,
							blenderer.color.g + (float)(random.NextDouble() * 0.01f) + brighter,
							blenderer.color.b + (float)(random.NextDouble() * 0.015f) + brighter,
							blenderer.color.a);
						break;
					default:
						throw new Exception("Impossibru!");
				}
				#endregion
				transform.parent = parent.transform;
			}
		}

		int x;
		int y;

		bool breakOff = false;
		for (x = 0; x < map.Width; ++x) {
			for (y = 0; y < map.Height; ++y) {
				if (walls[x, y] && (y == map.Height - 1 || !walls[x, y + 1])) {
					if (random.Next(0, 8) == 5) {
						var position = new Vector3(x, map.Height - y - 1, 0);
						GameObject ovWall = Object.Instantiate(overlayWall) as GameObject;
						ovWall.transform.parent = overlayContainer.transform;
						ovWall.transform.position = position + new Vector3(0, wallOverlayOffset, 0);
						ovWall.renderer.sortingOrder = PlaceWall(position.y) + 1;
						breakOff = true;
						break;
					}
				}
			}
			if (breakOff) {
				break;
			}
		}

		if (type1 + type2 + type3 == 1) {
			foreach (var gate in gates) {
				gate.LeverGateType = LeverGateType.None;
			}
			foreach (var lever in levers) {
				lever.LeverGateType = LeverGateType.None;
			}
		}

		#region Make Gradients
		for (int i = 0; i < map.Height; ++i) {
			var shadeLeft = makeGradient(map, -1, i);
			shadeLeft.renderer.material = shadeLeft.left;
			var shadeRight = makeGradient(map, map.Width, i);
			shadeRight.renderer.material = shadeRight.right;
		}
		for (int j = 0; j < map.Width; ++j) {
			var shadeTop = makeGradient(map, j, -1);
			shadeTop.renderer.material = shadeTop.top;
			var shadeBot = makeGradient(map, j, map.Height);
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
		#endregion

		foreach (var obj in map.Objects) {
			switch (obj.objectType) {
				case TmxObject.ObjectType.Tile:
					GetIndex(obj.position, out x, out y);
					Entity e = entities[x, y];
					if (e != null) {
						Trigger a = new Trigger(obj, settings);
						e.AddTriggerAction(a);
					}
					break;
				default:
					var triggerObj = Entity.Create<RectTrigger>("RectTrigger");
					var recTrigger = triggerObj.GetComponent<RectTrigger>();
					recTrigger.action = new Trigger(obj, settings);
					recTrigger.transform.parent = triggerContainer.transform;
					break;
			}
		}
	}

	void GetIndex(Rect rect, out int x, out int y)
	{
		float xx = rect.x + rect.width / 2;
		float yy = rect.y + 1 + rect.height / 2;
		x = (int)Mathf.Floor(xx);
		y = (int)Mathf.Floor(yy);
	}

	ShadeController makeGradient(TmxMap map, float x, float y)
	{
		var position = new Vector2(x, map.Height - y - 1);
		var gradient = Object.Instantiate(shade, position, Quaternion.identity) as GameObject;
		gradient.transform.GetComponent<SpriteRenderer>().sortingOrder = FloorOrder;
		gradient.transform.parent = shadeContainer.transform;
		return gradient.GetComponent<ShadeController>();
	}

}
