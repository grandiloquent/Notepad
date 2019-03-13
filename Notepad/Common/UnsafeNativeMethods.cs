namespace Common{using Accessibility;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
	 
	public  class NativeMethods{        // the delegate passed to USER for receiving a WinEvent
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

            public static HWND NULL
            {
                get
                {
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

            public bool IsEmpty
            {
                get
                {
                    return left >= right || top >= bottom;
                }
            }
        }
}	public static  class UnsafeNativeMethods{//
        // IAccessible / OLEACC / WinEvents
        //

        public const int CHILDID_SELF = 0;
public const int INFINITE = unchecked((int)0xFFFFFFFF);
public const int INPUT_KEYBOARD = 1;
public const int INPUT_MOUSE = 0;
public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
public const int KEYEVENTF_KEYUP = 0x0002;
public const int KEYEVENTF_SCANCODE = 0x0008;
public const int KEYEVENTF_UNICODE = 0x0004;
public const int LB_GETCURSEL = 0x0188;
internal const int MF_BYCOMMAND = 0x00000000;
internal const int MF_BYPOSITION = 0x00000400;
internal const int MF_DISABLED = 0x00000002;
internal const int MF_GRAYED = 0x00000001;
//
        // Hotkeys
        //
        public const int MOD_ALT = 0x0001;
public const int MOD_CONTROL = 0x0002;
public const int MOD_NOREPEAT = 0x4000;
public const int MOD_SHIFT = 0x0004;
public const int MOD_WIN = 0x0008;
public const int MOUSEEVENTF_VIRTUALDESK = 0x4000;
public const int OBJID_CARET = -8;
public const int OBJID_CLIENT = -4;
public const int OBJID_MENU = -3;
public const int OBJID_SYSMENU = -1;
public const int OBJID_WINDOW = 0;
public const int PM_REMOVE = 0x0001;
public const int PROCESS_DUP_HANDLE = 0x0040;
public const int PROCESS_QUERY_INFORMATION = 0x0400;
//
        // OpenProcess
        //

        public const int PROCESS_VM_READ = 0x0010;
//
        // MsgWaitForMultipleObjects...
        //

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
//
        // SendMessage and related
        //

        public const int SMTO_ABORTIFHUNG = 2;
public const int STATE_SYSTEM_FOCUSED = 0x00000004;
public const int STATE_SYSTEM_UNAVAILABLE = 0x00000001;
public const int SW_RESTORE = 0x0009;
public const int SWP_NOACTIVATE = 0x0010;
public const int SWP_NOSIZE = 0x0001;
public const int SWP_NOZORDER = 0x0004;
public const int VK_CONTROL = 0x11;
public const int VK_MENU = 0x12;
//
        // SendInput related
        //

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
//
        //  Messages and related structs / constants
        //

        public const int WM_NULL = 0x0000;
public const int WM_QUIT = 0x0012;
public const int WM_SYSCOMMAND = 0x0112;
//
        // Window Placement
        //

        public const int WPF_SETMINPOSITION = 0x0001;
public static readonly IntPtr HTCLIENT = new IntPtr(1);
public static readonly IntPtr HTTRANSPARENT = new IntPtr(-1);
public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

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


        //
        // CloseHandle
        //

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        //
        // GDI
        //
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeleteObject(IntPtr hrgn);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DispatchMessage(
            ref MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern short GetAsyncKeyState(int nVirtKey);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetMenuState(IntPtr hMenu, int uId, int uFlags);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetMessage(
            ref MSG msg, NativeMethods.HWND hwnd, int nMsgFilterMin, int nMsgFilterMax);

        [DllImport("oleacc.dll", SetLastError = true)]
        internal static extern IntPtr GetProcessHandleFromHwnd(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetWindowPlacement(NativeMethods.HWND hwnd, ref WINDOWPLACEMENT wp);

        [DllImport("user32.dll", EntryPoint = "WindowFromPhysicalPoint", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr IntWindowFromPhysicalPoint(POINTSTRUCT pt);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr IntWindowFromPoint(POINTSTRUCT pt);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MapVirtualKey(int nVirtKey, int nMapType);

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

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(NativeMethods.HWND hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT mi, int cbSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(
            NativeMethods.HWND hwnd, int Msg, IntPtr wParam, IntPtr lParam, int flags, int uTimeout, out IntPtr pResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(
            NativeMethods.HWND hwnd, int Msg, IntPtr wParam, ref MINMAXINFO lParam, int flags, int uTimeout, out IntPtr pResult);

        // Overload for use with WM_GETTEXT. StringBuilder is the type that P/Invoke maps to an output TCHAR*, see
        // see MSDN .Net docs on P/Invoke for more details.
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(
            NativeMethods.HWND hwnd, int Msg, IntPtr wParam, StringBuilder lParam, int flags, int uTimeout, out IntPtr pResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetWindowPlacement(NativeMethods.HWND hwnd, ref WINDOWPLACEMENT wp);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetWindowPos(NativeMethods.HWND hWnd, NativeMethods.HWND hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr hmodWinEventProc, NativeMethods.WinEventProcDef WinEventReentrancyFilter, uint idProcess, uint idThread, int dwFlags);


        //
        // Foreground window
        //

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(NativeMethods.HWND hwnd, bool fAltTab);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool TranslateMessage(
            ref MSG msg);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(IntPtr winEventHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnregisterHotKey(NativeMethods.HWND hWnd, int id);

        [DllImport("oleacc.dll")]
        public static extern int WindowFromAccessibleObject(IAccessible acc, ref IntPtr hwnd);

        public static IntPtr WindowFromPhysicalPoint(int x, int y)
        {
            POINTSTRUCT ps = new POINTSTRUCT(x, y);
            if (System.Environment.OSVersion.Version.Major >= 6)
                return IntWindowFromPhysicalPoint(ps);
            else
                return IntWindowFromPoint(ps);
        }
}}