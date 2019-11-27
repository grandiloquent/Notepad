namespace StringCompare
{
	using StringCompare;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using Microsoft.Ajax.Utilities;
	using Common;
	
	public static class AndroidDelegate
	{
		public static string FormatAndroidComments(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return string.Empty;
			var array =	Regex.Matches(Regex.Replace(s.RemoveComments(), "@[^ ]*? ", ""), "(?<=(protected|public|default) )[^)^{^=]*?\\)").Cast<Match>()
			            //.Where(i=>!i.Value.StartsWith("final"))
				.Select(i => Regex.Replace(i.Value, "[\r\n]+", " ").SubstringAfter("abstract ") + ";")
			.OrderBy(i => i)
				.Distinct().ToArray();
			var value = Regex.Replace(s, "\\{@(link|see|linkplain|code)[\\s\\*]+#?([^\\}]+?)\\}", new MatchEvaluator((m) => m.Groups[2].Value));
			value = Regex.Replace(value, @"<[^>]*>", String.Empty);
			var lines = value.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
					.Select(i => "         " + i.Trim().TrimStart(" \t/*".ToArray()).Trim());
			value = "         /*\n" + string.Join("\n", lines) + "\n         */\n";
			if (array.Any())
				value += array.First();
			if (string.IsNullOrWhiteSpace(value))
				return string.Empty;
			return value;
		}
		[BindMenuItem(Name = "评论", Control = "androidFormatStripSplitButton", Toolbar = "toolbar1")]
		public static void ZipCreateFromDirectory()
		{
			Methods.OnClipboardString(FormatAndroidComments);
		}
	}
}