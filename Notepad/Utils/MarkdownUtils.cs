using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Notepad;

namespace Utils
{
	
	public static class MarkdownUtils
	{
		
		private static MarkdownPipeline _sMarkdownPipeline;

		public static MarkdownPipeline GetMarkdownPipeline()
		{
			return  _sMarkdownPipeline ?? (_sMarkdownPipeline = new MarkdownPipelineBuilder().UsePipeTables().UseCustomContainers().UseEmphasisExtras().UseAutoIdentifiers().UseAutoLinks().UseGenericAttributes().Build());		}
		public static string FormatMarkdown(this string value)
		{
			var retVal = Markdown.ToHtml(value, GetMarkdownPipeline());
			var index = 0;
//			var isFirst = true;
//			retVal = Regex.Replace(retVal, "<(h[123])[^>]*?>", new MatchEvaluator((m) => {
//				if (isFirst) {
//					isFirst = false;
//					return  string.Format("<{0}>", m.Groups[1].Value);
//				}
//				return string.Format("<{0} id=\"section-{1}\">", m.Groups[1].Value, (++index));
//
//			}));
			return retVal;
		}
		public static string FormatHeading(this string value)
		{
			if (value.TrimStart().StartsWith("#")) {
				return string.Format("#{0}", value.TrimStart());
			}
			return string.Format("# {0}", value.TrimStart());

		}

		public static string FormatHr(this string value)
		{
			return string.Format("{0}\r\n***", value);
		}
		public static string FormatStrong(this string value)
		{
			return string.Format(" **{0}** ", value.Trim());
		}
		public static string FormatEm(this string value)
		{
			return string.Format(" *{0}* ", value.Trim());
		}
		public static string FormatCode(this string value)
		{
			if (value.IsVacuum()||value.Contains('\n')) {
				return string.Format("\r\n```\r\n{0}\r\n```\r\n", HtmlAgilityPack.HtmlEntity.DeEntitize(value.Trim()));
			}
			return string.Format(" `{0}` ", HtmlAgilityPack.HtmlEntity.DeEntitize(value.Trim()));
		}
		public static string FormatImage(this string value, string src)
		{

			return string.Format("![{0}]({1})", value, src);

		}
		public static string FormatLink(this string value, string src)
		{

			return string.Format("[{0}]({1})", value, src);

		}
		public static string FormatUl(this string value)
		{
			var ls = value.ToLines();
			var sb = new StringBuilder();
			foreach (var item in ls) {

				sb.AppendLine(string.Format("- {0}", item.TrimStart(new char[] {
					' ',
					'-'
				})));

			}
			return sb.ToString();
		}
		public static string FormatHtml(this string value)
		{
			return Regex.Replace(value, "<([a-zA-Z0-9]+)\\s [^>]*?>", "<$1>");
		}
		public static string FormatTab(this string value)
		{
			if (value.IsVacuum())
				return "    ";
			return string.Join(Environment.NewLine, value.Split('\n').Select(i => "    " + i.TrimEnd()));
		}

		public static string[] FormatArticle(this string value)
		{
			var p = value.IndexOf("---\r\n");
			if (p > -1) {
				var start = p;
				p = value.IndexOf("---\r\n", p + 5);
				if (p > -1) {
					return new string[] {
						value.Substring(0, start),
						value.Substring(start, p + 5 - start),
						value.Substring(p + 5)
					};
                	
                    
				}
				var splited = value.Trim().Split(new char[] { '\n' }, 2);
				if (splited.Count() == 1)
					return new string[] {
						splited[0].TrimEnd(),
						string.Empty,
						splited[0].TrimEnd()
					};

				return new string[] {
					splited[0].TrimEnd(),
					string.Empty,
					splited[1].TrimEnd()
				};

			}
			return null;
		}
   
	}
}
