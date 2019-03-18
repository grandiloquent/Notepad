namespace  Notepad
{
	using System.Net.Http;
	using System.Collections.Generic;
	using System;
	using System.Windows.Forms;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Text;
	using Common;
	using System.IO;
	
	public static class GoDelegate
	{
		private static string ConvertRequestHeaders(string value)
		{
			
			var lines = value.Split(new char[]{ '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(i => i.Split(new char[]{ ':' }, 2));
			
			var list1 = new List<string>();
			var list2 = new List<string>();
			
			
			var p2 = "w.Header().Set(\"{0}\",\"{1}\");";
			
			foreach (var element in lines) {
				list2.Add(string.Format(p2, element.First().Trim(), element.Last().Trim()));
				
			}
			return  list2.ConcatenateLines();
			
		}
		private static string MinifyHtml(string value)
		{
			// https://github.com/JadeX/JadeX.HtmlMinifier/blob/c6617d602eedc31d90a8637cbb7d32006f48605c/Filters/MinifyHtmlFilter.cs
			var m = new WebMarkupMin.Core.HtmlMinifier(
				        new WebMarkupMin.Core.HtmlMinificationSettings {
					AttributeQuotesRemovalMode = WebMarkupMin.Core.HtmlAttributeQuotesRemovalMode.KeepQuotes,
					RemoveTagsWithoutContent = false,
					EmptyTagRenderMode = WebMarkupMin.Core.HtmlEmptyTagRenderMode.Slash,
				}
			        );
			return m.Minify(value).MinifiedContent;

		}
		private static String GenerateTemplateInBytes(string v)
		{
			v = MinifyHtml(v);
			
			var list1 = new List<string>();
			
			var array = Regex.Split(v, "{{[^}]*?}}");
			var matches = Regex.Matches(v, "{{([^}]*?)}}").Cast<Match>().Select(i => i.Groups[1].Value).ToArray();
			var index = 0;
			var list2 = new List<string>();
			var dic = new Dictionary<string,int>();
			var list3 = new List<string>();
			
			list1.Add(":[");
			foreach (var item in array) {
				var bytes = new UTF8Encoding(false).GetBytes(item);
//				
//				var bytes = new UTF8Encoding(false).GetBytes(item).Select(i => {
//					if (i > 127) {
//						return "(byte)" + i;
//					} else {
//						return i.ToString();
//					}
//				}).ToArray();

				var name = index < matches.Length ? matches[index] : "tail";
				if (dic.ContainsKey(name)) {
					dic[name] = dic[name] + 1;
				} else {
					dic[name] = 1;
				}
				name = name + dic[name];
				list1.Add(string.Format("[{0}],", string.Join(",", bytes)));
				//list1.Add(string.Format("/*{0} {1}*/ {{{2}}},", index, name, string.Join(",", bytes)));
				
				//list1.Add(string.Format("var {0} = []byte{{{1}}}",name,string.Join(",", bytes)));
				// _, _ = 
				index++;
				
				list2.Add(string.Format("w.Write(VideoPageBuffer[{0}])", index - 1));
				//list2.Add(string.Format("b = append(b, {0}...)",name));
				list3.Add(string.Format("// {0} {1}", index - 1, name));
				
			
			
			}
			
			list1.Add("],");
			return list1.Concat(list2).ConcatenateLines() + string.Join(",", list3);
		}
		
		
		[BindMenuItem(Name = "移除文件中空白 (目录)", Toolbar = "toolStrip1", SplitButton = "goButton")]
		
		public static void RemoveWhiteSpace()
		{
		
			Forms.OnClipboardDirectory(dir => {
			                           	var files=Directory.GetFiles(dir,"*.go");
			                           	foreach (var element in files) {
			                           		element.WriteAllText(element.ReadAllText().RemoveWhiteSpaceLines());
			                           	}
			});
			
		}
		
		
		[BindMenuItem(Name = "HTML 数组模板", Toolbar = "toolStrip1", SplitButton = "goButton")]
		
		public static void GenerateHTMLArrayTemplate()
		{
		
			Forms.OnClipboardString(GenerateTemplateInBytes);
			
		}
		[BindMenuItem(Name = "格式化 Chrome 请求头 (文本)", Toolbar = "toolStrip1", SplitButton = "goButton")]
		
		public static void ConvertRequestHeaders()
		{
		
			Forms.OnClipboardString(ConvertRequestHeaders);
			
		}
		[BindMenuItem(Name = "排序代码 (文本)", Toolbar = "toolStrip1", SplitButton = "goButton")]
		public static void SortCode()
		{
		
			Forms.OnClipboardString(str => {
				var blocks = str.Replace("interface{}", "((()))").ToBlocks()
			                        		.OrderBy(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '));
			                        	
				return blocks.ConcatenateLines().Replace("((()))", "interface{}");
			                        	
			});
			
		}
	}
}