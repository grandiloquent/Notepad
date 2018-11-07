

using System;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using Shared;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Management;

namespace KeyStroke
{
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		#region
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public static implicit operator Point(POINT point)
			{
				return new Point(point.X, point.Y);
			}
		}
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pnt);

		public static Point GetCursorPosition()
		{
			POINT lpPoint;
			GetCursorPos(out lpPoint);
			//bool success = User32.GetCursorPos(out lpPoint);
			// if (!success)

			return lpPoint;
		}
		#endregion
		
		string _cFile = null;
		#region
		static sbyte[] unhex_table = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -1, -1, -1, -1, -1, -1
       , -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
		};

		public static int Convert(string hexNumber)
		{
			int decValue = unhex_table[(byte)hexNumber[0]];
			for (int i = 1; i < hexNumber.Length; i++) {
				decValue *= 16;
				decValue += unhex_table[(byte)hexNumber[i]];
			}
			return decValue;
		}
		public static void CPlusPlusSnippetsVSC()
		{
			OnClipboardString((str) => {
				var s = str.Trim();
				var ls = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray();
				var matches = Regex.Matches(ls.First(), "[a-zA-Z]+").Cast<Match>().Select(i => i.Value.First().ToString()).ToArray();
				
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix", string.Join("", matches).ToLower());
				obj.Add("body", ls.Select(i => i.EscapeString()));
				
				var r = new Dictionary<string,dynamic>();
				r.Add(ls.First(), obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return	sr.Substring(1, sr.Length - 2) + ",";
				
			});
		}
		public static void OnClipboardFile(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (File.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (File.Exists(ls[0])) {
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
		public static void RunGenerateGccCommand(string f)
		{
			
			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
			if (!Directory.Exists(dir)) {
				Directory.CreateDirectory(dir);
			}
				
			var arg = "";
			var argLines = f.ReadLines();
			foreach (var element in argLines) {
				if (element.IsVacuum())
					continue;
				if (element.StartsWith("// ")) {
					arg += element.Substring(3) + " ";
				} else
					break;
			}
			var exe = Path.GetFileNameWithoutExtension(f) + ".exe";
				
			try {
			 
				var ps =	Process.GetProcesses().Where(i => i.ProcessName == Path.GetFileNameWithoutExtension(f) || i.ProcessName == "cmd");
				if (ps.Any()) {
					foreach (var p in ps) {
						p.Kill();
					}
				}
			} catch {
			}
			var cmd = string.Format("/K gcc \"{0}\" -o \"{1}\\{3}\" {2} && \"{1}\\{3}\" ", f, dir, arg, exe);
			Process.Start("cmd", cmd);
				
			
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
		public static void CFormat()
		{
			OnClipboardString((str) => {
				var ls = FormatMethodList(Clipboard.GetText());
				var d = ls.Select(i => i.SubstringBefore(")") + ");").Where(i => i.IsReadable()).Select(i => i.Trim()).OrderBy(i => i.Split("(".ToArray(), 2).First().Split(' ').Last());
				var bodys = ls.OrderBy(i => Regex.Split(i.Split("(".ToArray(), 2).First(), "[: ]+").Last());
				return	string.Join("\n", d) + "\n\n\n" + string.Join("\n", string.Join("\n", bodys).Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries));
			});
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
		#endregion
		int _key1 = 0;
		int _key2 = 0;
		int _key3 = 0;
		int _key8 = 0;
		int _runType = 0;
		string _recordMouse = null;
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
		protected override void WndProc(ref Message m)
		{
			//Debug.Write("Msg " + m.Msg + " WParam " + m.WParam + " LParam " + m.LParam + "\n");
			if (m.Msg == 0x0312) {
				var k = ((int)m.LParam >> 16) & 0xFFFF;
				if (k == 0x75) {
					var point = GetCursorPosition();
					_recordMouse += string.Format("{{{0},{1}}},\n", point.X, point.Y);
				} else if (k == 0x78) {
					if (_runType == 1)
					if (_cFile != null)
						RunGenerateGccCommand(_cFile);
					else {
						OnClipboardFile((f) => {
							_cFile = f;
							RunGenerateGccCommand(f);
						});
					}
				} else if (k == 0x76) {
					if (_recordMouse == null) {
					
						this.globalEventProvider1.MouseDown += (o, e) => {
					
							if (e.Button == MouseButtons.Right) {
								_recordMouse += String.Format("{{{0},{1}}},\n", e.X, e.Y);
							}
						};
					} else {
					
						Clipboard.SetText(_recordMouse);
						_recordMouse = null;
					}
				} else if (k == 0x77) {
					CPlusPlusSnippetsVSC();
				}
			} else if (m.Msg == 0x100 || m.Msg == 0x101 || m.Msg == 0x104 || m.Msg == 0x105) {
				MessageBox.Show("Msg 0x" + m.Msg.ToString("X") + " WParam 0x" + m.WParam.ToString("X") + " LParam 0x" + m.LParam.ToString("X") + "\n");
				//Debug.WriteLine("Msg 0x" + m.Msg.ToString("X") + " WParam 0x" + m.WParam.ToString("X") + " LParam 0x" + m.LParam.ToString("X") + "\n");
				//Debug.WriteLine("PostMessage(hWnd,0x" + m.Msg.ToString("X") + " ,0x" + m.WParam.ToString("X") + " ,0x" + m.LParam.ToString("X") + ");\nSleep(100);\n");
				
			}
			//Debug.WriteLine("Msg 0x" + m.Msg.ToString("X") + " WParam 0x" + m.WParam.ToString("X") + " LParam 0x" + m.LParam.ToString("X") + "\n");
			
			base.WndProc(ref m);
		}
		void 获取当前坐标值热键FToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key1 == 0) {
				_key1 = 1 << 2;
				RegisterHotKey(Handle, _key1, 0, 0x75);
			} else {
				Clipboard.SetText(_recordMouse);
				_recordMouse = null;
			}
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (_key1 != 0) {
				UnregisterHotKey(Handle, _key1);
			}
			if (_key2 != 0) {
				UnregisterHotKey(Handle, _key2);
			}
			if (_key3 != 0) {
				UnregisterHotKey(Handle, _key3);
			}
			if (_key8 != 0) {
				UnregisterHotKey(Handle, _key8);
			}
		}
		void 编译CToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key2 == 0) {
				_key2 = 2 << 2;
				_runType = 1;
				RegisterHotKey(Handle, _key2, 0, 0x78);
			}
		}
	
		void ClearButtonClick(object sender, EventArgs e)
		{
			_cFile = null;
		}
		void 格式化C代码ToolStripMenuItemClick(object sender, EventArgs e)
		{
			CFormat();
		}
		void 取色器ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		void Aria2ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = "aria2c".GetDesktopPath();
			dir.CreateDirectoryIfNotExists();
			Process.Start(new ProcessStartInfo() {
				FileName = "aria2c",
				WorkingDirectory = dir,
				Arguments = Clipboard.GetText()
			});
		}
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			MessageBox.Show("123");
		}
		void 记录鼠标事件热键F7ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key3 == 0) {
				_key3 = 3;
				RegisterHotKey(Handle, _key3, 0, 0x76);
			}
		}
		void VSC代码段热键F8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_key8 == 0)
				_key8 = 8;
			RegisterHotKey(this.Handle, _key8, 0, (int)Keys.F8);
		}
		void 压缩目录不包含ZIP文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Shared.Win32.OnClipboardDirectory((dir) => {
				using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {
					var files = dir.GetFiles("zip", true);
					foreach (var element in files) {
						zip.AddFile(element, "");
					}
					var count = 0;
					var targetFileName = dir + ".zip";
					while (targetFileName.FileExists()) {
						targetFileName = dir + " V" + (++count).ToString().PadLeft(2, '0') + ".zip";
					}
					zip.Save(targetFileName);
				}
			});
		}
		void 压缩目录不包含ZIP文件ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			Shared.Win32.OnClipboardDirectory((dir) => {
				using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {
					var files = dir.GetFiles("zip", true);
					
					zip.AddDirectory(dir, "");
					var count = 0;
					var targetFileName = dir + ".zip";
					while (targetFileName.FileExists()) {
						targetFileName = dir + " V" + (++count).ToString().PadLeft(2, '0') + ".zip";
					}
					zip.Save(targetFileName);
				}
			});
		}
		void 重命名压缩文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Shared.Win32.OnClipboardDirectory((dir) => {
				var files = Directory.GetFiles(dir, "*.zip");
				var c = files.Select(i => i.ConvertToInt()).Max();
				foreach (var element in files) {
					var targetFileName = Path.Combine(dir, Regex.Replace(Path.GetFileNameWithoutExtension(element), "[0-9]+$", "") + (++c).ToString().PadLeft(2, '0') + ".zip");
					File.Move(element, targetFileName);
				}
			                                  	
			});
		}
		void 鼠标下窗口句柄ToolStripMenuItemClick(object sender, EventArgs e)
		{
			IntPtr hWnd = WindowFromPoint(Control.MousePosition);
			Clipboard.SetText("0X" + hWnd.ToString("X").PadLeft(8, '0'));
		}
		void MainFormDoubleClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
		}
		void CPUToolStripMenuItemClick(object sender, EventArgs e)
		{
			using (ManagementObjectSearcher win32Proc = new ManagementObjectSearcher("select * from Win32_Processor"), 
			       win32CompSys = new ManagementObjectSearcher("select * from Win32_ComputerSystem"),
			       win32Memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory")) {
				var sb = new StringBuilder();
				foreach (ManagementObject obj in win32Proc.Get()) {
//					var clockSpeed = obj["CurrentClockSpeed"].ToString();
//					var procName = obj["Name"].ToString();
//					var manufacturer = obj["Manufacturer"].ToString();
//					var	version = obj["Version"].ToString();
					sb.AppendLine(obj.GetText(TextFormat.Mof));
					//MessageBox.Show(string.Format(" 当前时钟频率： {0}\r\n 名称: {1}\r\n 制造商: {2}\r\n {3}",clockSpeed,procName,manufacturer,version));
				}
				
				foreach (ManagementObject obj in win32Memory.Get()) {
					
				 
					sb.AppendLine(obj.GetText(TextFormat.Mof));
					 
				}
				foreach (ManagementObject obj in win32CompSys.Get()) {
					
				 
					sb.AppendLine(obj.GetText(TextFormat.Mof));
					 
				}
				Clipboard.SetText(sb.ToString().Replace(";",";"+Environment.NewLine));
			}
		}
	 
		
	}
}
