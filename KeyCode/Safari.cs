using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Linq;
using System.Net;
namespace KeyCode
{
	public class Safari
	{
		public static string GenerateToc1(string dir)
		{
			var files = Directory.GetFiles(dir, "*.html");
			
			var hd = new HtmlAgilityPack.HtmlDocument();
			
			var list1 = new List<string>();
			var list2 = new List<string>();
			var count = 1;
			
			var stringBuilder = new System.Text.StringBuilder();
			foreach (var element in files) {
				hd.LoadHtml(element.ReadAllText());
				var ChapterTitle = hd.DocumentNode.SelectSingleNode("//*[@class='ChapterTitle']");
				if (ChapterTitle != null) {
					stringBuilder.Clear();
					stringBuilder.AppendLine(@"<div class=""sgc-toc-level-1"">");
					stringBuilder.AppendFormat(@"  <a href=""../Text/{0}"">{1}</a>", element.GetFileName(), ChapterTitle.InnerText);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(@"</div>");
					list1.Add(stringBuilder.ToString());
					
					stringBuilder.Clear();
				
					stringBuilder.AppendFormat(@"    <navPoint id=""navPoint-{0}"" playOrder=""{0}"">", count++);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(@"      <navLabel>");
					stringBuilder.AppendFormat(@"        <text>{0}</text>", ChapterTitle.InnerText);
					stringBuilder.AppendLine(); 
					stringBuilder.AppendLine(@"      </navLabel>");
					stringBuilder.AppendFormat(@"      <content src=""Text/{0}""/>", element.GetFileName());
					stringBuilder.AppendLine();
					
					list2.Add(stringBuilder.ToString());

				}
				var nodes = hd.DocumentNode.SelectNodes("//*[@class='Heading1']");
				if (nodes != null) {
					foreach (var n in nodes) {
						stringBuilder.Clear();
						stringBuilder.AppendLine(@"<div class=""sgc-toc-level-2"">");
						stringBuilder.AppendFormat(@"  <a href=""../Text/{0}#{1}"">{2}</a>", element.GetFileName(), n.GetAttributeValue("id", ""), n.InnerText);
						stringBuilder.AppendLine();
						
						stringBuilder.AppendLine(@"</div>");
						list1.Add(stringBuilder.ToString());
						
						stringBuilder.Clear();
				
						stringBuilder.AppendFormat(@"    <navPoint id=""navPoint-{0}"" playOrder=""{0}"">", count++);
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(@"      <navLabel>");
						stringBuilder.AppendFormat(@"        <text>{0}</text>", n.InnerText);
						stringBuilder.AppendLine(); 
						stringBuilder.AppendLine(@"      </navLabel>");
						stringBuilder.AppendFormat(@"      <content src=""Text/{0}#{1}""/>", element.GetFileName(), n.GetAttributeValue("id", ""));
						stringBuilder.AppendLine("</navPoint>");
						list2.Add(stringBuilder.ToString());
					}
					
					
				}
				if (ChapterTitle != null)
					list2.Add("</navPoint>");
				
					
				
			}
			return list1.Concat(list2).ConcatenateLines();
		
		}
		public static void FormatFileNamesBaseToc(string dir)
		{
			var toc = Path.Combine(dir, "目录.html");
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(toc.ReadAllText());
			var nodes = hd.DocumentNode.SelectNodes("//a").Select(i => i.GetAttributeValue("href", "")).Distinct();
			
			var files = Directory.GetFiles(dir, "*.html");
			var index = 1;
			foreach (var element in nodes) {
				try {
					var file =	files.First(i => i.EndsWith("\\" + element));
					File.Move(file, Path.Combine(dir, (index++).ToString().PadLeft(3, '0') + file.GetExtension()));
				} catch {
					
				}
			}
		}
		public static void MoveDownloadBooks(string dir)
		{
			var targetDirectory = (@"C:\Users\psycho\Desktop\Books\Downloads");
			targetDirectory.CreateDirectoryIfNotExists();
			
			var toc = Path.Combine(targetDirectory, "目录.txt");
			
			
			var files = Directory.GetFiles(@"C:\Users\psycho\Desktop\Books\书籍", "*.zip", SearchOption.AllDirectories)
				.Select(i => i.GetFileNameWithoutExtension())
				.OrderBy(i => i);
		
			if (File.Exists(toc)) {
				files = files.Concat(File.ReadAllLines(toc, new UTF8Encoding(false))).Distinct().OrderBy(i => i);
			}
			var directories = Directory.GetDirectories(dir).Where(i => files.Contains(Path.GetFileNameWithoutExtension(i)));
		 
			foreach (var element in directories) {
		
				Directory.Move(element, Path.Combine(targetDirectory, element.GetFileName()));
			}
			File.WriteAllLines(toc, files, new UTF8Encoding(false));
		}
	
		
		
	
		
		
		
		 
	
		
		
