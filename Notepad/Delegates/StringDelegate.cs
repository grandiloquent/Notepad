namespace Notepad
{
	using System;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	using System.Collections.Generic;
	
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
			                        	var pieces=Regex.Split(str, "{{[^}]*?}}");
			                        	var list1=new List<string>();
			                        	var index=1;
			                        	foreach (var element in pieces) {
			                        		list1.Add(string.Format("q{0}:=[]byte{{{1}}}",index,string.Join(",",Encoding.UTF8.GetBytes(element.Trim()))));
			                        		index++;
			                        	}
			                        	return list1.ConcatenateLines();
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
		[BindMenuItem(Name = "排序 分隔符, (文本)",SplitButton="stringButton", Toolbar = "toolStrip")]
		public static void SortBy()
		{
			Forms.OnClipboardString(str=>{
			                        
			                        	var pieces=str.Split(',').Where(i=>!string.IsNullOrWhiteSpace(i)).Select(i=>i.Trim()).OrderBy(i=>i);
			                        	return string.Join(",",pieces);
			                        });
		}
	}
}