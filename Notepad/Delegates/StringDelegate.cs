namespace Notepad
{
	using System;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	
	public static class StringDelegate
	{
		[BindMenuItem(Name = "转换为Base64 (文本)",SplitButton="stringButton", Toolbar = "toolStrip", AddSeparatorBefore = true)]
		public static void ToBase64()
		{
			Forms.OnClipboardString(str=>{
			                        
			                        	return  Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
			                        });
		}
		[BindMenuItem(Name = "转换为数组 (文本)",SplitButton="stringButton", Toolbar = "toolStrip", AddSeparatorBefore = true)]
		public static void ToByteArray()
		{
			Forms.OnClipboardString(str=>{
			                        
			                        	return string.Join(	",",Encoding.UTF8.GetBytes(str));
			                        });
		}
			
		[BindMenuItem(Name = "逃逸代码 (文本)",SplitButton="stringButton", Toolbar = "toolStrip", AddSeparatorBefore = true)]
		public static void EscapeCode()
		{
			Forms.OnClipboardString(str=>str.LiterallyInCs());
		}
		[BindMenuItem(Name = "逃逸为 StringBuilder (文本)",SplitButton="stringButton", Toolbar = "toolStrip")]
		public static void EscapeToStringBuilder()
		{
			Forms.OnClipboardString(str=>str.StringbuilderizeInCs());
		}
	}
}