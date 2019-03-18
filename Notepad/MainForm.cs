namespace Notepad
{

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Utils;
	using Common;

	public partial  class MainForm: Form
	{
		private Article _article;
		private readonly string _dataPath;
		private string _defaultDatabase;
	 
		public MainForm()
		{
			InitializeComponent();
			_dataPath = "datas".GetExecutingPath();
			_dataPath.CreateDirectoryIfNotExists();
			_defaultDatabase = _dataPath.Combine("db.dat");

			if (!_defaultDatabase.FileExists())
				DatabaseUtils.GetInstance(_defaultDatabase);

			comboBox.Items.AddRange(_dataPath.GetFiles("dat").Select(i => i.GetFileName()).ToArray());
		}
		
		void AaToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format(" <a href=\"{1}\">{0}</a> ", textBox.SelectedText.Trim(), Clipboard.GetText().Trim());
			
		}
		void AppButtonButtonClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("".GetExecutingPath());
		}
		void BoldButtonButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format(" **{0}** ", textBox.SelectedText.Trim());
			
		}
		void BYTE数组到GBKToolStripMenuItemClick(object sender, EventArgs e)
		{
		
			var buf = textBox.Text.Trim().Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => byte.Parse(i, System.Globalization.NumberStyles.HexNumber)).ToArray();
			textBox.Text = Encoding.GetEncoding("gbk").GetString(buf);
		}
	
		void CheatEngineMemoryViewer数组到BYTE数组ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormUtils.OnClipboardString((v) => {
				var str = v.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select((i) => {
					var vh = int.Parse(i, System.Globalization.NumberStyles.HexNumber);
					if (vh == 0) {
						return "0";
					} else {
						return "0x" + vh.ToString("X");
					}
				});
				return "{" + string.Join(",", str) + "}";
			});
		}
		void ChineseButtonClick(object sender, EventArgs e)
		{
			textBox.SelectLine(true);

			var val = textBox.SelectedText;
			if (val.IsVacuum())
				return;
			var json = TranslateUtils.GetInstance().QueryChinese(val);

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
		void CodeButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = MarkdownUtils.FormatCode(textBox.SelectedText);
		}
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			_defaultDatabase = _dataPath.Combine(comboBox.Text);
			DatabaseUtils.GetInstance(_defaultDatabase);
			UpdateList();
			_article = null;
			
		}
	 
		void FormatButtonClick(object sender, EventArgs e)
		{
			textBox.Format();
		}
		
		public string GetText()
		{
			return textBox.Text;
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
			
			Logic.FormatH2(textBox);
		}
		void H3ButtonClick(object sender, EventArgs e)
		{
			Logic.FormatH3(textBox);
		}
		void HtmlsButtonClick(object sender, EventArgs e)
		{
			 
			var fileName = @"assets\htmls".GetExecutingPath().Combine(textBox.Text.GetFirstReadable().TrimStart('#').TrimStart().GetValidFileName('-') + ".htm");
			if (!File.Exists(fileName))
				fileName.WriteAllText(Logic.ConvertToHtml(textBox));

			System.Diagnostics.Process.Start("chrome.exe", string.Format("\"{0}\"", fileName));
		}
		void IButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = textBox.SelectedText.FormatEm();
		}
		void ImportApressCode(string prefixTitle, string file)
		{
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(file.ReadAllText());
			
			var nodes = hd.DocumentNode.DescendantNodes();
			var sb = new StringBuilder();
			var ul = new StringBuilder();
			
			foreach (var element in nodes) {
				if (element.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
					if (element.Name == "h1") {
						var tn = Regex.Replace(element.InnerText, "[\r\n\t]+", " ").Trim();
						ul.AppendLine(string.Format("- {0}", tn));
						sb.Append(string.Format("## {0}\r\n\r\n", tn));
					} else if (element.Name == "pre") {
						// else if (element.GetAttributeValue("class", "") == "ProgramCode") {
						sb.Append("```\r\n\r\n");
						sb.AppendLine(HtmlAgilityPack.HtmlEntity.DeEntitize(element.InnerText));
//						var codes = element.ChildNodes;
//						foreach (var cn in codes) {
//							sb.AppendLine(HtmlAgilityPack.HtmlEntity.DeEntitize(cn.InnerText));
//						}
						sb.Append("\r\n\r\n```\r\n\r\n");
						
					}
				}
			}
			var title = prefixTitle + " " + hd.DocumentNode.SelectSingleNode("//h1").InnerText;
			var article = new Article {
				Title = title,
				Content = "# " + title + Environment.NewLine + Environment.NewLine + ul.ToString() + Environment.NewLine + Environment.NewLine + sb.ToString(),
				CreateAt = DateTime.UtcNow,
				UpdateAt = DateTime.UtcNow,
			};
			DatabaseUtils.GetInstance().Insert(article);
			
			
			
		}
		
		void ImportApressCode1(string prefixTitle, string file)
		{
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(file.ReadAllText());
			
			var nodes = hd.DocumentNode.DescendantNodes();
			var sb = new StringBuilder();
			var ul = new StringBuilder();
			
			foreach (var element in nodes) {
				if (element.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
					if (element.GetAttributeValue("class", "") == "Heading1") {
						var tn = Regex.Replace(element.InnerText, "[\r\n\t]+", " ").Trim();
						ul.AppendLine(string.Format("- {0}", tn));
						sb.Append(string.Format("## {0}\r\n\r\n", tn));
					} else if (element.Name == "pre") {
						sb.Append("```\r\n\r\n");
						var codes = element.ChildNodes;
						foreach (var cn in codes) {
							sb.AppendLine(HtmlAgilityPack.HtmlEntity.DeEntitize(cn.InnerText));
						}
						sb.Append("\r\n\r\n```\r\n\r\n");
						
					}
				}
			}
			var title = prefixTitle + " " + hd.DocumentNode.SelectSingleNode("//*[@class='ChapterNumber']").InnerText;
			var article = new Article {
				Title = title,
				Content = "# " + title + Environment.NewLine + Environment.NewLine + ul.ToString() + Environment.NewLine + Environment.NewLine + sb.ToString(),
				CreateAt = DateTime.UtcNow,
				UpdateAt = DateTime.UtcNow,
			};
			DatabaseUtils.GetInstance().Insert(article);
			
			
			
		}
		void LinkButtonButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format("[{0}]({1})", textBox.SelectedText.Trim(), Clipboard.GetText().Trim());
		}
		void ListBoxDoubleClick(object sender, EventArgs e)
		{
			if (listBox.SelectedIndex == -1)
				return;
			var title = listBox.SelectedItem.ToString();
			_article = DatabaseUtils.GetInstance().GetArticle(title);

			this.Text = _article.Title;
			textBox.Text = _article.Content;
		}
		
		private int _hotKeyF6 = -1;
		
		private int _hotKeyF7 = -1;
		private void DispatchF6()
		{
			CSSDelegate.CombineCssFiles();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			var handle = NativeMethods.HWND.Cast(this.Handle);
			if (_hotKeyF7 != -1) {
				Forms.UnregisterHotKey(handle, _hotKeyF7);
			}
			if (_hotKeyF6 != -1) {
				Forms.UnregisterHotKey(handle, _hotKeyF6);
			}
			"settings.txt".GetExecutingPath().WriteAllText(comboBox.Text);
		}
		void DispatchCtrlF()
		{
			if (!Regex.IsMatch(Clipboard.GetText(), "\\<\\w+"))
				StringDelegate.ToByteArray();
			else
				GoDelegate.GenerateHTMLArrayTemplate();
			
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0312) {
				var k = ((int)m.LParam >> 16) & 0xFFFF;
				if (k == 67) {
					DispatchCtrlF();
				} else if (k == 0x75) {

				
					DispatchF6();
				} else if (k == 0x76) {

					DispatchF7();
				} else if (k == 0x77) {

					
				}
				//                else if (k == 0x78)
				//                {
				//                    DispatchF9();
//
				//                }
				//                else if (k == 0x57)
				//                {
				//                    DispatchCtrlW();
				//                }
			}
			base.WndProc(ref m);
		}
		private void DispatchF7()
		{
			//JavaScriptDelegate.CompileTypeScript(@"C:\NetCore\wwwroot\typescripts");
			//WinFormHelper.OnClipboardDirectory(JavaScriptDelegate.CompileTypeScript);
			JavaScriptDelegate.CombineJavaScriptFiles(@"C:\NetCore\wwwroot\javascripts");
			
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			 
			var handle = NativeMethods.HWND.Cast(this.Handle);
			_hotKeyF7 = 7;
			_hotKeyALTC = 11;
			Forms.RegisterHotKey(handle, _hotKeyF7, 0, (int)Keys.F7);
			Forms.RegisterHotKey(handle, _hotKeyF6, 0, (int)Keys.F6);
			Forms.RegisterHotKey(handle, _hotKeyALTC, 0x0001, (int)Keys.C);
		
			Inject(typeof(CodeDelegate));
			Inject(typeof(StringDelegate));
			Inject(typeof(VideoDelegate));
			Inject(typeof(TranslatorDelegate));
			Inject(typeof(FindDelegate));
			Inject(typeof(JavaScriptDelegate));
			Inject(typeof(SafariDelegate));
			Inject(typeof(CSharpDelegate));
			Inject(typeof(GoDelegate));
			Inject(typeof(FileDelegate));
			
			if ("settings.txt".GetExecutingPath().FileExists()) {
				var value = "settings.txt".GetExecutingPath().ReadAllText();
				if (value.IsReadable()) {
					comboBox.SelectedItem = value.Trim();
				}
			}
			var directories = Directory.GetFiles(_dataPath, "*.dat").Where(i => i.EndsWith(".dat"));
			
			foreach (var ex in directories) {
				
				moveMenuItem.DropDownItems.Add(ex.GetFileName()).Click += (s, o) => {
					var cpp = DatabaseUtils.NewInstance(Path.Combine(_dataPath, ((ToolStripMenuItem)s).Text));
					foreach (var element in listBox.SelectedItems) {
						var article =	DatabaseUtils.GetInstance().GetArticle(element.ToString());
						cpp.Insert(article);
						DatabaseUtils.GetInstance().Delete(element.ToString());
					}
					UpdateList();
				};
			}
		}
		void NewButtonClick(object sender, EventArgs e)
		{
			var a = false;
			var b = false;
			a |= b;
			_article = null;
			
			textBox.Text = textBox.Text.GetFirstReadable() + Environment.NewLine + Environment.NewLine;

			this.Text = string.Empty;
		}
		void NoteButtonButtonClick(object sender, EventArgs e)
		{
			textBox.SelectedText = string.Format(" <p class=\"note\">{0}</p> ", textBox.SelectedText.Trim());
			
		}
		
		
		
		void PathButtonButtonClick(object sender, EventArgs e)
		{
			try {
				Clipboard.SetText(Clipboard.GetText().Trim().Replace('\\', '/'));
			} catch {
				
			}
		}
		void TextBoxTextChanged(object sender, EventArgs e)
		{
			if (!this.Text.EndsWith("*"))
				this.Text += " *";
		}
		void TitleButtonClick(object sender, EventArgs e)
		{
			
//			var array = textBox.Text.Trim().ToArray();
//			var stringBuilder = new StringBuilder();
			////			using (System.Security.Cryptography. RNGCryptoServiceProvider rng = new System.Security.Cryptography. RNGCryptoServiceProvider()) {
			////				for (int i = 0; i < array.Length; i++) {
			////					byte[] randomNumber = new byte[4];//4 for int32
			////					rng.GetBytes(randomNumber);
			////					int value = BitConverter.ToInt32(randomNumber, 0);
			////					stringBuilder.Append("/C"+value.ToString().PadLeft(2,' ')).Append(array[i]);
			////				}
			////			}
//			// DateTime.Now.Millisecond
//			var random=new Random(DateTime.Now.Millisecond);
//			for (int i = 0; i < array.Length; i++) {
//				stringBuilder.Append("/C" + (random.Next(0, 21)).ToString().PadLeft(2, '0')).Append(array[i]);
//			}
//			textBox.Text = stringBuilder.ToString();
			
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
		
		private void UpdateList()
		{

			listBox.Items.Clear();
			listBox.Items.AddRange(DatabaseUtils.GetInstance().GetTitleList().ToArray());
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
				DatabaseUtils.GetInstance().Insert(_article);
				_article = DatabaseUtils.GetInstance().GetArticle(title);
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

				DatabaseUtils.GetInstance().Update(_article);
				if (updateList) {
					UpdateList();

				}
				this.Text = title;
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
		void 导入Apress单文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormUtils.OnClipboardDirectory((dir) => {
			                            	
				var files = Directory.GetFiles(dir, "*.html");
				foreach (var element in files) {
					ImportApressCode("实例: C in a Nutshell", element);
				}
			                            	
			});
		}
		void 导入ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Logic.ImportSingleFile(textBox);
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
		
		void 复制ToolStripMenuItemClick(object sender, EventArgs e)
		{
			 

			if (string.IsNullOrEmpty(textBox.SelectedText)) {
				textBox.SelectLine(true);
			}
			textBox.Copy();
			
		}
		void 复制当前数据库ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var sc = new System.Collections.Specialized.StringCollection();
			sc.Add(Path.Combine(_dataPath, comboBox.Text));
			Clipboard.SetFileDropList(sc);
		}
		//		void 复制文件ToolStripMenuItemClick(object sender, EventArgs e)
		//		{
		//
		//			var dir = Clipboard.GetText();
		//			var lines = textBox.Text.ToLines().Select(i => i.SubstringBeforeLast("_").SubstringBeforeLast("_"));
		//			var targetDirectory = "assets".GetDesktopPath();
		//			targetDirectory.CreateDirectoryIfNotExists();
		//			if (Directory.Exists(dir)) {
		//				var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Where(i => lines.Any(ix => i.GetFileName().StartsWith(ix)));
		//				foreach (var element in files) {
		//
		//					File.Copy(element, Path.Combine(targetDirectory, element.GetFileName()));
		//				}
		//			}
		//		}
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
		void 计算匹配数量正则表达式ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			textBox.Text += "\r\n" + Regex.Matches(textBox.Text, findBox.Text).Cast<Match>().Count().ToString();
			
			
		}
		void 剪切ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBox.SelectedText)) {
				textBox.SelectLine(true);
			}
			textBox.Cut();
		}
		void 排序H3ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (textBox.SelectedText.IsReadable()) {
				var start = textBox.SelectedText.SubstringBefore("### ");
				var end = "### " + textBox.SelectedText.SubstringAfter("### ");
				textBox.SelectedText =	start + Logic.OrderH3(end);
			}
		}
		void 排序ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = Helper.SortString(textBox.SelectedText);
			
			//textBox.SelectedText = string.Join(Environment.NewLine, textBox.SelectedText.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).Distinct().OrderBy(i => i));
		}
		void 排序分隔符ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var search = findBox.Text;
		 
			if (search.IsVacuum())
				return;
			textBox.SelectedText = string.Join(search.Trim('\\'), Regex.Split(textBox.SelectedText.Trim(), search).OrderBy(i => i));
			
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
		void 全选ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectAll();
			
		}
		void 删除ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (listBox.SelectedItems.Count > 0) {
				var directories = Directory.GetFiles(_dataPath, "*").Where(i => i.GetExtension() == ".dat");
				//var baseDirectory = "assets".GetExecutingPath().Combine("exports");
				var baseDirectory = "md".GetExecutingPath();
		
				baseDirectory.CreateDirectoryIfNotExists();
			 
			
				var targetDirectory = Path.Combine(baseDirectory, comboBox.Text.GetFileNameWithoutExtension());
				targetDirectory.CreateDirectoryIfNotExists();
				 
		 
			
				foreach (var element in listBox.SelectedItems) {
					var c = DatabaseUtils.GetInstance().GetArticle(element.ToString());
					
					var tf = targetDirectory.Combine(c.Title.GetValidFileName() + ".md");
					
					tf.WriteAllText(c.Content);
					DatabaseUtils.GetInstance().Delete(element.ToString());
				}
				UpdateList();
				
			}
		}
	
		 
		void 替换ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.Text = Regex.Replace(textBox.Text, findBox.Text, replaceBox.Text);
		}
		
		
		void 替换成换行符ToolStripMenuItemClick(object sender, EventArgs e)
		{
			// Envoriment.NewLine
			textBox.Text = Regex.Replace(textBox.Text, findBox.Text, Environment.NewLine);
		}
		void 替换文件中ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormUtils.OnClipboardDirectory((v) => {
			                     	
				var files = Directory.GetFiles(v, "*", SearchOption.AllDirectories)
			                     		.Where(i => Regex.IsMatch(i, "\\.(?:java|kt|xml|css|cs|js|htm|c|h)"));
				foreach (var element in files) {
					var str = element.ReadAllText();
					element.WriteAllText(str.Replace(findBox.Text, replaceBox.Text));
				}
			                     	
			});
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
		
		void 粘贴代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var match =	Regex.Match(textBox.Text, "(?<=```)[^`]*?(?=```)").Value;
			
			textBox.Text = Regex.Replace(textBox.Text, "(?<=```)[^`]*?(?=```)", "\r\n" + Clipboard.GetText() + "\r\n");
