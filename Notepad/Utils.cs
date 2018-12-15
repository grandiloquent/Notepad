
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Notepad
{
	
	public static class Utils
	{
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
