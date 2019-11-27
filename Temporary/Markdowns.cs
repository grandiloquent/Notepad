namespace Temporary
{
	using System.Collections.Generic;
	using HtmlAgilityPack;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Linq;
	
	public static class Markdowns
	{
		
		private static void Visitor1(StringBuilder sb, HtmlNode htmlNode)
		{
			
			var className = htmlNode.GetAttributeValue("class", string.Empty);
			var tagName = htmlNode.Name;
			
			if (tagName == "body" || tagName == "div") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor1(sb,	child);
				}
				return;
			}

			if (tagName == "h3") {
				sb.Append("# ")
					.AppendLine(htmlNode.InnerText.SubstringAtfer('、').Trim())
					.AppendLine();
			} else if (tagName == "p") {
				var text = htmlNode.InnerText.Trim();
				if (string.IsNullOrWhiteSpace(text))
					return;
				
				if (className == "left-content") {
					sb.Append("## ")
						.AppendLine(htmlNode.InnerText.RemoveWhiteSpace())
						.AppendLine();
				} else if (className == "calibre8") {
					text = text.Trim();
					
					if (htmlNode.InnerHtml.Contains("<img")) {
						sb.Append("11").Append(". ").AppendLine(text.Substring(1))
							.AppendLine();
						return;
					}
					var c = text[0];
					if ("①②③④⑤⑥⑦⑧⑨⑩".IndexOf(c) != -1) {
						var startChar = "1";
						if (c == '②') {
							startChar = "2";
						} else if (c == '③') {
							startChar = "3";
							
						} else if (c == '④') {
							startChar = "4";
							
							
						} else if (c == '⑤') {
							startChar = "5";
							
							
						} else if (c == '⑥') {
							startChar = "6";
							
						} else if (c == '⑦') {
							startChar = "7";
							
						} else if (c == '⑧') {
							startChar = "8";
							
						} else if (c == '⑨') {
							startChar = "9";
							
						} else if (c == '⑩') {
							startChar = "10";
							
						}
						sb.Append(startChar).Append(". ").AppendLine(text.Substring(1))
							.AppendLine();
						return;
					}
					
					if (text.StartsWith("预案")) {
						sb.Append("* **")
							.Append(text.SubstringBefore('：'))
							.Append("：** ");
						var after = text.SubstringAtfer('：');
						if (!string.IsNullOrWhiteSpace(after)) {
							sb.AppendLine(after).AppendLine();
						}
						return;
					}
					sb.AppendLine(text.Trim())
						.AppendLine();
				} else if (className == "ff-kaiti") {
					if (text.StartsWith("预案")) {
						sb.Append("* **")
							.Append(text.SubstringBefore('：'))
							.Append("：** ");
						
						sb.AppendLine(text.SubstringAtfer('：'))
							.AppendLine();
						return;
					} else {
						sb.AppendLine(text)
							.AppendLine();
						return;
					}
				}
			} else if (tagName == "img") {
				var imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
				var imageFile = htmlNode.GetAttributeValue("src", "").SubstringAtferLast('/');
				var targetFileName = "qkyszlsc_" + imageFile;
				var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生诊疗手册（第三版） - 凌敏\images";

				try {
					File.Move(Path.Combine(dir, imageFile),
						Path.Combine(dir, targetFileName));
				} catch {
					
				}
				sb.AppendLine(string.Format(imageFormat, targetFileName))
					.AppendLine();
			} else {
				if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
					sb.AppendLine(htmlNode.InnerText.Trim())
						.AppendLine();
			}
		}
		private static void Visitor2(StringBuilder sb, HtmlNode htmlNode)
		{
			
			var className = htmlNode.GetAttributeValue("class", string.Empty);
			var tagName = htmlNode.Name;
			var contentText = HtmlEntity.DeEntitize(htmlNode.InnerText.Trim());
			
			if (className == "content") {
				if (Regex.IsMatch(contentText, "^[\\d+]\\.")) {
					sb.AppendLine(Regex.Replace(contentText, "^[\\d+]\\.", "$0 ")).AppendLine();
				} else if (Regex.IsMatch(contentText, "^（\\d+）")) {
					sb.AppendLine(Regex.Replace(contentText, "^（(\\d+)）", "    $1. ")).AppendLine();
				} else if (Regex.IsMatch(contentText, "\\[[\u4e00-\u9fa5]+\\]")) {
					var s1 = contentText.SubstringBeforeLast(']').SubstringAtfer('[');
					sb.Append("## ").AppendLine(s1.Trim()).AppendLine();
					var s2 = contentText.SubstringAtferLast(']');
					if (!string.IsNullOrWhiteSpace(s2))
						sb.AppendLine(s2.Trim()).AppendLine();
				} else {
					
					var c = contentText[0];
					if ("①②③④⑤⑥⑦⑧⑨⑩".IndexOf(c) != -1) {
						var startChar = "1";
						if (c == '②') {
							startChar = "2";
						} else if (c == '③') {
							startChar = "3";
							
						} else if (c == '④') {
							startChar = "4";
							
							
						} else if (c == '⑤') {
							startChar = "5";
							
							
						} else if (c == '⑥') {
							startChar = "6";
							
						} else if (c == '⑦') {
							startChar = "7";
							
						} else if (c == '⑧') {
							startChar = "8";
							
						} else if (c == '⑨') {
							startChar = "9";
							
						} else if (c == '⑩') {
							startChar = "10";
							
						}
						sb.Append(' ', 8).Append(startChar).Append(". ").AppendLine(contentText.Substring(1))
							.AppendLine();
						return;
					} else {
						sb.AppendLine(contentText).AppendLine();
					}
				}
				
				return;
			}
			if (className == "sectiontitle" || className == "listtitle") {
				sb.Append("# ").AppendLine(htmlNode.InnerText.SubstringAtfer('、').SubstringAtfer("　")).AppendLine();
				return;
			}
			if (tagName == "img") {
				var imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
				var imageFile = htmlNode.GetAttributeValue("src", "").SubstringAtferLast('/');
				var targetFileName = "fckmjzsc_" + imageFile;
				var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\妇产科门急诊手册(第2版) (实用门急诊丛书) - 沈宗姬等\images";

				try {
					File.Move(Path.Combine(dir, imageFile),
						Path.Combine(dir, targetFileName));
				} catch {
					
				}
				sb.AppendLine(string.Format(imageFormat, targetFileName))
					.AppendLine();
				return;
			}
			if (tagName == "body" || tagName == "div" || tagName == "a" || tagName == "p") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor2(sb,	child);
				}
				return;
			} else {
				if (!string.IsNullOrWhiteSpace(contentText))
					sb.AppendLine(contentText).AppendLine();
			}
			
		}
		private static void Visitor3(StringBuilder sb, HtmlNode htmlNode)
		{
			
			var className = htmlNode.GetAttributeValue("class", string.Empty);
			var tagName = htmlNode.Name;
			var contentText = HtmlEntity.DeEntitize(htmlNode.InnerText.Trim());
			
			if (tagName == "p") {
				if (className == "tp") {
					if (Regex.IsMatch(htmlNode.InnerHtml, "<img\\s+src=\"([^\"]+)\"[^>]*?>")) {
						var imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
						var imageFile = Regex.Match(htmlNode.InnerHtml, "<img\\s+src=\"([^\"]+)\"[^>]*?>").Groups[1].Value.SubstringAtferLast('/');
						var targetFileName = "nkmjzsc_" + imageFile;
						var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\内科门急诊手册(第2版) (实用门急诊丛书) - 吴爱勤等\images";

						try {
							File.Move(Path.Combine(dir, imageFile),
								Path.Combine(dir, targetFileName));
						} catch {
							
						}
						sb.Append(Regex.Replace(htmlNode.InnerHtml, "<img\\s+src=\"([^\"]+)\"[^>]*?>",
							string.Format(imageFormat, targetFileName)))
							.AppendLine()
							.AppendLine();
						
					} else {
						sb.AppendLine(contentText).AppendLine();
					}
				} else if (Regex.IsMatch(htmlNode.InnerHtml, "<img\\s+src=\"([^\"]+)\"[^>]*?>")) {
					var imageFormat = "<img alt=\"\" src=\"../static/pictures/{0}\">";
					var imageFile = Regex.Match(htmlNode.InnerHtml, "<img\\s+src=\"([^\"]+)\"[^>]*?>").Groups[1].Value.SubstringAtferLast('/');
					var targetFileName = "nkmjzsc_" + imageFile;
					var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\内科门急诊手册(第2版) (实用门急诊丛书) - 吴爱勤等\images";

					try {
						File.Move(Path.Combine(dir, imageFile),
							Path.Combine(dir, targetFileName));
					} catch {
						
					}
					sb.Append(Regex.Replace(htmlNode.InnerHtml, "<img\\s+src=\"([^\"]+)\"[^>]*?>",
						string.Format(imageFormat, targetFileName)))
						.AppendLine()
						.AppendLine();
					
				} else if (Regex.IsMatch(contentText, "^[\\d+]．")) {
					sb.AppendLine(Regex.Replace(contentText, "^([\\d+])．", "$1. ")).AppendLine();
				} else if (Regex.IsMatch(contentText, "^（\\d+）")) {
					sb.AppendLine(Regex.Replace(contentText, "^（(\\d+)）", "    $1. ")).AppendLine();
				} else if (Regex.IsMatch(contentText, "［[\u4e00-\u9fa5]+\\］")) {
					var s1 = contentText.SubstringBeforeLast('］').SubstringAtfer('［');
					sb.Append("## ").AppendLine(s1.Trim()).AppendLine();
					var s2 = contentText.SubstringAtferLast('］');
					if (!string.IsNullOrWhiteSpace(s2))
						sb.AppendLine(s2.Trim()).AppendLine();
					
				} else {
					sb.AppendLine(contentText).AppendLine();
				}
				return;
			}
			//.Append("（急诊）")
			if (tagName == "h2" || tagName == "h3") {
				sb.Append("# ").Append(Regex.Replace(htmlNode.InnerText.SubstringAtfer('、').SubstringAtfer("　"), "\\s+", ""))
					.AppendLine()
					.AppendLine();
				return;
			}
			
			if (tagName == "body" || tagName == "div" || tagName == "a" || tagName == "p") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor3(sb,	child);
				}
				return;
			} else {
				if (!string.IsNullOrWhiteSpace(contentText))
					sb.AppendLine(contentText).AppendLine();
			}
			
		}
		
		
		
		
		static string ParseList(string s)
		{
			const string p = "^(\\d+)．";
			if (Regex.IsMatch(s, p)) {
				return Regex.Replace(s, p, "$1. ");
			}
			return s;
		}
		static string ParseImage(HtmlNode n, string prefix, string imageFormat, string sourceDirectory)
		{
			//	var imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
			var imageFile = n.GetAttributeValue("src", "").SubstringAtferLast('/');
			var targetFileName = prefix + imageFile;
			var dir = sourceDirectory;
			
			try {
				File.Move(Path.Combine(dir, imageFile),
					Path.Combine(dir, targetFileName));
			} catch {
				
			}
			return string.Format(imageFormat, targetFileName);
		}
		static string TranslateChineseNumber(string s)
		{
			
			const string chinese = "一二三四五六七八九零";
			var numbers = "1234567890";
			foreach (var element in chinese) {
				
				s = s.Replace(element, numbers[chinese.IndexOf(element)]);
				
			}
			s = s.Replace("十", "10");
			return s;
		}
		static string TranslateChineseNumberSymbol(string s)
		{
			
			const string chinese = "①②③④⑤⑥⑦⑧⑨";
			var numbers = "123456789";
			foreach (var element in chinese) {
				
				s = s.Replace(element, numbers[chinese.IndexOf(element)]);
				
			}
			
			return s;
		}
		private static void Visitor4(StringBuilder sb, HtmlNode htmlNode)
		{
			const string imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
			const string imagePrefix = "icuscsc_";
			const string sourceImagesDirectory = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\ICU速查手册 (临床速查丛书) - 霍书花等\images";
			
			var className = htmlNode.GetClass();
			var tagName = htmlNode.Name;
			var contentText = htmlNode.GetInnerText();
			
			if (tagName == "h2") {
				var title =	htmlNode.GetInnerText().SubstringAtfer('节').RemoveWhiteSpace();
				sb.Append("# ").AppendLine(title).AppendLine();
				return;
			}
			
			if (tagName == "h4") {
				var title =	htmlNode.GetInnerText().RemoveWhiteSpace();
				sb.Append("### ").AppendLine(Regex.Replace(title, "^\\s*（([^）]+)）", new MatchEvaluator((m) => {
				                                                                                       	
					return TranslateChineseNumber(m.Groups[1].Value) + " ";
				}))).AppendLine();
				return;
			}
			if (tagName == "p") {
				if (htmlNode.Contains(i => i.Name == "b")) {
					var childs = htmlNode.ChildNodes;
					foreach (var element in childs) {
						if (element.NodeType == HtmlNodeType.Element && element.Name == "b") {
							sb.AppendLine(ParseList(element.GetInnerText())).AppendLine();
						} else {
							sb.AppendLine(element.GetInnerText()).AppendLine();
						}
					}
					
				} else if (htmlNode.Contains(i => i.Name == "span" && i.GetClass() == "bjcolor")) {
					sb.Append("## ").AppendLine(htmlNode.GetInnerText().RemoveWhiteSpace()).AppendLine();
				} else if (className == "right") {
				} else if (className == "tp" && htmlNode.ChildNodes.Count == 1) {
					
					sb.Append(ParseImage(htmlNode.ChildNodes.First(
						i => i.NodeType == HtmlNodeType.Element && i.Name == "img"), imagePrefix, imageFormat, sourceImagesDirectory)).AppendLine().AppendLine();
				} else {
					
					
					var childs = htmlNode.ChildNodes;
					foreach (var element in childs) {
						if (element.NodeType == HtmlNodeType.Element && element.Name == "img") {
							sb.Append(ParseImage(element, imagePrefix, imageFormat, sourceImagesDirectory));
						} else {
							var text = Regex.Replace(element.GetInnerText(), "^（(\\d+)）", "    $1. ");
							text = Regex.Replace(text, "^(\\d+)．", "$1. ");
							text = Regex.Replace(text, "^★\\s+", "* ");
							
							sb.Append(text);
						}
					}
					sb.AppendLine().AppendLine();
					
					
				}
				return;
			}
			
			if (tagName == "body" || tagName == "div" || tagName == "a" || tagName == "p") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor4(sb,	child);
				}
				return;
			} else {
				if (!string.IsNullOrWhiteSpace(contentText))
					sb.AppendLine(contentText).AppendLine();
			}
			
		}
		private static void ParseTable(StringBuilder sb, HtmlNode htmlNode)
		{
			var trs = htmlNode.SelectNodes(".//tr");
			bool first = true;
			sb.AppendLine()
				.Append("<div class=\"table-wrapper table-nowrap\">")
				.AppendLine();
			foreach (var tr in trs) {
				var tds = tr.SelectNodes(".//td");
				var list = new List<string>();
				foreach (var td in tds) {
					list.Add(td.GetInnerText());
				}
				
				sb.AppendLine("|" + string.Join("|", list) + "|");
				if (first) {
					for (int i = 0; i < list.Count - 1; i++) {
						sb.Append("|---");
					}
					sb.AppendLine("|");
					first = false;
				}
			}
			sb.AppendLine("</div>").AppendLine();
		}
		private static void Visitor5(StringBuilder sb, HtmlNode htmlNode)
		{
			
			
			if (htmlNode.NodeType == HtmlNodeType.Text) {
				
				var text = Regex.Replace(htmlNode.GetInnerText(), "^（(\\d+)）", "    $1. ");
				text = Regex.Replace(text, "^(\\d+)．", "$1. ");
				text = Regex.Replace(text, "^★\\s*", "* ");
				sb.Append(text);
				return;
			}
			const string imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
			const string imagePrefix = "icuscsc_";
			const string sourceImagesDirectory = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\ICU速查手册 (临床速查丛书) - 霍书花等\images";
			
			var tagName = htmlNode.Name;
			
			if (tagName == "img") {
				sb.Append(ParseImage(htmlNode, imagePrefix, imageFormat, sourceImagesDirectory));
				return;
			}
			if (tagName == "span") {
				if (htmlNode.GetClass() == "bjcolor")
					sb.AppendLine().AppendLine().Append("## ").AppendLine(htmlNode.GetInnerText().RemoveWhiteSpace()).AppendLine();
				else if (htmlNode.GetClass() == "font") {
					var text = Regex.Replace(htmlNode.GetInnerText(), "^(\\d+)．", "$1. ");
				
					sb.AppendLine().AppendLine().Append(text).AppendLine().AppendLine().Append("    ");
					return;
				} else
					sb.Append(htmlNode.GetInnerText());
				return;
				
			}
			switch (tagName) {
				case "h1":
					sb.AppendLine().AppendLine().Append("# ");
					break;
				case "h2":
					sb.AppendLine().AppendLine().Append("## ");
					break;
				case "h3":
					sb.AppendLine().AppendLine().Append("### ");
					break;
				case "h4":
					sb.AppendLine().AppendLine().Append("#### ");
					break;
				case "h5":
					sb.AppendLine().AppendLine().Append("##### ");
					break;
				case "p":
					if (htmlNode.GetClass() == "right")
						return;
					sb.AppendLine().AppendLine();
					break;
				case "b":
					sb.AppendLine().AppendLine();
					break;
			 
				case "table":
					ParseTable(sb, htmlNode);
					return;
			}
			
//			var className = htmlNode.GetClass();
//			var contentText = htmlNode.GetInnerText();
//
//			if (tagName == "h2") {
//				var title =	htmlNode.GetInnerText().SubstringAtfer('节').RemoveWhiteSpace();
//				sb.Append("# ").AppendLine(title).AppendLine();
//				return;
//			}
//
//			if (tagName == "h4") {
//				var title =	htmlNode.GetInnerText().RemoveWhiteSpace();
//				sb.Append("### ").AppendLine(Regex.Replace(title, "^\\s*（([^）]+)）", new MatchEvaluator((m) => {
//
//					return TranslateChineseNumber(m.Groups[1].Value) + " ";
//				}))).AppendLine();
//				return;
//			}
//			if (tagName == "p") {
//				if (htmlNode.Contains(i => i.Name == "b")) {
//					var childs = htmlNode.ChildNodes;
//					foreach (var element in childs) {
//						if (element.NodeType == HtmlNodeType.Element && element.Name == "b") {
//							sb.AppendLine(ParseList(element.GetInnerText())).AppendLine();
//						} else {
//							sb.AppendLine(element.GetInnerText()).AppendLine();
//						}
//					}
//
//				} else if (htmlNode.Contains(i => i.Name == "span" && i.GetClass() == "bjcolor")) {
//					sb.Append("## ").AppendLine(htmlNode.GetInnerText().RemoveWhiteSpace()).AppendLine();
//				} else if (className == "right") {
//				} else if (className == "tp" && htmlNode.ChildNodes.Count == 1) {
//
//					sb.Append(ParseImage(htmlNode.ChildNodes.First(
//						i => i.NodeType == HtmlNodeType.Element && i.Name == "img"), imagePrefix, imageFormat, sourceImagesDirectory)).AppendLine().AppendLine();
//				} else {
//
//
//					var childs = htmlNode.ChildNodes;
//					foreach (var element in childs) {
//						if (element.NodeType == HtmlNodeType.Element && element.Name == "img") {
//							sb.Append(ParseImage(element, imagePrefix, imageFormat, sourceImagesDirectory));
//						} else {
//							var text = Regex.Replace(element.GetInnerText(), "^（(\\d+)）", "    $1. ");
//							text = Regex.Replace(text, "^(\\d+)．", "$1. ");
//							text = Regex.Replace(text, "^★\\s+", "* ");
//
//							sb.Append(text);
//						}
//					}
//					sb.AppendLine().AppendLine();
//
//
//				}
//				return;
//			}
//
			if (tagName == "body"
			    || tagName == "div"
			    || tagName == "a"
			    || tagName == "p"
			    || tagName == "b"
			    || tagName == "font") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor5(sb,	child);
				}
				return;
			} else {
				var text = htmlNode.GetInnerText();
				if (!string.IsNullOrWhiteSpace(text))
					sb.AppendLine(text).AppendLine();
			}
			
		}
		private static string Format5(string str, StringBuilder sb)
		{
			var lines = str.Trim().Split('\n');
			sb.Clear();
			bool first = true;
			for (int i = 0, j = lines.Count(); i < j; i++) {
				string line = lines[i];
				if (Regex.IsMatch(line, "^#+ +")) {
					
					var s = line.SubstringBefore(' ');
					if (first) {
						s = s.Substring(1);
						first = false;
					}
					var e = line.SubstringAtfer(' ').SubstringAtfer('节').Trim().RemoveWhiteSpace().Trim();
					e = Regex.Replace(e, "^\\s*（([^）]+)）", new MatchEvaluator((m) => TranslateChineseNumber(m.Groups[1].Value) + " "));
					line = s + " " + e;
				}
				sb.Append(line).Append('\n');
				
				if (string.IsNullOrWhiteSpace(lines[i])) {
					while (i + 1 < j && string.IsNullOrWhiteSpace(lines[i + 1])) {
						i++;
					}
				}
			}
			return sb.ToString();
		}
		private static string Format6(string str, StringBuilder sb)
		{
			var lines = str.Trim().Split('\n');
			sb.Clear();
			bool first = true;
			for (int i = 0, j = lines.Count(); i < j; i++) {
				string line = lines[i];
				if (Regex.IsMatch(line, "^#+ +")) {
					
					var s = line.SubstringBefore(' ');
//					if(first){
//						s=s.Substring(1);
//						first=false;
//					}
					var e = line.SubstringAtfer(' ').SubstringAtfer('节').SubstringAtfer('、').Trim().RemoveWhiteSpace().Trim();
					e = Regex.Replace(e, "^\\s*（([^）]+)）", new MatchEvaluator((m) => TranslateChineseNumber(m.Groups[1].Value) + " "));
					line = s + " " + e;
				}
				sb.Append(line).Append('\n');
				
				if (string.IsNullOrWhiteSpace(lines[i])) {
					while (i + 1 < j && string.IsNullOrWhiteSpace(lines[i + 1])) {
						i++;
					}
				}
			}
			return sb.ToString();
		}
		private static void Visitor6(StringBuilder sb, HtmlNode htmlNode)
		{
			
			
			if (htmlNode.NodeType == HtmlNodeType.Text) {
				var text = htmlNode.GetInnerText();
				if (Regex.IsMatch(text, "^\\[\\w+]$")) {
					sb.Append("## ").Append(Regex.Match(text, "^\\[(\\w+)]$").Groups[1].Value).AppendLine().AppendLine();
					return;
				}
				
				text = Regex.Replace(text, "^（(\\d+)）", "    $1. ");
				text = Regex.Replace(text, "^(\\d+)[．\\.]", "$1. ");
				text = Regex.Replace(text, "^(\\d+)\\. (\\w+)\\s+", "$1. $2\r\n\r\n    ");
				
				text = Regex.Replace(text, "^★\\s*", "* ");
				sb.Append(text);
				return;
			}
			const string imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
			const string imagePrefix = "fckmjzsc_";
			const string sourceImagesDirectory = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\妇产科门急诊手册(第2版) (实用门急诊丛书) - 沈宗姬等\images";
			
			var tagName = htmlNode.Name;
			
			if (tagName == "img") {
				sb.Append(ParseImage(htmlNode, imagePrefix, imageFormat, sourceImagesDirectory));
				return;
			}
			if (tagName == "span") {
				if (htmlNode.GetClass() == "bjcolor")
					sb.AppendLine().AppendLine().Append("## ").AppendLine(htmlNode.GetInnerText().RemoveWhiteSpace()).AppendLine();
				else if (htmlNode.GetClass() == "font") {
					var text = Regex.Replace(htmlNode.GetInnerText(), "^(\\d+)．", "$1. ");
				
					sb.AppendLine().AppendLine().Append(text).AppendLine().AppendLine().Append("    ");
					return;
				} else
					sb.Append(htmlNode.GetInnerText());
				return;
				
			}
			switch (tagName) {
				case "h1":
					sb.AppendLine().AppendLine().Append("# ");
					break;
				case "h2":
					sb.AppendLine().AppendLine().Append("## ");
					break;
				case "h3":
					sb.AppendLine().AppendLine().Append("### ");
					break;
				case "h4":
					sb.AppendLine().AppendLine().Append("#### ");
					break;
				case "h5":
					sb.AppendLine().AppendLine().Append("##### ");
					break;
				case "p":
					if (htmlNode.GetClass() == "sectiontitle" || htmlNode.GetClass() == "listtitle") {
						sb.AppendLine().AppendLine().Append("# ").Append(htmlNode.GetInnerText()).AppendLine().AppendLine();
						return;
					}
					sb.AppendLine().AppendLine();
					break;
				case "b":
					sb.AppendLine().AppendLine();
					break;
			 
				case "table":
					ParseTable(sb, htmlNode);
					return;
			}
			

			if (tagName == "body"
			    || tagName == "div"
			    || tagName == "a"
			    || tagName == "p"
			    || tagName == "b"
			    || tagName == "font") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor6(sb,	child);
				}
				return;
			} else {
				var text = htmlNode.GetInnerText();
				if (!string.IsNullOrWhiteSpace(text))
					sb.AppendLine(text).AppendLine();
			}
			
		}
		private static string Format(string str, StringBuilder sb)
		{
			var lines = str.Trim().Split('\n');
			sb.Clear();
			bool first = true;
			for (int i = 0, j = lines.Count(); i < j; i++) {
				string line = lines[i];
				if (Regex.IsMatch(line, "^#+ +")) {
					
//					if(first){
//						s=s.Substring(1);
//						first=false;
//					}
					
					var e = line.Trim();
					e = Regex.Replace(e, "^\\s*（([^）]+)）", new MatchEvaluator((m) => TranslateChineseNumber(m.Groups[1].Value) + " "));
					line = e;
				}
				sb.Append(line).Append('\n');
				
				if (string.IsNullOrWhiteSpace(lines[i])) {
					while (i + 1 < j && string.IsNullOrWhiteSpace(lines[i + 1])) {
						i++;
					}
				}
			}
			return sb.ToString();
		}
		private static string FormatClear(string str)
		{
			var lines = str.Trim().Split('\n');
			StringBuilder sb = new StringBuilder();
			
			for (int i = 0, j = lines.Count(); i < j; i++) {
				string line = lines[i];
				
				sb.Append(line).Append('\n');
				
				if (string.IsNullOrWhiteSpace(lines[i])) {
					while (i + 1 < j && string.IsNullOrWhiteSpace(lines[i + 1])) {
						i++;
					}
				}
			}
			return sb.ToString();
		}
		private static void Visitor(StringBuilder sb, HtmlNode htmlNode)
		{
			
			
			if (htmlNode.NodeType == HtmlNodeType.Text) {
				var text = htmlNode.GetInnerText();
				if (Regex.IsMatch(text, "^\\[\\w+]$")) {
					sb.Append("## ").Append(Regex.Match(text, "^\\[(\\w+)]$").Groups[1].Value).AppendLine().AppendLine();
					return;
				}
				
				text = Regex.Replace(text, "^（(\\d+)）", "    $1. ");
				text = Regex.Replace(text, "^(\\d+)[．\\.]", "$1. ");
				text = Regex.Replace(text, "^(\\d+)\\. (\\w+)\\s+", "$1. $2\r\n\r\n    ");
				
				text = Regex.Replace(text, "^★\\s*", "* ");
				sb.Append(text);
				return;
			}
			const string imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
			const string imagePrefix = "qkysywsc_";
			const string sourceImagesDirectory = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生药物手册 (1500余种全科医生常用药物，4000多种药物商品名快速索引) - 邵柏\images";
			
			var tagName = htmlNode.Name;
			
			if (tagName == "img") {
				sb.Append(ParseImage(htmlNode, imagePrefix, imageFormat, sourceImagesDirectory));
				return;
			}
			if (tagName == "span") {
				if (htmlNode.GetClass() == "bjcolor")
					sb.AppendLine().AppendLine().Append("## ").AppendLine(htmlNode.GetInnerText().RemoveWhiteSpace()).AppendLine();
				else if (htmlNode.GetClass() == "font") {
					var text = Regex.Replace(htmlNode.GetInnerText(), "^(\\d+)．", "$1. ");
				
					sb.AppendLine().AppendLine().Append(text).AppendLine().AppendLine().Append("    ");
					return;
				} else
					sb.Append(htmlNode.GetInnerText());
				return;
				
			}
			switch (tagName) {
				case "h1":
					sb.AppendLine().AppendLine().Append("# ");
					break;
				case "h2":
					sb.AppendLine().AppendLine().Append("## ");
					break;
				case "h3":
					sb.AppendLine().AppendLine().Append("### ");
					break;
				case "h4":
					sb.AppendLine().AppendLine().Append("#### ");
					break;
				case "h5":
					sb.AppendLine().AppendLine().Append("##### ");
					break;
				case "h6":
					sb.AppendLine().AppendLine().Append("# ");
					break;
				case "p":
					if (htmlNode.GetClass() == "normaltext3" && Regex.IsMatch(htmlNode.GetInnerText(), "（[\\w\\s]+）")) {
						return;
					}
					sb.AppendLine().AppendLine();
					break;
				case "b":
					if (htmlNode.GetClass() == "calibre4") {
						sb.AppendLine().AppendLine().Append("## ").AppendLine(Regex.Match(htmlNode.GetInnerText(), "【(\\w+)】").Groups[1].Value.RemoveWhiteSpace()).AppendLine();
						return;
					}
					sb.AppendLine().AppendLine();
					break;
			 
				case "table":
					ParseTable(sb, htmlNode);
					return;
			}
			

			if (tagName == "body"
			    || tagName == "div"
			    || tagName == "a"
			    || tagName == "p"
			    || tagName == "b"
			    || tagName == "font") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor(sb,	child);
				}
				return;
			} else {
				var text = htmlNode.GetInnerText();
				if (!string.IsNullOrWhiteSpace(text))
					sb.AppendLine(text).AppendLine();
			}
			
		}
		public static void ConvertToMarkdowns(string path)
		{
			var doc = new HtmlDocument();
			var text = File.ReadAllText(path, new UTF8Encoding(false));
			
			doc.LoadHtml(text.Sub());
			
			var sb = new StringBuilder();
			Visitor(sb, doc.DocumentNode.SelectSingleNode("//body"));
			var str = sb.ToString().FromSub();
			
			str = Format(str, sb);
			
			
			
			File.WriteAllText(Path.ChangeExtension(path, ".txt"), str.Replace("\r", ""), new UTF8Encoding(false));
		}
		
		public static void FormatFile(string fileName, string dir)
		{
			var lines = File.ReadAllLines(fileName, new UTF8Encoding(false));
			string chapter = string.Empty;
			string paragraph = string.Empty;
			string total=string.Empty;
			List<string> results = new List<string>();
			for (int i = 0, j = lines.Length; i < j; i++) {
				string line = lines[i];
				if (Regex.IsMatch(line, "^#+\\s+第[一二三四五六七八九十]+[章篇]\\s+")) {
					total=Regex.Replace(line, "^#+\\s+第[一二三四五六七八九十]+[章篇]\\s+", "").Trim();
					continue;
				}
				if (Regex.IsMatch(line, "^#+\\s+第[一二三四五六七八九十]+节\\s+")) {
					paragraph = Regex.Replace(line, "^#+\\s+第[一二三四五六七八九十]+节\\s+", "").Trim();
					continue;
				}
				if (Regex.IsMatch(line, "^#+\\s+[一二三四五六七八九十]+、\\s*")) {
					chapter = Regex.Replace(line, "^#+\\s+[一二三四五六七八九十]+、\\s*", "").Trim();
					continue;
				}
				if (line.StartsWith("# ")) {
					if (results.Any(k => k.StartsWith("# "))) {
						results.Add(">====>");
						results.Add("id:");
						results.Add("toc:西药");
						var ls=new List<string>();
						if(!string.IsNullOrWhiteSpace(total))
							ls.Add(total);
						if(!string.IsNullOrWhiteSpace(paragraph))
							ls.Add(paragraph);
						if(!string.IsNullOrWhiteSpace(chapter))
							ls.Add(chapter);
						
						results.Add("tags:西药," + string.Join(",",ls).Replace("、",""));
				
						results.Add(">====>");
						
						
						bool marked = false;
						// results.Count - 1; h > -1; h--
						for (int h = 0, k = results.Count; h < k; h++) {
							string l=results[h];
							if (string.IsNullOrWhiteSpace(results[h]))
								continue;
//							if (results[h].IndexOf("※") != -1) {
//								marked = false;
//								break;
//							}
							if (Regex.IsMatch(results[h], "#+\\s+适应证")) {
								marked = true;
								continue;
							}
							if (marked && !string.IsNullOrWhiteSpace(l)) {
								
								if (Regex.IsMatch(l,"^\\s*\\d+\\. +")) {
									var f=l.Trim().Split(new char[]{'\n'},2);
									results[h]=f[0].SubstringBefore(' ')+" ※" + f[0].SubstringAtfer(' ').Trim()+ "※"
										+(f.Length>1?"\n\n"+f[1]:string.Empty);
								}else
								if (l.Contains('。')) {
									var start = l.SubstringBefore('。');
									var end = l.SubstringAtfer('。');
									results[h]="※" + start + "。※" + end;
								} else {
									results[h]="※" + l+ "※";
						
								}
								break;
							
							}
						}
				
//						if (marked && !string.IsNullOrWhiteSpace(line)) {
//							if (line.Contains('。')) {
//								var start = line.SubstringBefore('。');
//								var end = line.SubstringAtfer('。');
//								results.Add("※" + start + "。※" + end);
//							} else {
//								results.Add("※" + line + "※");
//						
//							}
//							marked = false;
//							continue;
//						}
						
						
						string str = string.Join("\n", results).Trim();
						var fn =	str.SubstringBefore('\n').SubstringAtfer(' ').Trim();
						
						File.WriteAllText(Path.Combine(dir, fn.GetValidateFileName() + ".txt"), str.RemoveRedundancyLines(), new UTF8Encoding(false));
						results.Clear();
					} 
					results.Add(line);
					
					continue;
				}
				
				if (Regex.IsMatch(line, "[①②③④⑤⑥⑦⑧⑨]")) {
					
					results.Add(Regex.Replace(line, "[①②③④⑤⑥⑦⑧⑨]", 
						new MatchEvaluator((m) => "\n\n" + TranslateChineseNumberSymbol(m.Value) + ". ")));
					continue;
				}
				
				results.Add(line);
			
			}
			if(results.Count>0){
						results.Add(">====>");
						results.Add("id:");
						results.Add("toc:西药");
						var ls=new List<string>();
						if(!string.IsNullOrWhiteSpace(total))
							ls.Add(total);
						if(!string.IsNullOrWhiteSpace(paragraph))
							ls.Add(paragraph);
						if(!string.IsNullOrWhiteSpace(chapter))
							ls.Add(chapter);
						
						results.Add("tags:西药," + string.Join(",",ls).Replace("、",""));
				
						results.Add(">====>");
						
						
						bool marked = false;
						// results.Count - 1; h > -1; h--
						for (int h = 0, k = results.Count; h < k; h++) {
							string l=results[h];
							if (string.IsNullOrWhiteSpace(results[h]))
								continue;
//							if (results[h].IndexOf("※") != -1) {
//								marked = false;
//								break;
//							}
							if (Regex.IsMatch(results[h], "#+\\s+适应证")) {
								marked = true;
								continue;
							}
							if (marked && !string.IsNullOrWhiteSpace(l)){
								
								if (Regex.IsMatch(l,"^\\s*\\d+\\. +")) {
									var f=l.Trim().Split(new char[]{'\n'},2);
									results[h]=f[0].SubstringBefore(' ')+" ※" + f[0].SubstringAtfer(' ').Trim()+ "※"
										+(f.Length>1?"\n\n"+f[1]:string.Empty);
								}else
								if (l.Contains('。')) {
									var start = l.SubstringBefore('。');
									var end = l.SubstringAtfer('。');
									results[h]="※" + start + "。※" + end;
								} else {
									results[h]="※" + l+ "※";
						
								}
								break;
							
							}
						}
			}}
	}
}