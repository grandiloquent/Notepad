
using System;

namespace  Shared
{
	 
	public static class Mouse
	{
		public static NativeMethods.	POINT  GetCursorPosition()
		{
			NativeMethods.POINT p = new NativeMethods.POINT();
			NativeMethods.GetCursorPos(p);
			//bool success = User32.GetCursorPos(out lpPoint);
			// if (!success)

			return p;
		}
	}
}
