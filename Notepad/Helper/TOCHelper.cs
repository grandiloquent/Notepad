
using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Forms;
using Helpers;
using Common;
namespace Notepad
{
	public static class TOCHelper
	{
		public static void GenerateTOCAndOrder(TextBox textBox){
			var start = textBox.Text.SubstringBefore("## ");
			var end = "## " + textBox.Text.SubstringAfter("## ");
			textBox.Text =	start +OrderByH2(end);
			
			textBox.SelectedText= GenerateTOC(textBox.Text);
		}
		public static String OrderByH2(String value)
		{
			var list = value.Trim().Split('\n');
			var dictionary = new Dictionary<String,List<string>>();
			List<String> lines = null;
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
			var collection =	dictionary.OrderBy(i => Regex.Replace(i.Key, "[0-9]+(?=\\.)", new MatchEvaluator((v) => {
				var vx = int.Parse(v.Value);
			                                                                                                  	
				return vx.ToString().PadLeft(5, '0');
			})));
			foreach (var element in collection) {
				result = result.Concat(element.Value).ToList();
			}
			return string.Join(Environment.NewLine, result);
		}
		public static string GenerateTOC(string value){
			var lines = value.ToLines();
			var index = 0;
			var prefix = "#section-";
			var sb = new StringBuilder();
			foreach (var element in lines) {
//				if (element.StartsWith("# ")) {
//					sb.AppendFormat(string.Format("- [{0}](#{1})\r\n", element.SubstringAfter(" "), GetId(element.SubstringAfter(" ").Trim())));
//				} else
				if (element.StartsWith("## ")) {
					sb.AppendFormat(string.Format("- [{0}](#{1})\r\n", element.SubstringAfter(" "),Markdig.Helpers.LinkHelper.UrilizeAsGfm(element.SubstringAfter(" ").Trim())));
				} else if (element.StartsWith("### ")) {
					sb.AppendFormat(string.Format("\t- [{0}](#{1})\r\n", element.SubstringAfter(" "), GetId(element.SubstringAfter(" ").Trim())));
				}
			}
			return sb.ToString();
		}
		public static String GetId(string value)
		{
				return UrilizeAsGfm(value);
		   	
		}
		public static string UrilizeAsGfm(string headingText)
		{
			// Following https://github.com/jch/html-pipeline/blob/master/lib/html/pipeline/toc_filter.rb
			var headingBuffer = new StringBuilder();
			for (int i = 0; i < headingText.Length; i++) {
				var c = char.ToLowerInvariant(headingText[i]);
				if (char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_'||c=='.') {
					headingBuffer.Append(c == ' ' ? '-' : c);
				}
			}
			var result = headingBuffer.ToString();
			headingBuffer.Length = 0;
			return result;
		}
	}
}
