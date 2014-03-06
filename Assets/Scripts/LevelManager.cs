using System;
using System.Collections.Generic;
using System.IO;
using TiledSharp;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelManager
{
    public int Level;

    // Entities
    private GameObject entityContainer;
    private GameObject wallContainer, collectibleContainer, pushableContainer, accessibleContainer, switchableContainer;
    private readonly Dictionary<TileType, GameObject> prefabs;
    private readonly Dictionary<int, TextAsset> tileMaps;

    public LevelManager()
    {
        Level = 3;

        prefabs = new Dictionary<TileType, GameObject>();
        prefabs[TileType.Crate] = Resources.Load<GameObject>("Crate");
        prefabs[TileType.Door] = Resources.Load<GameObject>("Door");
        prefabs[TileType.Player] = Resources.Load<GameObject>("Dr Handrew");
        prefabs[TileType.Explosive] = Resources.Load<GameObject>("ExplosiveCrate");
        prefabs[TileType.Fountain] = Resources.Load<GameObject>("Fountain");
        prefabs[TileType.Gate] = Resources.Load<GameObject>("Gate");
        prefabs[TileType.LaserEmitter] = Resources.Load<GameObject>("LaserEmitter");
        prefabs[TileType.Lever] = Resources.Load<GameObject>("Lever");
        prefabs[TileType.Mirror] = Resources.Load<GameObject>("Mirror");
        prefabs[TileType.Patient] = Resources.Load<GameObject>("Patient");
        prefabs[TileType.Sanitizer] = Resources.Load<GameObject>("Sanitizer");
        prefabs[TileType.Trolley] = Resources.Load<GameObject>("Trolley");
        prefabs[TileType.Wall] = Resources.Load<GameObject>("Wall");

        tileMaps = new Dictionary<int, TextAsset>();
    }

    public void Next()
    {
        if (!Load(Level + 1))
        {
            Load(1);
        }
    }

    public bool Reload()
    {
        return Load(Level);
    }

    public bool Load(int level)
    {
        TextAsset asset;

        if (tileMaps.ContainsKey(level))
        {
            asset = tileMaps[level];
        }
        else
        {
            var name = string.Format("Level{0}", level);

            asset = Resources.Load<TextAsset>(name);
            if (asset == null) return false;
            
            tileMaps[level] = asset;
        }

        Level = level;
        var reader = new StringReader(asset.text);
        var map = new TmxMap(reader);
        var tiles = map.Layers[0].Tiles;

        // Instantiate the containers
        entityContainer = new GameObject("21 Entities");

        wallContainer = new GameObject("Walls");
        wallContainer.transform.parent = entityContainer.transform;

        collectibleContainer = new GameObject("Collectibles");
        collectibleContainer.transform.parent = entityContainer.transform;

        pushableContainer = new GameObject("Pushables");
        pushableContainer.transform.parent = entityContainer.transform;

        accessibleContainer = new GameObject("Accessibles");
        accessibleContainer.transform.parent = entityContainer.transform;

        switchableContainer = new GameObject("Switchables");
        switchableContainer.transform.parent = entityContainer.transform;

        var config = LoadConfig(map);
        var levers = new Dictionary<Vector2, Lever>();
        var gates = new Dictionary<Vector2, Gate>();

        foreach (var tile in tiles)
        {
            var tileType = (TileType)tile.Gid;

            if (tileType == TileType.Empty) continue;

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
                case TileType.Lever:
                    parent = switchableContainer;
                    levers[position] = gameObj.GetComponent<Lever>();
                    break;
                case TileType.Gate:
                    parent = accessibleContainer;
                    gates[position] = gameObj.GetComponent<Gate>();
                    break;                
                case TileType.LaserEmitter:
                    parent = wallContainer;
                    if (config.ContainsKey(position))
                    {
                        var laserEmitter = gameObj.GetComponent<LaserEmitter>();
                        var directionVector = ParseVector2(config[position]);
                        laserEmitter.Direction = directionVector.ToDirection();
                        transform.rotation = Quaternion.FromToRotation(DirectionExtensions.Down, directionVector);
                    }
                    break;
                case TileType.Mirror:
                    parent = switchableContainer;
                    if (config.ContainsKey(position))
                    {
                        var mirror = gameObj.GetComponent<Mirror>();
                        mirror.Forward = false;
                        transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    }
                    break;
                case TileType.Wall:
                    parent = wallContainer;
                    transform.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(4 * transform.position.y);
                    break;
                default:
                    throw new Exception("Impossible");
            }

            transform.parent = parent.transform;
        }

        foreach (var lever in levers)
        {
            var gatePosition = ParseVector2(config[lever.Key]);
            lever.Value.gate = gates[gatePosition];
        }

        return true;
    }

    public void Clear()
    {
        if (entityContainer != null) Object.Destroy(entityContainer);
    }

    private static Dictionary<Vector2, string> LoadConfig(TmxMap map)
    {
        var config = new Dictionary<Vector2, string>();

        foreach (var property in map.Properties)
        {
            var position = ParseVector2(property.Key);
            config[position] = property.Value;
        }

        return config;
    }

    private static Vector2 ParseVector2(string s)
    {
        var nums = s.Split(',');
        var x = int.Parse(nums[0]);
        var y = int.Parse(nums[1]);
        return new Vector2(x, y);
    }
}
