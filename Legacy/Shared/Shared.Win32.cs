using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Runtime.InteropServices;

namespace Shared
{
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

    class Win32
    {
        public static IntPtr GetHandleOfWindowMouseIsOver()
        {
            POINT location;
            GetCursorPos(out location);

            IntPtr window = WindowFromPoint(location);
            IntPtr windowParent = IntPtr.Zero;

            while (window != IntPtr.Zero)
            {
                windowParent = window;
                window = GetParent(window);
            }

            return windowParent;
        }
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
    }
}