		public static void DownloadBooks(string dir)
		{
			
			var files = Directory.GetFiles(dir, "*.html");
			foreach (var element in files) {
				try {
					var hd = new HtmlAgilityPack.HtmlDocument();
					hd.LoadHtml(element.ReadAllText());
					var title = HtmlAgilityPack.HtmlEntity.DeEntitize(hd.DocumentNode.SelectSingleNode("//title").InnerText.SubstringBefore('[').Trim().GetValidFileName());

					var targetDirectory = Path.Combine(dir, title.GetValidFileName().Trim().Replace("™", "").Replace("®", ""));
					if (!Directory.Exists(targetDirectory))
						Directory.CreateDirectory(targetDirectory);

					var targetFile = Path.Combine(targetDirectory, "目录.html");
					var node = hd.DocumentNode.SelectSingleNode("//*[@class='detail-toc']");
					var sb = new List<string>();
					var str = Regex.Replace(node.InnerHtml, "(?<=\\<a href\\=\")[#\\:\\w\\d\\-\\./]+", new MatchEvaluator((m) => {
						sb.Add(m.Value.SubstringBeforeLast("#").TrimEnd('"'));
						return m.Value.SubstringBeforeLast(".").SubstringAfterLast('/') + ".html";
					}));
					Path.Combine(targetDirectory, "links.txt").WriteAllText(string.Join(Environment.NewLine, sb.Distinct()));
					targetFile.WriteAllText(
						"<!DOCTYPE html> <html lang=en> <head> <meta charset=utf-8> <meta content=\"IE=edge http-equiv=X-UA-Compatible> <meta content=\"width=device-width,initial-scale=1\" name=viewport><link href=\"style.css\" rel=\"stylesheet\"></head><body><ol>" +
						str +
						"</ol></body>");
				} catch {
                	                                                              
				}
			}
		}
		public static string[] ParseSearch(string value)
		{

			var hd = new HtmlDocument();
			hd.LoadHtml(value); // //a[contains(@class,'js-book-title')]
			var nodes = hd.DocumentNode.SelectNodes("//h4/a");
			if (nodes.Any()) {
				return nodes.Select(i => i.GetAttributeValue("href", ""))
                .Where(i => i.IsReadable())
                .Distinct()
                .Select(i => "https://www.safaribooksonline.com" + i)
                .ToArray();
			}
			return null;
		}
      
	
		public async static void ExtractParseSearch(string s)
		{
			var dstDir = "aria2c".GetDesktopPath();
			dstDir.CreateDirectoryIfNotExists();
			
			var links =	ParseSearch(s);

			var index = 1;
			foreach (var link in links) {	
				var htm = await Utils.GetWebDatacAsync(link);
				Path.Combine(dstDir, index + ".html").WriteAllText(htm);
				index++;
				
			}
				
//			var node = string.Join(Environment.NewLine, ParseSearch(s));
//			("aria2c".GetDesktopPath()).CreateDirectoryIfNotExists();
//			//var node = hd.DocumentNode.SelectNodes("//a[contains(@class,'js-search-link t-title')]").ToArray().Select(i => "https://www.safaribooksonline.com" + i.GetAttributeValue("href", "")).Distinct().ToArray();
//			("aria2c".GetDesktopPath() + "\\links.txt").WriteAllText(string.Join(Environment.NewLine, node));
//			Process.Start(new ProcessStartInfo() {
//				FileName = "aria2c",
//				WorkingDirectory = "aria2c".GetDesktopPath(),
//				Arguments = "-i \"" + ("aria2c".GetDesktopPath() + "\\links.txt") + "\""
//			});
		}
	}
}