
namespace Notepad
{
	
	using System;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	public static class Methods
		
		
	{
		public static string Combine(this string path, string fileName)
		{
			return Path.Combine(path, fileName);
		}

		public static string GetFirstReadable(this string value)
		{
			return  value.TrimStart().Split(new [] { '\n' }, 2).First().Trim();
		}
		public static void WriteAllText(this String path, String contents)
		{
			using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
				sw.Write(contents);
		}
		private	const string SplitMark = ">====>";
		
		public static void FormatH2(TextBox textBox)
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
		public static void FormatH3(TextBox textBox)
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
			textBox.SelectedText = "### ";
		}
	}
}
