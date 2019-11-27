namespace Notepad
{
	using Helpers;
	using Helpers;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Utils;
	using Common;
	public partial    class MainForm: Form
	{
		public Article _article;
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
		void AndroidToolStripMenuItemClick(object sender, EventArgs e)
		{
			var result =	Helper.StringTemplateAndroid(textBox.Text, findBox.Text, replaceBox.Text);
			if (!string.IsNullOrWhiteSpace(result)) {
				Clipboard.SetText(result);
			}
		}
		public void AppendText(string s)
		{
			textBox.Text += s;
		}
		void BindEvents()
		{
			this.大小写切换ToolStripMenuItem.Click += (s, o) => Helper.ToggleCase(textBox);
			this.全选ToolStripMenuItem1.Click += (s, o) => textBox.SelectAll();
			this.stringpatternButton.ButtonClick += (s, o) => Helper.GenerateString(textBox, findBox.Text, replaceBox.Text);
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
		 
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			_defaultDatabase = _dataPath.Combine(comboBox.Text);
			DatabaseUtils.GetInstance(_defaultDatabase);
			UpdateList();
			_article = null;
		}
		void FilterStripButtonButtonClick(object sender, EventArgs e)
		{
			textBox.Text = textBox.Text.Split('\n').Where(i => i.Contains(findBox.Text)).ConcatenateLines();
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
		public void HandleException(Exception e)
		{
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
				CreateAt = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
				UpdateAt = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds,
			};
			DatabaseUtils.GetInstance().Insert(article);
		}
		
		void LinkButtonButtonClick(object sender, EventArgs e)
		{
			
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
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			"settings.txt".GetExecutingPath().WriteAllText(comboBox.Text);
		}
	
		
		
		public void SelectedText(string str)
		{
			textBox.SelectedText = str;
		}
		
		void SwitchCase正则表达式ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.Text = StringHelper.KeepMatchesIntoSwitch(textBox.Text, findBox.Text);
		}
		void TemplateButtonButtonClick(object sender, EventArgs e)
		{
		
			var titles =	DatabaseUtils.GetInstance().GetTitleList(comboBox1.Text);
			foreach (var element in titles) {
				var a = DatabaseUtils.GetInstance().GetArticle(element);
				if (a.CreateAt.ToString().StartsWith("6")) {
					a.CreateAt =	Common.Extensions.GetTimeStampInMillis(new DateTime(a.CreateAt));
					
				}
				if (a.UpdateAt.ToString().StartsWith("6")) {
					a.UpdateAt =	Common.Extensions.GetTimeStampInMillis(new DateTime(a.UpdateAt));
					
				}
				
				DatabaseUtils.GetInstance().Update(a);
			}
		
//			textBox.SelectAll();
//			textBox.Paste();
//			var str = textBox.Text;
//			if (str.Contains("\\\"")) {
//				str = str.Replace("\\\"", "\"");
//			}
//			var items = Regex.Matches(str, "(?<=\")[^\"]*?(?=\",|\" FROM)")
//				.Cast<Match>().Select(i => i.Value).ToArray();
//			var ls = new List<string>();
//			var index = 0;
//			foreach (var element in items) {
//				ls.Add(string.Format("{0}=reader.GetString({1}),", element, index++));
//			}
//			textBox.Text += ls.ConcatenateLines();
		}
		async	void TestStripButtonClick(object sender, EventArgs e)
		{
//			try
//			{
//					var d = new List<dynamic>();
//			d.Add(1);
//			d.Add("");
//			d.Add("33355");
//			("http://127.0.0.1:5000" + "/api/playlist").PostJson(d.ToJsonString(), "J3ebj2iT");
//			}catch(Exception ex){
//				textBox.Text=ex.Message;
//			}
			//	await	MusicDelegate.GetUrlAsync("000z8oQD1vZWYg");
			try {
				Https.SetCertificatePolicy();
				var message =	("https://106.12.125.201" + "/api/touch").PostJson(textBox.Text.Trim(), "J3ebj2iT");
				textBox.Text += Environment.NewLine + message.FormatJson();
			} catch (Exception ex) {
				textBox.Text = ex.Message;
			}
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
		void TextBoxKeyUp(object sender, KeyEventArgs e)
		{
		}
		void TextBoxTextChanged(object sender, EventArgs e)
		{
			if (!this.Text.EndsWith("*"))
				this.Text += " *";
		}
		
		public void UpdateList()
		{
			listBox.Items.Clear();
			listBox.Items.AddRange(DatabaseUtils.GetInstance().GetTitleList(comboBox1.Text).ToArray());
		}
		
		
		
		void 参数ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var text = textBox.Text;
			var find = "[a-zA-Z0-9_]+(?= [a-zA-Z]+[,\\[ ])";
			if (string.IsNullOrWhiteSpace(text)) {
				return;
			}
			var matches = Regex.Matches(text, find).Cast<Match>().Select(i => i.Value);
			var ls1 = new List<string>();
			var ls2 = new List<string>();
			var ls3 = new List<string>();
			var index = 0;
			foreach (var element in matches) {
				ls1.Add(string.Format("{0}", element));
				ls2.Add(string.Format("{0}:=", element));
				ls3.Add(string.Format("${0}", index++));
			}
			var result = string.Join(",", ls1.Distinct())
			             + Environment.NewLine + Environment.NewLine
			             + "var array []interface{}\narray = append(array," + string.Join(",", ls1.Distinct()) + ")"
				//+"var array []interface{}\narray = append(array,\""+string.Join("\",\"",ls1.Distinct())+"\")"
			             + Environment.NewLine + Environment.NewLine
			             + ls2.ConcatenateLines()
			             + Environment.NewLine + Environment.NewLine
			             + string.Join(",", ls3.Distinct());
			if (!string.IsNullOrWhiteSpace(result))
				Clipboard.SetText(result);
		}
		async	void 测试ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Wins.OnClipboardDirectory(dir => {
				var files = Directory.GetFiles(dir, "*");
				var epubs = files.Where(i => i.EndsWith(".epub"));
				var dst = Path.Combine(dir, "EPUB");
				dst.CreateDirectoryIfNotExists();
				foreach (var element in epubs) {
					File.Move(element, Path.Combine(dst, element.GetFileName()));
					var f =	files.First(i => !i.EndsWith(".epub") && i.GetFileNameWithoutExtension() == element.GetFileNameWithoutExtension()
					        );
					File.Move(f, Path.Combine(dst, f.GetFileName()));
				}
			});
			//textBox.Text = await Https.GetUrlAsync(textBox.SelectedText.Trim());
			//textBox.Text+=Environment.NewLine+Https.Handshake(textBox.SelectedText).Trim();
		}
		void 查找ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var search = textBox.SelectedText;
			if (search.IsVacuum()) {
				search = findBox.Text;
			}
			if (search.IsVacuum())
				return;
			var regex = new Regex(search);
			var m = regex.Match(textBox.Text, textBox.SelectionStart + textBox.SelectionLength);
			if (!m.Success) {
				m = regex.Match(textBox.Text);
			}
			if (m.Success) {
				textBox.SelectionStart = m.Index;
				textBox.SelectionLength = m.Value.Length;
				textBox.ScrollToCaret();
			}
		}
		void 打开ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Logic.OpenLink(textBox);
		}
		void 大写序列ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var result =	Helper.StringTemplateUpperCaseSerial(textBox.Text, findBox.Text, replaceBox.Text);
			if (!string.IsNullOrWhiteSpace(result)) {
				Clipboard.SetText(result);
			}
		}
		void 代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = ListHelper.ToCodeList(textBox.SelectedText);
		}
		void 代码数字ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = ListHelper.ToCodeListByOrder(textBox.SelectedText);
		}
		
		
		void 导出代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var match =	Regex.Match(textBox.Text, "(?<=```)[^`]*?(?=```)").Value;
			var fileName =	textBox.Text.GetFirstReadable().SubstringAfter(':').Trim() + ".java";
			fileName.GetDesktopPath().WriteAllText(match);
		}
		
		
		
		void 第一个小写后面大写ToolStripMenuItemClick(object sender, EventArgs e)
		{
			{
				var text = textBox.Text;
				var find = findBox.Text;
				var replace = replaceBox.Text;
				if (string.IsNullOrWhiteSpace(text)
				    || string.IsNullOrWhiteSpace(find)
				    || string.IsNullOrWhiteSpace(replace)) {
					return;
				}
				var patternCount = Regex.Matches(replace, "(?<!\\{)\\{\\d+\\}(?!\\})").Count;
				var matches = Regex.Matches(text, find).Cast<Match>().Select(i => i.Value);
				var ls = new List<string>();
				foreach (var element in matches) {
					var elements = new string[patternCount];
					for (int i = 0; i < patternCount; i++) {
						if (i == 0) {
							elements[i] = element.Decapitalize();
						} else {
							elements[i] = element.ToUpper();//.Capitalize();
						}
					}
					ls.Add(string.Format(replaceBox.Text.Replace("\\n", "\n"), elements));
				}
				var result = ls.ConcatenateLines();
				if (!string.IsNullOrWhiteSpace(result))
					Clipboard.SetText(result);
			}
		}
		void 复制ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBox.SelectedText)) {
				textBox.SelectLine(true);
			}
			textBox.Copy();
		}
		void 复制代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var match =	Regex.Match(textBox.Text, "(?<=```)[^`]*?(?=```)").Value;
			Clipboard.SetText(match);
		}
		
		void 格式化ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var s = Clipboard.GetText().Trim();
			if (!string.IsNullOrWhiteSpace(s)) {
				s = Regex.Replace(s, "[\t\n\r]+", "");
				s = Regex.Replace(s, "\\s{2,}", " ");
				Clipboard.SetText(s.Replace("{", "{{").Replace("}", "}}"));
			}
		}
		void 行SQLToolStripMenuItemClick(object sender, EventArgs e)
		{
			var pieces = Regex.Split(textBox.Text, "\\-{3}");
			if (pieces.Length < 2) {
				return;
			}
			var pattern = Regex.Replace(pieces[1], "{(?![0-9]+})", "{{");
			pattern = Regex.Replace(pattern, "(?<!{[0-9]+)}", "}}");
			var items = Regex.Matches(pieces[0], "(?<=\")[^\"]*?(?=\",|\" FROM)")
				.Cast<Match>().Select(i => i.Value).ToArray();
			var list = new List<string>();
			foreach (var element in items) {
				list.Add(string.Format(pieces[1], element));
			}
			textBox.Text += "\r\n---\r\n" + list.ConcatenateLines();
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
			var pieces = Regex.Split(textBox.Text, "\\-{3}");
			if (pieces.Length < 2) {
				return;
			}
			var pattern = Regex.Replace(pieces[1], "{(?![0-9]+})", "{{");
			pattern = Regex.Replace(pattern, "(?<!{[0-9]+)}", "}}");
			var items = Regex.Matches(pieces[0], "(?<=\")[^\"\r\n]+(?=\")")
				.Cast<Match>().Select(i => i.Value).ToArray();
			var list = new List<string>();
			foreach (var element in items) {
				list.Add(string.Format(pieces[1], element));
			}
			textBox.Text += "\r\n---\r\n" + list.ConcatenateLines();
		}
		void 计算ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = textBox.SelectedText.Split('&').OrderBy(i => i).ConcatenateLines();
