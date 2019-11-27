namespace Utils
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Notepad;
	using Helpers;
	 using Common;
	public static  class Logic
	{
		
		
		public static void AddCodeLanguage(TextBox textBox, string language)
		{
			var lines = textBox.Text.Split('\n').Select(i => i.TrimEnd('\r'));
			var sb = new StringBuilder();
			var skip = false;
			foreach (var element in lines) {
				if (element.StartsWith("```")) {
					if (skip) {
						sb.AppendLine("```");
						
					} else if (element.Trim() == "```")
						sb.AppendLine("```" + language);
					else
						sb.AppendLine(element);
					skip = !skip;
					
				} else {
					sb.AppendLine(element);
				}
			}
			textBox.Text = sb.ToString();
		}
		public static string ConvertToHtml(TextBox textBox)
		{
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
	
		
		public static void OpenLink(TextBox textBox)
		{
			var selected =	textBox.SelectedText.Trim();
			if (selected.IsVacuum()) {
				textBox.SelectLine();
				selected =	textBox.SelectedText.Trim();
			}
			if (selected.IsVacuum())
				return;
			selected = selected.TrimNonLetterOrDigitStart();
			selected = Regex.Replace(selected, "[^a-zA-Z]$", "");
			if (Directory.Exists(selected) || File.Exists(selected)) {
				Process.Start(selected);
			} else if (selected.StartsWith("https://") || selected.StartsWith("http://")) {
				Process.Start("chrome.exe", selected);
			} else {
				Process.Start("cmd","/K \""+selected+"\"");
				
			}
		}
		
		
		
		
		public static void ImportSingleFile(TextBox textBox)
		{
			WinFormUtils.OnClipboardFile(file => {
				var extension = file.GetExtension();
				switch (extension) {
					case ".fsx":
						extension = "F#: ";
						break;
				}
				var sb = new StringBuilder();
				var title = file.GetFileNameWithoutExtension();
				sb.AppendLine("# " + extension + title).AppendLine();
				var str = file.ReadAllText().Trim();
				while (str.StartsWith("/*")) {
					str = str.SubstringAfter("*/").Trim();
				}    
				sb
			                     			.AppendLine()
			                     			.AppendLine("```")
			                     			.AppendLine()
			                     			.AppendLine(Regex.Replace(str.Replace("`", "\u0060"), "[\r\n]+", "\r\n"))
			                     			.AppendLine("```")
			                     			.AppendLine();	
				textBox.Text = sb.ToString();
			});
		}
	}
}