
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace Panda
{
	public partial class MainForm : Form
	{
		internal struct KEYBDINPUT
		{
			public ushort wVk;
			public ushort wScan;
			public uint dwFlags;
			public long time;
			public uint dwExtraInfo;
		}
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
		[StructLayout(LayoutKind.Explicit, Size = 28)]
		internal struct INPUT
		{
			[FieldOffset(0)] public uint type;
			[FieldOffset(4)] public KEYBDINPUT keyboardInput;
		}
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool SendMessage(IntPtr hWnd, int wMsg, uint wParam, uint lParam);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetDesktopWindow();
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetWindowDC(IntPtr window);
		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern uint GetPixel(IntPtr dc, int x, int y);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int ReleaseDC(IntPtr window, IntPtr dc);
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
		
		
		private const int INPUT_KEYBOARD = 1;
		private const ushort KEY_CONTROL = 0x11;
		private const uint KEYEVENTF_KEYUP = 0x2;
		private const uint MAPVK_VK_TO_VSC_EX = 0x04;
		private const int KEY_DOWN = 0x0100;
		private const int KEY_UP = 0x0101;
		private const uint MAPVK_VK_TO_VSC = 0x00;
		CancellationTokenSource mCts1;
		int mHotkeyF1;
		
	 
		
		public MainForm()
		{
			
			InitializeComponent();
			
			
		}
		protected override void WndProc(ref Message m)
		{
			//Debug.Write("Msg " + m.Msg + " WParam " + m.WParam + " LParam " + m.LParam + "\n");
//			if (m.Msg == 0x0312) {
//				var k = ((int)m.LParam >> 16) & 0xFFFF;
//				if (k == 112) {
//					if (mCts1 != null) {
//						mCts1.Cancel();
//						mCts1 = null;
//					} else {
//						mCts1 = new CancellationTokenSource();
//						var token = mCts1.Token;
//						var handle =	Process.GetProcesses().First(i => i.ProcessName == "qqffo").MainWindowHandle;
//						AutoFight(handle, token);
//					}
//				}
////				Debug.Write("Msg " + m.Msg + " WParam " + m.WParam + " LParam " + m.LParam + "\n");
////				vkButton.Text = m.WParam.ToString();
//			}
//			else 
				
				if(m.Msg== 0x100||m.Msg==0x101){
				Debug.Write("Msg " + m.Msg + " WParam " + m.WParam + " LParam " + m.LParam + "\n");
				
			}
			base.WndProc(ref m);
		}
		void QqxxzButtonButtonClick(object sender, EventArgs e)
		{
			
			var hWnd = this.Handle;
			//SendMessage(hWnd, 0x100, 0x11, 0x002C0001);
			PostMessage(hWnd, 0x100, 0x41, 1966081);
			PostMessage(hWnd, 0x101, 0x41, MapVirtualKey(0x41, 0x00));
			SendKeys.Send("a");
			//SendMessage(hWnd, 0x101, 0x11, 0xC02C0001);
	 
//			VKey k = new VKey(Messaging.VKeys.KEY_C, Messaging.VKeys.KEY_CONTROL, Messaging.ShiftType.CTRL);
//			var processFile = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "txt");
//			File.WriteAllLines(processFile, Process.GetProcesses().Select(i => i.ProcessName), new UTF8Encoding(false));
//			
//			var processes = Process.GetProcesses().Where(i => i.ProcessName == "notepad").First();
//			// XXZShell.exe
//			SendMessageAll(processes.MainWindowHandle, 65);
		}
		private static UInt64 GetLParam(short repeatCount, ushort key, byte extended, byte contextCode, byte previousState,
			byte transitionState)
		{
			var lParam = (UInt64)repeatCount;
			//uint scanCode = MapVirtualKey((uint)VKey, MAPVK_VK_TO_CHAR);
			var scanCode = GetScanCode(key);
			lParam += scanCode * 0x10000;
			lParam += (UInt64)(extended * 0x1000000);
			lParam += (UInt64)(contextCode * 2 * 0x10000000);
			lParam += (UInt64)(previousState * 4 * 0x10000000);
			lParam += (UInt64)(transitionState * 8 * 0x10000000);
			return lParam;
		}
		private static uint GetScanCode(ushort key)
		{
			return MapVirtualKey(key, MAPVK_VK_TO_VSC_EX);
		}
		public static bool SendMessage(IntPtr hWnd, ushort key, bool checkKeyboardState, int delay = 100)
		{
//			if (checkKeyboardState)
//				CheckKeyShiftState();
//			
			if (SendMessage(hWnd, KEY_DOWN, key, (uint)GetLParam(1, key, 0, 0, 0, 0)))
				return false;
			Thread.Sleep(delay);
			
//			if (SendMessage(hWnd, (int)Message.VM_CHAR, (uint)vKey.Vk, GetLParam(1, vKey.Vk, 0, 0, 0, 0)))
//				return false;
//			Thread.Sleep(delay);
//			
			if (SendMessage(hWnd, KEY_UP, key, (uint)GetLParam(1, key, 0, 0, 1, 1)))
				return false;
			Thread.Sleep(delay);
			return true;
		}

		private void SendMessageAll(IntPtr hWnd, ushort key, int delay = 100)
		{
			//SetForegroundWindow(hWnd);
			Thread.Sleep(delay);
			SendMessage(hWnd, 0x100, 0x11, 0x002C0001);
			SendMessage(hWnd, 0x100, 0x41, 0x002C0001);
			SendMessage(hWnd, 0x101, 0x41, 0xC02C0001);
			SendMessage(hWnd, 0x101, 0x11, 0xC02C0001);
				

//			uint intReturn;
//			var structInput = new INPUT {
//				type = INPUT_KEYBOARD,
//				keyboardInput = {
//					wScan = 0,
//					time = 0,
//					dwFlags = 0
//				}
//			};
//			
//			structInput.keyboardInput.wVk = KEY_CONTROL;
//			intReturn = SendInput(1, ref structInput, Marshal.SizeOf(new INPUT()));
//			Thread.Sleep(delay);
//			SendMessage(hWnd, KEY_DOWN, 0x41, 0);
//			Thread.Sleep(delay);
//			SendMessage(hWnd, KEY_UP, 0x41, 0);
//			Thread.Sleep(delay);			
//
//			structInput.keyboardInput.dwFlags = KEYEVENTF_KEYUP;
//			structInput.keyboardInput.wVk = KEY_CONTROL;
//			intReturn = SendInput(1, ref structInput, Marshal.SizeOf(new INPUT()));
//			Thread.Sleep(delay);
//			
				
		}
		void 其他ToolStripMenuItemClick(object sender, EventArgs e)
		{
			MessageBox.Show("123");
		}
		void 自动挂机ToolStripMenuItemClick(object sender, EventArgs e)
		{
			mHotkeyF1 = 1;
			RegisterHotKey(Handle, mHotkeyF1, 0, 112);
		
			
//			if (mCts1 != null) {
//				mCts1.Cancel();
//				mCts1 = null;
//			} else {
//				mCts1 = new CancellationTokenSource();
//				var token = mCts1.Token;
//				var handle =	Process.GetProcesses().First(i => i.ProcessName == "qqffo").MainWindowHandle;
//				AutoFight(handle, token);
//			}
//

			
			
		}
	
		void AutoFight(IntPtr handle, CancellationToken token)
		{
			var count = 0;
//			Task perdiodicTask = PeriodicTaskFactory.Start(() => {
//				PostMessage(handle, 0x100, 112, 0);
//				PostMessage(handle, 0x101, 112, 0);
//			}, 1000, 0, -1, -1, false, token);
//			Task t2 = PeriodicTaskFactory.Start(() => {
//			      		
//				INPUT structInput;
//				structInput = new INPUT();
//				structInput.type = INPUT_KEYBOARD;
//
//				// VKey down shift, ctrl, and/or alt
//				structInput.keyboardInput.wScan = 0;
//				structInput.keyboardInput.time = 0;
//				structInput.keyboardInput.dwFlags = 0;
//				// VKey down the actual VKey-code
//				structInput.keyboardInput.wVk = (ushort)0x12;
//				SendInput(1, ref structInput, Marshal.SizeOf(typeof(INPUT)));         
//			
//				SendMessage(handle, 0x100, 87, 0);
//				SendMessage(handle, 0x101, 87, 0);
//			                                    	
//				// VKey up the actual VKey-code
//				structInput.keyboardInput.dwFlags = KEYEVENTF_KEYUP;
//				SendInput(1, ref structInput, Marshal.SizeOf(typeof(INPUT)));                                	 
//				                       	
//			}, 3000, 0, -1, -1, false, token);
//			Task t2 = PeriodicTaskFactory.Start(() => {
//				var c = GetColorAt(411, 31);
//				if (c != 0) {
//					count++;
//					//tab
//					SendMessage(handle, 0x100, 9, 0);
//					SendMessage(handle, 0x101, 9, 0);
//				} else {
//					count--;
//				}
//				Debug.WriteLine("Count: "+count);
//				if (Math.Abs(count) > 1) {
//					count=0;
//					POINT lpPoint;
//					GetCursorPos(out lpPoint);
//					var x = lpPoint.X-50;
//					var y = lpPoint.Y-50;
//					var v = (uint)((y << 16) | (x & 0xFFFF));
//			
//					PostMessage(handle, 0x200, 0, v);
//					SendMessage(handle, 0x201, 0x1, v);
//					SendMessage(handle, 0x202, 0x1, v);
//				}
//			}, 3000, 0, -1, -1, false, token);
//			
 
		 new 	System.Threading.Timer((v) => {
			                       	//f2
				SendMessage(handle, 0x100, 113, 0);
				SendMessage(handle, 0x101, 113, 0);
			}, null, 0, 10000);
		 new	System.Threading.Timer((v) => {
			                       	//f10
				SendMessage(handle, 0x100, 117, 0);
				SendMessage(handle, 0x101, 117, 0);
			}, null, 0, 5000);
			 new System.Threading.Timer((v) => {
			                       	//f10
				SendMessage(handle, 0x100, 121, 0);
				SendMessage(handle, 0x101, 121, 0);
			}, null, 0, 30000);
			
			
		}
		void VkButtonKeyPress(object sender, KeyPressEventArgs e)
		{
			SendMessage(Handle, 0x100, (uint)e.KeyChar, 0);
		}
//		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
//		{
//			vkButton.Text = ((int)keyData).ToString();
//			 
//			return base.ProcessCmdKey(ref msg, keyData);
//		}
		public static uint GetColorAt(int x, int y)
		{
			IntPtr desk = GetDesktopWindow();
			IntPtr dc = GetWindowDC(desk);
			//int a = (int)GetPixel(dc, x, y);
			var a = GetPixel(dc, x, y);
			ReleaseDC(desk, dc);
			return a;
			//return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
		}
		 
		void QqzhxButtonButtonClick(object sender, EventArgs e)
		{
			
			
			
		}
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			var x = 700;
			var y = 369;
			var v = (uint)((y << 16) | (x & 0xFFFF));
			var handle =	Process.GetProcesses().First(i => i.ProcessName == "qqffo").MainWindowHandle;
			var xx = handle.ToInt32();
			PostMessage(handle, 0x200, 0, v);
			SendMessage(handle, 0x201, 0x1, v);
			SendMessage(handle, 0x202, 0x1, v);
			
		}
	}
}
