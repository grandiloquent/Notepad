using System;
using System.Runtime.InteropServices;
namespace KeyCode
{
	public struct HWND
	{
		public IntPtr h;

		public static HWND Cast(IntPtr h)
		{
			HWND hTemp = new HWND();
			hTemp.h = h;
			return hTemp;
		}

		public static implicit operator IntPtr(HWND h)
		{
			return h.h;
		}

		public static HWND NULL {
			get {
				HWND hTemp = new HWND();
				hTemp.h = IntPtr.Zero;
				return hTemp;
			}
		}

		public static bool operator ==(HWND hl, HWND hr)
		{
			return hl.h == hr.h;
		}

		public static bool operator !=(HWND hl, HWND hr)
		{
			return hl.h != hr.h;
		}

		override public bool Equals(object oCompare)
		{
			HWND hr = Cast((HWND)oCompare);
			return h == hr.h;
		}

		public override int GetHashCode()
		{
			return (int)h;
		}
	}


	  
	[StructLayout(LayoutKind.Sequential)]
	public struct MSG
	{
		public IntPtr hwnd;
		public int message;
		public IntPtr wParam;
		public IntPtr lParam;
		public int time;
		// pt was a by-value POINT structure
		public int pt_x;
		public int pt_y;
	}
	
	public static class HotKeys
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool RegisterHotKey(HWND hWnd, int id, int fsModifiers, int vk);

		public const int WM_HOTKEY = 0x312;
		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern bool GetMessageW([In, Out] ref MSG msg, HandleRef hWnd, int uMsgFilterMin, int uMsgFilterMax);
		public static bool HotKey(HWND hWnd, int id, int fsModifiers, int vk)
		{
			bool result = RegisterHotKey(hWnd, id, fsModifiers, vk);
			int lastWin32Error = Marshal.GetLastWin32Error();

			if (!result) {
				//ThrowWin32ExceptionsIfError(lastWin32Error);
			}

			return result;
		}
	
	}
}