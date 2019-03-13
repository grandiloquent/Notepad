namespace Notepad
{
	using System;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	using System.Collections.Generic;
	using Microsoft.Ajax.Utilities;
	
	public static class JavaScriptDelegate
	{
		private static string SortPrototypeInternal(string str)
		{
			
			var list = str.ToBlocks();
			
			
			
			return 
				string.Join("\n", list.OrderBy(v => v.SubstringBefore('=').SubstringAfterLast('.').Trim()));
		}
		private static string SortFunctionsInternal(string str)
		{
			
			var list = str.ToBlocks();
			
//			var names = list.Select(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '));
//			
//			
//			return "/*\n " + string.Join("\n", names.OrderBy(i => i).Select(i => {
//				if (i.StartsWith("jqLite")) {
//					return	i.Substring(6).Decapitalize() + ":" + i + ",";
//				} else {
//					return	i + ":" + i + ",";
//				}
//			})) +
//			"\n*/\n" +
//			"\n\n\n" +
			return	string.Join("\n", list.OrderBy(v => v.SubstringBefore('(').TrimEnd().SubstringAfterLast(' ')));
		}
		public static string CollectFunctionNamesInternal(string str)
		{
			
			var names = str.ToBlocks().Select(i => i.SubstringBefore('(').Trim().SubstringAfter(' '));
			var list1 = new List<string>();
			var pattern1 = "{0}:{0}";
			
			foreach (var element in names) {
				list1.Add(string.Format(pattern1, element));
			}
			return string.Join(",\n", list1);
		}
		public static void CombineJavaScriptFiles(String dir)
		{
		
			var files = Directory.GetFiles(dir, "*.js").OrderBy(i => i.GetFileName()).ToArray();
			var sb = new StringBuilder();
			foreach (var element in files) {
				sb.AppendLine(element.ReadAllText());
				
				//sb.AppendLine((Path.Combine(dir, element).ReadAllText()));
			}
			var min = new Minifier();
			var str = min.MinifyJavaScript(sb.ToString());
			
			Path.Combine(Path.GetDirectoryName(dir), "app.min.js").WriteAllText(str);
		}
		public static void GenerateScriptsInternal(string dir){
			var files=Directory.GetFiles(dir,"*.js").OrderBy(i=>i.GetFileNameWithoutExtension());
			var list=new List<string>();
			var pattern="<script src=\"~/{0}/{1}\" asp-append-version=\"true\"></script>";
			foreach (var element in files) {
				list.Add(string.Format(pattern,element.GetDirectoryName().GetFileName(),element.GetFileName()));
			}
			Clipboard.SetText(string.Join("\n",list));
		}
		[BindMenuItem(Name = "生成脚本引用代码 (目录)", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
		public static void GenerateScripts()
		{
			Forms.OnClipboardDirectory(GenerateScriptsInternal);
		}
		[BindMenuItem(Name = "排序 Prototype (文本)", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true, NeedBinding = true)]
		public static void SortPrototype(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			Forms.OnClipboardString(SortPrototypeInternal);
		}
		[BindMenuItem(Name = "排序函数 (文本)", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true, NeedBinding = true)]
		public static void SortFunctions(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			Forms.OnClipboardString(SortFunctionsInternal);
		}
		
		[BindMenuItem(Name = "收集函数名 (文本)", SplitButton = "javaScriptButton", Toolbar = "toolStrip1", AddSeparatorBefore = true, NeedBinding = true)]
		public static void CollectFunctionNames(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			Forms.OnClipboardString(CollectFunctionNamesInternal);
		}
		
	}
}