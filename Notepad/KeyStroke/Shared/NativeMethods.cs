namespace Shared{	using Accessibility;
	using System;
	using System.Drawing;
	using System.Runtime.InteropServices;
	using System.Runtime.Versioning;
	using System.Text;
	public  static     class NativeMethods{public const int CHILDID_SELF = 0;
public const string Gdi32 = "gdi32.dll";
public static readonly IntPtr HTCLIENT = new IntPtr(1);
public static readonly IntPtr HTTRANSPARENT = new IntPtr(-1);
public const int INFINITE = unchecked((int)0xFFFFFFFF);
public const int INPUT_KEYBOARD = 1;
public const int INPUT_MOUSE = 0;
public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
public const string Kernel32 = "kernel32.dll";
internal const String KERNEL32 = "kernel32.dll";
public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
public const int KEYEVENTF_KEYUP = 0x0002;
public const int KEYEVENTF_SCANCODE = 0x0008;
public const int KEYEVENTF_UNICODE = 0x0004;
public const int LB_GETCURSEL = 0x0188;
public const int MEM_COMMIT = 0x1000;
public const int MEM_FREE = 0x10000;
public  const int MEM_PRIVATE = 0x20000;
public const int MEM_RELEASE = 0x8000;
public const int MEM_RESERVE = 0x2000;
internal const int MF_BYCOMMAND = 0x00000000;
internal const int MF_BYPOSITION = 0x00000400;
internal const int MF_DISABLED = 0x00000002;
internal const int MF_GRAYED = 0x00000001;
public const int MOUSEEVENTF_VIRTUALDESK = 0x4000;
public const int OBJID_CARET = -8;
public const int OBJID_CLIENT = -4;
public const int OBJID_MENU = -3;
public const int OBJID_SYSMENU = -1;
public const int OBJID_WINDOW = 0;
public  const int PAGE_GUARD = 0x100;
public   const int PAGE_READWRITE = 0x04;
public const int PM_REMOVE = 0x0001;
public const int PROCESS_DUP_HANDLE = 0x0040;
public const int PROCESS_QUERY_INFORMATION = 0x0400;
internal const int PROCESS_VM_OPERATION = 0x0008;
public const int PROCESS_VM_READ = 0x0010;
internal const int PROCESS_VM_WRITE = 0x0020;
public const int QS_KEY = 0x0001,
			QS_MOUSEMOVE = 0x0002,
			QS_MOUSEBUTTON = 0x0004,
			QS_POSTMESSAGE = 0x0008,
			QS_TIMER = 0x0010,
			QS_PAINT = 0x0020,
			QS_SENDMESSAGE = 0x0040,
			QS_HOTKEY = 0x0080,
			QS_ALLPOSTMESSAGE = 0x0100,
			QS_MOUSE = QS_MOUSEMOVE | QS_MOUSEBUTTON,
			QS_INPUT = QS_MOUSE | QS_KEY,
			QS_ALLEVENTS = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY,
			QS_ALLINPUT = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY | QS_SENDMESSAGE;
public const int SC_CLOSE = 0xF060;
public const int SC_MAXIMIZE = 0xF030;
public const int SC_MINIMIZE = 0xF020;
public const int SC_MOVE = 0xF010;
public const int SC_RESTORE = 0xF120;
public const int SC_SIZE = 0xF000;
public const int SMTO_ABORTIFHUNG = 2;
public const int STATE_SYSTEM_FOCUSED = 0x00000004;
public const int STATE_SYSTEM_UNAVAILABLE = 0x00000001;
public const int SW_RESTORE = 0x0009;
public const int SWP_NOACTIVATE = 0x0010;
public const int SWP_NOSIZE = 0x0001;
public const int SWP_NOZORDER = 0x0004;
public const string User32 = "user32.dll";
public const int VK_CONTROL = 0x11;
public const int VK_MENU = 0x12;
public const int VK_SHIFT = 0x10;
public const int WAIT_FAILED = unchecked((int)0xFFFFFFFF);
public const int WAIT_TIMEOUT = 0x00000102;
public const int WM_GETMINMAXINFO = 0x0024;
public const int WM_GETOBJECT = 0x003D;
public const int WM_GETTEXT = 0x000D;
public const int WM_GETTEXTLENGTH = 0x000E;
public const int WM_HOTKEY = 0x0312;
public const int WM_MDIACTIVATE = 0x0222;
public const int WM_NCHITTEST = 0x0084;
public const int WM_NULL = 0x0000;
public const int WM_QUIT = 0x0012;
public const int WM_SYSCOMMAND = 0x0112;
public const int WPF_SETMINPOSITION = 0x0001;
	
		/// <summary>
		/// SystemMetrics.  SM_*
		/// </summary>
		internal enum SM
		{
			CXSCREEN = 0,
			CYSCREEN = 1,
			CXVSCROLL = 2,
			CYHSCROLL = 3,
			CYCAPTION = 4,
			CXBORDER = 5,
			CYBORDER = 6,
			CXFIXEDFRAME = 7,
			CYFIXEDFRAME = 8,
			CYVTHUMB = 9,
			CXHTHUMB = 10,
			CXICON = 11,
			CYICON = 12,
			CXCURSOR = 13,
			CYCURSOR = 14,
			CYMENU = 15,
			CXFULLSCREEN = 16,
			CYFULLSCREEN = 17,
			CYKANJIWINDOW = 18,
			MOUSEPRESENT = 19,
			CYVSCROLL = 20,
			CXHSCROLL = 21,
			DEBUG = 22,
			SWAPBUTTON = 23,
			CXMIN = 28,
			CYMIN = 29,
			CXSIZE = 30,
			CYSIZE = 31,
			CXFRAME = 32,
			CXSIZEFRAME = CXFRAME,
			CYFRAME = 33,
			CYSIZEFRAME = CYFRAME,
			CXMINTRACK = 34,
			CYMINTRACK = 35,
			CXDOUBLECLK = 36,
			CYDOUBLECLK = 37,
			CXICONSPACING = 38,
			CYICONSPACING = 39,
			MENUDROPALIGNMENT = 40,
			PENWINDOWS = 41,
			DBCSENABLED = 42,
			CMOUSEBUTTONS = 43,
			SECURE = 44,
			CXEDGE = 45,
			CYEDGE = 46,
			CXMINSPACING = 47,
			CYMINSPACING = 48,
			CXSMICON = 49,
			CYSMICON = 50,
			CYSMCAPTION = 51,
			CXSMSIZE = 52,
			CYSMSIZE = 53,
			CXMENUSIZE = 54,
			CYMENUSIZE = 55,
			ARRANGE = 56,
			CXMINIMIZED = 57,
			CYMINIMIZED = 58,
			CXMAXTRACK = 59,
			CYMAXTRACK = 60,
			CXMAXIMIZED = 61,
			CYMAXIMIZED = 62,
			NETWORK = 63,
			CLEANBOOT = 67,
			CXDRAG = 68,
			CYDRAG = 69,
			SHOWSOUNDS = 70,
			CXMENUCHECK = 71,
			CYMENUCHECK = 72,
			SLOWMACHINE = 73,
			MIDEASTENABLED = 74,
			MOUSEWHEELPRESENT = 75,
			XVIRTUALSCREEN = 76,
			YVIRTUALSCREEN = 77,
			CXVIRTUALSCREEN = 78,
			CYVIRTUALSCREEN = 79,
			CMONITORS = 80,
			SAMEDISPLAYFORMAT = 81,
			IMMENABLED = 82,
			CXFOCUSBORDER = 83,
			CYFOCUSBORDER = 84,
			TABLETPC = 86,
			MEDIACENTER = 87,
			REMOTESESSION = 0x1000,
			REMOTECONTROL = 0x2001,
		}
		/// <summary>
		/// Window message values, WM_*
		/// </summary>
		internal enum WindowMessage
		{
			WM_NULL = 0x0000,
			WM_CREATE = 0x0001,
			WM_DESTROY = 0x0002,
			WM_MOVE = 0x0003,
			WM_SIZE = 0x0005,
			WM_ACTIVATE = 0x0006,
			WM_SETFOCUS = 0x0007,
			WM_KILLFOCUS = 0x0008,
			WM_ENABLE = 0x000A,
			WM_SETREDRAW = 0x000B,
			WM_SETTEXT = 0x000C,
			WM_GETTEXT = 0x000D,
			WM_GETTEXTLENGTH = 0x000E,
			WM_PAINT = 0x000F,
			WM_CLOSE = 0x0010,
			WM_QUERYENDSESSION = 0x0011,
			WM_QUIT = 0x0012,
			WM_QUERYOPEN = 0x0013,
			WM_ERASEBKGND = 0x0014,
			WM_SYSCOLORCHANGE = 0x0015,
			WM_ENDSESSION = 0x0016,
			WM_SHOWWINDOW = 0x0018,
			WM_CTLCOLOR = 0x0019,
			WM_WININICHANGE = 0x001A,
			WM_SETTINGCHANGE = 0x001A,
			WM_DEVMODECHANGE = 0x001B,
			WM_ACTIVATEAPP = 0x001C,
			WM_FONTCHANGE = 0x001D,
			WM_TIMECHANGE = 0x001E,
			WM_CANCELMODE = 0x001F,
			WM_SETCURSOR = 0x0020,
			WM_MOUSEACTIVATE = 0x0021,
			WM_CHILDACTIVATE = 0x0022,
			WM_QUEUESYNC = 0x0023,
			WM_GETMINMAXINFO = 0x0024,
			WM_PAINTICON = 0x0026,
			WM_ICONERASEBKGND = 0x0027,
			WM_NEXTDLGCTL = 0x0028,
			WM_SPOOLERSTATUS = 0x002A,
			WM_DRAWITEM = 0x002B,
			WM_MEASUREITEM = 0x002C,
			WM_DELETEITEM = 0x002D,
			WM_VKEYTOITEM = 0x002E,
			WM_CHARTOITEM = 0x002F,
			WM_SETFONT = 0x0030,
			WM_GETFONT = 0x0031,
			WM_SETHOTKEY = 0x0032,
			WM_GETHOTKEY = 0x0033,
			WM_QUERYDRAGICON = 0x0037,
			WM_COMPAREITEM = 0x0039,
			WM_GETOBJECT = 0x003D,
			WM_COMPACTING = 0x0041,
			WM_COMMNOTIFY = 0x0044,
			WM_WINDOWPOSCHANGING = 0x0046,
			WM_WINDOWPOSCHANGED = 0x0047,
			WM_POWER = 0x0048,
			WM_COPYDATA = 0x004A,
			WM_CANCELJOURNAL = 0x004B,
			WM_NOTIFY = 0x004E,
			WM_INPUTLANGCHANGEREQUEST = 0x0050,
			WM_INPUTLANGCHANGE = 0x0051,
			WM_TCARD = 0x0052,
			WM_HELP = 0x0053,
			WM_USERCHANGED = 0x0054,
			WM_NOTIFYFORMAT = 0x0055,
 
			WM_CONTEXTMENU = 0x007B,
			WM_STYLECHANGING = 0x007C,
			WM_STYLECHANGED = 0x007D,
			WM_DISPLAYCHANGE = 0x007E,
			WM_GETICON = 0x007F,
			WM_SETICON = 0x0080,
			WM_NCCREATE = 0x0081,
			WM_NCDESTROY = 0x0082,
			WM_NCCALCSIZE = 0x0083,
			WM_NCHITTEST = 0x0084,
			WM_NCPAINT = 0x0085,
			WM_NCACTIVATE = 0x0086,
			WM_GETDLGCODE = 0x0087,
			WM_SYNCPAINT = 0x0088,
			WM_MOUSEQUERY = 0x009B,
			WM_NCMOUSEMOVE = 0x00A0,
			WM_NCLBUTTONDOWN = 0x00A1,
			WM_NCLBUTTONUP = 0x00A2,
			WM_NCLBUTTONDBLCLK = 0x00A3,
			WM_NCRBUTTONDOWN = 0x00A4,
			WM_NCRBUTTONUP = 0x00A5,
			WM_NCRBUTTONDBLCLK = 0x00A6,
			WM_NCMBUTTONDOWN = 0x00A7,
			WM_NCMBUTTONUP = 0x00A8,
			WM_NCMBUTTONDBLCLK = 0x00A9,
			WM_NCXBUTTONDOWN = 0x00AB,
			WM_NCXBUTTONUP = 0x00AC,
			WM_NCXBUTTONDBLCLK = 0x00AD,
			WM_INPUT = 0x00FF,
			WM_KEYFIRST = 0x0100,
			WM_KEYDOWN = 0x0100,
			WM_KEYUP = 0x0101,
			WM_CHAR = 0x0102,
			WM_DEADCHAR = 0x0103,
 
			WM_SYSKEYDOWN = 0x0104,
			WM_SYSKEYUP = 0x0105,
			WM_SYSCHAR = 0x0106,
			WM_SYSDEADCHAR = 0x0107,
			WM_KEYLAST = 0x0108,
			WM_IME_STARTCOMPOSITION = 0x010D,
			WM_IME_ENDCOMPOSITION = 0x010E,
			WM_IME_COMPOSITION = 0x010F,
			WM_IME_KEYLAST = 0x010F,
			WM_INITDIALOG = 0x0110,
 
			WM_COMMAND = 0x0111,
			WM_SYSCOMMAND = 0x0112,
			WM_TIMER = 0x0113,
			WM_HSCROLL = 0x0114,
			WM_VSCROLL = 0x0115,
			WM_INITMENU = 0x0116,
			WM_INITMENUPOPUP = 0x0117,
			WM_MENUSELECT = 0x011F,
			WM_MENUCHAR = 0x0120,
			WM_ENTERIDLE = 0x0121,
			WM_UNINITMENUPOPUP = 0x0125,
			WM_CHANGEUISTATE = 0x0127,
			WM_UPDATEUISTATE = 0x0128,
			WM_QUERYUISTATE = 0x0129,
			WM_CTLCOLORMSGBOX = 0x0132,
			WM_CTLCOLOREDIT = 0x0133,
			WM_CTLCOLORLISTBOX = 0x0134,
			WM_CTLCOLORBTN = 0x0135,
			WM_CTLCOLORDLG = 0x0136,
			WM_CTLCOLORSCROLLBAR = 0x0137,
			WM_CTLCOLORSTATIC = 0x0138,
 
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSEFIRST = WM_MOUSEMOVE,
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_LBUTTONDBLCLK = 0x0203,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205,
			WM_RBUTTONDBLCLK = 0x0206,
			WM_MBUTTONDOWN = 0x0207,
			WM_MBUTTONUP = 0x0208,
			WM_MBUTTONDBLCLK = 0x0209,
			WM_MOUSEWHEEL = 0x020A,
			WM_XBUTTONDOWN = 0x020B,
			WM_XBUTTONUP = 0x020C,
			WM_XBUTTONDBLCLK = 0x020D,
			WM_MOUSEHWHEEL = 0x020E,
			WM_MOUSELAST = WM_MOUSEHWHEEL,
			WM_PARENTNOTIFY = 0x0210,
			WM_ENTERMENULOOP = 0x0211,
			WM_EXITMENULOOP = 0x0212,
			WM_NEXTMENU = 0x0213,
			WM_SIZING = 0x0214,
			WM_CAPTURECHANGED = 0x0215,
			WM_MOVING = 0x0216,
			WM_POWERBROADCAST = 0x0218,
			WM_DEVICECHANGE = 0x0219,
			WM_POINTERDEVICECHANGE = 0X0238,
			WM_POINTERDEVICEINRANGE = 0x0239,
			WM_POINTERDEVICEOUTOFRANGE = 0x023A,
			WM_POINTERUPDATE = 0x0245,
			WM_POINTERDOWN = 0x0246,
			WM_POINTERUP = 0x0247,
			WM_POINTERENTER = 0x0249,
			WM_POINTERLEAVE = 0x024A,
			WM_POINTERACTIVATE = 0x024B,
			WM_POINTERCAPTURECHANGED = 0x024C,
			WM_IME_SETCONTEXT = 0x0281,
			WM_IME_NOTIFY = 0x0282,
			WM_IME_CONTROL = 0x0283,
			WM_IME_COMPOSITIONFULL = 0x0284,
			WM_IME_SELECT = 0x0285,
			WM_IME_CHAR = 0x0286,
			WM_IME_REQUEST = 0x0288,
			WM_IME_KEYDOWN = 0x0290,
			WM_IME_KEYUP = 0x0291,
			WM_MDICREATE = 0x0220,
			WM_MDIDESTROY = 0x0221,
			WM_MDIACTIVATE = 0x0222,
			WM_MDIRESTORE = 0x0223,
			WM_MDINEXT = 0x0224,
			WM_MDIMAXIMIZE = 0x0225,
			WM_MDITILE = 0x0226,
			WM_MDICASCADE = 0x0227,
			WM_MDIICONARRANGE = 0x0228,
			WM_MDIGETACTIVE = 0x0229,
			WM_MDISETMENU = 0x0230,
			WM_ENTERSIZEMOVE = 0x0231,
			WM_EXITSIZEMOVE = 0x0232,
			WM_DROPFILES = 0x0233,
			WM_MDIREFRESHMENU = 0x0234,
			WM_MOUSEHOVER = 0x02A1,
			WM_NCMOUSELEAVE = 0x02A2,
			WM_MOUSELEAVE = 0x02A3,
        
			WM_WTSSESSION_CHANGE = 0x02b1,
 
			WM_TABLET_DEFBASE = 0x02C0,
			WM_DPICHANGED = 0x02E0,
			WM_TABLET_MAXOFFSET = 0x20,
 
			WM_TABLET_ADDED = WM_TABLET_DEFBASE + 8,
			WM_TABLET_DELETED = WM_TABLET_DEFBASE + 9,
			WM_TABLET_FLICK = WM_TABLET_DEFBASE + 11,
			WM_TABLET_QUERYSYSTEMGESTURESTATUS = WM_TABLET_DEFBASE + 12,
 
			WM_CUT = 0x0300,
			WM_COPY = 0x0301,
			WM_PASTE = 0x0302,
			WM_CLEAR = 0x0303,
			WM_UNDO = 0x0304,
			WM_RENDERFORMAT = 0x0305,
			WM_RENDERALLFORMATS = 0x0306,
			WM_DESTROYCLIPBOARD = 0x0307,
			WM_DRAWCLIPBOARD = 0x0308,
			WM_PAINTCLIPBOARD = 0x0309,
			WM_VSCROLLCLIPBOARD = 0x030A,
			WM_SIZECLIPBOARD = 0x030B,
			WM_ASKCBFORMATNAME = 0x030C,
			WM_CHANGECBCHAIN = 0x030D,
			WM_HSCROLLCLIPBOARD = 0x030E,
			WM_QUERYNEWPALETTE = 0x030F,
			WM_PALETTEISCHANGING = 0x0310,
			WM_PALETTECHANGED = 0x0311,
			WM_HOTKEY = 0x0312,
			WM_PRINT = 0x0317,
			WM_PRINTCLIENT = 0x0318,
			WM_APPCOMMAND = 0x0319,
			WM_THEMECHANGED = 0x031A,
 
			WM_DWMCOMPOSITIONCHANGED = 0x031E,
			WM_DWMNCRENDERINGCHANGED = 0x031F,
			WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,
			WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
			WM_HANDHELDFIRST = 0x0358,
			WM_HANDHELDLAST = 0x035F,
			WM_AFXFIRST = 0x0360,
			WM_AFXLAST = 0x037F,
			WM_PENWINFIRST = 0x0380,
			WM_PENWINLAST = 0x038F,
 
			#region Windows 7
			WM_DWMSENDICONICTHUMBNAIL = 0x0323,
			WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,
			#endregion
 
			WM_USER = 0x0400,
 
			WM_APP = 0x8000,
		}
		public delegate void WinEventProcDef(int winEventHook, int eventId, IntPtr hwnd, int idObject, int idChild, int eventThread, uint eventTime);








 

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
 
			public static bool operator==(HWND hl, HWND hr)
			{
				return hl.h == hr.h;
			}
 
			public static bool operator!=(HWND hl, HWND hr)
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
		public struct INPUT
		{
			public int type;
			public INPUTUNION union;
		};
 
		[StructLayout(LayoutKind.Explicit)]
		public struct INPUTUNION
		{
			[FieldOffset(0)] public MOUSEINPUT mouseInput;
			[FieldOffset(0)] public KEYBDINPUT keyboardInput;
		};
 
		[StructLayout(LayoutKind.Sequential)]
		public struct KEYBDINPUT
		{
			public short wVk;
			public short wScan;
			public int dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		};
	
		[StructLayout(LayoutKind.Sequential)]
		public struct MEMORY_BASIC_INFORMATION
		{
			internal IntPtr BaseAddress;
			internal IntPtr AllocationBase;
			internal uint AllocationProtect;
			internal UIntPtr RegionSize;
			internal uint State;
			internal uint Protect;
			internal uint Type;
		}
		[StructLayout(LayoutKind.Sequential)]
		internal struct MENUBARINFO
		{
			internal int cbSize;
			internal NativeMethods.RECT rcBar;
			internal IntPtr hMenu;
			internal NativeMethods.HWND hwndMenu;
			internal int focusFlags;
		}
 
		[StructLayout(LayoutKind.Sequential)]
		public struct MINMAXINFO
		{
			public NativeMethods.POINT ptReserved;
			public NativeMethods.POINT ptMaxSize;
			public NativeMethods.POINT ptMaxPosition;
			public NativeMethods.POINT ptMinTrackSize;
			public NativeMethods.POINT ptMaxTrackSize;
		};
 
		[StructLayout(LayoutKind.Sequential)]
		public struct MOUSEINPUT
		{
			public int dx;
			public int dy;
			public int mouseData;
			public int dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		};
 
		[StructLayout(LayoutKind.Sequential)]
		public struct MSG
		{
			public NativeMethods.HWND hwnd;
			public int message;
			public IntPtr wParam;
			public IntPtr lParam;
			public int time;
			public int pt_x;
			public int pt_y;
		}
 
 
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
 
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}
 
		private struct POINTSTRUCT
		{
			public int x;
			public int y;
 
			public POINTSTRUCT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
 
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}
 
			public bool IsEmpty {
				get {
					return left >= right || top >= bottom;
				}
			}
		}
		[StructLayout(LayoutKind.Sequential)]
		internal struct SYSTEM_INFO
		{
			internal ushort wProcessorArchitecture;
			internal ushort wReserved;
			internal int dwPageSize;
			internal IntPtr lpMinimumApplicationAddress;
			internal IntPtr lpMaximumApplicationAddress;
			internal IntPtr dwActiveProcessorMask;
			internal int dwNumberOfProcessors;
			internal int dwProcessorType;
			internal int dwAllocationGranularity;
			internal short wProcessorLevel;
			internal short wProcessorRevision;
		}
 
		[StructLayout(LayoutKind.Sequential)]
		public struct WINDOWPLACEMENT
		{
			public int length;
			public int flags;
			public int showCmd;
			public NativeMethods.POINT ptMinPosition;
			public NativeMethods.POINT ptMaxPosition;
			public NativeMethods.RECT rcNormalPosition;
		};
		[DllImport("oleacc.dll")]
		public static extern int AccessibleObjectFromEvent(
			IntPtr hwnd,
			int idObject,
			int idChild,
			ref IAccessible ppvObject,
			ref Object varChild);
		[DllImport(Gdi32, SetLastError = true, ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		[ResourceExposure(ResourceScope.None)]
		public static extern int BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight,
			HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);
 
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DeleteObject(IntPtr hrgn);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DispatchMessage(
			ref MSG msg);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern short GetAsyncKeyState(int nVirtKey);
		[DllImport(KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
		[ResourceExposure(ResourceScope.Process)]
		internal static extern uint GetCurrentProcessId();
     
		[DllImport(User32, ExactSpelling = true, CharSet = CharSet.Auto)]
		[ResourceExposure(ResourceScope.None)]
		public static extern bool GetCursorPos([In, Out] POINT pt);
 
		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		internal static extern int GetMenuState(IntPtr hMenu, int uId, int uFlags);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetMessage(
			ref MSG msg, NativeMethods.HWND hwnd, int nMsgFilterMin, int nMsgFilterMax);
 
		[DllImport("oleacc.dll", SetLastError = true)]
		internal static extern IntPtr GetProcessHandleFromHwnd(IntPtr hwnd);
		[DllImport(KERNEL32, SetLastError = true)]
		[ResourceExposure(ResourceScope.None)]
		internal static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);
		
		[DllImport(User32, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		[ResourceExposure(ResourceScope.None)]
		public static extern int GetSystemMetrics(int nIndex);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetWindowPlacement(NativeMethods.HWND hwnd, ref WINDOWPLACEMENT wp);
	
		
		[DllImport(User32, CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
		[ResourceExposure(ResourceScope.Machine)]
		public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);
 
		[DllImport("user32.dll", EntryPoint = "WindowFromPhysicalPoint", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern IntPtr IntWindowFromPhysicalPoint(POINTSTRUCT pt);
  
		[DllImport("user32.dll", EntryPoint = "WindowFromPoint", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern IntPtr IntWindowFromPoint(POINTSTRUCT pt);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int MapVirtualKey(int nVirtKey, int nMapType);
        
		[DllImport("msvcrt.dll")]
		public  static extern IntPtr memcmp(byte[] b1, byte[] b2, IntPtr count);
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
 
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int MsgWaitForMultipleObjects(int nCount, IntPtr[] handles, bool fWaitAll, int dwMilliseconds, int dwWakeMask);
 
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool fInherit, int dwProcessId);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage(
			ref MSG msg, NativeMethods.HWND hwnd, int nMsgFilterMin, int nMsgFilterMax, int wRemoveMsg);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PostMessage(
			NativeMethods.HWND hWnd, int nMsg, IntPtr wParam, IntPtr lParam);
		[DllImport(Kernel32, SetLastError = true)]
		//public  static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr Source, IntPtr Dest, IntPtr /*SIZE_T*/ size, out IntPtr /*SIZE_T*/ bytesRead);
 public static extern bool ReadProcessMemory(IntPtr hProcess,IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool RegisterHotKey(NativeMethods.HWND hWnd, int id, int fsModifiers, int vk);
 
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int SendInput(int nInputs, ref INPUT mi, int cbSize);
		[DllImport(User32, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		[ResourceExposure(ResourceScope.None)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessageTimeout(
			NativeMethods.HWND hwnd, int Msg, IntPtr wParam, IntPtr lParam, int flags, int uTimeout, out IntPtr pResult);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessageTimeout(
			NativeMethods.HWND hwnd, int Msg, IntPtr wParam, ref MINMAXINFO lParam, int flags, int uTimeout, out IntPtr pResult);
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessageTimeout(
			NativeMethods.HWND hwnd, int Msg, IntPtr wParam, StringBuilder lParam, int flags, int uTimeout, out IntPtr pResult);
		[DllImport(User32, ExactSpelling = true, CharSet = CharSet.Auto)]
		[ResourceExposure(ResourceScope.None)]
		public static extern bool SetForegroundWindow(HandleRef hWnd);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetWindowPlacement(NativeMethods.HWND hwnd, ref WINDOWPLACEMENT wp);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetWindowPos(NativeMethods.HWND hWnd, NativeMethods.HWND hWndInsertAfter, int x, int y, int cx, int cy, int flags);
 
		[DllImport("user32.dll")]
		internal static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr hmodWinEventProc, NativeMethods.WinEventProcDef WinEventReentrancyFilter, uint idProcess, uint idThread, int dwFlags);
      
		[DllImport("user32.dll")]
		public static extern void SwitchToThisWindow(NativeMethods.HWND hwnd, bool fAltTab);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool TranslateMessage(
			ref MSG msg);
 
		[DllImport("user32.dll")]
		internal static extern bool UnhookWinEvent(IntPtr winEventHook);
 
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool UnregisterHotKey(NativeMethods.HWND hWnd, int id);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int VirtualQueryEx(IntPtr hProcess, 
			IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
 
		[DllImport("oleacc.dll")]
		public static extern int WindowFromAccessibleObject(IAccessible acc, ref IntPtr hwnd);
		[DllImport(Kernel32, SetLastError = true)]
		public  static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr Dest, IntPtr sourceAddress, IntPtr /*SIZE_T*/ size, out IntPtr /*SIZE_T*/ bytesWritten);
		public static Color ColorFromCOLORREF(int colorref)
		{
			int r = colorref & 0xFF;
			int g = (colorref >> 8) & 0xFF;
			int b = (colorref >> 16) & 0xFF;
			return Color.FromArgb(r, g, b);
		}
 
		public static int ColorToCOLORREF(Color color)
		{
			return (int)color.R | ((int)color.G << 8) | ((int)color.B << 16);
		}
		public static int HIWORD(int n)
		{
			return (n >> 16) & 0xffff;
		}
      
		public static int LOWORD(int n)
		{
			return n & 0xffff;
		}
        
		public static int RGBToCOLORREF(int rgbValue)
		{
 
			// clear the A value, swap R & B values
			int bValue = (rgbValue & 0xFF) << 16;
 
			rgbValue &= 0xFFFF00;
			rgbValue |= ((rgbValue >> 16) & 0xFF);
			rgbValue &= 0x00FFFF;
			rgbValue |= bValue;
			return rgbValue;
		}
		public static IntPtr WindowFromPhysicalPoint(int x, int y)
		{
			POINTSTRUCT ps = new POINTSTRUCT(x, y);
			if (System.Environment.OSVersion.Version.Major >= 6)
				return IntWindowFromPhysicalPoint(ps);
			else
				return IntWindowFromPoint(ps);
		}
}	/// <summary>
	/// Common native constants.
	/// </summary>
	internal static      class Win32Constant{internal const int FALSE = 0;
internal const int INFOTIPSIZE = 1024;
internal const int MAX_PATH = 260;
internal const int TRUE = 1;
}}