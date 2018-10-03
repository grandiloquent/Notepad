using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shared;
using System.Diagnostics;

namespace Notepad
{

	public static class Helper
	{
		public static void SortVSCSnippets()
		{
			
			OnClipboardString((str) => {
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(str);
				obj = obj.OrderBy(i => i.Value["prefix"]).ThenBy(i => i.Key).ToDictionary(k => k.Key, k => k.Value);
				return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			});
		}
		public static void ZipDirectories()
		{
			OnClipboardDirectory((dir) => {
				var directories = Directory.GetDirectories(dir);
				foreach (var element in directories) {
					using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {
						zip.AddDirectory(element);
						zip.Save(element + ".zip");
					}
					
				}
			});
		}
		
		public static void CFormat()
		{
			OnClipboardString((str) => {
			                  	var ls = Helper.FormatMethodList(string.Join("\n",Clipboard.GetText().Split("\r\n".ToArray(),StringSplitOptions.RemoveEmptyEntries)));
				var d = ls.Select(i => i.SubstringBefore(")") + ");").Where(i => i.IsReadable()).Select(i => i.Trim()).OrderBy(i => i.Split("(".ToArray(),2).First().Split(' ').Last());
				var bodys = ls.OrderBy(i => Regex.Split(i.Split("(".ToArray(), 2).First(), "[: ]+").Last());
				return	string.Join("\n", d) + "\n\n\n" + string.Join("\n", bodys);
			});
		}
		public static void CPlusPlusSnippetsVSC()
		{
			OnClipboardString((str) => {
				var s = str.Trim();
				var ls = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray();
				var matches = Regex.Matches(ls.First(), "[a-zA-Z]+").Cast<Match>().Select(i => i.Value.First().ToString()).ToArray();
				
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix", string.Join("", matches).ToLower());
				obj.Add("body", ls.Select(i => i.EscapeString()));
				
				var r = new Dictionary<string,dynamic>();
				r.Add(ls.First(), obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return	sr.Substring(1, sr.Length - 2) + ",";
				
			});
		}
		public static void GenerateDigit()
		{
			var ranges = Enumerable.Range(0, 11);
			Clipboard.SetText(string.Join(",", ranges));
		}
		public static void RemoveAria2File()
		{
			OnClipboardDirectory((dir) => {
				var directories = Directory.GetDirectories(dir);
				foreach (var element in directories) {
					var files = Directory.GetFiles(element, "*", SearchOption.AllDirectories);
					var filesAria2 = files.Where(i => i.EndsWith(".aria2")).Select(i => i.GetFileNameWithoutExtension()).OrderBy(i => i).Distinct().ToArray();
					foreach (var f in files) {
						if (filesAria2.Contains(f.GetFileName()) || f.EndsWith(".aria2")) {
							File.Delete(f);
						}
					}
				}
			});
		}
	
		public static void CleanHtmls()
		{
			OnClipboardDirectory((dir) => {
				var diretories = Directory.GetDirectories(dir);
				foreach (var r in diretories) {
					const string str = "<div><div><img src=\"./images/\"><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";
					var files = Directory.GetFiles(r, "*.html", SearchOption.TopDirectoryOnly);
					foreach (var element in files) {
						element.WriteAllText(Regex.Replace(element.ReadAllText().Replace(str, ""), "style=\"[^\"]*?\"", ""));
					}
				}
			});
			
			
		}
		public static void GenerateGccCommand()
		{
			OnClipboardFile((f) => {
				var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
				if (!Directory.Exists(dir)) {
					Directory.CreateDirectory(dir);
				}
				//var cmd = string.Format("/K gcc \"{0}\" -o \"{1}\\t.exe\" && \"{1}\\t.exe\" ", f, dir);
				//Process.Start("cmd",cmd);
				var cmd = string.Format("gcc \"{0}\" -o \"{1}\\t.exe\" && \"{1}\\t.exe\" ", f, dir);
				
				Clipboard.SetText(cmd);
			});
		}
		public static void RunGenerateGccCommand()
		{
			OnClipboardFile((f) => {
				var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
				if (!Directory.Exists(dir)) {
					Directory.CreateDirectory(dir);
				}
				
				var arg = "";
				var argLines = f.ReadLines();
				foreach (var element in argLines) {
					if (element.IsVacuum())
						continue;
					if (element.StartsWith("// ")) {
						arg += element.Substring(3) + " ";
					} else
						break;
				}
				
				
				var cmd = string.Format("/K gcc \"{0}\" -o \"{1}\\t.exe\" {2} && \"{1}\\t.exe\" ", f, dir,arg);
				Process.Start("cmd", cmd);
				
			});
		}
		public static void RunGoCommand()
		{
			OnClipboardFile((f) => {
				var arg = "";
				var argLines = f.ReadLines();
				foreach (var element in argLines) {
					if (element.IsVacuum())
						continue;
					if (element.StartsWith("// ")) {
						arg += element.Substring(3) + " ";
					} else
						break;
				}
				
				var cmd = string.Format("/K go run \"{0}\" {1}", f, arg);
				Process.Start("cmd", cmd);
				
			});
		}
		public static void RunGenerateGPlusPlusCommand()
		{
			OnClipboardFile((f) => {
				var arg = "";
				var argLines = f.ReadLines();
				foreach (var element in argLines) {
					if (element.IsVacuum())
						continue;
					if (element.StartsWith("// ")) {
						arg += element.Substring(3) + " ";
					} else
						break;
				}
			                	
				var dir = f.GetDirectoryName().Combine("bin");
				dir.CreateDirectoryIfNotExists();
				var cmd = string.Format("g++ -std=c++17 {2} \"{0}\" -o \"{1}\\t.exe\" && \"{1}\\t.exe\" ", f, dir, arg);
				//Clipboard.SetText(cmd);
				Process.Start("cmd", "/K " + cmd);
				/*
				var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
				if (!Directory.Exists(dir)) {
					Directory.CreateDirectory(dir);
				}
				//  -DFILESYSTEM_EXPERIMENTAL -lstdc++fs 
				//	 var cmd = string.Format("g++ -std=c++17 \"{0}\" -lstdc++fs -o \"{1}\\t.exe\" && \"{1}\\t.exe\" ", f, dir);
				
				var cmd = string.Format("g++ -std=c++17 \"{0}\" -o \"{1}\\t.exe\" && \"{1}\\t.exe\" ", f, dir);
				Clipboard.SetText(cmd);
				//var cmd = string.Format("/K g++ \"{0}\" -o \"{1}\\t.exe\" && \"{1}\\t.exe\" ", f, dir);
				
				*/
			});
		}
		public static void GenerateGPlusPlusCommand()
		{
			OnClipboardFile((f) => {
				var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
				if (!Directory.Exists(dir)) {
					Directory.CreateDirectory(dir);
				}
				//var cmd = string.Format("/K gcc \"{0}\" -o \"{1}\\t.exe\" && \"{1}\\t.exe\" ", f, dir);
				//Process.Start("cmd",cmd);
				var cmd = string.Format("g++ \"{0}\" -o \"{1}\\x.exe\" && \"{1}\\x.exe\" ", f, dir);
				
				Clipboard.SetText(cmd);
			});
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
			} else {
				Process.Start("chrome.exe", selected);
			}
		}
		
