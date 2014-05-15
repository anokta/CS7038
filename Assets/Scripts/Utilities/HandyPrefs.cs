using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Linq;
using Encoding = System.Text.Encoding;
using System.Globalization;

/// <summary>
/// Placeholder. Doesn't really work :(
/// </summary>
public static class HandyPrefs
{
	/*
	static CspParameters _params;
	static RSACryptoServiceProvider _provider;
	static CultureInfo _culture;

	static HandyPrefs() {
		_params = new CspParameters();
		_params.KeyContainerName = "pk_%7z^I6(/lq@S.?E,Y5%cjS+h-";
		_provider = new RSACryptoServiceProvider(_params);
		_culture = CultureInfo.CreateSpecificCulture("en-US");
	}

	public static string GetString(string setting) {
		//var bytes = _provider.Encrypt(Encoding.ASCII.GetBytes(setting), false);
		//var value = _provider.Decrypt(bytes, false);
		string key = Encrypt(setting);
		if (PlayerPrefs.HasKey(key)) {
			return Decrypt(PlayerPrefs.GetString(key));
		}
		return null;
	}

	public static float GetFloat(string setting) {
		return float.Parse(GetString(setting), _culture);
	}

	public static int GetInt(string setting) {
		return int.Parse(GetString(setting), _culture);
	}

	public static bool GetInt(string setting, out int value) {
		var str = GetString(setting);
		value = 0;
		if (str == null) {
			return false;
		}
		return int.TryParse(str, NumberStyles.Any, _culture, out value);
	}

	public static bool GetFloat(string setting, out float value) {
		var str = GetString(setting);
		value = 0;
		if (str == null) {
			return false;
		}
		return float.TryParse(str, NumberStyles.Any, _culture, out value);
	}

	public static void RecalculateTotal()
	{
		//MaxScore = LevelManager.instance.scores.Sum();
	}

	public static int MaxScore {get; private set;}

	//public static float? TryFloat(string setting) {
	//		float outVal;
	//	if (float.TryParse(
	//}

	public static void SetFloat(string setting, float value) {
		var bytes = _provider.Encrypt(Encoding.ASCII.GetBytes(setting), false);
		var valBytes = _provider.Encrypt(Encoding.ASCII.GetBytes(value.ToString(_culture)), false);
		PlayerPrefs.SetString(Encoding.ASCII.GetString(bytes), Encoding.ASCII.GetString(valBytes));
	}

	public static void SetInt(string setting, int value) {
		var bytes = _provider.Encrypt(Encoding.ASCII.GetBytes(setting), false);
		var valBytes = _provider.Encrypt(Encoding.ASCII.GetBytes(value.ToString(_culture)), false);
		PlayerPrefs.SetString(Encoding.ASCII.GetString(bytes), Encoding.ASCII.GetString(valBytes));
	}

	public static void SetString(string setting, string value) {
		var bytes = _provider.Encrypt(Encoding.ASCII.GetBytes(setting), false);
		var valBytes = _provider.Encrypt(Encoding.ASCII.GetBytes(value), false);
		PlayerPrefs.SetString(Encoding.ASCII.GetString(bytes), Encoding.ASCII.GetString(valBytes));
	}

	public static void Save() {
		PlayerPrefs.Save();
	}

	public static string Encrypt(string value) {
		return Encoding.ASCII.GetString(_provider.Encrypt(Encoding.ASCII.GetBytes(value), false));
	}

	public static string Decrypt(string value) {
		return Encoding.ASCII.GetString(_provider.Decrypt(Encoding.ASCII.GetBytes(value), false));
	}

	public static void SetScore(int level, int score) {
		if (score < 0 || score > 3) {
			return;
		}
		if (GetScore(level) < score) {
			LevelManager.instance.scores[level] = score;
			RecalculateTotal();
			if ((level & 2) == 0) {
				score += 7;
			} else {
				score += 13;
			}
			score += level;
			SetInt("Level." + level.ToString(), score);
		}
	}

	public static int GetScore(int level) {
		int offset = 0;
		if ((level & 2) == 0) {
			offset += 7;
		} else {
			offset += 13;
		}
		offset += level;
		string lvl = "Level." + level.ToString(_culture);
		string key = Encrypt(lvl);
		if (PlayerPrefs.HasKey(key)) {
			int score = GetInt(lvl) - offset;
			if (score >= 0 || score <= 3) {
				return score;
			}
		}
		return 0;
	}
*/
	//private string 
}

