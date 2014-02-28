using System;
public class StringExtensions
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

}
