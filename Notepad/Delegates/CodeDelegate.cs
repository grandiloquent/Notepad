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
	
	public static class CodeDelegate
	{
		private static string ImportFile(string file)
		{
			
			var j = "\u0060";
				
			 
			var sb = new StringBuilder();
				
					
			sb.AppendLine("## " + Path.GetFileName(file)).AppendLine();
			var str = file.ReadAllText().Trim();
			while (str.StartsWith("/*")) {
				str = str.SubstringAfter("*/").Trim();
			}
			sb.AppendLine()
			                     			.AppendLine("```")
			                     			.AppendLine()
			                     			.AppendLine(Regex.Replace(str.Replace("`", j), "[\r\n]+", "\r\n"))
			                     			.AppendLine("```")
			                     			.AppendLine();
			                     		
			return sb.ToString();
		}
			
		[BindMenuItem(Name = "导入目录 (目录)", SplitButton = "codeButton", Toolbar = "toolStrip", AddSeparatorBefore = true, NeedBinding = true)]
		public static void ImportDirectory(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			Forms.OnClipboardDirectory(dir => {
				mainForm.SelectedText(ImportDirectoryInternal(dir));
			                           	
			});
		}
		[BindMenuItem(Name = "导出 C#", SplitButton = "codeButton", Toolbar = "toolStrip", AddSeparatorBefore = true, NeedBinding = true)]
		public static void ExportToFiles(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			ExportToFilesInternal(mainForm.GetText(),".cs");
		}
		private static string ExportToFilesInternal(string str,string extension)
		{

			var groups = new List<List<string>>();
			
			var dir=str.SubstringBefore("## ").SubstringAfter(":").Trim().GetDesktopPath();
			dir.CreateIfNotExists();
			
			var lines = ("## " + str.SubstringAfter("## ")).ToLines();
			var isEntered = true;
			
			for (int i = 0; i < lines.Length; i++) {
				if (lines[i].StartsWith("## ")) {
					var j=i;
					while(i+1<lines.Length&&!lines[i+1].StartsWith("## ")){
						i++;
					}
					var list = new List<string>();

					for (int h = j; h < i+1; h++) {
						list.Add(lines[h]);
					}
				
					groups.Add(list);
				}
				
			}
			
			foreach (var element in groups) {
				var first=element.First().SubstringAfter("## ").Trim()+extension;
				var second=string.Join(Environment.NewLine,element.Skip(1)).SubstringAfter("## ").Trim();
				Path.Combine(dir,first).WriteAllText(second.SubstringAfter("```").SubstringBeforeLast("```"));
				
				
			}

			return "";
		}
		private static string ImportDirectoryInternal(string dir)
		{
			var files = Directory.GetFiles(dir)
				.Where(i => Regex.IsMatch(i.GetFileName(), "\\.(?:cs|css|java|cshtml)$"));
			var sb = new StringBuilder();
			foreach (var element in files) {
				sb.AppendLine(ImportFile(element));
			}
			return sb.ToString();
		}
	}
}