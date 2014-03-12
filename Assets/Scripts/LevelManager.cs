using System.Collections.Generic;
using System.IO;
using TiledSharp;
using UnityEngine;
using YamlDotNet.Serialization;

public class LevelManager
{
    /// <summary>
    /// 0-based
    /// </summary>
    public int Level { get; private set; }

    private readonly LevelLoader loader;

    private readonly string[] levels;
    private readonly Dictionary<int, TmxMap> tileMaps;

    public LevelManager()
    {
        Level = PlayerPrefs.GetInt("Level", 0) - 1;
        loader = new LevelLoader();

        tileMaps = new Dictionary<int, TmxMap>();

        var asset = Resources.Load<TextAsset>("Levels");
        var reader = new StringReader(asset.text);

        var d = new Deserializer();
        levels = d.Deserialize<string[]>(reader);
    }

    public void Next()
    {
        Level++;

        if (Level >= levels.Length) Level = 0;

        Load(Level);

        PlayerPrefs.Save();
    }

    public bool Reload()
    {
        return Load(Level);
    }

    public bool Load(int level)
    {
        if (level < 0 || level > levels.Length) level = 0;
        Level = level;

        PlayerPrefs.SetInt("Level", Level);

        TmxMap map;

        if (tileMaps.ContainsKey(level))
        {
            map = tileMaps[level];
        }
        else
        {
            var name = levels[level];
            var asset = Resources.Load<TextAsset>(name);
            if (asset == null) return false;

            var reader = new StringReader(asset.text);
            map = new TmxMap(reader);

            tileMaps[level] = map;
        }
        
        return loader.Load(map);
    }

    public void Clear()
    {
        loader.Clear();
    }
}