//			var interpreter = new DynamicExpresso. Interpreter();
//			textBox.SelectedText =	textBox.SelectedText + " = " + interpreter.Eval(textBox.SelectedText);
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
			var lines = textBox.Text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
				.Where(i => !(i.TrimEnd().EndsWith(".srt") || i.TrimEnd().EndsWith(".mp4")))
				.Select(i => i.SubstringAfterLast('/').TrimEnd(':'))
				.Distinct().OrderBy(i => i);
			textBox.Text = lines.ConcatenateLines();
//			var hd = new HtmlAgilityPack.HtmlDocument();
//			hd.LoadHtml(Clipboard.GetText());
//			var links = hd.DocumentNode.SelectNodes("//a");
//			var list = new List<String>();
//			if (links.Any()) {
//				foreach (var element in links) {
//					list.Add(element.InnerText.Trim());
//				}
//			}
//			list = list.Distinct().Where(i => Regex.IsMatch(i, "^[0-9]+\\."))
//				.Select(i => HtmlAgilityPack.HtmlEntity.DeEntitize(i))
//				.ToList();
//			var dir = "cpp".GetDesktopPath();
//			dir.CreateDirectoryIfNotExists();
//			foreach (var element in list) {
//				if (!char.IsDigit(element[1])) {
//					dir.Combine("0" + element.GetValidFileName() + ".cpp").WriteAllText("");
//				} else {
//					dir.Combine(element.GetValidFileName() + ".cpp").WriteAllText("");
//				}
//			}
		}
		void 删除ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			textBox.SelectLine();
			textBox.SelectedText = string.Empty;
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
		
		
		void 双引号ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var text = textBox.Text;
			var find = "(?<=\")\\w+(?=\")";
			if (string.IsNullOrWhiteSpace(text)) {
				return;
			}
			var matches = Regex.Matches(text, find).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			foreach (var element in matches) {
				ls.Add(string.Format("\"{0}\"", element));
			}
			var result = string.Join(",", ls.Distinct());
			if (!string.IsNullOrWhiteSpace(result))
				Clipboard.SetText(result);
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
	
		void 移动到ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var array = System.Buffers.ArrayPool<int>.Shared.Rent(213);
		}
		
		void 粘贴代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var start = textBox.Text.SubstringBefore("```") + "```";
			var middle = "```" + textBox.Text.SubstringAfter("```").SubstringAfter("```");
			textBox.Text = start + Environment.NewLine + Clipboard.GetText() + Environment.NewLine + middle;
