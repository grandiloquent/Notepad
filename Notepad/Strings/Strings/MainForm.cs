
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

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
			var vs = textBox1.SelectedText.Trim();
			if (vs.IsNotNullOrWhiteSpace()) {
				var	str = vs.GbkStringToByteArray();
				Clipboard.SetText(str);
				var hexList = System.Text.Encoding.GetEncoding("gbk").GetBytes(vs).Select(i => "0x" + i.ToHex());
				var ass = new List<String>();
				for (int i = 0; i < hexList.Count(); i++) {
					
					ass.Add(string.Format("gbk[{0}]={1};", i, hexList.ElementAt(i)));
				}
				ass.Add(string.Format("gbk[{0}]={1};", hexList.Count(), "0x0"));
				
				textBox1.SelectedText += Environment.NewLine + str + Environment.NewLine + "{" + string.Join(",", hexList) + "}" + Environment.NewLine + string.Join(Environment.NewLine, ass);
			}
			
		}
		void 计算表达式ToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				var str = textBox1.SelectedText.Trim();
				if (str.IsNotNullOrWhiteSpace()) {
					str = Regex.Replace(str, "0x([0-9a-fA-F]+)", new MatchEvaluator((v) => v.Groups[1].Value.HexStringToInt().ToString()));
					// 1986BD11-1e6c0128
					textBox1.SelectedText += Environment.NewLine + str.EvaluateToDouble();
				}
			} catch {
				
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
		void 数组逻辑比较ToolStripMenuItemClick(object sender, EventArgs e)
		{
			 
			var splited = textBox1.SelectedText.SplitString(" ");
			var count = -1;
			if (splited.Length > 2) {
				int.TryParse(splited[2], out count);
			}
			if (count == -1) {
				MessageBox.Show("格式: A B 5");
			} else {
				var list = new List<string>(count);
				for (int i = 0; i < count; i++) {
					list.Add(string.Format("{0}[{2}]=={1}[{2}]", splited[0], splited[1], i));
				}
				var logFormattor = "";
				var log = "";
				for (int i = 0; i < count; i++) {
					logFormattor += splited[0] + "[" + i + "]: " + "%x,";
					log += splited[0] + "[" + i + "],";
				}
				textBox1.SelectedText += Environment.NewLine + string.Join("&&\n", list) + Environment.NewLine +
					
				"printf(\"" + logFormattor.TrimEnd(',') + "\\n\"," + log.TrimEnd(',') + ");";
			}
		}
		void MemoryViewer到BYTE数组ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Extensions.OnClipboardString((v) => {
			                             
				return string.Join(",", v.SplitString(" ").Select(i => "0x" + i));
			});
		}
		void CformatButtonClick(object sender, EventArgs e)
		{
			Extensions.CFormat();
		}
		 
		void Bytetoint32ButtonClick(object sender, EventArgs e)
		{
			Extensions.OnClipboardString((v) => {
				var value = v.ByteToInt32();
				textBox1.Text = value.ToString();
				return null;
			});
		}
		void IntotbyteButtonClick(object sender, EventArgs e)
		{
			Extensions.OnClipboardString((v) => {
				var value = v.IntToBytes();
				var str = string.Join(" ", value.Select(i => i.ToHex()));
				textBox1.Text = str;
				return null;
			});
		}
		void BYTEINTToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox1.SelectedText += Environment.NewLine + textBox1.SelectedText.Trim().ByteToInt32();
		}
		void INTBYTEToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox1.SelectedText += Environment.NewLine + string.Join(" ", textBox1.SelectedText.Trim().IntToBytes().Select(i => i.ToHex()));
	
		}
		void ToolStripMenuItem2Click(object sender, EventArgs e)
		{
			var dictionary = new Dictionary<string,dynamic>();
			dictionary.Add("query", "*");
			dictionary.Add("page", 0);
			dictionary.Add("publishers", new String[]{ "Packt Publishing" });
			dictionary.Add("sort", "publication_date");
			var ls = new List<string>();
			for (int i = 0; i < 1000; i++) {
				dictionary["page"] = i;
				
				var str =	Newtonsoft.Json.JsonConvert.SerializeObject(dictionary).HttpPost("https://www.safaribooksonline.com/api/v2/search/");
				if (i == 200) {
					i = 200;
				}
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(str);

				if (obj.ContainsKey("results")) {
					Newtonsoft.Json.Linq.JArray list = obj["results"];
					if (list.Count < 1) {
						ls.Add(i.ToString());
						break;
					}
					foreach (Newtonsoft.Json.Linq.JObject element in list) {
						var item = element;
						if (item.ContainsKey("title")) {
							ls.Add(item["title"].ToString());
						}
					}
				} else {
					ls.Add(i.ToString());
					break;
				}
				
			}
			
			Clipboard.SetText(string.Join(Environment.NewLine, ls.OrderBy(i => i).Distinct()));
		}
		void CompareFileButtonButtonClick(object sender, EventArgs e)
		{
	
			var file1 = fileNameBox1.Text;
			var file2 = fileNameBox2.Text;
			
			if (File.Exists(file1) && File.Exists(file1)) {
				var ls=new List<string>();
				var ls1 = file1.ReadAllLinesWithTrim();
				var ls2 = file2.ReadAllLinesWithTrim();
				var count=0;
				foreach (var element in ls1) {
					if(ls2.Count()>count){
						if(element!=ls2.ElementAt(count)){
								ls.Add(string.Format("{0} {1} <===> {2}",count+1,element,ls2.ElementAt(count)));
						}
					
					
					}
						count++;
				}
				textBox1.Text=string.Join(Environment.NewLine,ls);
			}
		}
		void 倒序字符串ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox1.SelectedText=new String( textBox1.SelectedText.ToArray().Reverse().ToArray());
		}
			 
		 
	}
	#region 常用

	public static class Extensions
	{
		public static string[] ReadAllLinesWithTrim(this string path)
		{
			String line;
			List<String> lines = new List<String>();
			var encoding = new UTF8Encoding(false);
			using (StreamReader sr = new StreamReader(path, encoding))
				while ((line = sr.ReadLine()) != null) {
					if (string.IsNullOrWhiteSpace(line))
						continue;
					lines.Add(line.Trim());
				}
 
			return lines.ToArray();
		}
		public static string HttpPost(this string value, string url, string contentType = "application/json")
		{
			// Could not create SSL/TLS secure channel.
			
			System.Net.ServicePointManager.Expect100Continue = true;
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
			var req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
			req.ContentType = contentType;
			req.Method = "POST";
			using (var w = new System.IO.StreamWriter(req.GetRequestStream())) {
				w.Write(value);
				w.Flush();
				w.Close();
				
			}
			var rsp = (System.Net.HttpWebResponse)req.GetResponse();
			using (var r = new System.IO.StreamReader(rsp.GetResponseStream())) {
				return r.ReadToEnd();
			}
		}
		public static int ByteToInt32(this string value)
		{
			var buffer =	value.SplitString(" ").Select(i => i.HexStringToByte()).ToArray();
			if (buffer.Length == 2) {
				return BitConverter.ToInt16(buffer, 0);
			}
			return BitConverter.ToInt32(buffer, 0);
		}
		public static IEnumerable<string> FormatMethodList(string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (int i = 0; i < value.Length; i++) {
				sb.Append(value[i]);

				if (value[i] == '{') {
					count++;
				} else if (value[i] == '}') {
					count--;
					if (count == 0) {
						ls.Add(sb.ToString());
						sb.Clear();
					}
				}

			}
			//if (ls.Any())
			//{
			//    var firstLine = ls[0];
			//    ls.RemoveAt(0);
			//    ls.Add(firstLine.)

			//}
			return ls;
			//return ls.Select(i => i.Split(new char[] { '{' }, 2).First().Trim() + ";").OrderBy(i => i.Trim());

		}
		public static bool IsReadable(this string value)
		{
			return  !string.IsNullOrWhiteSpace(value);
		}
		public static void CFormat()
		{
			OnClipboardString((str) => {
				var ls = FormatMethodList(string.Join("\r\n", Clipboard.GetText().Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)));
				var d = ls.Select(i => i.SubstringBefore("{").TrimEnd() + ";").Where(i => i.IsReadable()).Select(i => i.Trim()).OrderBy(i => i.Split("(".ToArray(), 2).First().Split(' ').Last().Trim("*".ToCharArray()));
				var bodys = ls.OrderBy(i => Regex.Split(i.Split("(".ToArray(), 2).First(), "[: ]+").Last());
				return	string.Join("\r\n", d) + "\r\n\r\n\r\n" + string.Join("", bodys);
			});
		}
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBefore(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBeforeLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
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
		
			return value.Trim().Split(splitor.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}
		public static byte[] IntToBytes(this string value)
		{
			var bytes = BitConverter.GetBytes(int.Parse(value));
			//Array.Reverse(bytes);
			return bytes;
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
