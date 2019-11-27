namespace StringCompare
{
	using Microsoft.Ajax.Utilities;
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using System.Diagnostics;

	using Common;
	public partial  class MainForm: Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void ClipboardButtonButtonClick(object sender, EventArgs e)
		{
			var s = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(s))
				return;
			var ls1 = textBox1.Text.FormatString().Lines();
			var ls2 =	s.FormatString().Lines();
			var ls3 = ls1.Except(ls2);
			textBox2.Text = ls3.ConcatenateLines();
		}
		public static void ClipboardText(Action<string> f)
		{
			var s = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(s))
				return;
			f(s);
		}
		
		
		public static String GenerateTemplate(string v)
		{
			var min = new	WebMarkupMin.Core.HtmlMinifier(new WebMarkupMin.Core.HtmlMinificationSettings() {
				RemoveCdataSectionsFromScriptsAndStyles = false,
				UseShortDoctype = false,
				PreserveCase = false,
				UseMetaCharsetTag = false,
				EmptyTagRenderMode = WebMarkupMin.Core.HtmlEmptyTagRenderMode.NoSlash,
				RemoveOptionalEndTags = false,
				CollapseBooleanAttributes = false,
				AttributeQuotesRemovalMode = WebMarkupMin.Core.HtmlAttributeQuotesRemovalMode.KeepQuotes,
				RemoveJsTypeAttributes = false,
				RemoveCssTypeAttributes = false                                  
			});
			v = min.Minify(v).MinifiedContent;
			var sb = new StringBuilder();
			var array = Regex.Split(v, "{{[^}]*?}}");
			var matches = Regex.Matches(v, "{{([^}]*?)}}").Cast<Match>().Select(i => i.Groups[1].Value).ToArray();
			var index = 0;
			foreach (var item in array) {
				sb.Append("\"" + item.Trim().Replace("\"", "\\\"") + "\"").Append('+').AppendLine().Append(index < matches.Length ? matches[index] : "").Append('+');
				index++;
			}
			return Regex.Replace(sb.ToString(), "[\\s\\+]+$", "") + ";";
		}
		//		void JsFileButtonButtonClick(object sender, EventArgs e)
		//		{
		//			ClipboardText(s => {
		//				var dir = @"C:\Users\psycho\Desktop\网站\static\javascripts";
		//				var fileName = WriteFile(s, dir, ".js");
		//				Clipboard.SetText(string.Format("<script type=\"text/javascript\" src=\"static/javascripts/{0}\"></script>", fileName));
		//			});
		//		}
		//
		public static void OnClipboardFile(Action<string> action)
		{
			var dir = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir)) {
				var files = Clipboard.GetFileDropList();
				if (files.Count > 0)
					dir = files[0];
			}
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir))
				return;
			action(dir);
		}
		public static void OnClipboardString(Func<string, string> func)
		{
			var value = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(value))
				return;
			var result = func(value);
			if (string.IsNullOrWhiteSpace(result))
				return;
			Clipboard.SetText(result);
		}
		
		
	 
		void 差集ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var s = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(s))
				return;
			var ls1 = textBox1.Text.FormatString().Lines();
			var ls2 =	s.FormatString().Lines();
			var ls3 = ls1.Union(ls2).Except(ls1.Intersect(ls2));
			textBox2.Text = ls3.ConcatenateLines();
		}
		void 格式化CSSToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox1.Text = textBox1.Text.FormatString();
		}
		 
		void 解压文件ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			OnClipboardFile(f => {
				var min = new Minifier();
				var r = min.MinifyStyleSheet(f.ReadAllText(), new CssSettings() {
					MinifyExpressions = false,
					RemoveEmptyBlocks = false,
					OutputMode = OutputMode.MultipleLines
				});
				f.AppendFileName("_prettyprint").WriteAllText(r);
			});
		}
		 
		
		void 压缩ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			Methods.OnClipboardDirectory(CreateFromJavaScriptDirectory);
		}
		void 压缩ToolStripMenuItemClick(object sender, EventArgs e)
		{
			OnClipboardString(s => {
				var min = new	WebMarkupMin.Core.HtmlMinifier(new WebMarkupMin.Core.HtmlMinificationSettings() {
					RemoveCdataSectionsFromScriptsAndStyles = false,
					UseShortDoctype = false,
					PreserveCase = false,
					UseMetaCharsetTag = false,
					EmptyTagRenderMode = WebMarkupMin.Core.HtmlEmptyTagRenderMode.NoSlash,
					RemoveOptionalEndTags = false,
					CollapseBooleanAttributes = false,
					AttributeQuotesRemovalMode = WebMarkupMin.Core.HtmlAttributeQuotesRemovalMode.KeepQuotes,
					RemoveJsTypeAttributes = false,
					RemoveCssTypeAttributes = false                                  
				});
				return min.Minify(s).MinifiedContent;
			});
		}
		
		public	static void CreateFromJavaScriptDirectory(String  dir)
		{
			if (dir != null) {
				var desitination = dir + " " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".zip";
				
				var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
					.Where(i => Path.GetExtension(i) != string.Empty
				            && i.IndexOf("\\videos\\") == -1
				            && !Regex.IsMatch(i, "\\.(?:vlog)$"));
				var length = dir.Length;
				
				using (var s = new FileStream(desitination, FileMode.Create))
				using (var a = new  ZipArchive(s, ZipArchiveMode.Create, true, Encoding.GetEncoding("gbk"))) {
					foreach (var element in files) {
						a.CreateEntryFromFile(element, element.Replace('\\', '/').Substring(length + 1));
					}
				}
				
			} else {
			}
			
		}
		public	static void CreateFromGoDirectory(String  dir)
		{
			if (dir != null) {
				var desitination = dir + " " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".zip";
				
				var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
					.Where(i => Path.GetExtension(i) != string.Empty
				            && i.IndexOf("\\videos\\") == -1
				            && !Regex.IsMatch(i, "\\.(?:vlog)$"));
				var length = dir.Length;
				
				using (var s = new FileStream(desitination, FileMode.Create))
				using (var a = new  ZipArchive(s, ZipArchiveMode.Create, true, Encoding.GetEncoding("gbk"))) {
					foreach (var element in files) {
						a.CreateEntryFromFile(element, element.Replace('\\', '/').Substring(length + 1));
					}
				}
				
			} else {
			}
			
		}
		 
		
		void MainFormLoad(object sender, EventArgs e)
		{
			var handle = NativeMethods.HWND.Cast(this.Handle);
			Methods.RegisterHotKey(handle, 1, 0, (int)Keys.F6);
			
			Delegates.Inject(typeof(OtherDelegate), this);
			
			Delegates.Inject(typeof(GoDelegate), this);
			Delegates.Inject(typeof(StringDelegate), this);
			Delegates.Inject(typeof(JavaScriptDelegate), this);
			Delegates.Inject(typeof(CSSDelegate), this);
			Delegates.Inject(typeof(ImageDelegate), this);
			Delegates.Inject(typeof(AndroidDelegate), this);
			Delegates.Inject(typeof(CSharpDelegate), this);
			Delegates.Inject(typeof(SplitDelegate), this);
			 
			 
			Methods.BindConvertToBytes(intByteToolStripMenuItem);
		}
		void JsFileButtonButtonClick(object sender, EventArgs e)
		{
	
		}
		void SublimeButtonClick(object sender, EventArgs e)
		{
	
		}
		
		
		
		
		
	
	}
}