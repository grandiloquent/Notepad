namespace Notepad
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	public static class FindDelegate
	{



		
		
		public static string StringTemplateUpperCaseSerial(string text, string matchPattern, string pattern)
		{
			if (string.IsNullOrWhiteSpace(text)
			    || string.IsNullOrWhiteSpace(matchPattern)
			    || string.IsNullOrWhiteSpace(pattern)) {
				return string.Empty;
			}
			var patternCount = Regex.Matches(pattern, "(?<!\\{)\\{\\d+\\}(?!\\})").Count;
			var matches = Regex.Matches(text, matchPattern).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			var index = 0;
			foreach (var element in matches) {
				var elements = new string[patternCount];
				for (int i = 0; i < patternCount; i++) {
//					if (i == patternCount - 1) {
//						continue;
//					}
					if (i == 0)
						elements[i] = element.Capitalize();
					else
						elements[i] = element;
						
				}
				//elements[elements.Length - 1] = (index++).ToString();
				ls.Add(string.Format(pattern.Replace("\\n", "\n"), elements));
			}
			var result = ls.ConcatenateLines();
				
			return result;
		}
		public static string StringTemplateParameterize(string text, string matchPattern, string pattern)
		{
			if (string.IsNullOrWhiteSpace(text)
			    || string.IsNullOrWhiteSpace(matchPattern)
			    || string.IsNullOrWhiteSpace(pattern)) {
				return string.Empty;
			}
			var patternCount = Regex.Matches(pattern, "(?<!\\{)\\{\\d+\\}(?!\\})").Count;
			var matches = Regex.Matches(text, matchPattern).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			var index = 0;
			foreach (var element in matches) {
				var elements = new string[patternCount];
				for (int i = 0; i < patternCount; i++) {
//					if (i == patternCount - 1) {
//						continue;
//					}
					if (i == 0)
						elements[i] = Regex.Replace(element.ToLower(), "[\\-_].", new MatchEvaluator(m => m.Value.Trim("-_".ToArray()).ToUpper()));
					else
						elements[i] = element;
						
				}
				//elements[elements.Length - 1] = (index++).ToString();
				ls.Add(string.Format(pattern.Replace("\\n", "\n"), elements));
			}
			var result = ls.Concatenates();
				
			return result;
		}
		public static string StringTemplateParameterizeUpperCase(string text, string matchPattern, string pattern)
		{
			if (string.IsNullOrWhiteSpace(text)
			    || string.IsNullOrWhiteSpace(matchPattern)
			    || string.IsNullOrWhiteSpace(pattern)) {
				return string.Empty;
			}
			var patternCount = Regex.Matches(pattern, "(?<!\\{)\\{\\d+\\}(?!\\})").Count;
			var matches = Regex.Matches(text, matchPattern).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			var index = 0;
			foreach (var element in matches) {
				var elements = new string[patternCount];
				for (int i = 0; i < patternCount; i++) {
//					if (i == patternCount - 1) {
//						continue;
//					}
					if (i == 0)
						elements[i] = Regex.Replace(element.ToLower(), "[\\-_].", new MatchEvaluator(m => m.Value.Trim("-_".ToArray()).ToUpper())).Capitalize();
					else
						elements[i] = element;
						
				}
				//elements[elements.Length - 1] = (index++).ToString();
				ls.Add(string.Format(pattern.Replace("\\n", "\n"), elements));
			}
			var result = ls.Concatenates();
				
			return result;
		}
		public static string StringTemplateSerial(string text)
		{
			
			var match = Regex.Match(text, "\\{([0-9]+) ([0-9]+)}");
			if (!match.Success) {
				return null;
			}
			var m1 = match.Groups[1].Value;
			var m2 = match.Groups[2].Value;
			
			var i1 = int.Parse(m1);
			var i2 = int.Parse(m2);

			var min = Math.Min(i1, i2);
			var max = Math.Max(i1, i2);
			var ls = new List<string>();
			var pad = Regex.Match(m1, "^0+").Value.Length + 1;
			
			for (int i = min; i < max + 1; i++) {
				if (pad != 1)
					ls.Add(Regex.Replace(text, "\\{([0-9]+) ([0-9]+)}", i.ToString().PadLeft(pad, '0')));
				else
					ls.Add(Regex.Replace(text, "\\{([0-9]+) ([0-9]+)}", i.ToString()));
			}
			return ls.ConcatenateLines();
			 
//			var ls = new List<string>();
//			var index = 1;
//			for (int j = 0; j < 50; j++) {
//				
//				ls.Add(string.Format(text.Replace("\\n", "\n"), index++.ToString()));
//			}
//		
//			var result = ls.Concatenates();
//				
//			return result;
		}
		public static string StringTemplate(string text, string matchPattern, string pattern)
		{
			if (string.IsNullOrWhiteSpace(text)
			    || string.IsNullOrWhiteSpace(matchPattern)
			    || string.IsNullOrWhiteSpace(pattern)) {
				return string.Empty;
			}
			var patternCount = Regex.Matches(pattern, "(?<!\\{)\\{\\d+\\}(?!\\})").Count;
			var matches = Regex.Matches(text, matchPattern).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			var index = 0;
			foreach (var element in matches) {
				var elements = new string[patternCount];
				for (int i = 0; i < patternCount; i++) {
//					if (i == patternCount - 1) {
//						continue;
//					}
					if (i == 0)
						elements[i] = element;
					else
						elements[i] = element;
						
				}
				//elements[elements.Length - 1] = (index++).ToString();
				ls.Add(string.Format(pattern.Replace("\\n", "\n"), elements));
			}
			var result = ls.Concatenates();
				
			return result;
		}
		[BindMenuItem(Name = "查找", Control = "findStripSplitButton", Toolbar = "toolStrip2", NeedBinding = true, ShortcutKeys = Keys.Control | Keys.F)]
	
		public static void Find(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			var findBox = mainForm.findBox;
			var search = textBox.SelectedText;
			if (string.IsNullOrWhiteSpace(search)) {
				search = findBox.Text;
			}
			if (string.IsNullOrWhiteSpace(search))
				return;
			var regex = new Regex(search);
			var m = regex.Match(textBox.Text, textBox.SelectionStart + textBox.SelectionLength);
			if (!m.Success) {
				m = regex.Match(textBox.Text);
			}
			if (m.Success) {
				textBox.SelectionStart = m.Index;
				textBox.SelectionLength = m.Value.Length;
				textBox.ScrollToCaret();
			}
		}
		
	
		[BindMenuItem(Control = "格式化模板", Toolbar = "stringToolbar", NeedBinding = true)]
	
		public static void FormatPattern(ToolStripItem menuItem, MainForm mainForm)
		{
				var s = Clipboard.GetText().Trim();
			if (!string.IsNullOrWhiteSpace(s)) {
				s = Regex.Replace(s, "[\t\n\r]+", "");
				s = Regex.Replace(s, "\\s{2,}", " ");
				Clipboard.SetText(s.Replace("{", "{{").Replace("}", "}}"));
			}
		
			
		}
		[BindMenuItem(Control = "大写化", Toolbar = "stringToolbar", NeedBinding = true)]
	
		public static void ReplaceUppercase(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			var matchPattern = mainForm.findBox.Text;
			var pattern = mainForm.replaceBox.Text;
			var text =	StringTemplateUpperCaseSerial(textBox.Text, matchPattern, pattern);
			try {
				Clipboard.SetText(text);
				
			} catch {
				
			}
			
		}
		[BindMenuItem(Control = "参数化", Toolbar = "stringToolbar", NeedBinding = true)]
	
		public static void ReplaceParameterize(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			var matchPattern = mainForm.findBox.Text;
			var pattern = mainForm.replaceBox.Text;
			var text =	StringTemplateParameterize(textBox.Text, matchPattern, pattern);
			try {
				Clipboard.SetText(text);
				
			} catch {
				
			}
			
		}
		[BindMenuItem(Control = "大写参数化", Toolbar = "stringToolbar", NeedBinding = true)]
	
		public static void ReplaceParameterizeUpperCase(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			var matchPattern = mainForm.findBox.Text;
			var pattern = mainForm.replaceBox.Text;
			var text =	StringTemplateParameterizeUpperCase(textBox.Text, matchPattern, pattern);
			try {
				Clipboard.SetText(text);
				
			} catch {
				
			}
			
		}
		[BindMenuItem(Control = "序列化", Toolbar = "stringToolbar", NeedBinding = true)]
	
		public static void ReplaceSerial(ToolStripItem menuItem, MainForm mainForm)
		{
			Wins.OnClipboardString(StringTemplateSerial);
			
		}
		//
		
		
		[BindMenuItem(Control = "保留", Toolbar = "toolStrip", NeedBinding = true)]
	
		public static void KeepMatches(ToolStripItem menuItem, MainForm mainForm)
		{
			var r = mainForm
				.findBox
				.Text
				.Replace("\\n", "\n");
	
			var s = Regex.Matches(mainForm.textBox.Text,
				        r)
				.Cast<Match>()
				.Select(i => "\"" + i.Value + "\",")
			        ///.OrderBy(i => i)
				.Distinct()
				.Aggregate(new StringBuilder(), 
				        (builder, nextValue) => builder.AppendLine(nextValue))
				.ToString();
			if (!string.IsNullOrWhiteSpace(s))
				mainForm.textBox.Text = s;
			
		}
		
		[BindMenuItem( Control = "模板",Toolbar = "stringToolbar",NeedBinding=true)]
	
		public static void Replace(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			var matchPattern = mainForm.findBox.Text;
			var pattern = mainForm.replaceBox.Text;
			var text =	StringTemplate(textBox.Text, matchPattern, pattern);
			try {
				Clipboard.SetText(text);
				
			} catch {
				
			}
		}
		[BindMenuItem(Control = "替换", Toolbar = "toolStrip", NeedBinding = true)]
	
		public static void ReplaceNormal(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			var matchPattern = mainForm.findBox.Text;
			var pattern = mainForm.replaceBox.Text;
			SaveReplaceSettings(matchPattern);
			textBox.Text = textBox.Text.Replace(matchPattern, pattern);
		}
		//		[BindMenuItem(Name = "移除非中文行",SplitButton="findButton", Toolbar = "toolStrip2", AddSeparatorBefore = true,NeedBinding=true)]
		//		public static void RemoveNoChineseLines(ToolStripMenuItem menuItem,MainForm mainForm)
		//		{
		//
		//			var lines=mainForm.textBox.Lines.Where(i=>Regex.IsMatch(i,"\\p{Lo}"));
		//			mainForm.textBox.Text=string.Join(Environment.NewLine,lines);
		//
		//		}
		
		private static void SaveReplaceSettings(string findString)
		{
			
			var findFileName = "find.txt".GetEntryPath();
			var ls = new List<string>();
			
			if (File.Exists(findFileName)) {
				ls.AddRange(File.ReadAllLines(findFileName, new UTF8Encoding(false)));
			} else {
				ls.Add(findString);
			}
			
			File.WriteAllLines(findFileName, ls.Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.Trim()).OrderBy(i => i));
			
		}

	}
}