

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
				
			try {
				var ps =	Process.GetProcesses().Where(i => i.ProcessName == "t" || i.ProcessName == "cmd");
				if (ps.Any()) {
					foreach (var p in ps) {
						p.Kill();
					}
				}
			} catch {
			}
			var exe=Path.GetFileNameWithoutExtension(f)+".exe";
			var cmd = string.Format("/K gcc \"{0}\" -o \"{1}\\{3}\" {2} && \"{1}\\{3}\" ", f, dir, arg,exe);
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
		int _runType = 0;
		
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
					Clipboard.SetText(string.Format("{0},{1}", point.X, point.Y));
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
			var dir="aria2c".GetDesktopPath();
			dir.CreateDirectoryIfNotExists();
			Process.Start(new ProcessStartInfo() {
				FileName = "aria2c",
				WorkingDirectory=dir,
				Arguments=Clipboard.GetText()
			});
		}
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			MessageBox.Show("123");
		}
	}
}
