using System;
public static class StringExt
{
	public static bool IsNullOrWhitespace(string s)
	{
		if (s == null) {
			return true;
		}
			
		for (var i = 0; i < s.Length; i++) {
			if (!char.IsWhiteSpace(s, i)) {
				return false;
			}
		}

		return true;
	}

	public static bool StartsWith(this string s, char c) {
		return s.Length > 0 && s[0] == c;
	}

}