//			var value = Clipboard.GetText();
//			value = Regex.Replace(value, "([,\\(])\\s+", "$1");
//			textBox.SelectedText = "`" + value.Trim() + "`";
//			if (value.Contains('\n')) {
//				textBox.SelectedText = string.Format("```\r\n\r\n{0}\r\n\r\n```\r\n\r\n", Clipboard.GetText().Trim().Replace("`", "\u0060"));
//
//			} else {
//				textBox.SelectedText = string.Format("`{0}`", Clipboard.GetText().Trim().Replace("`", "\u0060"));
//
//			}
		}
		void 粘贴注释ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var value = Clipboard.GetText();
			value = Regex.Replace(value, "\\s+\\*\\s+", "");
			value = Regex.Replace(value, "[\r\n]+", "");
			value = Regex.Replace(value, "\\s{2,}", " ");
			textBox.SelectedText = value;
		}
		
	
	 
		void 复制代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var match =	Regex.Match(textBox.Text, "(?<=```)[^`]*?(?=```)").Value;
			Clipboard.SetText(match);
		}
		void 预览目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			textBox.SelectedText = TOCHelper.GenerateTOC(textBox.Text) + "\r\n\r\n";
		}
		
		void ScrollHeadButtonClick(object sender, EventArgs e)
		{
			textBox.SelectionStart = 0;
			textBox.ScrollToCaret();
		}
		void ScrollDownButtonClick(object sender, EventArgs e)
		{
			textBox.SelectionStart = textBox.Text.Length - 1;
			textBox.ScrollToCaret();
		}
		void TextBoxKeyUp(object sender, KeyEventArgs e)
		{
			
		}
		void TextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Tab) {
				if (textBox.SelectedText.Length == 0)
					return;
				var lines = textBox.SelectedText.Split('\n');
				textBox.SelectedText = string.Join(Environment.NewLine, lines.Select(i => "\t" + i.TrimEnd()));
			}
		}
		void 移动到ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var array = System.Buffers.ArrayPool<int>.Shared.Rent(213);
		}
		void 生成ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			WinFormUtils.OnClipboardString(v => {
				var nodes = v.GetHtmlNode().SelectNodes("//*[@class='item-name']").ToArray();
				var sb = new StringBuilder();
				foreach (var element in nodes) {
				 
					var str = element.InnerText.Trim().DeEntitize();
//					str+=" "+element.SelectSingleNode(".//*[@class='by']").InnerText;
//					str+=" "+element.SelectSingleNode(".//*[@itemprop='author']").InnerText.Trim().DeEntitize();
					sb.AppendLine(string.Format("- {0}", str));
					 
				}
				return sb.ToString();
			});
			
