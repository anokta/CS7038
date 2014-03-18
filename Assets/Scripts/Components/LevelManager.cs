using System.Collections.Generic;
using System.IO;
using TiledMax;
using UnityEngine;
using YamlDotNet.Serialization;

public class LevelManager
{
    /// <summary>
    /// 0-based
    /// </summary>
    public int Level { get; set; }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float AspectRatio { get { return (float)Width / Height; } }
    public Vector3 CameraPosition { get { return new Vector3((Width - 1) / 2.0f, (Height - 1) / 2.0f, -10); } }
    public float OrthographicSize { get { return (Camera.main.aspect > AspectRatio ? Height : Width / Camera.main.aspect) / 2.0f + 0.5f; } }

    private readonly LevelLoader loader;

    private readonly string[] levels;
    private readonly Dictionary<int, TmxMap> tileMaps;


    private static LevelManager instance;

    private LevelManager()
    {
        //Level = PlayerPrefs.GetInt("Level", 0) - 1;
        Level = -1;

        loader = new LevelLoader();

        tileMaps = new Dictionary<int, TmxMap>();

        var asset = Resources.Load<TextAsset>("Levels");
        var reader = new StringReader(asset.text);

        var d = new Deserializer();
        levels = d.Deserialize<string[]>(reader);
    }

    public static LevelManager Instance
    {
        get { return instance ?? (instance = new LevelManager()); }
    }

    public void Next()
    {
        Load(Level + 1);
    }

    public void Reload()
    {
        Load(Level);
    }

    public void Load(int level)
    {
        level %= levels.Length;
        if (level < 0) level += levels.Length;

        Level = level;

        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.Save();

        TmxMap map;

        if (tileMaps.ContainsKey(level))
        {
            map = tileMaps[level];
        }
        else
        {
            var name = levels[level];
            var asset = Resources.Load<TextAsset>(name);
            var reader = new StringReader(asset.text);

            map = TmxMap.Open(reader);
            tileMaps[level] = map;
        }

        Width = map.Width;
        Height = map.Height;
        Camera.main.transform.position = CameraPosition;
        Camera.main.orthographicSize = OrthographicSize;

        loader.Load(map);
    }

    public void Clear()
    {
        loader.Clear();
    }
}
