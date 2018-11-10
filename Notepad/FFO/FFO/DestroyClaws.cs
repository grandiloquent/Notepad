namespace FFO
{
	using Shared;
	using System;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Threading;
	using System.Windows.Input;
	
	public class DestoryClaws{
		
		IntPtr _hWnd;
		int _delay=1000;
		public DestoryClaws(IntPtr hWnd){
			_hWnd=hWnd;
		}
		
		public void Start(){
			
			Win32.SetForegroundWindow(_hWnd);
			Thread.Sleep(_delay);
			
		 
			
			//Keyboard.ReleaseModifierKeys(ModifierKeys.Alt);
			
		Thread.Sleep(_delay);
			//WindowsInput.InputSimulator.SimulateModifiedKeyStroke(WindowsInput.VirtualKeyCode.MENU,WindowsInput.VirtualKeyCode.VK_5);
			WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.F2);
			
		}
	}
}