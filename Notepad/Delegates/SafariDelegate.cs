namespace  Notepad
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Xml.Linq;
	using System.IO;
	using System.Threading;
	using HtmlAgilityPack;
	using System.IO.Compression;
	using Common;
	
	public static class SafariDelegate
	{
		private const string BaseTocFileName = "目录.html";

		private static void GenerateTocFromEpubFile(string dir)
		{
			var file = Directory.GetFiles(dir, "*.ncx")[0];
			var hd = new HtmlDocument();
			hd.LoadHtml(file.ReadAllText());
			var nodes = hd.DocumentNode.SelectNodes("//navpoint");
			var list = new List<string>();

			list.Add("<ol>");
			foreach (var item in nodes) {
 
            	
				list.Add(string.Format("<li><a href=\"{0}\">{1}</a></li>",
					item.SelectSingleNode(".//content").GetAttributeValue("src", ""),
					item.SelectSingleNode(".//text").InnerText));
			}
			list.Add("</ol>");

			Path.Combine(dir, BaseTocFileName).WriteAllLines(list);

		}
		
		[BindMenuItem(Name = "整理 EPUB 文件 (目录)", Toolbar = "toolStrip1", SplitButton = "safariSplitButton", AddSeparatorBefore = true)]
		
		public static void OrganizeEPUB()
		{
			Forms.OnClipboardDirectory(PrettyName);
		}
		[BindMenuItem(Name = "移除 HTML 文件中的冗余标记 (目录)", Toolbar = "toolStrip1", SplitButton = "safariSplitButton", AddSeparatorBefore = true)]
		
		public static void RemoveRedundancyTags()
		{
			Forms.OnClipboardDirectory(PrettyFormat);
		}
		[BindMenuItem(Name = "从 EPUB 解压文件生成 TOC (目录)", Toolbar = "toolStrip1", SplitButton = "safariSplitButton", AddSeparatorBefore = true)]
		
		public static void ZipNetCoreProject()
		{
			Forms.OnClipboardDirectory(GenerateTocFromEpubFile);
		}
		private static void PrettyFormat(string dir)
		{
			var diretories = Directory.GetDirectories(dir);
			foreach (var r in diretories) {
				const string str = "<div><div><img src=\"./images/\"><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";
				const string str1 = "<div><div><img><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";


				foreach (var element in Directory.GetFiles(r, "*.html", SearchOption.TopDirectoryOnly)) {
					var sv = Regex.Replace(element.ReadAllText().Replace(str, "").Replace(str1, ""), "(style|width|height)=\"[^\"]*?\"", "");


					element.WriteAllText(sv);
				}
			}

			var files = Directory.GetFiles(dir, "*.html", SearchOption.AllDirectories);
			foreach (var element in files) {
				// <a href="06_Chapter01.xhtml#c

				var valuue = Regex.Replace(element.ReadAllText(), "(?<=\\<a href\\=\")[\\w\\d\\-\\.]+", new MatchEvaluator((m) => {
					if (m.Value == "https" || m.Value == "http")
						return m.Value;
					return m.Value.SubstringBeforeLast(".") + ".html";
				}));
				element.WriteAllText(valuue);
			}
			files = Directory.GetFiles(dir, "*.ncx", SearchOption.AllDirectories);
			foreach (var element in files) {
				// <a href="06_Chapter01.xhtml#c

				var value = Regex.Replace(element.ReadAllText(), "(?<=\\<content src\\=\")[\\:\\w\\d\\-\\./#]+\"", new MatchEvaluator((m) => {

					return m.Value.SubstringAfterLast("/");
				}));
				element.WriteAllText(value);
			}

		}
		private static void ProcessBook(string fileName, string directory)
		{
			XNamespace dc = "http://purl.org/dc/elements/1.1/";
			var val = ZipFile.Open(fileName, ZipArchiveMode.Read);
			var val2 = val.Entries.First(i => i.Name.EndsWith(".opf"));
			var memoryStream = val2.Open();
			// memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			XDocument xDocument = XDocument.Load((Stream)memoryStream);
			XElement[] source = xDocument.Descendants().ToArray();
			XElement xElement = (from i in source
			                              where i.Name == dc + "title"
			                              select i).First();
			XElement xElement2 = null;
			try {
				xElement2 = (from i in source
				                         where i.Name == dc + "creator"
				                         select i).First();
			} catch {
				xElement2 = (from i in source
				                         where i.Name == dc + "description"
				                         select i).First();
			}
			if (xElement != null) {
				string value = xElement.Value;
				value = value.Split("/;:".ToArray(), 2).First();
				value = value.GetValidFileName(' ').Trim();
				string arg = "Anonymous";
				if (xElement2 != null) {
					arg = xElement2.Value.Split('/').Last().Trim().GetValidFileName(' ');
				}
				//$"{value}-{arg}.epub"
				string text = directory.Combine(string.Format("{0} - {1}.epub", value, arg));
				val.Dispose();
				memoryStream.Dispose();
				streamReader.Dispose();
				if (fileName != text && !text.FileExists()) {
					File.Move(fileName, text);
				}
			}
		}
		private static void PrettyName(string directory)
		{
			const string outputDirectory = ".EPUB";
			var targetDirectory = Path.Combine(directory, outputDirectory);
			targetDirectory.CreateDirectoryIfNotExists();
			Directory.GetFiles(directory, "*.epub", SearchOption.AllDirectories)
                .ForEach(file => {
				if (!file.Contains(outputDirectory))
					ProcessBook(file, targetDirectory);
			});
		}
	}
}