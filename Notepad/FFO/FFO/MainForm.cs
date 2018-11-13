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
	using System.Runtime.InteropServices;
	
	public partial   class MainForm: Form
	{
		int _key1 = 0;
		int _ks6 = 0;
		int _ks7 = 0;
		int _ks8 = 0;
		int _ks9 = 0;
		
		bool _ys = false;
		bool _b1 = false;
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
				NativeMethods.UnregisterHotKey(NativeMethods.HWND.Cast(Handle), _key1);
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
				NativeMethods.RegisterHotKey(NativeMethods.HWND.Cast(Handle), _key1, NativeMethods.MOD_CONTROL, (int)Keys.B);
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
					var intptr =	NativeMethods.WindowFromPoint(Control.MousePosition.X, Control.MousePosition.Y);
					Clipboard.SetText(intptr.ToString("X"));
				}
				if (_ys) {
					var hWnd = new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber));
					NativeMethods.SetForegroundWindow(new HandleRef(this, Handle));
					if (k == 0x36) {
						
						SendKeys.SendWait("{F3}");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("3");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("5");
						Thread.Sleep(4000);
						
					} else if (k == 0x37) {
						Point p1 = new Point(236, 131);
						Point p2 = new Point(237, 137);
						var c1 =	Screens.GetColorAt(p1, null);
						var c2 =	Screens.GetColorAt(p2, null);
						
						if ((c1.R == 102 && c1.G == 102 && c1.B == 85) && (c2.R == 204 && c2.G == 51 && c2.B == 0)) {
							SendKeys.SendWait("{F3}");
							Thread.Sleep(1000);
							SendKeys.SendWait("%");
							SendKeys.SendWait("9");
							Thread.Sleep(1000);
						}
						
						SendKeys.SendWait("{F3}");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("3");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("5");
						Thread.Sleep(4000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("9");
					} else if (k == 0x38) {
						SendKeys.SendWait("{F3}");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("3");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("6");
						Thread.Sleep(2000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("3");
						Thread.Sleep(1000);
						SendKeys.SendWait("%");
						SendKeys.SendWait("7");
						Thread.Sleep(3000);
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
		IntPtr GetHWnd(string str)
		{
			return  new IntPtr(int.Parse(str, System.Globalization.NumberStyles.HexNumber));
		}
		void StartButton1ButtonClick(object sender, EventArgs e)
		{
			if (!_b1) {
				var hWnd = new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber));
				var threshold = int.Parse(thresholdBox1.Text);
				var address = new IntPtr(int.Parse(memoryBox1.Text, System.Globalization.NumberStyles.HexNumber));
				int pid;
		 
			NativeMethods.GetWindowThreadProcessId(new HandleRef(this, new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber))), out pid);
 
				if (pid < 1) {
					return;
				}
				var hProcess = NativeMethods.OpenProcess(NativeMethods.PROCESS_QUERY_INFORMATION | NativeMethods.PROCESS_VM_READ, false, pid);
			
				Task.Run(() => {
					
					while (true) {
						if (Memory.ReadMemoryInt(hProcess, address) < threshold) {
							Keyboard.SendKey(hWnd, 0x71);
							Thread.Sleep(1000);
							Keyboard.SendKey(hWnd, 0x72);
							Thread.Sleep(1000);
						
							continue;
						}
						Thread.Sleep(1000);
					}
			        
				});
				_b1 = true;
			
			}
		}
		void StartButton2ButtonClick(object sender, EventArgs e)
		{
			var hWnd = new IntPtr(int.Parse(handleBox2.Text, System.Globalization.NumberStyles.HexNumber));
			var threshold = int.Parse(thresholdBox2.Text);
			var address = new IntPtr(int.Parse(memoryBox2.Text, System.Globalization.NumberStyles.HexNumber));
			int pid;
		 
			NativeMethods.GetWindowThreadProcessId(new HandleRef(this, new IntPtr(int.Parse(handleBox2.Text, System.Globalization.NumberStyles.HexNumber))), out pid);
			 
			if (pid < 1) {
				return;
			}
			var hProcess = NativeMethods.OpenProcess(NativeMethods.PROCESS_QUERY_INFORMATION | NativeMethods.PROCESS_VM_READ, false, pid);
			
			Task.Run(() => {
			 
				while (true) {
					if (Memory.ReadMemoryInt(hProcess, address) < threshold) {
						Keyboard.SendKey(hWnd, 0x77);
						
					}
					Thread.Sleep(5000);
				}
			        
			});
			this.startButton2.Enabled = false;
		}
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
	
			
			int pid;
		 
			NativeMethods.GetWindowThreadProcessId(new HandleRef(this, new IntPtr(int.Parse(handleBox2.Text, System.Globalization.NumberStyles.HexNumber))), out pid);
			 
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
			var address =	Memory.ScanSegments(pid, pattern) + 0x80;
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
						NativeMethods.PostMessage(hWnd, (int)NativeMethods.WindowMessage.WM_CHAR, (uint)str[i], 0);
					}
					Thread.Sleep(1000);
					         		
					Keyboard.SendKey(hWnd, (uint)Keys.Enter);
					         		
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
			
			int pid;
		 
			NativeMethods.GetWindowThreadProcessId(new HandleRef(this, new IntPtr(int.Parse(handleBox1.Text, System.Globalization.NumberStyles.HexNumber))), out pid);
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
			var address =	Memory.ScanSegments(pid, pattern) + 0x80;
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
				_ks8 = 18;
				_ks9 = 19;
				_ks6 = 16;
				var k = 0x36;
				for (int i = 16; i < 20; i++) {
					NativeMethods.RegisterHotKey(NativeMethods.HWND.Cast(Handle), i, 0, k++);
				}
				
				 
				/*
				 * [6] 集中+加速
				 * [7] 集中+加速+坐骑
				 * [8] 集中+祝福+防御
				 *  
				 */
				
			}
		}
		void KeyPressContinue(string h, object sender)
		{
			var hWnd = GetHWnd(h);
			
			var menuItem = sender as ToolStripMenuItem;
			uint keyString = 0;
			switch (menuItem.Text) {
				case "F8":
					keyString = (uint)Keys.F8;
					break;
			}
			if (keyString != 0) {
				Task.Factory.StartNew(() => {
					while (true) {
						Keyboard.SendKey(hWnd, keyString);
						Thread.Sleep(1000);
					}
				});
			}
			menuItem.Enabled = false;
		}
		void F8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			KeyPressContinue(handleBox1.Text, sender);
		
		}
		void F8Item2Click(object sender, EventArgs e)
		{
			KeyPressContinue(handleBox2.Text, sender);
		 
		}
	}
}