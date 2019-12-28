namespace Notepad
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	public static class FormatDelegate
	{
		
		public static string FormatCodeInternal(this string value)
		{
			if (string.IsNullOrWhiteSpace(value) || value.Contains('\n')) {
				return string.Format("\r\n```javascript\r\n{0}\r\n```\r\n", HtmlAgilityPack.HtmlEntity.DeEntitize(value.Trim()));
			}
			return string.Format("`{0}`", HtmlAgilityPack.HtmlEntity.DeEntitize(value.Trim()));
		}
		
	
		public static string ToNumberList(string value)
		{
			var count = 1;
			
			return string.Join(Environment.NewLine, value.Trim()
			                   .Split(Environment.NewLine.ToArray(), 
				StringSplitOptions.RemoveEmptyEntries).
				                   Select(i => (count++) + ". " + i.Trim()));
		}
		
		//		[BindMenuItem(Control = "imgStripButton", Toolbar = "toolStrip3", NeedBinding = true)]
		//		public static void ForamtImage(ToolStripItem menuItem, MainForm mainForm)
		//		{
		//			Wins.OnClipboardFile(f => {
		//				if (!Regex.IsMatch(f, "\\.(?:png|jpg|svg)$"))
		//					return;
		//
		//				var dest = Path.Combine(
		//					           Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
		//					           "assets",
		//					           "static",
		//					           "pictures",
		//					           Path.GetFileName(f));
		//				if (File.Exists(dest))
		//					return;
		//				File.Move(f, dest);
		//				var textBox = mainForm.textBox;
		//				textBox.SelectedText =
		//					string.Format("<img class=\"img-center\" alt=\"\" src=\"../static/pictures/{0}\">", Path.GetFileName(f));
		//				//string.Format("![](../static/pictures/{0})", Path.GetFileName(f));
		//
		//
		//			});
		//		}
		public static String OrderH3(String value)
		{
			var list = value.Trim().Split('\n');
			var dictionary = new Dictionary<String,List<string>>();
			List<String> lines = null;
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
			var collection =	dictionary.OrderBy(i => Regex.Replace(i.Key, "[0-9]+(?=\\.)", new MatchEvaluator((v) => {
				var vx = int.Parse(v.Value);
			                                                                                                  	
				return vx.ToString().PadLeft(5, '0');
			})));
			foreach (var element in collection) {
				result = result.Concat(element.Value).ToList();
				result.Add(Environment.NewLine);
			}
			return string.Join(Environment.NewLine, result);
		}
		public static String OrderH2(String value)
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
				result.Add(Environment.NewLine);
			}
			return string.Join(Environment.NewLine, result);
		}
		[BindMenuItem(Control = "格式化", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void Foramt(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.Format();
		}
		
		[BindMenuItem(Control = "排序H2块", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void OrderH2s(ToolStripItem menuItem, MainForm mainForm)
		{
			var str =	mainForm.textBox.Text.Trim();

			var sb = new StringBuilder();
		
			sb.AppendLine(str.SubstringBefore('\n').Trim()).AppendLine().AppendLine(OrderH2(str.SubstringAfter('\n')));

			mainForm.textBox.Text = sb.ToString();
		}
		[BindMenuItem(Control = "排序H3块", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void OrderH3s(ToolStripItem menuItem, MainForm mainForm)
		{
//			var str =	mainForm.textBox.Text.Trim();
//
//			var sb = new StringBuilder();
//		
//			sb.AppendLine(str.SubstringBefore('\n').Trim()).AppendLine().AppendLine(OrderH3(str.SubstringAfter('\n')));

			mainForm.textBox.SelectedText = OrderH3(mainForm.textBox.SelectedText);
		}
		
		[BindMenuItem(Control = "链接", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtLink(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.SelectedText = string.Format("[{0}]({1})", textBox.SelectedText.Trim(), Clipboard.GetText().Trim());
		}
	
		[BindMenuItem(Control = "代码", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtCode(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.SelectedText = FormatCodeInternal(textBox.SelectedText);
		}
		
		[BindMenuItem(Control = "supStripButton", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtSup(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.SelectedText = string.Format("<sup>{0}</sup>", textBox.SelectedText.Trim());
		}
		[BindMenuItem(Control = "h3SplitButton", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtH3(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			Methods.FormatH3(textBox);
		}
		[BindMenuItem(Control = "h2SplitButton", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtH2(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			Methods.FormatH2(textBox);
		}
		[BindMenuItem(Control = "subButton", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtSub(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.SelectedText = string.Format("<sub>{0}</sub>", textBox.SelectedText.Trim());
		}
		[BindMenuItem(Control = "numberListStripButton", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtNumberList(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.SelectedText = ToNumberList(textBox.SelectedText);
			
		}
		[BindMenuItem(Control = "列表", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void ForamtList(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			var start = textBox.SelectionStart;
			var end = textBox.SelectionStart + textBox.SelectionLength;
			var length = textBox.Text.Length;
			var value = textBox.Text;
			while (start > 0 && value[start - 1] != '\n') {
				start--;
			}
			while (end < length && value[end] != '\n') {
				end++;
			}
			textBox.SelectionStart = start;
			textBox.SelectionLength = end - start;
			textBox.SelectedText = ListHelper.ToList(textBox.SelectedText);
			
		}
		[BindMenuItem(Control = "图片", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void InsertImagesInDirectory(ToolStripItem menuItem, MainForm mainForm)
		{
			Wins.OnClipboardDirectory(dir => {
				var extensions = new []{ ".jpg" };
				var files = Directory.GetFiles(dir)
			                          		.Where(i => extensions.Any(x => x == i.GetExtension().ToLower()))
			                          		.OrderBy(i => i.GetFileName());
				var ls = new List<string>();
				var ls1 = new List<string>();
				
				
				foreach (var element in files) {
					ls.Add(string.Format("![](/commodities/static/images/{0})", element.GetFileName()));
					ls1.Add(string.Format("\"{0}\",", element.GetFileName()));
				}
				mainForm.textBox.SelectedText += ls.ConcatenateLines() + "\r\n\r\n" + ls1.ConcatenateLines();
			                          	
			});
			
		}
		[BindMenuItem(Control = "标题", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void InsertTitles(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			//Wins.OnClipboardText(s => textBox.SelectedText = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => "### " + i.SubstringAfterLast(".")).ConcatenateLines());
//
//			Wins.OnClipboardText(s => {
//			                     
//				var hd = new HtmlAgilityPack.HtmlDocument();
//				hd.LoadHtml(s);
//				var ls=new List<string>();
//			                     	
//				hd.DocumentNode.SelectNodes("//ul[@class='_entry-list']//a").ForEach((e,i) => {
//				                                                                     	ls.Add(string.Format("- [{0}](https://devdocs.io{1})",e.InnerText,e.GetAttributeValue("href","")));
//				});
//				
//				textBox.SelectedText=ls.OrderBy(i=>i).ConcatenateLines();
//			                  
//			});
			
			
			Wins.OnClipboardText(s => {
			                     
				var hd = new HtmlAgilityPack.HtmlDocument();
				hd.LoadHtml(s);
				var ls = new List<string>();
			                     	
				hd.DocumentNode.SelectNodes("//li[contains(@class,'toc-level-1')]").ForEach((e, i) => {
				                                                                            	
					var h1 = e.SelectSingleNode("./a");
					ls.Add(string.Empty);
					ls.Add(string.Format("- {0}", h1.InnerText));
					ls.Add(string.Empty);
					
					
					var nodes = e.SelectNodes(".//li[contains(@class,'toc-level-2')]/a");
					if (nodes != null && nodes.Any())
						nodes.ForEach((x, j) => {
					                               
							ls.Add(string.Format("  - {0}", System.Web.HttpUtility.HtmlDecode(x.InnerText)));
						});
					 
					
				});
				
				textBox.SelectedText = ls.ConcatenateLines();
			                  
			});
		}
		[BindMenuItem(Control = "排序", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void OrderSelected(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.SelectedText = textBox.SelectedText.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
				.Select(i => i.Trim())
				.Distinct()
				.OrderBy(i => i)
				.ConcatenateLines();
		}
		[BindMenuItem(Control = "其他", Toolbar = "toolStrip3", NeedBinding = true)]
		public static void OtherOperation(ToolStripItem menuItem, MainForm mainForm)
		{
			Wins.OnClipboardString(s => {
				return Regex.Replace(s, "(?<=\\s)[0-9\\.]+", new MatchEvaluator(m => {
			                       	                                                           
					return (float.Parse(m.Value) * 2).ToString();
				}));
			});
			
		}
	}
}