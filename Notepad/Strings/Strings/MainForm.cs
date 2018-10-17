
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text.RegularExpressions;

namespace Strings
{
		
	public partial class MainForm : Form
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
		void ToGbkButtonClick(object sender, EventArgs e)
		{
			Extensions.OnClipboardString((value) => {
				textBox1.Text = value.ByteArrayToGbkString();
				return null;
			});
		}
		void TobyteButtonClick(object sender, EventArgs e)
		{
			Extensions.OnClipboardString((value) => {
				textBox1.Text = value.GbkStringToByteArray();
				return null;
			});
		}
		void GBKBYTEToolStripMenuItemClick(object sender, EventArgs e)
		{
			var str = textBox1.SelectedText.Trim();
			if (str.IsNotNullOrWhiteSpace()) {
				str = str.GbkStringToByteArray();
				Clipboard.SetText(str);
				textBox1.SelectedText += Environment.NewLine + str;
			}
			
		}
		void 计算表达式ToolStripMenuItemClick(object sender, EventArgs e)
		{
			try{
			var str = textBox1.SelectedText.Trim();
			if (str.IsNotNullOrWhiteSpace()) {
				str = Regex.Replace(str, "0x([0-9a-fA-F]+)", new MatchEvaluator((v) => v.Groups[1].Value.HexStringToInt().ToString()));
				// 1986BD11-1e6c0128
				textBox1.SelectedText += Environment.NewLine + str.EvaluateToDouble();
			}
			}catch{
				
			}
		}
		void HEXINTToolStripMenuItemClick(object sender, EventArgs e)
		{
			var str = textBox1.SelectedText.Trim();
			if (str.IsNotNullOrWhiteSpace()) {
				textBox1.SelectedText += Environment.NewLine + str.HexStringToInt();
			}
		}
		void 全选ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox1.SelectAll();
		}
		void 微秒到分钟ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var str = textBox1.SelectedText.Trim();
			if (str.IsNotNullOrWhiteSpace()) {
				var timespan = new TimeSpan(0, 0, 0, 0, int.Parse(str));
				textBox1.SelectedText += Environment.NewLine + timespan.ToString("hh\\:mm\\:ss\\:fffffff");
			}
		}
		void 粘贴ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox1.Paste();
	
		}
		void INTHEXToolStripMenuItemClick(object sender, EventArgs e)
		{
			var str = textBox1.SelectedText.Trim();
			if (str.IsNotNullOrWhiteSpace()) {
				textBox1.SelectedText += Environment.NewLine + str.ToHex();
			}
		}
	}
	#region 常用

	public static class Extensions
	{
		public static bool IsNotNullOrWhiteSpace(this string value)
		{
			return !string.IsNullOrWhiteSpace(value);
		}
		public static void OnClipboardString(Func<string,string> func)
		{
			try {
				var str = Clipboard.GetText().Trim();
				if (!string.IsNullOrWhiteSpace(str)) {
					str = func(str);
					if (!string.IsNullOrWhiteSpace(str))
						Clipboard.SetText(str);
				}
			} catch {
				
			}
		}
		
		public static string[] SplitString(this string value, string splitor)
		{
		
			return value.Split(splitor.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}
		
		public static string ToHex(this string value)
		{
			return	int.Parse(value).ToString("x");
		}
		public static string ToHex(this byte value)
		{
			return	value.ToString("x");
		}
		public static int HexStringToInt(this string value)
		{
			return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
		}
		public static byte HexStringToByte(this string value)
		{
			return byte.Parse(value, System.Globalization.NumberStyles.HexNumber);
		}
		public static string ByteArrayToGbkString(this string value)
		{
			var buffer =	value.SplitString(" ").Select(i => i.HexStringToByte()).ToArray();
			return System.Text.Encoding.GetEncoding("gbk").GetString(buffer);
		}
		public static string GbkStringToByteArray(this string value)
		{
			 
			return string.Join(" ", System.Text.Encoding.GetEncoding("gbk").GetBytes(value).Select(i => i.ToHex()));
		}
		public static double  EvaluateToDouble(this string value)
		{
			return Convert.ToDouble(new System.Data.DataTable().Compute(value, null));
			
		}
	}
		
	#endregion
}
