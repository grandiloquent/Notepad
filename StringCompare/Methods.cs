namespace StringCompare
{
	using System.Runtime.InteropServices;
	using Microsoft.Ajax.Utilities;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Common;
	public static class Methods
	{
		private const int s_aLower = (int)'a';
		private const int s_aUpper = (int)'A';
      
		private const int s_zeroChar = (int)'0';
		public static bool RegisterHotKey(NativeMethods.HWND hWnd, int id, int fsModifiers, int vk)
		{
			bool result = UnsafeNativeMethods.RegisterHotKey(hWnd, id, fsModifiers, vk);
			int lastWin32Error = Marshal.GetLastWin32Error();

			if (!result) {
				//ThrowWin32ExceptionsIfError(lastWin32Error);
			}

			return result;
		}
		public static bool UnregisterHotKey(NativeMethods.HWND hWnd, int id)
		{
			bool result = UnsafeNativeMethods.UnregisterHotKey(hWnd, id);
			int lastWin32Error = Marshal.GetLastWin32Error();

			if (!result) {
				//ThrowWin32ExceptionsIfError(lastWin32Error);
			}

			return result;
		}
		public static string RemoveComments(this string code)
		{
			var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}
		static private int ParseHexChar(char c)
		{
			int intChar = (int)c;
			if ((intChar >= s_zeroChar) && (intChar <= (s_zeroChar + 9))) {
				return (intChar - s_zeroChar);
			}
			if ((intChar >= s_aLower) && (intChar <= (s_aLower + 5))) {
				return (intChar - s_aLower + 10);
			}
			if ((intChar >= s_aUpper) && (intChar <= (s_aUpper + 5))) {
				return (intChar - s_aUpper + 10);
			}
			throw new FormatException();
		}
       
		public static byte[] ParseHexColor(this string trimmedColor)
		{
			int a, r, g, b;
			a = 255;
			if (trimmedColor.Length > 7) {
				a = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
				r = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
				g = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
				b = ParseHexChar(trimmedColor[7]) * 16 + ParseHexChar(trimmedColor[8]);
			} else if (trimmedColor.Length > 5) {
				r = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
				g = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
				b = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
			} else if (trimmedColor.Length > 4) {
				a = ParseHexChar(trimmedColor[1]);
				a = a + a * 16;
				r = ParseHexChar(trimmedColor[2]);
				r = r + r * 16;
				g = ParseHexChar(trimmedColor[3]);
				g = g + g * 16;
				b = ParseHexChar(trimmedColor[4]);
				b = b + b * 16;
			} else {
				r = ParseHexChar(trimmedColor[1]);
				r = r + r * 16;
				g = ParseHexChar(trimmedColor[2]);
				g = g + g * 16;
				b = ParseHexChar(trimmedColor[3]);
				b = b + b * 16;
			}
			return new[] { (byte)a, (byte)r, (byte)g, (byte)b };
		}
       
       
  
      
        
		public static string FormatChromeHeader(string str)
		{
			
			var pieces = str.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);

			var list1 = new List<string>();
			var list2 = new List<string>();
//				var list3 = new List<string>();
//				var list4 = new List<string>();
//				var list5 = new List<string>();
//				var list6 = new List<string>();
//				var list7 = new List<string>();
//				var list8 = new List<string>();
//				var list9 = new List<string>();
			foreach (var item in pieces) {
				var p1 = item.SubstringBefore(':').Trim();
				var p2 = item.SubstringAfter(':').Trim().Replace("\"", "\\\"");
                	
				list1.Add(string.Format("{{\"{0}\",\"{1}\"}},", p1, p2));
				list2.Add(string.Format("headers[\"{0}\"] =\"{1}\";", p1, p2));
//                	list3.Add(string.Format("req.Header.Set(\"{0}\",\"{1}\")",p1,p2));
//                	list4.Add(string.Format("connection.setRequestProperty(\"{0}\",\"{1}\");",p1,p2));
//                	var header=p1.Replace('-','_').ToLower();
//                	list5.Add(string.Format("const char * {0}=\"{1}:\";",header,p1));
//                	list6.Add(string.Format("const char * {0}=\" {1}\\r\\n\";",header+"_value",p2));
//                	list8.Add(string.Format("buf_size+=strlen({0});",header));
//                	list8.Add(string.Format("buf_size+=strlen({0});",header+"_value"));
//                	//list9.Add(string.Format("strbuf_addf(&buf,\"%s: %s\\r\\n\",\"{0}\",\"{1}\");",p1,p2));
//                	list9.Add(string.Format("strbuf_addstr(&buf,\"{0}: {1}\\r\\n\");",p1,p2));
//                	
//                	
//                	list7.Add(string.Format("strcat(buf,{0});\r\nstrcat(buf,{1});",header,header+"_value"));
                	
			}
			// 	.OrderBy(i=>i)
			return list1
   	.Concat(list2)
//                	.Concat(list3)
//                	.Concat(list4)
//                	.Concat(list5)
//                	.Concat(list6)
//                	.Concat(list7)
//                	.Concat(list8)
//                	.Concat(list9)
                .ConcatenateLines();
			                        
		}
		public static void BindConvertToBytes(ToolStripMenuItem b)
		{
		
			b.Click += (s, o) => OnClipboardString(v => string.Join(",", v.ConvertToInt().ConvertToBytes()));
		}
		
	
		public static void OnClipboardText(Action<string> f)
		{
			var s = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(s))
				return;
			f(s);
		}
		
		

	
 
		public static string WriteFile(string s, string dir, string extension)
		{
			var fileName = s.Trim().GetValidFileName() + extension;
			var r = fileName;
			if (Directory.Exists(dir)) {
				fileName = Path.Combine(dir, fileName);
			} else {
				fileName =	fileName.GetDesktopPath();
			}
			if (!File.Exists(fileName)) {
				fileName.WriteAllText(string.Empty);
			}
			return r;
		}
		
		public static void OnClipboardString(Func<string, string> func)
		{
			var value = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(value))
				return;
			var result = func(value);
			if (string.IsNullOrWhiteSpace(result))
				return;
			Clipboard.SetText(result);
		}
		
		
		
	
		public static void OnClipboardDirectory(Action<string> action)
		{
			var dir = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir)) {
				var files = Clipboard.GetFileDropList();
				if (files.Count > 0)
					dir = files[0];
			}
			if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
				return;
			action(dir);
		}
		public static void OnClipboardFile(Action<string> action)
		{
			var dir = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir)) {
				var files = Clipboard.GetFileDropList();
				if (files.Count > 0)
					dir = files[0];
			}
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir))
				return;
			action(dir);
		}
	}
}