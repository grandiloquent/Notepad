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

	public static class JavaScriptDelegate
	{
		private static string MinifyJavaScript(string s)
		{
			var min = new Minifier();
			var r = min.MinifyJavaScript(s, PrettyPrintOptions());
			return r;
		}
		private static CodeSettings PrettyPrintOptions()
		{
			var settings = new CodeSettings();
			settings.MinifyCode = false;
			settings.OutputMode = OutputMode.MultipleLines;
			settings.CollapseToLiteral = false;
			//settings.CombineDuplicateLiterals = false;
			settings.EvalTreatment = EvalTreatment.Ignore;
			settings.IndentSize = settings.IndentSize;
			settings.InlineSafeStrings = false;
			settings.LocalRenaming = LocalRenaming.KeepAll;
			settings.MacSafariQuirks = settings.MacSafariQuirks;
			settings.PreserveFunctionNames = true;
			settings.RemoveFunctionExpressionNames = false;
			settings.RemoveUnneededCode = false;
			settings.StripDebugStatements = false;
			return settings;
		}
	
		[BindMenuItem(Name = "从HTML合并文件", Control = "javaScriptStripCompressSplitButton", Toolbar = "toolstrip1")]
		public static void CombineAndCompressJavaScriptFromHTML()
		{
			Methods.OnClipboardFile(file => {
				var htmlStr = file.ReadAllText();
				var src = Regex.Matches(htmlStr, "(?<=src=\")[^\"]+(?=\"></script>)")
			                        		.Cast<Match>()
			                        		.Select(i => i.Value);
				var sb = new StringBuilder();
				var dir = Path.GetDirectoryName(file);
				foreach (var s in src) {
					var f = Path.Combine(dir, s);
					if (File.Exists(f)) {
						sb.AppendLine(f.ReadAllText());
					}
				}
				var sf = Path.Combine(dir, src.First());
				sf = Path.GetDirectoryName(sf);
				var dst = Path.Combine(Path.GetDirectoryName(sf), "app.min.js");
				dst.WriteAllText(MinifyJavaScript(sb.ToString()));
			                        	             
			});
			
		}
	
		
		
		[BindMenuItem(Name = "目录", Control = "jsUncompressButton", Toolbar = "toolstrip1")]
		public static void UnCompressDirectory()
		{
			Methods.OnClipboardDirectory(dir => {
				var td = Path.Combine(dir, "prettyprint");
				td.CreateDirectoryIfNotExists();
				var files = Directory.GetFiles(dir);
				foreach (var f in files) {
					try {
						var min = new Minifier();
						var r = min.MinifyJavaScript(f.ReadAllText(), PrettyPrintOptions());
						Path.Combine(td, f.GetFileName()).WriteAllText(r);
					} catch {
					}
				}
			});
		}
		[BindMenuItem(Name = "文件", Control = "jsUncompressButton", Toolbar = "toolstrip1")]
		public static void UnCompressFile()
		{
			Methods.OnClipboardFile(f => {
				var min = new Minifier();
				var r = min.MinifyJavaScript(f.ReadAllText(), PrettyPrintOptions());
				f.AppendFileName("_prettyprint").WriteAllText(r);
			});
			/*
			 *   var settings = new CodeSettings();
        MinifyCode = settings.MinifyCode;
        OutputMode = settings.OutputMode;
        CollapseToLiteral = settings.CollapseToLiteral;
        CombineDuplicateLiterals = settings.CombineDuplicateLiterals;
        EvalTreatment = settings.EvalTreatment;
        IndentSize = settings.IndentSize;
        InlineSafeStrings = settings.InlineSafeStrings;
        LocalRenaming = settings.LocalRenaming;
        MacSafariQuirks = settings.MacSafariQuirks;
        PreserveFunctionNames = settings.PreserveFunctionNames;
        RemoveFunctionExpressionNames = settings.RemoveFunctionExpressionNames;
        RemoveUnneededCode = settings.RemoveUnneededCode;
        StripDebugStatements = settings.StripDebugStatements;
        */
		}
		
		[BindMenuItem(Name = "Prototype", Control = "jsSortSplitButton", Toolbar = "toolstrip1")]
		public static void SortPrototype()
		{
			Methods.OnClipboardString(SortPrototypeInternal);
		}
		private static string SortPrototypeInternal(string str)
		{
			var list = str.ToBlocks();
			var s = string.Join("\n", list.OrderBy(v => v.SubstringBefore('=').SubstringAfterLast('.').Trim()));
			s = s = Regex.Replace(s, "}\\s+;", "};");
			return s.RemoveWhiteSpaceLines();
		}
		
	}
}