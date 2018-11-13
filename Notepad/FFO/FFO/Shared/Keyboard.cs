
using System;

namespace Shared
{
	using System.Threading;
	 
	public static class Keyboard
	{
		public static void SendKey(IntPtr hWnd,uint vk){
			NativeMethods.PostMessage(hWnd,0x0100,vk,0);
			Thread.Sleep(100);
			NativeMethods.PostMessage(hWnd,0x0101,vk,0);
			
		}
	}
}
