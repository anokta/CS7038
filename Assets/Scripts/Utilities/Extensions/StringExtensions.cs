//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
