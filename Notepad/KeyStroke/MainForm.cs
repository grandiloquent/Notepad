

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
		string _cProcessName = null;
		IntPtr _hWnd = IntPtr.Zero;
		bool _bSurveillance=false;
		#region
		
		public static void CPlusPlusSnippetsVSC()
		{
			WinForms.OnClipboardString((str) => {
				var s = Regex.Replace(str, "[\r\n]+", "").Trim();// changed
				s = Regex.Replace(s, "\\s{2,}", " ");// changed
				var ls = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray();
				// var matches = Regex.Matches(ls.First(), "[a-zA-Z]+").Cast<Match>().Select(i => i.Value.First().ToString()).ToArray();
				
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix", s.SubstringBefore('(').SubstringAfter(" ").SubstringAfter("*").SubstringAfter("WINAPI ").Trim());
				//obj.Add("prefix", string.Join("", matches).ToLower());
				obj.Add("body", ls.Select(i => Regex.Replace(i.EscapeString(), "[a-z _A-Z]+[ \\*]+(?=[a-zA-Z]+[, )])", "").SubstringAfter(" ").TrimStart('*')));// changed
				
				var r = new Dictionary<string,dynamic>();
				r.Add(ls.First(), obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return	sr.Substring(1, sr.Length - 2) + ",";
				
			});
		}
		
		public static void RunGenerateGccCommand(string f)
		{
			
//			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
//			if (!Directory.Exists(dir)) {
//				Directory.CreateDirectory(dir);
//			}
			var dir = Path.Combine(Path.GetDirectoryName(f), "bin");
			dir.CreateDirectoryIfNotExists();
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
			//var cmd = string.Format("/K gcc -Wall -g -finput-charset=GBK -fexec-charset=GBK \"{0}\" -o \"{1}\\{3}\" {2} && \"{1}\\{3}\" ", f, dir, arg, exe);
			var cmd = string.Format("/K gcc -Wall -g -finput-charset=UTF-8 -fexec-charset=GBK \"{0}\" -o \"{1}\\{3}\" {2} && \"{1}\\{3}\" ", f, dir, arg, exe);
		
			Process.Start("cmd", cmd);
				
			
		}
		
		public static void CFormat()
		{
			WinForms.OnClipboardString((str) => {
				var ls = Codes.FormatMethodList(Clipboard.GetText());
				var d = ls.Select(i => i.SubstringBefore(")") + ");").Where(i => i.IsReadable()).Select(i => i.Trim()).OrderBy(i => i.Split("(".ToArray(), 2).First().Split(' ').Last());
				var bodys = ls.OrderBy(i => Regex.Split(i.Split("(".ToArray(), 2).First(), "[: ]+").Last());
				return	string.Join("\n", d) + "\n\n\n" + string.Join("\n", string.Join("\n", bodys).Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries));
			});
		}
		
		#endregion
		int _key1 = 0;
		int _key2 = 0;
		int _key3 = 0;
		int _key7 = 0;
		int _key8 = 0;
		int _key10 = 0;
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
						WinForms.OnClipboardFile((f) => {
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
					//Codes.FormatVSCTypeDef();
					CPlusPlusSnippetsVSC();
				} else if (k == 0x79) {
//					if (_cFile != null)
//						Codes.FormatWithClangFormat(_cFile);
//					else {
//						WinForms.OnClipboardFile((f) => {
//							_cFile = f;
//						Codes.FormatWithClangFormat(f);
//						});
//					}
					var files =	Process.GetProcesses().Where(i => i.ProcessName == "cb_console_runner" || i.ProcessName == _cProcessName);
					foreach (var element in files) {
						element.Kill();
					}
					
					
				} 
			} else if (_bSurveillance&&(m.Msg == 0x100 || m.Msg == 0x101 || m.Msg == 0x104 || m.Msg == 0x105)) {
				
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
			
			if (_key7 == 0) {
				_key7 = 7;
				RegisterHotKey(Handle, _key7, 0, 0x79);
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
			_hWnd = hWnd;
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
				Clipboard.SetText(sb.ToString().Replace(";", ";" + Environment.NewLine));
			}
		}
		void CLangFormatToolStripMenuItemClick(object sender, EventArgs e)
		{

		}
		void 压缩AndroidToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory(WinForms.ZipAndroidProject);
		}
		void 压缩子目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.ZipDirectories();
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
		void 删除Aria2文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.RemoveAria2File();
		}
		void 清理HTMLSToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.CleanHtmls();
		}
		void WkhtmlToPdfToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText().Trim();
			if (!Directory.Exists(dir))
				return;

			foreach (var item in Directory.GetDirectories(dir)) {
				WinForms.InvokeWkhtmltopdf(item);
			}
		}
		void CodeBlocksToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardString((v) => {
				_cProcessName = v;
				return null;
			});
			if (_key10 == 0)
				_key10 = 10;
			RegisterHotKey(this.Handle, _key10, 0, (int)Keys.F10);
		}
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			_cFile = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
		}
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else
				e.Effect = DragDropEffects.None;
		}
		void 清空CodeBlocks项目ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
			                              
				var ls = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);
				foreach (var element in ls) {
					var f = element.GetFileName();
					if (f == "bin" || f == "obj")
						Directory.Delete(element, true);
				}
			});
		}
		void GBKToUTF8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinForms.OnClipboardDirectory((dir) => {
				var files = Directory.GetFiles(dir, "*").Where(i => Regex.IsMatch(i, "\\.(?:c|h|txt)"));
				foreach (var element in files) {
			                              		
					element.GbkToUTF8();
				}
			});
		}
		void 按键F8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_hWnd != IntPtr.Zero) {
				Win32.SetForegroundWindow(_hWnd);
				System.Threading.Tasks.Task.Factory.StartNew(() => {
					while (true) {
						SendKeys.SendWait("{F8}");
						System.Threading.Thread.Sleep(1000);
					}
				});
				
			}
		}
		void 监视按键ToolStripMenuItemClick(object sender, EventArgs e)
		{
			_bSurveillance=!_bSurveillance;
		}
	 
		
	}
}