//			var match =	Regex.Match(textBox.Text, "(?<=```).*?(?=```)").Value;
//			textBox.Text = Regex.Replace(textBox.Text, "(?<=```).*?(?=```)", "\r\n" + Clipboard.GetText() + "\r\n");
//			var value = Clipboard.GetText();[^`]
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
		
		
		void MainFormLoad(object sender, EventArgs e)
		{
			 
			BindEvents();
			Delegates.Inject(typeof(OtherDelegate), this);
			Delegates.Inject(typeof(FormatDelegate), this);
			Delegates.Inject(typeof(WebServerDelegate), this);
			
			Delegates.Inject(typeof(FindDelegate), this);
			;
			//Delegates.Inject(typeof(FTPDelegate), this);
			

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
			
			var findFileName = "find.txt".GetEntryPath();
			var ls = new List<string>();
			
			if (File.Exists(findFileName)) {
				findBox.Items.AddRange(File.ReadAllLines(findFileName, new UTF8Encoding(false)));
			}
		}
	
		
		
		void 保留行ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.Text = StringHelper.KeepMatchesArray(textBox.Text, findBox.Text);
	
		}
		void 移除空格ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.SelectedText = Regex.Replace(textBox.SelectedText, "\\s+", "");
		}
		void 导入ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Wins.OnClipboardFiles(ImportFiles);
		}
		private void ImportFiles(IEnumerable<string> files)
		{
			Regex regex = new Regex("\\.(?:java|css|cs|c|h|xml)$");
			foreach (var file in files) {
				if (!regex.IsMatch(file.GetFileName()))
					continue;
				var text = file.ReadAllText();
				Article article = new Article();
				article.Title = file.GetFileName();
				article.Content = string.Format("# {0} \r\n\r\n```\r\n\r\n{1}\r\n\r\n```\r\n\r\n", file.GetFileNameWithoutExtension(), text);
				article.UpdateAt = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
				
				Article c = DatabaseUtils.GetInstance().GetArticle(article.Title);
				if (c != null) {
					article.Id = c.Id;
					DatabaseUtils.GetInstance().Update(article);
					
				} else {
					article.CreateAt = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
					DatabaseUtils.GetInstance().Insert(article);
					
				}
			}
			UpdateList();
		}
		void 导入文件夹ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Wins.OnClipboardDirectory(dir => {
				// s\\.\\w+
				var files = Directory.GetFiles(dir).Where(
					            i => Regex.IsMatch(i, "\\.(?:java|css|cs|c|h|xml|txt)$"));
				ImportFiles(files);
			});
		}
		void 导出ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = "Common".GetDesktopPath();
			dir.CreateDirectoryIfNotExists();
			var articles =	DatabaseUtils.GetInstance().GetTitleContentList();
			foreach (var  article in articles) {
				if (Regex.IsMatch(article.Title, "s\\.\\w+")) {
					Path.Combine(dir, article.Title).WriteAllText(Regex.Match(article.Content, "(?<=```)[^`]*?(?=```)").Value);
				}
			}
		}
		void ComboBox1KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				UpdateList();
			}
		}
		void 代码ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			textBox.SelectedText =FormatDelegate. FormatCodeInternal(textBox.SelectedText);
		}
		
	
	}
}