using System;
using System.Collections.Generic;
using TiledSharp;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelLoader
{
    // Entities
    private GameObject entityContainer;
    private GameObject wallContainer, floorContainer, collectibleContainer, pushableContainer, accessibleContainer, switchableContainer;
    private readonly Dictionary<TileType, GameObject> prefabs;

    public LevelLoader()
    {
        prefabs = new Dictionary<TileType, GameObject>();
        prefabs[TileType.Crate] = Resources.Load<GameObject>("Crate");
        prefabs[TileType.Door] = Resources.Load<GameObject>("Door");
        prefabs[TileType.Player] = Resources.Load<GameObject>("Dr Handrew");
        prefabs[TileType.Explosive] = Resources.Load<GameObject>("ExplosiveCrate");
        prefabs[TileType.Fountain] = Resources.Load<GameObject>("Fountain");
        prefabs[TileType.Gate1] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.Gate2] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.Gate3] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.Lever1] = Resources.Load<GameObject>("Lever");
        prefabs[TileType.Lever2] = Resources.Load<GameObject>("Lever");
        prefabs[TileType.Lever3] = Resources.Load<GameObject>("Lever");
        prefabs[TileType.LaserDown] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.LaserUp] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.LaserRight] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.LaserLeft] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.Mirror] = Resources.Load<GameObject>("Mirror");
        prefabs[TileType.MirrorInverse] = Resources.Load<GameObject>("Mirror");
        prefabs[TileType.Patient] = Resources.Load<GameObject>("Patient");
        prefabs[TileType.Sanitizer] = Resources.Load<GameObject>("Sanitizer");
        prefabs[TileType.Trolley] = Resources.Load<GameObject>("Trolley");
        prefabs[TileType.Wall] = Resources.Load<GameObject>("Wall");
        prefabs[TileType.Floor] = Resources.Load<GameObject>("Floor");
    }

    public void Clear()
    {
        if (entityContainer != null) Object.Destroy(entityContainer);
    }

    public void Load(TmxMap map)
    {
        // Instantiate the containers
        entityContainer = new GameObject("21 Entities");

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
                const int maxY = 11;
                var position = new Vector2(indexX, maxY - indexY);

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
                        parent = pushableContainer;
                        break;
                    case TileType.Sanitizer:
                        parent = collectibleContainer;
                        break;
                    case TileType.Door:
                    case TileType.Fountain:
                        parent = accessibleContainer;
                        break;
                    case TileType.Patient:
                        parent = switchableContainer;
                        break;
                    case TileType.Gate1:
                        parent = accessibleContainer;
                        var gate1 = gameObj.GetComponent<Gate>();
                        gate1.LeverGateType = LeverGateType.Type1;
                        leverGateManager.Add(gate1);
                        break;
                    case TileType.Gate2:
                        parent = accessibleContainer;
                        var gate2 = gameObj.GetComponent<Gate>();
                        gate2.LeverGateType = LeverGateType.Type2;
                        leverGateManager.Add(gate2);
                        break;
                    case TileType.Gate3:
                        parent = accessibleContainer;
                        var gate3 = gameObj.GetComponent<Gate>();
                        gate3.LeverGateType = LeverGateType.Type3;
                        leverGateManager.Add(gate3);
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
                        transform.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(4 * transform.position.y);
                        break;
                    case TileType.Floor:
                        parent = floorContainer;
                        transform.GetComponent<SpriteRenderer>().sortingOrder = -1000;
                        break;
                    default:
                        throw new Exception("Impossible");
                }

                transform.parent = parent.transform;
            }
        }
    }
}
