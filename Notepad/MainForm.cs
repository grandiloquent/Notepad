
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Notepad
{
	

	public partial class MainForm : Form
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
		private string _defaultDatabase;
		private readonly string _dataPath;
		private Article _article;
		int _idF8 = -1;
		int _idF9 = -1;
		int _idF10 = -1;
		// 1 -> go
		int _runType = 1;
		public MainForm()
		{
			InitializeComponent();
			_dataPath = "datas".GetCommandPath();
			_dataPath.CreateDirectoryIfNotExists();
			_defaultDatabase = _dataPath.Combine("db.dat");

			if (!_defaultDatabase.FileExists())
				HelperSqlite.GetInstance(_defaultDatabase);

			comboBox.Items.AddRange(_dataPath.GetFiles("*.dat").Select(i => i.GetFileName()).ToArray());
		}
		void 逃逸路径ToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Clipboard.SetText(Clipboard.GetText().Trim().Replace("\\", "\\\\"));
			} catch {
				
			}
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			javaMethodParameterToolStripMenuItem.Click += (s, o) => Helper.KotlinFormatJavaMethodParameters();
			打开ToolStripMenuItem.Click += (s, o) => Helper.OpenLink(textBox);
			kotlinExtractParametersToolStripMenuItem.Click += (s, o) => Helper.KotlinExtractParameters();
			cSplitButton.ButtonClick += (s, o) => Helper.GenerateGccCommand();
			格式化C代码ToolStripMenuItem.Click += (s, o) => Helper.CFormat();
			删除Aria2文件ToolStripMenuItem.Click += (s, o) => Helper.RemoveAria2File();
			清理HTMLSToolStripMenuItem.Click += (s, o) => Helper.CleanHtmls();
			cplusSplitButton.ButtonClick += (s, o) => Helper.GenerateGPlusPlusCommand();
			数字序列数组ToolStripMenuItem.Click += (s, o) => Helper.GenerateDigit();
			压缩子目录ToolStripMenuItem.Click += (s, o) => Helper.ZipDirectories();
			//C代码段VSCToolStripMenuItem
		 
			排序代码段ToolStripMenuItem.Click += (s, o) => Helper.SortVSCSnippets();
			if ("settings.txt".GetCommandPath().FileExists()) {
				var value = "settings.txt".GetCommandPath().ReadAllText();
				if (value.IsReadable()) {
					comboBox.SelectedItem = value.Trim();
				}
			}
		}
		private void UpdateList()
		{

			listBox.Items.Clear();
			listBox.Items.AddRange(HelperSqlite.GetInstance().GetTitleList().ToArray());
		}
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			_defaultDatabase = _dataPath.Combine(comboBox.Text);
			HelperSqlite.GetInstance(_defaultDatabase);
			UpdateList();
		}
		void 复制ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (textBox.SelectedText.IsVacuum()) {
				textBox.SelectLine(true);
			}
			textBox.Copy();
			
		}
		void 剪切ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (textBox.SelectedText.IsVacuum()) {
				textBox.SelectLine(true);
			}
			textBox.Cut();
		}
		void H1ButtonClick(object sender, EventArgs e)
		{
			var start = textBox.SelectionStart;

			while (start - 1 > -1 && textBox.Text[start - 1] != '\n') {
				start--;
			}
			var end = start;
			while (end + 1 < textBox.Text.Length && textBox.Text[end + 1] == '#') {
				end++;
			}
			textBox.SelectionStart = start;
			textBox.SelectionLength = end - start;
			textBox.SelectedText = "# ";
		}
		void H2ButtonClick(object sender, EventArgs e)
		{
	
			var start = textBox.SelectionStart;

			while (start - 1 > -1 && textBox.Text[start - 1] != '\n') {
				start--;
			}
			var end = start;
			while (end + 1 < textBox.Text.Length && textBox.Text[end + 1] == '#') {
				end++;
			}
			textBox.SelectionStart = start;
			textBox.SelectionLength = end - start;
			textBox.SelectedText = "## ";
		}
		void FormatButtonClick(object sender, EventArgs e)
		{
			textBox.Format();
		}
		void HtmlsButtonClick(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
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
			var fileName = @"assets\htmls".GetCommandPath().Combine(textBox.Text.GetFirstReadable().TrimStart('#').TrimStart().GetValidFileName('-') + ".htm");
			fileName.WriteAllText(sb.ToString());

			System.Diagnostics.Process.Start("chrome.exe", string.Format("\"{0}\"", fileName));
		}
		void IButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = textBox.SelectedText.FormatEm();
		}
		void 全选ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectAll();
	
		}
		void TitleButtonClick(object sender, EventArgs e)
		{
			var start = textBox.SelectionStart;

			while (start - 1 > -1 && textBox.Text[start - 1] != '\n') {
				start--;
			}
			textBox.SelectionStart = start;
			textBox.SelectionLength = 0;
			if (textBox.Text[start] == '#')
				textBox.SelectedText = "#";
			else
				textBox.SelectedText = "# ";
		}
		void ListBoxDoubleClick(object sender, EventArgs e)
		{
			if (listBox.SelectedIndex == -1)
				return;
			var title = listBox.SelectedItem.ToString();
			_article = HelperSqlite.GetInstance().GetArticle(title);

			this.Text = _article.Title;
			textBox.Text = _article.Content;
		}
		void 保存ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (textBox.Text.IsVacuum())
				return;
			if (_article == null) {
				var title = textBox.Text.GetFirstReadable().TrimStart(new char[] {
					' ',
					'#'
				});
				_article = new Article {
					Title = title,
					Content = textBox.Text,
					CreateAt = DateTime.UtcNow,
					UpdateAt = DateTime.UtcNow,
				};
				HelperSqlite.GetInstance().Insert(_article);
				_article = HelperSqlite.GetInstance().GetArticle(title);
				UpdateList();
				this.Text = title;
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
				_article.UpdateAt = DateTime.UtcNow;

				HelperSqlite.GetInstance().Update(_article);
				if (updateList) {
					UpdateList();

				}
				this.Text = title;
			}
		}
		void TextBoxTextChanged(object sender, EventArgs e)
		{
			if (!this.Text.EndsWith("*"))
				this.Text += " *";
		}
		void AppButtonButtonClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("".GetCommandPath());
		}
		void 粘贴ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.Paste();
	
		}
		void NewButtonClick(object sender, EventArgs e)
		{
			_article = null;
			 
			textBox.Text = string.Empty;

			this.Text = string.Empty;
		}
		void ChineseButtonClick(object sender, EventArgs e)
		{
			textBox.SelectLine(true);

			var val = textBox.SelectedText;
			if (val.IsVacuum())
				return;
			var json = HelperTranslator.GetInstance().QueryChinese(val);

			var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
			Newtonsoft.Json.Linq.JToken jtoken;

			if (!obj.TryGetValue("sentences", out jtoken))
				return;

			var sb = new StringBuilder();
			foreach (var item in jtoken) {
				sb.AppendLine(item["trans"].ToString()).AppendLine(item["orig"].ToString());
			}
			textBox.SelectedText = sb.ToString();
		}
		void EnglishButtonClick(object sender, EventArgs e)
		{
	
			textBox.SelectLine(true);

			var val = textBox.SelectedText;
			if (val.IsVacuum())
				return;
			var json = HelperTranslator.GetInstance().QueryEnglish(val);

			var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
			Newtonsoft.Json.Linq.JToken jtoken;

			if (!obj.TryGetValue("sentences", out jtoken))
				return;

			var sb = new StringBuilder();
			foreach (var item in jtoken) {
				sb.AppendLine(item["trans"].ToString()).AppendLine(item["orig"].ToString());
			}
			textBox.SelectedText = sb.ToString();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (_idF8 != -1) {
				UnregisterHotKey(this.Handle, _idF8);
			}
			if (_idF9 != -1) {
				UnregisterHotKey(this.Handle, _idF9);
			}
			if (_idF10 != -1) {
				UnregisterHotKey(this.Handle, _idF10);
			}
			"settings.txt".GetCommandPath().WriteAllText(comboBox.Text);
		}
		void 删除ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (listBox.SelectedItems.Count > 0) {
				foreach (var element in listBox.SelectedItems) {
					HelperSqlite.GetInstance().Delete(element.ToString());
				}
				UpdateList();
				                                  
			}
		}
		void 查找ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var search = textBox.SelectedText;
			if (search.IsVacuum()) {
				search = findBox.Text;
			}
			if (search.IsVacuum())
				return;
			var p = textBox.Text.IndexOf(search, textBox.SelectionStart + textBox.SelectionLength);
			if (p == -1) {
				p = textBox.Text.IndexOf(search);
			}
			if (p > -1) {
				textBox.SelectionStart = p;
				textBox.SelectionLength = search.Length;
				textBox.ScrollToCaret();
			}
		}
		void CodeButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = HelperMarkdownFormat.FormatCode(textBox.SelectedText);
		}
		void 排序ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Join(Environment.NewLine, textBox.SelectedText.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).Distinct().OrderBy(i => i));
		}
		void 格式化C代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		void 保留正则表达式ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var ls = Regex.Matches(textBox.Text, findBox.Text).Cast<Match>().Select(i => i.Value).Distinct();
			var j = replaceBox.Text.Trim();
			if (j.IsVacuum())
				j = ",";
			textBox.Text = string.Join(j, ls);
		}
		void DToolStripMenuItemClick(object sender, EventArgs e)
		{
			var ls = textBox.Text.Split(',').Select(i => i.Trim());
	
		
			textBox.Text = "printf(\"" + string.Join(",\\n ", ls.Select(i => i + ":%d")) + "\"," + string.Join(",", ls) + ");";
		}
		void PathButtonButtonClick(object sender, EventArgs e)
		{
			try {
				Clipboard.SetText(Clipboard.GetText().Trim().Replace('\\', '/'));
			} catch {
				
			}
		}
		void GccToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardDirectory((path) => {
				var cf = Directory.GetFiles(path, "*.c").Select(i => i.GetFileName());
				var sb = new StringBuilder();
				sb.AppendFormat("gcc -c {0} && ", string.Join(" ", cf.Select(i => "\"" + i + "\"")));
				var of = cf.Select(i => i.ChangeExtension(".o"));
				sb.AppendFormat("gcc -o \"{1}\" {0} && \"{1}\"", string.Join(" ", of.Select(i => "\"" + i + "\"")), path.GetFileName());
				Clipboard.SetText(sb.ToString());
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
		void 导入ToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardDirectory((p) => {
				var files = Directory.GetFiles(p, "*", SearchOption.AllDirectories).Where(i => Regex.IsMatch(i, "\\.(?:c|h|cpp|java|txt)$") || i.GetExtension().IsVacuum());
				var j = "\u0060";
				var sb = new StringBuilder();
				sb.AppendLine("# " + p.GetFileName()).AppendLine();
				foreach (var element in files) {
					var str = element.ReadAllText().Trim();
					while (str.StartsWith("/*")) {
						str = str.SubstringAfter("*/").Trim();
					}
					sb.AppendLine("## " + element.GetFileName())
						.AppendLine()
						.AppendLine()
						.AppendLine("```")
						.AppendLine()
						.AppendLine(Regex.Replace(str.Replace("`", j), "[\r\n]+", "\r\n"))
						.AppendLine("```")
						.AppendLine();
				}
				textBox.Text = sb.ToString();
			});
		}
		void LinkButtonButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format("[{0}]({1})", textBox.SelectedText.Trim(), Clipboard.GetText().Trim());
		}
		void UlButtonButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Join(Environment.NewLine, textBox.SelectedText.Trim().Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => "- " + i));
		}
		void 粘贴代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format("```\r\n\r\n{0}\r\n\r\n```\r\n\r\n", Clipboard.GetText().Trim().Replace("`", "\u0060"));
		}
		void 导入目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardDirectory((p) => {
				var files = Directory.GetFiles(p, "*", SearchOption.AllDirectories).Where(i => Regex.IsMatch(i, "\\.(?:c|h|cpp|java|txt)$") || i.GetExtension().IsVacuum());
				var j = "\u0060";
				
				foreach (var element in files) {
					var title = p.GetFileName() + ": " + element.GetFileName();
					var sb = new StringBuilder();
					sb.AppendLine("# " + title).AppendLine();
					var str = element.ReadAllText().Trim();
					while (str.StartsWith("/*")) {
						str = str.SubstringAfter("*/").Trim();
					}
					sb 
						.AppendLine()
						.AppendLine("```")
						.AppendLine()
						.AppendLine(Regex.Replace(str.Replace("`", j), "[\r\n]+", "\r\n"))
						.AppendLine("```")
						.AppendLine();
					
					var article = new Article {
						Title = title,
						Content = sb.ToString(),
						CreateAt = DateTime.UtcNow,
						UpdateAt = DateTime.UtcNow,
					};
					try {
						HelperSqlite.GetInstance().Insert(article);
					} catch {
						
					}
				}
				UpdateList();
				
			});
		}
		
		void 字符串到数组GBKToolStripMenuItemClick(object sender, EventArgs e)
		{
			var encoding = Encoding.GetEncoding("gbk");
			var bytes =	encoding.GetBytes(Clipboard.GetText().Trim());
			textBox.Text = string.Join(",", bytes);
		}
		void 字符串到数组UTF8ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			var encoding = new UTF8Encoding(false);
			var bytes =	encoding.GetBytes(Clipboard.GetText().Trim());
			textBox.Text = string.Join(",", bytes);
		}
		void 数组到字符串GBKToolStripMenuItemClick(object sender, EventArgs e)
		{
			 
			var bytes = Regex.Matches(Clipboard.GetText(), "[0-9]+").Cast<Match>().Select(i => byte.Parse(i.Value)).ToArray();
			textBox.Text = Encoding.GetEncoding("gbk").GetString(bytes);
		}
		void 替换成换行符ToolStripMenuItemClick(object sender, EventArgs e)
		{
			// Envoriment.NewLine
			textBox.Text = Regex.Replace(textBox.Text, findBox.Text, Environment.NewLine);
		}
		void 字符列到字符常量ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var str = Clipboard.GetText().Trim().Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());
			var ls = new List<string>();
			foreach (var element in str) {
				ls.Add(string.Format("const val {0}=\"{1}\"", element.ToUpper(), element.ToLower()));
			}
			Clipboard.SetText(string.Join("\n", ls));
			
		}
		void JavaStaticToConstToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardString(Helpers.FormatJavaStaticFinalFieldToKotlin);
		}
		void 排序funToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardString(Helpers.FormatFun);
		}
		void 替换ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.Text = textBox.Text.Replace(findBox.Text, replaceBox.Text);
		}
		void 行ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			var ls = new List<String>();
			
			var lines = textBox.Text.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			lines = lines.Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.Trim()).Distinct().ToArray();
			var r = replaceBox.Text.Trim();
			foreach (var element in lines) {
				ls.Add(string.Format(r, element));
			}
			textBox.Text = string.Join(Environment.NewLine, ls);
		}
		void 替换文件中ToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardDirectory((v) => {
			                     	
				var files = Directory.GetFiles(v, "*", SearchOption.AllDirectories)
			                     		.Where(i => Regex.IsMatch(i, "\\.(?:java|kt|xml|css|cs|js|htm|c|h)"));
				foreach (var element in files) {
					var str = element.ReadAllText();
					element.WriteAllText(str.Replace(findBox.Text, replaceBox.Text));
				}
			                     	
			});
		}
		void 压缩AndroidToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardDirectory(Helpers.ZipAndroidProject);
		}
		void Wkhtml2pdfMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText().Trim();
			if (!Directory.Exists(dir))
				return;

			foreach (var item in Directory.GetDirectories(dir)) {
				InvokeWkhtmltopdf(item);
			}
		}
		private void InvokeWkhtmltopdf(string f)
		{

			if (!File.Exists(Path.Combine(f, "目录.html")))
				return;

			var styleFile = "safari".GetDesktopPath().Combine("style.css");
			if (File.Exists(styleFile)) {
				var targetStyleFile = Path.Combine(f, "style.css");
				if (File.Exists(targetStyleFile))
					File.Delete(targetStyleFile);
				File.Copy(styleFile, targetStyleFile);
			}
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(Path.Combine(f, "目录.html").ReadAllText());
			var nodes = hd.DocumentNode.SelectNodes("//a");
			var ls = new List<string>();
			foreach (var item in nodes) {
				var href = item.GetAttributeValue("href", "").Split('#').First();

				if (ls.Contains(href))
					continue;
				ls.Add(href);
			}

			var str = "\"C:\\wkhtmltox\\wkhtmltopdf.exe\"";
			var arg = "--footer-center [page] -s Letter " + string.Join(" ", ls.Select(i => string.Format("\"{0}\"", i))) + string.Format("  \"{0}.pdf\"", f);

			var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
				FileName = "wkhtmltopdf.exe",
				Arguments = arg,
				WorkingDirectory = f
			});
			p.WaitForExit();
		}
		void AaToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format(" <a href=\"{1}\">{0}</a> ", textBox.SelectedText.Trim(), Clipboard.GetText().Trim());
	
		}
		void NoteButtonButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format(" <p class=\"note\">{0}</p> ", textBox.SelectedText.Trim());
	
		}
		void BoldButtonButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format(" **{0}** ", textBox.SelectedText.Trim());
	
		}
		void 下载ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			var lines = textBox.Text.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).Distinct().OrderBy(i => i);
			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "downloads");
			dir.CreateDirectoryIfNotExists();
			foreach (var element in lines) {
				var client = new System.Net.WebClient();
				client.DownloadFile(element, Path.Combine(dir, element.Split('/').Last()));
			}
		}
		void 导出全部ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var directories = Directory.GetFiles(_dataPath, "*.dat");
			var targetDirectory = "assets".GetCommandPath().Combine("exports");
			targetDirectory.CreateDirectoryIfNotExists();
			foreach (var element in directories) {
				var sql =	HelperSqlite.GetInstance(element);
				var contentList =	sql.GetTitleContentList();
				foreach (var c in contentList) {
					var tf = targetDirectory.Combine(Path.GetFileNameWithoutExtension(element) + " - " + c.Title.GetValidFileName() + ".html");
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
					sb.AppendLine("    \u003Clink rel=\u0022stylesheet\u0022 href=\u0022../stylesheets/markdown.css\u0022\u003E");
					sb.AppendLine("\u003C/head\u003E");
					sb.AppendLine("\u003Cbody\u003E");
					sb.AppendLine(c.Content.FormatMarkdown());

					sb.AppendLine("\u003C/body\u003E");
					sb.AppendLine("\u003C/html\u003E");
					tf.WriteAllText(sb.ToString());
				}
			}
			
		}
		void 打开ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		void 运行C文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_idF9 == -1)
				_idF9 = 1 << 2;
			RegisterHotKey(this.Handle, _idF9, 0, (int)Keys.F9);
			_runType=2;
		}
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg == 0x0312) {
				/* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

				//Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
				//KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
				//int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.

				var k = ((int)m.LParam >> 16) & 0xFFFF;
				if (k == 119) {
					Helper.CPlusPlusSnippetsVSC();
				} else if (k == 120/*F9*/) {
					if(_runType==1)
						Helper.RunGoCommand();
					else if(_runType==2)
					Helper.RunGenerateGccCommand();
					else if(_runType==3)
						Helper.RunGenerateGPlusPlusCommand();
				}
				// do something
			}
		}
		void 运行C文件热键F9ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_idF9 == -1)
				_idF9 = 1 << 2;
			RegisterHotKey(this.Handle, _idF9, 0, (int)Keys.F9);
			_runType=3;
		}
		void C代码段VSCToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_idF8 == -1)
				_idF8 = 1 << 5;
			RegisterHotKey(this.Handle, _idF8, 0, (int)Keys.F8);
		}
		void 其他ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(Clipboard.GetText());
			var links = hd.DocumentNode.SelectNodes("//a");
			var list = new List<String>();
			if (links.Any()) {
				foreach (var element in links) {
					list.Add(element.InnerText.Trim());
				}	
			}
			list = list.Distinct().Where(i => Regex.IsMatch(i, "^[0-9]+\\."))
				.Select(i => HtmlAgilityPack.HtmlEntity.DeEntitize(i))
				.ToList();
			var dir = "cpp".GetDesktopPath();
			dir.CreateDirectoryIfNotExists();
			foreach (var element in list) {
				if (!char.IsDigit(element[1])) {
					dir.Combine("0" + element.GetValidFileName() + ".cpp").WriteAllText("");
				} else {
					dir.Combine(element.GetValidFileName() + ".cpp").WriteAllText("");
				}
			}
		}
		void 删除Aria2文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		void 其他ToolStripMenuItem1Click(object sender, EventArgs e)
		{
	
			var dir = Clipboard.GetText().Trim();
			if (Directory.Exists(dir)) {
				var files = Directory.GetFiles(dir, "*.mp3");
				var count = 49;
				for (int i = 0; i < files.Length; i++) {
					++count;
					File.Move(files[i], Path.Combine(files[i].GetDirectoryName(), count + files[i].GetFileName()));
				}
			}
		}
		void 移动未转换ZipToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText().Trim();
			if (!Directory.Exists(dir))
				return;
			var epubDir = @"C:\Users\Administrator\Desktop\Safari\epub";
			var files = Directory.GetFiles(epubDir, "*.epub").Select(i => i.GetFileNameWithoutExtension());
			var zipDir = Directory.GetFiles(dir, "*.zip").Where(i => !files.Contains(i.GetFileNameWithoutExtension()));
			if (zipDir.Any()) {
				var td = "zip".GetDesktopPath();
				td.CreateDirectoryIfNotExists();
				foreach (var element in zipDir) {
					
					File.Move(element, Path.Combine(td, element.GetFileName()));
				}
			}
		}
		void 解压目录中文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText().Trim();
			if (!Directory.Exists(dir))
				return;
			var zipFiles = Directory.GetFiles(dir, "*.zip");
			foreach (var element in zipFiles) {
				using (var zip = new Ionic.Zip.ZipFile(element, Encoding.GetEncoding("gbk"))) {
					zip.ExtractAll(Path.Combine(element.GetDirectoryName(), element.GetFileNameWithoutExtension()));
				}
			}
		}
		void 运行Go文件全局热键F9ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_idF9 == -1)
				_idF9 = 1 << 2;
			RegisterHotKey(this.Handle, _idF9, 0, (int)Keys.F9);
			_runType=1;
		}
		void 字符串到数组ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Helper.OnClipboardString((v)=>{
			                         
			                         	var ls=new UTF8Encoding(false).GetBytes(v).Select(i=>i.ToString());
			                         	return "int buf["+ls.Count()+"]="+"{"+string.Join(",",ls)+"};";
			                         });
		}
		 
	}
	
	public static class Helpers
	{
		public static void ZipAndroidProject(string dir)
		{
			 
			using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {

				zip.AddFiles(Directory.GetFiles(dir).Where(i => !i.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase)).ToArray(), "");
				zip.AddFiles(Directory.GetFiles(Path.Combine(dir, "app")), "app");
				zip.AddDirectory(Path.Combine(Path.Combine(dir, "app"), "src"), "app/src");
				zip.AddDirectory(Path.Combine(dir, "gradle"), "gradle");
				var targetFileName = Path.Combine(dir, Path.GetFileName(dir) + ".zip");
				var count = 0;
				while (File.Exists(targetFileName)) {
					targetFileName = Path.Combine(dir, string.Format("{0} {1:000}.zip", Path.GetFileName(dir), ++count));
				}
				zip.Save(targetFileName);
			}
		}
		public static String FormatProperties(string value)
		{
		
			string[] lsr = null;
			// var isFirst = false;
// isFirst = true;
			lsr = Regex.Split(value, "\\s+(?=va[rl]\\s+[a-zA-Z_0-9]+\\:)", RegexOptions.Multiline);
//			if (Regex.IsMatch(value, "}\\s+(?:var|val)")) {
//               
//
//			} else {
//				// lsr = Regex.Split(value, "^\\s+(?=var\\s+|val\\s+)", RegexOptions.Multiline);
//			}
			if (lsr.Any()) {

				var result = lsr.Where(i => !string.IsNullOrWhiteSpace(i))
                    .OrderBy(i => i.Split(':').First().Split(' ').Last()).ToList();
				//if (isFirst) {
				//result = result.Select(i => i + "}").ToList();
				//	}
				//.OrderBy(i => i.Trim().Split(new[] { ' ' }, 2).First()).Select(i =>
				// {
				//     i = i.Trim();
				//     if (!i.StartsWith("var ") && i.Contains("set("))
				//         return "var " + i;
				//     else if (!i.StartsWith("val ") && !i.Contains("set("))
				//         return "val " + i;
				//     else
				//         return i;
				// });

				return string.Join("\n", result);
			}

			return null;
		}
		public static String FormatDelegate(string value)
		{
			  
			var lines = value.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			var singleItems = lines.Where(i => (i.StartsWith("val") || i.StartsWith("fun ") || i.StartsWith("private fun") || i.StartsWith("private val")) && i.Contains(") = ") && !i.EndsWith("{")).ToArray();
			var sss = lines.Except(singleItems).ToArray();
			var ls = Helper.FormatMethodList(string.Join("\n", lines.Where(i => !singleItems.Contains(i)))).Select(i => i.Trim()).OrderBy(i => i.SubstringBefore("by").Trim().Split(' ').Last()).ToArray();

			return string.Join("\n", singleItems.OrderBy(i => i)) + "\n" + string.Join("\n", ls);
		}
		public static String FormatFun(string value)
		{
			  
			var lines = value.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			var singleItems = lines.Where(i => (i.StartsWith("val") || i.StartsWith("fun ") || i.StartsWith("private fun") || i.StartsWith("private val")) && i.Contains(") = ") && !i.EndsWith("{")).ToArray();
			var sss = lines.Except(singleItems).ToArray();
			var ls = Helper.FormatMethodList(string.Join("\n", lines.Where(i => !singleItems.Contains(i)))).Select(i => i.Trim()).OrderBy(i => Regex.Match(i, "fun ([^\\(]*?)(?:\\()").Groups[1].Value).ToArray();

			return string.Join("\n", singleItems.OrderBy(i => i)) + "\n" + string.Join("\n", ls);
		}
		public static String FormatJavaStaticFinalFieldToKotlin(string value)
		{
			var ls = Regex.Matches(value, "(?<=private|public|protected)([^\\=\n\r]*?)\\=([^;]*?);").Cast<Match>().ToList();
			var strings = new List<string>();

			foreach (var item in ls) {
				var name = item.Groups[1].Value.TrimEnd().Split(' ').Last();
				var v = item.Groups[2].Value;
				//var m="private";
				strings.Add(string.Format("// const val {0}={1}", name, v));
				 
				strings.Add(string.Format("private const val {0}={1}", name, v));
			}
			return string.Join("\n", strings.OrderBy(i => i));
		}
		
	}
}
