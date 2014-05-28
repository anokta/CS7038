using System.Collections.Generic;
using System.IO;
using TiledMax;
using UnityEngine;
using System.Linq;

public class LevelManager
{
    /// <summary>
    /// Level number is 0-based in this property. When showing in GUI, level number should be this property + 1
    /// </summary>
    public int Level { get; set; }
    public int LevelCount
    {
        get { return levels.Count; }
    }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float AspectRatio { get { return (float)Width / Height; } }
    public Vector3 CameraPosition { get { return new Vector3((Width - 1) / 2.0f, (Height - 1) / 2.0f, -10); } }
    public float OrthographicSize { get { return (Camera.main.aspect > AspectRatio ? Height : Width / Camera.main.aspect) / 2.0f + 0.5f; } }

    private readonly LevelLoader loader;

    private readonly List<string> levels;
	public readonly int[] scores;
	private int totalStars;
    private readonly Dictionary<int, TmxMap> tileMaps;
	private readonly Dictionary<int, DialogueMap> dialogues;
	private ulong totalScore;

	private static LevelManager _instance;

    private LevelManager()
    {
		//_instance = this;
        Level = PlayerPrefs.GetInt("Level", 0) - 1;

        loader = new LevelLoader();

        tileMaps = new Dictionary<int, TmxMap>();
		dialogues = new Dictionary<int, DialogueMap>();
		settings = new LevelSettings();

        var asset = Resources.Load<TextAsset>("Levels");
		levels = new List<string>();
        using (var reader = new StringReader(asset.text)) {
        //var d = new Deserializer();
        //levels = d.Deserialize<string[]>(reader);
			string line;
			while ((line=reader.ReadLine()) != null) {
				if (!StringExt.IsNullOrWhitespace(line)) {
					levels.Add(line.Trim());
				}
			}
		}
		scores = new int[levels.Count];
		Debug.Log("Loaded levels: " + levels.Count);
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

//	public DialogueMap dialogueMap {
//		get; private set;
//	}

    public void Load(int level)
    {
        level %= levels.Count;
        if (level < 0) level += levels.Count;

        Level = level;

        TmxMap map;
		DialogueMap dialogue;

        if (tileMaps.ContainsKey(level))
        {
            map = tileMaps[level];
        }
        else
        {
            var name = levels[level];
            var asset = Resources.Load<TextAsset>(name);
            using (var reader = new StringReader(asset.text)) {
            	map = TmxMap.Open(reader);
			}
			tileMaps[level] = map;
        }
		if (dialogues.ContainsKey(level)) {
			dialogue = dialogues[level];
		}
		else {
			var name = levels[level];
			var dia = Resources.Load<TextAsset>("Dialogues/" + name);
			if (dia != null ) {
				using (var reader = new StringReader(dia.text)) {
					dialogue = DialogueMap.Open(Object.FindObjectOfType<DialogueManager>(), reader);
				}
			}
			else {
				dialogue = new DialogueMap(DialogueManager.instance);
			}
			dialogues[level] = dialogue;
		}
			
		Width = map.Width;
        Height = map.Height;
        Camera.main.transform.position = CameraPosition;
        Camera.main.orthographicSize = OrthographicSize;
		   
		
	//	dialogueMap = dialogue;
		settings.dialogueMap = dialogue;
        loader.Load(map, settings);

		//Load map settings
		settings.minScore = map.Properties.GetInt("MinScore", 0);
		settings.maxScore = map.Properties.GetInt("MaxScore", 0);
		string nameIntro = map.Properties.GetTag("Intro", null);
		string nameOutro = map.Properties.GetTag("Outro", null);
		if (nameIntro != null) {
			settings.intro = settings.dialogueMap[nameIntro];
		}
		else {
			settings.intro = null;
		}
		if (nameOutro != null) {
			settings.outro = settings.dialogueMap[nameOutro];
		}
		else {
			settings.outro = null;
		}

		var hand = Object.FindObjectOfType<HandController>();
		if (hand != null) {
			hand.value = map.Properties.GetInt("Health", HandController.DefValue);
		}
		//string dialogueName;
		//if (!map.
    }

	public LevelSettings settings
	{
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
