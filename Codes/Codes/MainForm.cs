/*
 * Created by SharpDevelop.
 * User: HIAPAD
 * Date: 2018/9/5
 * Time: 23:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Codes
{
	 
	public partial class MainForm : Form
	{
		
		[DllImport("user32.dll")]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
		const UInt32 SWP_NOSIZE = 0x0001;
		const UInt32 SWP_NOMOVE = 0x0002;
		const UInt32 SWP_SHOWWINDOW = 0x0040;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
		}
		void TextBoxMouseDoubleClick(object sender, MouseEventArgs e)
		{
			textBox.Text = Regex.Replace(StripComments(Clipboard.GetText()), "[\r\n]+", "\r\n");
	
		}
		static string StripComments(string code)
		{
			var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}
		void MainFormLoad(object sender, EventArgs e)
		{
	
			SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
		}
	}
}
