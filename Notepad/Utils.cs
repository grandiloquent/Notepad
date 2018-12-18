
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using Shared;

namespace Notepad
{
	
	public static class Utils
	{
		public static string ConvertToHtml(TextBox textBox){
			var sb = new StringBuilder();
			sb.AppendLine("\u003C!doctype html\u003E");
			sb.AppendLine("\u003Chtml class=\u0022no-js\u0022 lang=\u0022zh-hans\u0022 dir=\u0022ltr\u0022\u003E");
			sb.AppendLine("");
			sb.AppendLine("\u003Chead\u003E");
			sb.AppendLine("    \u003Cmeta charset=\u0022utf-8\u0022\u003E");
			sb.AppendLine("    \u003Cmeta http-equiv=\u0022x-ua-compatible\u0022 content=\u0022ie=edge\u0022\u003E");
			sb.AppendLine("    \u003Ctitle\u003E");
			sb.AppendLine(HtmlAgilityPack.HtmlEntity.Entitize(textBox.Text.GetFirstReadable().TrimStart("# ".ToCharArray())));
			sb.AppendLine("    \u003C/title\u003E");
			sb.AppendLine("    \u003Cmeta name=\u0022viewport\u0022 content=\u0022width=device-width, initial-scale=1\u0022\u003E");
			sb.AppendLine("    \u003Clink rel=\u0022stylesheet\u0022 href=\u0022../stylesheets/markdown.css\u0022\u003E");
			sb.AppendLine("\u003C/head\u003E");
			sb.AppendLine("\u003Cbody\u003E");
			sb.AppendLine(textBox.Text.FormatMarkdown());

			sb.AppendLine("\u003C/body\u003E");
			sb.AppendLine("\u003C/html\u003E");
			return sb.ToString();
		}
		public static void FormatH2(TextBox textBox){
			var start = textBox.SelectionStart;

			while (start - 1 > -1 && textBox.Text[start - 1] != '\n') {
				start--;
			}
			var end = start;
			while (end + 1 < textBox.Text.Length && textBox.Text[end + 1] == '#') {
				end++;
			}
			textBox.SelectionStart = start;
			textBox.SelectionLength = end - start;
			textBox.SelectedText = "## ";
		}
		public static void FormatH3(TextBox textBox){
			var start = textBox.SelectionStart;

			while (start - 1 > -1 && textBox.Text[start - 1] != '\n') {
				start--;
			}
			var end = start;
			while (end + 1 < textBox.Text.Length && textBox.Text[end + 1] == '#') {
				end++;
			}
			textBox.SelectionStart = start;
			textBox.SelectionLength = end - start;
			textBox.SelectedText = "### ";
		}
				public static String OrderH2(String value)
		{
			var list = value.Trim().Split('\n');
			var dictionary = new Dictionary<String,List<string>>();
			List<String> lines=null;
			foreach (var element in list) {
				if (element.StartsWith("## ")) {
					lines = new List<string>();
					dictionary.Add(element.TrimEnd(), lines);
				}
				if (lines != null) {
					lines.Add(element.TrimEnd());
				}
			}
			var result = new List<string>();
			var collection =	dictionary.OrderBy(i => i.Key);
			foreach (var element in collection) {
				result = result.Concat(element.Value).ToList();
			}
			return string.Join(Environment.NewLine, result);
		}
		public static String OrderH3(String value)
		{
			var list = value.Trim().Split('\n');
			var dictionary = new Dictionary<String,List<string>>();
			List<String> lines=null;
			foreach (var element in list) {
				if (element.StartsWith("### ")) {
					lines = new List<string>();
					dictionary.Add(element.TrimEnd(), lines);
				}
				if (lines != null) {
					lines.Add(element.TrimEnd());
				}
			}
			var result = new List<string>();
			var collection =	dictionary.OrderBy(i => i.Key);
			foreach (var element in collection) {
				result = result.Concat(element.Value).ToList();
			}
			return string.Join(Environment.NewLine, result);
		}
	}
}
