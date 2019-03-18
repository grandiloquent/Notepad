namespace Notepad
{
	using System;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	
	public static class FileDelegate
	{
		[BindMenuItem(Name = "Hash值 (文件)",SplitButton="fileButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
		public static void ToBase64()
		{
			Forms.OnClipboardFile(str=>{
			                      	Clipboard.SetText(str.GetHashForFile());
			                                 });
		}
	}
}