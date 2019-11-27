namespace  Notepad
{
	using System.Diagnostics;
	using System.Net;
	using System.Net.Http;
	using System.Collections.Generic;
	using System;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.IO;
	using Helpers;
	using Newtonsoft.Json.Linq;
	using Markdig;
	using Common;
	
	public static class OtherDelegate
	{
		private static HttpClient _client;
		
		
		private static MarkdownPipeline _sMarkdownPipeline;
		
		
	public static MarkdownPipeline GetMarkdownPipeline()
		{
			return  _sMarkdownPipeline ?? (_sMarkdownPipeline = new MarkdownPipelineBuilder().UsePipeTables()
			                               .UseCustomContainers()
			                               .UseEmphasisExtras()
			                               .UseAutoIdentifiers()
			                               .UseAutoLinks()
			                               .UseGenericAttributes().Build());
		}
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
		private static string ConvertToHtml(TextBox textBox)
		{
			var sb = new StringBuilder();
			sb.AppendLine("\u003C!doctype html\u003E");
			sb.AppendLine("\u003Chtml class=\u0022no-js\u0022 lang=\u0022zh-hans\u0022 dir=\u0022ltr\u0022\u003E");
			sb.AppendLine("");
			sb.AppendLine("\u003Chead\u003E");
			sb.AppendLine("    \u003Cmeta charset=\u0022utf-8\u0022\u003E");
			sb.AppendLine("    \u003Cmeta http-equiv=\u0022x-ua-compatible\u0022 content=\u0022ie=edge\u0022\u003E");
			sb.AppendLine("    \u003Ctitle\u003E");
			sb.AppendLine(HtmlAgilityPack.HtmlEntity.Entitize(textBox.Text.GetFirstReadable().TrimStart("# ".ToCharArray())));
			sb.AppendLine("    \u003C/title\u003E");
			sb.AppendLine("    \u003Cmeta name=\u0022viewport\u0022 content=\u0022width=device-width, initial-scale=1\u0022\u003E");
			sb.AppendLine("    \u003Clink rel=\u0022stylesheet\u0022 href=\u0022../stylesheets/markdown.css\u0022\u003E");
			sb.AppendLine("\u003C/head\u003E");
			sb.AppendLine("\u003Cbody\u003E");
			sb.AppendLine(textBox.Text.FormatMarkdown());

			sb.AppendLine("\u003C/body\u003E");
			sb.AppendLine("\u003C/html\u003E");
			return sb.ToString();
		}
		
		private async static Task<string> QueryFromBaidu(string q, string from, string to)
		{
			var url = "http://api.fanyi.baidu.com/api/trans/vip/translate";
			var appid = "20190312000276185";
			
			var parameters = new Dictionary<string,string>();
			parameters.Add("q", q);
			parameters.Add("from", from);
			parameters.Add("to", to);
			var salt = (DateTime.Now.Ticks / 1000).ToString();
			parameters.Add("salt", salt);
			parameters.Add("appid", appid);
			var securityKey = "sdK6QhtFE64Qm0ID_SjG";
			var sb = new StringBuilder();
			sb.Append(appid)
				.Append(q)
				.Append(salt)
				.Append(securityKey);
			parameters.Add("sign", sb.ToString().Md5());
			if (_client == null) {
				_client = Https.GetHttpClient();
			}
			var json = await _client.PostWithParameters(url, parameters);
			var array = json.ToJObject().GetArray<Newtonsoft.Json.Linq.JObject>("trans_result");
			sb.Clear();
			foreach (var element in array) {
				sb.AppendLine(element.GetString("dst"));
			}
			return sb.ToString();
		}
		private async static Task<string> QueryEnglish(string q)
		{
			var url = "https://translate.google.cn/translate_a/single?client=gtx&sl=auto&tl="
			          + "zh" + "&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" + WebUtility.UrlEncode(q);

			var res = await _client.GetAsync(url);

			return await res.Content.ReadAsStringAsync();
		}
		// , ShortcutKeys = Keys.F2
		[BindMenuItem(Control = "englishStripButton", Toolbar = "toolStrip5", NeedBinding = true)]
		public async static void Baidu(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			
			textBox.SelectLine(true);

			var val = textBox.SelectedText;
			if (string.IsNullOrWhiteSpace(val))
				return;
			var result = await QueryFromBaidu(val, "en", "zh");
			
			textBox.SelectedText = result + textBox.SelectedText;

		 
			
			Google(menuItem, mainForm);
		}
		private  async static void Google(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			
		
			textBox.SelectLine(true);

			var val = textBox.SelectedText;
			if (string.IsNullOrWhiteSpace(val))
				return;
			var json = await QueryEnglish(val);

			var obj = json.ToJObject();
			Newtonsoft.Json.Linq.JToken jtoken;

			if (!obj.TryGetValue("sentences", out jtoken))
				return;

			var sb = new StringBuilder();
			foreach (var item in jtoken) {
				//.AppendLine(item["orig"].ToString()
				sb.AppendLine(item["trans"].ToString());
			}
			textBox.SelectedText = Environment.NewLine + sb.ToString() + Environment.NewLine + textBox.SelectedText;

		}
			
		[BindMenuItem(Control = "程序", Toolbar = "toolStrip5")]
		public static void Run()
		{
			Process.Start(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
		}
		[BindMenuItem(Control = "预览", Toolbar = "toolStrip5",NeedBinding = true)]
		public static void Generate(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			
			var fileName = @"assets\htmls".GetExecutingPath().Combine(textBox.Text.GetFirstReadable().TrimStart('#').TrimStart().GetValidFileName('-') + ".htm");
			fileName.WriteAllText(ConvertToHtml(textBox));
			System.Diagnostics.Process.Start("chrome.exe", string.Format("\"{0}\"", fileName));

		}
		[BindMenuItem(Control = "预览", Toolbar = "toolStrip5" ,NeedBinding = true)]
		public static void GenerateHtml(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			if(string.IsNullOrWhiteSpace(textBox.Text))return;
			
			var fileName = @"assets\htmls".GetExecutingPath().Combine(textBox.Text.GetFirstReadable()
			                                                          .TrimStart('#').TrimStart().GetValidFileName('-') + ".htm");
			fileName.WriteAllText(ConvertToHtml(textBox));
			System.Diagnostics.Process.Start("chrome.exe", string.Format("\"{0}\"", fileName));

		}
		[BindMenuItem(Control = "chineseStripButton", Toolbar = "toolStrip5", NeedBinding = true)]
	
		public static void TranslateChninese(ToolStripItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			textBox.SelectLine(true);
			var val = textBox.SelectedText;
			if (string.IsNullOrWhiteSpace(val))
				return;
			var json = TranslateUtils.GetInstance().QueryChinese(val);
			var obj = JObject.Parse(json);
			JToken jtoken;
			if (!obj.TryGetValue("sentences", out jtoken))
				return;
			var sb = new StringBuilder();
			foreach (var item in jtoken) {
				sb.AppendLine(item["trans"].ToString()).AppendLine(item["orig"].ToString());
			}
			textBox.SelectedText = sb.ToString();
		}
		[BindMenuItem(Control = "pageEndStripButton", Toolbar = "toolStrip5", NeedBinding = true)]
	
		public static void ScrollPageEnd(ToolStripItem menuItem, MainForm mainForm)
		{
			mainForm.textBox.SelectionStart = mainForm.textBox.Text.Length - 1;
			mainForm.textBox.ScrollToCaret();
		}
		[BindMenuItem(Control = "pageStartStripButton", Toolbar = "toolStrip5", NeedBinding = true)]
	
		public static void ScrollPageStart(ToolStripItem menuItem, MainForm mainForm)
		{
			mainForm.textBox.SelectionStart = 0;
			mainForm.textBox.ScrollToCaret();
		}
		[BindMenuItem(Control = "newSplitButton", Toolbar = "toolStrip5", NeedBinding = true)]
	
		public static void New(ToolStripItem menuItem, MainForm mainForm)
		{
			mainForm._article = null;
			var stringBuilder = new System.Text.StringBuilder(58);
			stringBuilder.AppendLine(@"# ");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"## Properties");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"## Methods");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"## Events");

			mainForm.textBox.Text = stringBuilder.ToString();
		
			//textBox.Text = textBox.Text.GetFirstReadable().SubstringBefore(':') + ": " + Environment.NewLine + Environment.NewLine + "```\r\n\r\n```\r\n";
			mainForm.Text = string.Empty;
		}
		
		
		[BindMenuItem(Name = "保存", Control = "saveStripSplitButton", Toolbar = "toolStrip5", NeedBinding = true, ShortcutKeys = Keys.F1)]
	
		public static void Save(ToolStripItem menuItem, MainForm mainForm)
		{
			var _article = mainForm._article;
			var textBox = mainForm.textBox;
			
			if (string.IsNullOrWhiteSpace(textBox.Text))
				return;
			if (_article == null) {
				var title = textBox.Text.GetFirstReadable().TrimStart(new char[] {
					' ',
					'#'
				});
				_article = new Utils.Article {
					Title = title,
					Content = textBox.Text,
					CreateAt = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
					UpdateAt =(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
				};
				Utils.DatabaseUtils.GetInstance().Insert(_article);
				_article = Utils.DatabaseUtils.GetInstance().GetArticle(title);
				mainForm._article=_article;
				mainForm.UpdateList();
				mainForm.Text = title;
			} else {
				var title = textBox.Text.GetFirstReadable().TrimStart(new char[] {
					' ',
					'#'
				});
				var updateList = false;
				if (_article.Title != title) {
					updateList = true;
				}
				_article.Title = title;
				_article.Content = textBox.Text;
				_article.UpdateAt = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;;
				Utils.DatabaseUtils.GetInstance().Update(_article);
				if (updateList) {
					mainForm.UpdateList();
				}
				mainForm.Text = title;
			}
		}
		
		[BindMenuItem(Name = "Markdown", Control = "saveStripSplitButton", Toolbar = "toolStrip5", NeedBinding = true)]
	
		public static void SaveMarkdowns(ToolStripItem menuItem, MainForm mainForm)
		{
			var targetDirectory = "MarkDown".GetDesktopPath();
			targetDirectory.CreateDirectoryIfNotExists();
			var sql = Utils.DatabaseUtils.GetInstance();
			var contentList =	sql.GetTitleContentList();
			foreach (var c in contentList) {
				
				var fileName = c.Title.Trim().TrimStart('#').Trim().GetValidFileName() + ".md";
				Path.Combine(targetDirectory, fileName).WriteAllText(c.Content);
				
			}
		}
	}
}
/*
 
		void 导出全部ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var directories = Directory.GetFiles(_dataPath, "*").Where(i => i.GetExtension() == ".dat");
			//var baseDirectory = "assets".GetExecutingPath().Combine("exports");
			var baseDirectory = "md".GetExecutingPath();
			baseDirectory.CreateDirectoryIfNotExists();
			foreach (var element in directories) {
				var sql =	DatabaseUtils.GetInstance(element);
				var contentList =	sql.GetTitleContentList();
				var targetDirectory = Path.Combine(baseDirectory, element.GetFileNameWithoutExtension());
				targetDirectory.CreateDirectoryIfNotExists();
				foreach (var c in contentList) {
					// Path.GetFileNameWithoutExtension(element) + " - " +
					var tf = targetDirectory.Combine(c.Title.GetValidFileName() + ".md");
					//tf =	tf.GetUniqueFileName();
//					StringBuilder sb = new StringBuilder();
//					sb.AppendLine("\u003C!doctype html\u003E");
//					sb.AppendLine("\u003Chtml class=\u0022no-js\u0022 lang=\u0022zh-hans\u0022 dir=\u0022ltr\u0022\u003E");
//					sb.AppendLine("");
//					sb.AppendLine("\u003Chead\u003E");
//					sb.AppendLine("    \u003Cmeta charset=\u0022utf-8\u0022\u003E");
//					sb.AppendLine("    \u003Cmeta http-equiv=\u0022x-ua-compatible\u0022 content=\u0022ie=edge\u0022\u003E");
//					sb.AppendLine("    \u003Ctitle\u003E");
//					sb.AppendLine(HtmlAgilityPack.HtmlEntity.Entitize(c.Title));
//					sb.AppendLine("    \u003C/title\u003E");
//					sb.AppendLine("    \u003Cmeta name=\u0022viewport\u0022 content=\u0022width=device-width, initial-scale=1\u0022\u003E");
//					sb.AppendLine("    \u003Clink rel=\u0022stylesheet\u0022 href=\u0022../stylesheets/markdown.css\u0022\u003E");
//					sb.AppendLine("\u003C/head\u003E");
//					sb.AppendLine("\u003Cbody\u003E");
//					sb.AppendLine(c.Content.FormatMarkdown());
//
//					sb.AppendLine("\u003C/body\u003E");
//					sb.AppendLine("\u003C/html\u003E");
//					tf.WriteAllText(sb.ToString());
					tf.WriteAllText(c.Content);
				}
			}
		}
	void 导入代码文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormUtils.OnClipboardDirectory((v) => {
				var str = "";
				var files = Directory.GetFiles(v, "*").Where(i => Regex.IsMatch(i, "\\.(?:c|h|txt)$"));
				foreach (var element in files) {
					str +=	string.Format("```\r\n\r\n{0}\r\n\r\n```\r\n\r\n", element.ReadAllText().Replace("`", "\u0060"));
				}
				textBox.SelectedText += str;
			});
		}
 * 	void 导入SRT文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText();
			if (!Directory.Exists(dir))
				return;
			var files = Directory.GetFiles(dir, "*.srt");
			var regex = new Regex("(^[0-9]+$)|(^[0-9]+[^a-zA-Z]*?[0-9]+$)");
			foreach (var f  in files) {
				var srtContents = f.ReadAllText();
				var lines = srtContents.ToLines();
				var sb = new StringBuilder();
				foreach (var element in lines) {
					if (Regex.IsMatch(element, "(^[0-9]+$)|(^[0-9]+[^a-zA-Z]*?[0-9]+$)"))
						continue;
					sb.Append(element.Trim() + " ");
				}
				var content = "# " + f.GetFileName() + "\r\n\r\n"
				              +
				              Regex.Replace(sb.ToString(), "[\\.]+", ".\r\n\r\n");
				var article = new Article {
					Title = f.GetFileName(),
					Content = content,
					CreateAt = DateTime.UtcNow,
					UpdateAt = DateTime.UtcNow,
				};
				DatabaseUtils.GetInstance().Insert(article);
			}
		}
		
void 导出当前数据库ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var targetDirectory = comboBox.Text.GetFileNameWithoutExtension().GetDesktopPath();
			targetDirectory.CreateDirectoryIfNotExists();
			File.Copy("assets".GetExecutingPath().Combine("stylesheets").Combine("markdown.css"), targetDirectory.Combine("markdown.css"));
			var sql =	DatabaseUtils.GetInstance();
			var contentList =	sql.GetTitleContentList();
			foreach (var c in contentList) {
				var tf = targetDirectory.Combine(c.Title.GetValidFileName() + ".html");
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\u003C!doctype html\u003E");
				sb.AppendLine("\u003Chtml class=\u0022no-js\u0022 lang=\u0022zh-hans\u0022 dir=\u0022ltr\u0022\u003E");
				sb.AppendLine("");
				sb.AppendLine("\u003Chead\u003E");
				sb.AppendLine("    \u003Cmeta charset=\u0022utf-8\u0022\u003E");
				sb.AppendLine("    \u003Cmeta http-equiv=\u0022x-ua-compatible\u0022 content=\u0022ie=edge\u0022\u003E");
				sb.AppendLine("    \u003Ctitle\u003E");
				sb.AppendLine(HtmlAgilityPack.HtmlEntity.Entitize(c.Title));
				sb.AppendLine("    \u003C/title\u003E");
				sb.AppendLine("    \u003Cmeta name=\u0022viewport\u0022 content=\u0022width=device-width, initial-scale=1\u0022\u003E");
				sb.AppendLine("    \u003Clink rel=\u0022stylesheet\u0022 href=\u0022markdown.css\u0022\u003E");
				sb.AppendLine("\u003C/head\u003E");
				sb.AppendLine("\u003Cbody\u003E");
				sb.AppendLine(c.Content.FormatMarkdown());
				sb.AppendLine("\u003C/body\u003E");
				sb.AppendLine("\u003C/html\u003E");
				tf.WriteAllText(sb.ToString());
			}
		}
 */