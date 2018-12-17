namespace FileCompare
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	public partial   class MainForm: Form
	{
		public MainForm()
		{
			
			InitializeComponent();
			textBox1.AllowDrop = true;
			textBox2.AllowDrop = true;
			
		}
		IEnumerable<String> FormatCode(string value)
		{
			return value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i=>i.Trim());
			
		}
		string FormatNewLine(string value)
		{
			return Regex.Replace(value, "[\r\n]+", "\r\n");
		}
		void TextBox1DragDrop(object sender, DragEventArgs e)
		{
			string[] files = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files droppeds  
			if (files != null && files.Any()) {
				textBox1.Text = FormatNewLine(files.First().ReadAllText().StripComments());
				path1Box.Text = files.First();
			}
		}
		
		void TextBox1DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else
				e.Effect = DragDropEffects.None;  
		}
		void TextBox2DragDrop(object sender, DragEventArgs e)
		{
			string[] files = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files droppeds  
			if (files != null && files.Any()) {
				textBox2.Text = FormatNewLine(files.First().ReadAllText().StripComments());
				path2Box.Text = files.First();
			}
		}
		 
		void TextBox2DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else
				e.Effect = DragDropEffects.None;  
		}
		void CompareButtonButtonClick(object sender, EventArgs e)
		{
			var s1 = textBox1.Text;
			var s2 = textBox2.Text;
			
			if(s1.IsVacuum()&& path1Box.Text.IsFile()){
				s1=path1Box.Text.ReadAllText();
			}
			if(s2.IsVacuum()&& path2Box.Text.IsFile()){
				s2=path2Box.Text.ReadAllText();
			}
			if(s1.IsVacuum()||s2.IsVacuum())return;
			
			var ls1 = FormatCode(s1.StripComments());
			var ls2 = FormatCode(s2.StripComments());
			
			var count = 0;
			var ls = new List<string>();
			
			foreach (var element in ls1) {
				if (ls2.Count() > count) {
					if (element != ls2.ElementAt(count)) {
						ls.Add(string.Format("{0} {1} <===> {2}", count + 1, element, ls2.ElementAt(count)));
					}
					
					
				}
				count++;
			}
			textBox1.Text = string.Join(Environment.NewLine, ls);
		}
	}
	public static  class Utilities
	{
		public static bool IsReadable(this string value)
		{
			return  !string.IsNullOrWhiteSpace(value);
		}
		public static bool IsVacuum(this string value)
		{
			return  string.IsNullOrWhiteSpace(value);
		}
		public static bool IsFile(this string path){
			return File.Exists(path);
		}
		public static String ReadAllText(this String path)
		{
			var encoding = new UTF8Encoding(false);
			
			using (StreamReader sr = new StreamReader(path, encoding, true, 1024))
				return sr.ReadToEnd();
		}
		
		public	static string StripComments(this string code)
		{
			var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}
	}
}