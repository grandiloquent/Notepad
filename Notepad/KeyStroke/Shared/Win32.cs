namespace Shared
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Threading;
	 
	public static  class Win32
	{
		public       const int MEM_COMMIT = 0x00001000;
		public       const int MEM_PRIVATE = 0x20000;
		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-registerhotkey
		public const int MOD_CONTROL = 0x0002;
		public const int MOUSEEVENTF_LEFTDOWN = 0x02;
		public const int MOUSEEVENTF_LEFTUP = 0x04;
		public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		public const int MOUSEEVENTF_RIGHTUP = 0x10;
		public       const int	PAGE_GUARD = 0x100;
		public    const int PAGE_READWRITE = 0x04;
		public   const int PROCESS_QUERY_INFORMATION = 0x0400;
		public    const int PROCESS_VM_OPERATION = 0x0008;
		public   const int PROCESS_VM_WRITE = 0x0020;
		public  const int PROCESS_WM_READ = 0x0010;
		public const int WM_CHAR = 0x0102;
		public const int WM_KEYDOWN = 0x100;
		public const int WM_KEYUP = 0x101;
		public const int WM_UNICHAR = 0x0109;

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

		public struct SYSTEM_INFO
		{
			public ushort processorArchitecture;
			ushort reserved;
			public uint pageSize;
			public IntPtr minimumApplicationAddress;
			public IntPtr maximumApplicationAddress;
			public IntPtr activeProcessorMask;
			public uint numberOfProcessors;
			public uint processorType;
			public uint allocationGranularity;
			public ushort processorLevel;
			public ushort processorRevision;
		}

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
   
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
    
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(ref Point lpPoint);
		[DllImport("kernel32.dll")]
		public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
		[DllImport("User32.dll", CharSet = CharSet.Auto)]  
		public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "PostMessage")]
		public static extern int PostMessage(IntPtr hwnd, int wMsg, uint wParam, uint lParam);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(int hProcess, 
			int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int SendMessage(IntPtr hwnd, int wMsg, uint wParam, uint lParam);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(Point pnt);
		[DllImport("kernel32.dll", SetLastError = true)]

		public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, 
			byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
		
		public static Color GetColorAt(Point location, Bitmap screenPixel)
		{
			if (screenPixel == null) {
				screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
			}
			using (Graphics gdest = Graphics.FromImage(screenPixel)) {
				using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
					IntPtr hSrcDC = gsrc.GetHdc();
					IntPtr hDC = gdest.GetHdc();
					int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
					gdest.ReleaseHdc();
					gsrc.ReleaseHdc();
				}
			}

			return screenPixel.GetPixel(0, 0);
		}
		
		
    
	

		public static void PostKey(IntPtr hWnd, uint key)
		{
			PostMessage(hWnd, WM_KEYDOWN, key, 0);
			Thread.Sleep(100);
			PostMessage(hWnd, WM_KEYUP, key, 0);
			
		}
		
		
	}
	
}