		public static void KotlinFormatJavaMethodParameters()
		{
			OnClipboardString((v) => {
				var ls = v.Split(new []{ ',' }, StringSplitOptions.RemoveEmptyEntries);
				var r = new List<String>();
				foreach (var element in ls) {
					var s = Regex.Split(element.Trim(), "\\s+");
					r.Add(string.Format("var {0}:{1}", s.Last(), s.First().Capitalize()));
				}
				
				return string.Join(",\n", r);
			});
		}
		
		public static void KotlinExtractParameters()
		{
			OnClipboardString((v) => {
				var ls = v.Split(new []{ ':' }, StringSplitOptions.RemoveEmptyEntries);
				var r = new List<String>();
				foreach (var element in ls) {
					var s = Regex.Split(element.Trim(), "\\s+");
					r.Add(s.Last());
				}
				
				return string.Join(",", r);
			});
		}
		
		public static void OnClipboardString(Func<String,String> func)
		{
			try {
				var str = Clipboard.GetText().Trim();
				if (str.IsVacuum())
					return;
				str = func(str);
				if (str.IsReadable())
					Clipboard.SetText(str);
			} catch {
				
			}
		}
		public static void OnClipboardDirectory(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (Directory.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (Directory.Exists(ls[0])) {
							dir = ls[0];
						}
					}
				}
				if (found) {
					action(dir);
				}
			} catch {
				
			}
		}
		public static void OnClipboardFile(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (File.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (File.Exists(ls[0])) {
							dir = ls[0];
						}
					}
				}
				if (found) {
					action(dir);
				}
			} catch {
				
			}
		}
		public static IEnumerable<string> FormatMethodList(string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (int i = 0; i < value.Length; i++) {
				sb.Append(value[i]);

				if (value[i] == '{') {
					count++;
				} else if (value[i] == '}') {
					count--;
					if (count == 0) {
						ls.Add(sb.ToString());
						sb.Clear();
					}
				}

			}
			//if (ls.Any())
			//{
			//    var firstLine = ls[0];
			//    ls.RemoveAt(0);
			//    ls.Add(firstLine.)

			//}
			return ls;
			//return ls.Select(i => i.Split(new char[] { '{' }, 2).First().Trim() + ";").OrderBy(i => i.Trim());

		}
	}
}
