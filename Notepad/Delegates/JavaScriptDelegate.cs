namespace Notepad
{
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using System.Collections.Generic;
	using Helpers;
	using Microsoft.Ajax.Utilities;
	using Common;
	
	public static class JavaScriptDelegate
	{
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
		 
		[BindMenuItem(Name = "清除空行", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
		public static void RemoveWhiteSpaceLines()
		{
			Forms.OnClipboardString((s) => {
				var lines = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			                        	
				return string.Join("\n", lines);
			});
		}
		
		[BindMenuItem(Name = "生成代码(HTML)", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
		public static void MakeCodeFromHtml()
		{
			Forms.OnClipboardString((s) => {
				return GenerateCodeFormHtml(s);
			});
		}
		
		
	 
	
		[BindMenuItem(Name = "排序函数(TypeScript)", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
		public static void SortFunctionsTypeScript()
		{
			Forms.OnClipboardString(SortFunctionsInternalTypeScript);
		}
			
	
//		[BindMenuItem(Name = "压缩 JavaScripts", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
//		public static void CompressProjectsJavaScripts()
//		{
//			Helper.CompressScripts(@"C:\Users\psycho\go\src\psycho\static\javascripts"
//			                       , @"C:\Users\psycho\go\src\psycho\static"
//			                      , @"C:\Users\psycho\go\src\psycho\templates\_footer.html");
//			
//		}
//		[BindMenuItem(Name = "压缩 SheetStyles", SplitButton = "javaScriptButton", Toolbar = "toolStrip1")]
//		public static void CompressProjectsSheetStyles()
//		{
//			Helper.CompressScripts(@"C:\Users\psycho\go\src\psycho\static\styles"
//			                       , @"C:\Users\psycho\go\src\psycho\static"
//			                      , @"C:\Users\psycho\go\src\psycho\templates\_header.html", false);
//		}
		
		 
		public	static void CompressSheetStyles()
		{
			var minifier = new Minifier();
			
			const string javaScriptSourceDirectory = @"C:\Users\psycho\go\src\psycho\static\styles";
			const string javaScriptDestinationDirectory = @"C:\Users\psycho\go\src\psycho\static";
			
			var files =	Directory.GetFiles(javaScriptSourceDirectory, "*.css")
				.Where(i => !i.GetFileName().StartsWith(".") && !i.GetFileName().StartsWith("$"))
				.OrderBy(i => i.GetFileName()).ToArray();
			var sb = new StringBuilder();
			foreach (var element in files) {
				sb.AppendLine(element.ReadAllText());
			}
			Path.Combine(javaScriptDestinationDirectory, "app.min.css").WriteAllText(minifier.MinifyStyleSheet(sb.ToString()));
				
			var javaScripts = Directory.GetFiles(javaScriptSourceDirectory, "*.css")
				.Where(i => i.GetFileName().StartsWith("."));
			
			foreach (var element in javaScripts) {
				var m =	minifier.MinifyStyleSheet(element.ReadAllText());
				Path.Combine(javaScriptDestinationDirectory, Path.GetFileNameWithoutExtension(element).TrimStart('.') + ".min.css").WriteAllText(m);
			}
		}
			
		public static void  CompressCssFile()
		{
			var source = @"C:\Users\psycho\go\src\psycho\static\styles\.song.css";
			
			var m = new Minifier();
			var r =	m.MinifyStyleSheet(source.ReadAllText());
			Path.Combine(source.GetDirectoryName().GetDirectoryName(), source.GetFileNameWithoutExtension().TrimStart('.') + ".min.css").WriteAllText(r);
			
		}
		
		static string GenerateCodeFormHtml(string s)
		{
			
			var matches =
				Regex.Matches(s, "(?<=(class|id)=\")[^\"]*?(?=\")")
				.Cast<Match>()
				.Select(i => i.Value);
			
			var items = new List<string>();
			
			foreach (var element in matches) {
				
				var p = element.Split(' ');	
				items.AddRange(p);
			}
			
			
			var list1 = new List<string>();
			var list2 = new List<string>();
			var list3 = new List<string>();
			var list4 = new List<string>();
			var list5 = new List<string>();
			var list6 = new List<string>();
			
			items = items.Select(i => i.Trim()).Where(i => !string.IsNullOrWhiteSpace(i)).Distinct().ToList();
			
			foreach (var item in items) {
				
				var p = item.Split(new char[]{ '-', '_' });
				var n = string.Join(
					        "", p.Select(i => i.Capitalize())).Decapitalize();
				
				list1.Add(string.Format("var {0} = document.querySelector('.{1}');", n, item));
				
				list2.Add(string.Format("this.{0} = document.querySelector('.{1}');if(!this.{0}){{console.log(\"{1}\");return false;}}", n, item));
				
				list3.Add(string.Format("if(!{0}){{console.log(\"{1}\");return;}}", n, item));
				
				list4.Add(string.Format("Audio.prototype.get{0} = function () {{ if (!this.{1}) {{ this.{1} = document.querySelector('.{2}') }} return this.{1}; }};",n.CapitalizeOnlyFirst(),n,item));
	
				list5.Add(string.Format("{0}: HTMLElement;", n));
				list6.Add(string.Format("this.{0} = document.querySelector('.{1}');", n,item));
				
			}
			list1.Add(Environment.NewLine + Environment.NewLine + Environment.NewLine);
			list2.Add(Environment.NewLine + Environment.NewLine + Environment.NewLine);
			return list1.Concat(list2).Concat(list3).Concat(list4)
				.Concat(list5)
				.Concat(list6)
				.ConcatenateLines();
				 
				
		}
		
		static void CompressJavaScripts()
		{
			var minifier = new Minifier();
			
			const string javaScriptSourceDirectory = @"C:\Users\psycho\go\src\psycho\static\javascripts";
			const string javaScriptDestinationDirectory = @"C:\Users\psycho\go\src\psycho\static";
			
			var files =	Directory.GetFiles(javaScriptSourceDirectory, "*.js")
				.Where(i => !i.GetFileName().StartsWith(".") && !i.GetFileName().StartsWith("$"))
				.OrderBy(i => i.GetFileName()).ToArray();
			var sb = new StringBuilder();
			foreach (var element in files) {
				sb.AppendLine(element.ReadAllText());
			}
			Path.Combine(javaScriptDestinationDirectory, "app.min.js").WriteAllText(minifier.MinifyJavaScript(sb.ToString()));
				
			var javaScripts = Directory.GetFiles(javaScriptSourceDirectory, "*.js")
				.Where(i => i.GetFileName().StartsWith("."));
			
			foreach (var element in javaScripts) {
				var m =	minifier.MinifyJavaScript(element.ReadAllText());
				Path.Combine(javaScriptDestinationDirectory, Path.GetFileNameWithoutExtension(element).TrimStart('.') + ".min.js").WriteAllText(m);
			}
		}
		private static string SortFunctionsInternalTypeScript(string str)
		{
			
			var list = str.ToBlocks();
			
			
			return	string.Join("\n", list.OrderBy(v =>{
			                                      	var s= v.SubstringBefore('(').Trim();
			                                      	if(s.StartsWith("static ")){
			                                      		return s.Replace("static ",".");
			                                      	}else{
			                                      		return s.Split(' ').Last();
			                                      	}
			                                      })).RemoveWhiteSpaceLines();
		 
		}
		
		
	}
}