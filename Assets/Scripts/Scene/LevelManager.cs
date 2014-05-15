using System.Collections.Generic;
using System.IO;
using TiledMax;
using UnityEngine;
using YamlDotNet.Serialization;
using System.Linq;

public class LevelManager
{
    /// <summary>
    /// Level number is 0-based in this property. When showing in GUI, level number should be this property + 1
    /// </summary>
    public int Level { get; set; }
    public int LevelCount
    {
        get { return levels.Length; }
    }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float AspectRatio { get { return (float)Width / Height; } }
    public Vector3 CameraPosition { get { return new Vector3((Width - 1) / 2.0f, (Height - 1) / 2.0f, -10); } }
    public float OrthographicSize { get { return (Camera.main.aspect > AspectRatio ? Height : Width / Camera.main.aspect) / 2.0f + 0.5f; } }

    private readonly LevelLoader loader;

    private readonly string[] levels;
	public readonly int[] scores;
	private int totalStars;
    private readonly Dictionary<int, TmxMap> tileMaps;
	private ulong totalScore;

	private static LevelManager _instance;

    private LevelManager()
    {
		//_instance = this;
        Level = PlayerPrefs.GetInt("Level", 0) - 1;

        loader = new LevelLoader();

        tileMaps = new Dictionary<int, TmxMap>();

        var asset = Resources.Load<TextAsset>("Levels");
        var reader = new StringReader(asset.text);
        var d = new Deserializer();
        levels = d.Deserialize<string[]>(reader);
		scores = new int[levels.Length];
		for (int i = 0; i < scores.Length; ++i) {
			scores[i] = GetScore(i);
			//Debug.Log(scores[i]);
		}
		//totalStars = 0;
		//for (int i = 0; i < scores.Length; ++i) {
		//		totalStars += scores[i];
		//}
		RecalculateTotal();
    }

	public static ulong TotalScore { get { return instance.totalScore; } }

	public static int TotalStars { get { return instance.totalStars; } }

	public void RecalculateTotal() {
		totalStars = 0;
		totalScore = 0;
		for (int i = 0; i < scores.Length; ++i) {
			totalStars += scores[i];
			if (scores[i] >= 1 && scores[i] <= 3) {
				totalScore += (ulong)((i + 1) * 100);
				if (scores[i] == 2) {
					totalScore += 50UL;
				}
				if (scores[i] == 3) {
					totalScore += 100UL;
				}
			}
		}
	}

    public static LevelManager instance
    {
		get {
			if (_instance == null) { 
				_instance = new LevelManager();
			}
			return _instance;
		}
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

		//Load map settings
		int min;
		int max;
		if (!map.Properties.GetInt("MinScore", out min)) {
			min = 0;
		}
		if (!map.Properties.GetInt("MaxScore", out max)) {
			max = 0;
		}

		minScore = min;
		maxScore = max;
    }

	public int maxScore {
		get; private set;
	}

	public int minScore {
		get; private set;
	}

    public void Clear()
    {
        loader.Clear();
    }

	private static int DecryptScore(int level, int value) {
		int extra;
		if ((level &2) == 0) {
			extra = 137;
		}
		else {
			extra = 71;
		}
		//value -= extra;
		return ((value ^ 0xFFFF) - extra - level) >> 1;;
	}

	private static int EncryptScore(int level, int value) {
		int extra;
		if ((level & 2) == 0) {
			extra = 137;
		}
		else {
			extra = 71;
		}
		return ((value << 1) + extra + level) ^ 0xFFFF;
	}

	public static void SetScore(int level, int score) {
		if (score < 0 || score > 3) {
			return;
		}
		string key = "Level:" + level.ToString();
		int currentScore = 0;
		if (PlayerPrefs.HasKey(key)) {
			currentScore = DecryptScore(level, PlayerPrefs.GetInt(key));
		}
		if (score > currentScore) {
			instance.scores[level] = score;
			PlayerPrefs.SetInt(key, EncryptScore(level, score));
			instance.RecalculateTotal();
		}
		//score = EncryptScore(level, score);
	}

	public static int GetScore(int level) {
		//int 
		string key = "Level:" + level.ToString();
		if (PlayerPrefs.HasKey(key)) {
			int score = DecryptScore(level, PlayerPrefs.GetInt(key));
			if (score < 0 || score > 3) {
				PlayerPrefs.DeleteKey(key);
			} else {
				return score;
			}
		}
		return 0;
	}
}
