namespace FFO
{
	using Shared;

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	public partial   class MainForm: Form
	{
		int _key1 = 0;
		int _ks7 = 0;
		int _ks9 = 0;
		
		bool _ys = false;
		
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
		void DestroyClawsButtonClick(object sender, EventArgs e)
		{
			
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (_key1 != 0) {
				Win32.UnregisterHotKey(this.Handle, _key1);
			}
			
			var settings1 = new Dictionary<string,string>();
			settings1.Add("hWnd", handleBox1.Text);
			settings1.Add("threshold", thresholdBox1.Text);
			settings1.Add("address", memoryBox1.Text);
			settings1.Add("chat", chatBox1.Text);
			
			var settings2 = new Dictionary<string,string>();
			settings2.Add("hWnd", handleBox2.Text);
			settings2.Add("threshold", thresholdBox2.Text);
			settings2.Add("address", memoryBox2.Text);
			
			
			var ls = new List<Dictionary<string,string>>();
			ls.Add(settings1);
			ls.Add(settings2);
			
			"settings.json".GetExePath().WriteAllText(Newtonsoft.Json.JsonConvert.SerializeObject(ls));
			
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			if (_key1 == 0) {
				_key1 = 1;
				Win32.RegisterHotKey(this.Handle, _key1, Win32.MOD_CONTROL, (int)Keys.B);
			}
			var str = "settings.json".GetExePath().ReadAllText();
			var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string,string>>>(str);
			var s1 = obj[0];
			handleBox1.Text = s1["hWnd"];
			thresholdBox1.Text = s1["threshold"];
			memoryBox1.Text = s1["address"];
			chatBox1.Text = s1["chat"];
			
			var s2 = obj[1];
			handleBox2.Text = s2["hWnd"];
			thresholdBox2.Text = s2["threshold"];
			memoryBox2.Text = s2["address"];
		}
		protected	override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0312) {
				var k = ((int)m.LParam >> 16) & 0xFFFF;
				if (k == (int)Keys.B) {
					var intptr =	Win32.WindowFromPoint(Control.MousePosition);
					Clipboard.SetText(intptr.ToString("X"));
				}
				if (_ys) {
					var hWnd = new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber));
					Win32.SetForegroundWindow(hWnd);
					if (k == 0x37) {
						SendKeys.SendWait("{F3}");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("3");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("5");
					} else if (k == 0x39) {
						while (true) {
							SendKeys.SendWait("{F10}");
							Thread.Sleep(1000);
						}
					}
					
				}
			} 		
			base.WndProc(ref m);
		}
		void StartButton1ButtonClick(object sender, EventArgs e)
		{
			var hWnd = new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber));
			var threshold = int.Parse(thresholdBox1.Text);
			var address = int.Parse(memoryBox1.Text, System.Globalization.NumberStyles.HexNumber);
			int pid;
		 
			Win32.GetWindowThreadProcessId(new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber)), out pid);
			if (pid < 1) {
				return;
			}
			var hProcess = Win32.OpenProcess(Win32.PROCESS_QUERY_INFORMATION | Win32.PROCESS_WM_READ, false, pid);
			
			Task.Run(() => {
				var h = hProcess.ToInt32();
				while (true) {
					if (Win32.ReadMemoryInt(h, address) < threshold) {
						Win32.PostKey(hWnd, 0x71);
						Thread.Sleep(1000);
						Win32.PostKey(hWnd, 0x72);
						Thread.Sleep(1000);
						
						continue;
					}
					Thread.Sleep(1000);
				}
			        
			});
			this.startButton1.Enabled = false;
		}
		void StartButton2ButtonClick(object sender, EventArgs e)
		{
			var hWnd = new IntPtr(int.Parse(handleBox2.Text, System.Globalization.NumberStyles.HexNumber));
			var threshold = int.Parse(thresholdBox2.Text);
			var address = int.Parse(memoryBox2.Text, System.Globalization.NumberStyles.HexNumber);
			int pid;
		 
			Win32.GetWindowThreadProcessId(new IntPtr(int.Parse(handleBox2.Text, System.Globalization.NumberStyles.HexNumber)), out pid);
			if (pid < 1) {
				return;
			}
			var hProcess = Win32.OpenProcess(Win32.PROCESS_QUERY_INFORMATION | Win32.PROCESS_WM_READ, false, pid);
			
			Task.Run(() => {
				var h = hProcess.ToInt32();
				while (true) {
					if (Win32.ReadMemoryInt(h, address) < threshold) {
						Win32.PostKey(hWnd, 0x77);
						Thread.Sleep(1000);
						
						continue;
					}
					Thread.Sleep(1000);
				}
			        
			});
			this.startButton2.Enabled = false;
		}
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
	
			if (memoryBox2.Text.IsReadable())
				return;
			int pid;
		 
			Win32.GetWindowThreadProcessId(new IntPtr(int.Parse(handleBox2.Text, System.Globalization.NumberStyles.HexNumber)), out pid);
			if (pid < 1) {
				return;
			}
			var pattern = new byte [] {
				0xFF,
				0xFF,
				0xFF,
				0xFF,
				0x50,
				0,
				0,
				0,
				0x26,
				0,
				0,
				0,
				0x5B
			};
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			var address =	Win32.ScanSegments(pid, pattern) + 0x80;
			stopWatch.Stop();
			decimal micro = stopWatch.Elapsed.Ticks / 10m;
			this.Text = string.Format("Execution time was {0:F1} microseconds. seconds{1}", micro, micro / 1000000);
			memoryBox2.Text = address.ToString("X");
		}
		void 发送文本消息ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var hWnd = new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber));
			var str = chatBox1.Text;
			if (str.IsVacuum())
				return;
			Task.Run(() => {

	         
				while (true) {
				
					for (int i = 0; i < str.Length; i++) {
						Win32.PostMessage(hWnd, Win32.WM_CHAR, (uint)str[i], 0);
					}
					Thread.Sleep(1000);
					         		
					Win32.PostKey(hWnd, (uint)Keys.Enter);
					         		
					Thread.Sleep(7000);
				}
			        
			});
			
		}
		void 勾魂利爪ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var hWnd = new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber));
			var dc =	new DestoryClaws(hWnd);
			dc.Start();
		}
		void 扫描血量内存地址ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (memoryBox1.Text.IsReadable())
				return;
			int pid;
		 
			Win32.GetWindowThreadProcessId(new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber)), out pid);
			if (pid < 1) {
				return;
			}
			var pattern = new byte [] {
				0xFF,
				0xFF,
				0xFF,
				0xFF,
				0x50,
				0,
				0,
				0,
				0x26,
				0,
				0,
				0,
				0x5B
			};
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			var address =	Win32.ScanSegments(pid, pattern) + 0x80;
			stopWatch.Stop();
			decimal micro = stopWatch.Elapsed.Ticks / 10m;
			this.Text = string.Format("Execution time was {0:F1} microseconds. seconds{1}", micro, micro / 1000000);
			memoryBox1.Text = address.ToString("X");
			
		}
		void 注册药师技能热键ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (_ks7 == 0) {
				_ys = true;
				_ks7 = 17;
				_ks9=19;
				Win32.RegisterHotKey(Handle, _ks9, 0, (int)Keys.D9);
				Win32.RegisterHotKey(Handle, _ks7, 0, (int)Keys.D7);
				
			}
		}
	}
}