//			this.OnClipboardString(v => {
//				var nodes = v.GetHtmlNode().Descendants();
//				var sb=new StringBuilder();
//				foreach (var element in nodes) {
//					var name=(element.Name ?? "");
//					if (name == "h1" ) {
//						sb.AppendLine(string.Format("## {0}\r\n",element.InnerText.Trim().DeEntitize()));
//					}else if(name=="pre"){
//						sb.AppendLine(string.Format("```\r\n{0}\r\n```\r\n",element.InnerHtml.Replace("<br>","\r\n").StripHtmlTag().DeEntitize()));
//					}
//				}
//				return sb.ToString();
//			});
//			this.OnClipboardDirectory((v) => {
//				var lines = textBox.Text.ToLines().ToArray();
//				var index = 0;
//				v.GetDirectories().ForEach((dir) => {
//					var targetFileName = Path.Combine(v, lines[index] + ".cpp");
//					File.Move(Path.Combine(dir, "main.cpp"), targetFileName);
//					index++;
//				});
//			                         
//			});
//			this.OnClipboardDirectory(v=>{
//			                          	var files=Directory.GetFiles(v,"*");
//			                          	foreach (var element in files) {
//			                          		File.Move(element,element.Replace("_",". "));
//			                          	}
//			                          });
		
			
//			var hd = new HtmlAgilityPack.HtmlDocument();
//			hd.LoadHtml(Clipboard.GetText());
//			var nodes = hd.DocumentNode.SelectNodes("//tbody/tr/td/a")
//				.Select(i => string.Format("- [{0}](https://docs.microsoft.com/en-us/cpp/dotnet/{1})", i.InnerText, i.GetAttributeValue("href", "")));
//			
//		
//			Clipboard.SetText(string.Join(Environment.NewLine, nodes));
//				
				
			
				 
			
		}
		void 导出mdToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = "C:/codes";
			dir.CreateDirectoryIfNotExists();
			dir = Path.Combine(dir, "Notes");
			dir.CreateDirectoryIfNotExists();
			var fileName = textBox.Text.Trim().SubstringBefore('\n').SubstringAfter(" ").Trim().GetValidFileName() + ".md";
			Path.Combine(dir, fileName).WriteAllText(textBox.Text);
		}
		void 标记cToolStripMenuItemClick(object sender, EventArgs e)
		{
			Logic.AddCodeLanguage(textBox, "c++");
		}
		void NotesButtonClick(object sender, EventArgs e)
		{
			var dir = "C:/codes";
			dir = Path.Combine(dir, "Notes");
			System.Diagnostics.Process.Start(dir);
		}
		void 标记csharpToolStripMenuItemClick(object sender, EventArgs e)
		{
			Logic.AddCodeLanguage(textBox, "csharp");
	
		}
		void 标记javaToolStripMenuItemClick(object sender, EventArgs e)
		{
			Logic.AddCodeLanguage(textBox, "java");
	
		}
		void 标记cToolStripMenuItem1Click(object sender, EventArgs e)
		{
			Logic.AddCodeLanguage(textBox, "c");
	
		}
		void 打开ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Logic.OpenLink(textBox);
		}
		void CmdButtonClick(object sender, EventArgs e)
		{
			var selected = textBox.SelectedText.Trim();
			if (selected.IsVacuum()) {
				textBox.SelectLine();
			}
			selected = textBox.SelectedText.Trim();
			if (selected.IsVacuum())
				return;
			System.Diagnostics.Process.Start("cmd", string.Format("/K {0}", selected));
		}
		
		
		
		void 导出ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			var targetDirectory = "c:\\Codes\\Notes";
			targetDirectory.CreateDirectoryIfNotExists();
			
			var sql =	DatabaseUtils.GetInstance();
			var contentList =	sql.GetTitleContentList();
			foreach (var c in contentList) {
				var tp = "";
				 
				
				if (c.Title.StartsWith("F#"))
					tp = "fsharp";
				else if (c.Title.StartsWith("C#"))
					tp = "csharp";
				else if (c.Title.StartsWith("Java"))
					tp = "java";
				targetDirectory.Combine(tp).CreateDirectoryIfNotExists();
				
				var tf = targetDirectory.Combine(tp).Combine(c.Title.SubstringAfter(':').Trim().GetValidFileName() + ".md");
				var lines = c.Content.Split('\n').Select(i => i.TrimEnd()).ToArray();
				var skip = true;
				for (int i = 0; i < lines.Length; i++) {
					
					if (lines[i].StartsWith("```")) {
						
						if (skip)
							lines[i] = "```" + tp;
						
						skip = !skip;
						
					}
					
				}
				
				tf.WriteAllLines(lines);
			}
		}
		void ActionGenerateStringFromArray(object sender, EventArgs e)
		{
	
			var array = Regex.Split(textBox.Text.Trim(), "\n---");
			if (array.Length < 1) {
				return;
			}
			var data = array[0].Split(new char[]{ ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			var pattern = array[1].Trim();
			var sb = new StringBuilder();
			foreach (var element in data) {
				var s =	string.Format(pattern, element.Trim(), element.Replace("Manager", "").UpperCase());
				sb.AppendLine(s);
			}
			textBox.Text = sb.ToString();
		}
		void 预览重新生成ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var fileName = @"assets\htmls".GetExecutingPath().Combine(textBox.Text.GetFirstReadable().TrimStart('#').TrimStart().GetValidFileName('-') + ".htm");
			
			fileName.WriteAllText(Logic.ConvertToHtml(textBox));

			System.Diagnostics.Process.Start("chrome.exe", string.Format("\"{0}\"", fileName));
		}
		void 计算ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			var interpreter = new DynamicExpresso. Interpreter();
		
			textBox.SelectedText =	textBox.SelectedText + " = " + interpreter.Eval(textBox.SelectedText);
			
		}
		void 逃逸ToolStripMenuItemClick(object sender, EventArgs e)
		{
			StringTemplate.EscapePattern(textBox);
		}
		void 收集文件名ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormUtils.OnClipboardDirectory(v => {
				var fileNames =	Directory.GetFiles(v).Select(i => i.GetFileNameWithoutExtension());
				textBox.Text = string.Join(" ", fileNames);
			});
		}
		void 导入ScreenShots图片ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormUtils.OnClipboardDirectory((dir) => {
				var files = Directory.GetFiles(dir, "*");
				var list = new List<string>();
				
				foreach (var element in files) {
					list.Add(string.Format("<img width=\"360\" src=\"screenshots\\{0}\">", Path.GetFileName(element)));
				}
				textBox.SelectedText = string.Join(Environment.NewLine, list);
				                               
			  
			});
		
		}
		void 数组正则表达式ToolStripMenuItemClick(object sender, EventArgs e)
		{
			 
			textBox.Text = StringHelper.KeepMatchesIntoArray(textBox.Text, findBox.Text);
		}
		void SwitchCase正则表达式ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.Text = StringHelper.KeepMatchesIntoSwitch(textBox.Text, findBox.Text);
	
		}
		void Inject(Type type)
		{
			foreach (var method in type.GetMethods()) {
				var attributes = method.GetCustomAttributes(
					                 typeof(BindMenuItemAttribute), false);
				
				if (!attributes.Any())
					continue;
				
				
				var attribute = (BindMenuItemAttribute)attributes.First();
				
				var toolStrip = (ToolStrip)this.Controls[attribute.Toolbar];
				
				var splitButton = (ToolStripSplitButton)toolStrip.Items[attribute.SplitButton];
				
				if (attribute.AddSeparatorBefore) {
					splitButton.DropDownItems.Add(new ToolStripSeparator());
				}
				
				var item = new ToolStripMenuItem(attribute.Name);
				
				if (attribute.NeedBinding)
					item.Click += (a, b) => method.Invoke(null, new Object[]{ a, this });
				else
					item.Click += (a, b) => method.Invoke(null, null);
				if (attribute.ShortcutKeys != null) {
					item.ShortcutKeys = attribute.ShortcutKeys;
				}
				splitButton.DropDownItems.Add(item);
				
				
			}
		}
		
		public void SelectedText(string str)
		{
			textBox.SelectedText = str;
		}
		
		void 生成目录和排序ToolStripMenuItemClick(object sender, EventArgs e)
		{
			TOCHelper.GenerateTOCAndOrder(textBox);
		}
		void 代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = ListHelper.ToCodeList(textBox.SelectedText);
		}
		void 代码数字ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = ListHelper.ToCodeListByOrder(textBox.SelectedText);
	
		}
		void 列表ToolStripMenuItem1Click(object sender, EventArgs e)
		{ 
			var start = textBox.SelectionStart;
			var end = textBox.SelectionStart + textBox.SelectionLength;

			var length = textBox.Text.Length;
			var value = textBox.Text;
			while (start > -1 && value[start - 1] != '\n') {
				start--;
			}
			while (end < length && value[end] != '\n') {
				end++;
			}
			textBox.SelectionStart = start;
			textBox.SelectionLength = end - start;
			
			textBox.SelectedText = ListHelper.ToList(textBox.SelectedText);
		}
		private int _hotKeyALTC = -1;
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			var filename = Clipboard.GetFileDropList()[0];
			var collection = filename.ReadAllText().ToObject<List<Dictionary<string,dynamic>>>();
			var obj = new List<Dictionary<string,dynamic>>();
			
			foreach (var element in collection) {
				var keys = element.Keys.ToList();
				foreach (var key in keys) {
					if (element[key] == null) {
						element.Remove(key);
					} else if (key == "Id") {
						var v = element[key];
						element.Remove(key);
						 
						element.Add(key.Decapitalize(), v);
						
					} else if (key == "CreatedAt" || key == "UpdatedAt") {
						var dt = (DateTime)element[key];
						if (dt.Year == 1)
							continue;
						var timestamp = (dt).ToUnixTimestamp();
						element.Remove(key);
						 
						element.Add(key, timestamp);
						
					} else if (key == "Tags") {
						var xxx = element[key];
						var a = element[key] as Newtonsoft.Json.Linq.JArray;
						var c = new List<string>();
						
						foreach (var b in a) {
							c.Add(b.ToObject<Newtonsoft.Json.Linq.JObject>()["Name"].ToString());
						}
						
						element["Tags"] = c;
						
					} else if (key == "Album") {
						var o1 = element[key] as Newtonsoft.Json.Linq.JObject;

						foreach (var o2 in o1.Values<Newtonsoft.Json.Linq.JProperty>()) {
						
							if (o2.Name.StartsWith("Base")) {
								element.Add(o2.Name, o2.Value);
							} else if (o2.Name == "CreatedAt") {
//								var dt = (DateTime)o2.Value;
//								if (dt.Year == 1)
//									continue;
//								var timestamp = dt.ToUnixTimestamp();
//						
//						 
//								element.Add("BaseCreatedAt", timestamp);
						
							} else if (o2.Name != "Id" && o2.Name != "ProfileIcon") {
								
								element.Add("Base" + o2.Name, o2.Value);
							} 
						}
						element.Remove(key);
					}
				}
				obj.Add(element);
				//break;
			}
			(filename + ".json").WriteAllText(obj.ToJsonString());
			
		}
		void ToolStripButton2Click(object sender, EventArgs e)
		{
	
		}
		void 生成序列化行ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var list1 = new List<string>();
			for (int i = 0; i < 100; i++) {
				list1.Add(Regex.Replace(textBox.Text, "(?<=\\[)[0-9]+(?=\\])", i.ToString()));
			}
			textBox.Text = list1.ConcatenateLines();
		}
	
		
		
	}
}