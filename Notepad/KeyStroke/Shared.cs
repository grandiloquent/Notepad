
#define ENTITY_ENCODE_HIGH_ASCII_CHARS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Shared
{
	public static class WinForms
	{
		public static void InvokeWkhtmltopdf(string f)
		{

			if (!File.Exists(Path.Combine(f, "目录.html")))
				return;

			var styleFile = "safari".GetDesktopPath().Combine("style.css");
			if (File.Exists(styleFile)) {
				var targetStyleFile = Path.Combine(f, "style.css");
				if (File.Exists(targetStyleFile))
					File.Delete(targetStyleFile);
				File.Copy(styleFile, targetStyleFile);
			}
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(Path.Combine(f, "目录.html").ReadAllText());
			var nodes = hd.DocumentNode.SelectNodes("//a");
			var ls = new List<string>();
			foreach (var item in nodes) {
				var href = item.GetAttributeValue("href", "").Split('#').First();

				if (ls.Contains(href))
					continue;
				ls.Add(href);
			}

			var str = "\"C:\\wkhtmltox\\wkhtmltopdf.exe\"";
			var arg = "--footer-center [page] -s Letter " + string.Join(" ", ls.Select(i => string.Format("\"{0}\"", i))) + string.Format("  \"{0}.pdf\"", f);

			var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
				FileName = "wkhtmltopdf.exe",
				Arguments = arg,
				WorkingDirectory = f
			});
			p.WaitForExit();
		}
		public static void CleanHtmls()
		{
			OnClipboardDirectory((dir) => {
				var diretories = Directory.GetDirectories(dir);
				foreach (var r in diretories) {
					const string str = "<div><div><img src=\"./images/\"><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";
					const string str1 = "<div><div><img><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";
					
					var files = Directory.GetFiles(r, "*.html", SearchOption.TopDirectoryOnly);
					foreach (var element in files) {
						var sv = Regex.Replace(element.ReadAllText().Replace(str, "").Replace(str1, ""), "(style|width|height)=\"[^\"]*?\"", "");
					
						
						element.WriteAllText(sv);
					}
				}
			});
			
			
		}
		public static void RemoveAria2File()
		{
			OnClipboardDirectory((dir) => {
				var directories = Directory.GetDirectories(dir);
				foreach (var element in directories) {
					var files = Directory.GetFiles(element, "*", SearchOption.AllDirectories);
					var filesAria2 = files.Where(i => i.EndsWith(".aria2")).Select(i => i.GetFileNameWithoutExtension()).OrderBy(i => i).Distinct().ToArray();
					foreach (var f in files) {
						if (filesAria2.Contains(f.GetFileName()) || f.EndsWith(".aria2")) {
							File.Delete(f);
						}
					}
				}
			});
		}
	
		
		public static void ZipDirectories()
		{
			OnClipboardDirectory((dir) => {
				var directories = Directory.GetDirectories(dir);
				foreach (var element in directories) {
					using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {
						zip.AddDirectory(element);
						zip.Save(element + ".zip");
					}
					
				}
			});
		}
		
		public static void OnClipboardDirectory(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (Directory.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (Directory.Exists(ls[0])) {
							dir = ls[0];
						}
					}
				}
				if (found) {
					action(dir);
				}
			} catch {
				
			}
		}
		public static void ZipAndroidProject(string dir)
		{
			 
			using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {

				zip.AddFiles(Directory.GetFiles(dir).Where(i => !i.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase)).ToArray(), "");
				zip.AddFiles(Directory.GetFiles(Path.Combine(dir, "app")), "app");
				zip.AddDirectory(Path.Combine(Path.Combine(dir, "app"), "src"), "app/src");
				zip.AddDirectory(Path.Combine(dir, "gradle"), "gradle");
				var targetFileName = Path.Combine(dir, Path.GetFileName(dir) + ".zip");
				var count = 0;
				while (File.Exists(targetFileName)) {
					targetFileName = Path.Combine(dir, string.Format("{0} {1:000}.zip", Path.GetFileName(dir), ++count));
				}
				zip.Save(targetFileName);
			}
		}
		public static void OnClipboardString(Func<String,String> func)
		{
			try {
				var str = Clipboard.GetText().Trim();
				if (str.IsVacuum())
					return;
				str = func(str);
				if (str.IsReadable())
					Clipboard.SetText(str);
			} catch {
				
			}
		}
		
	
		public static void OnClipboardFile(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (File.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (File.Exists(ls[0])) {
							dir = ls[0];
						}
					}
				}
				if (found) {
					action(dir);
				}
			} catch {
				
			}
		}
	}
	
	public static class Codes
	{
		public static string FormatCSharpCode(string value)
		{

			var s = new StringBuilder();

			var rootNode = CSharpSyntaxTree.ParseText(value).GetRoot();

			var namespace_ = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

			if (namespace_.Any()) {

				s.Append(namespace_.First().NamespaceKeyword.Text).Append(' ').Append(namespace_.First().Name).Append('{');
			}

			var using_ = rootNode.DescendantNodes().OfType<UsingDirectiveSyntax>();
			if (using_.Any()) {

				using_ = using_.OrderBy(i => i.Name.ToString());//.Distinct(i => i.Name.GetText());

				foreach (var item in using_) {
					s.Append(item.ToFullString());
				}
			}
			var enum_ = rootNode.DescendantNodes().OfType<EnumDeclarationSyntax>();
			if (enum_.Any()) {
				foreach (var item in enum_) {
					enum_ = enum_.OrderBy(i => i.Identifier.ToFullString());
					s.Append(item.ToFullString());
				}
			}
			var struct_ = rootNode.DescendantNodes().OfType<StructDeclarationSyntax>();
			if (struct_.Any()) {
				foreach (var item in struct_) {
					struct_ = struct_.OrderBy(i => i.Identifier.ToFullString());
					s.Append(item.ToFullString());
				}
			}
			var class_ = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

			if (class_.Any()) {
				class_ = class_.OrderBy(i => i.Identifier.ValueText);

				foreach (var item in class_) {
                    
					s.Append(item.Modifiers.ToFullString()).Append(" class ").Append(item.Identifier.ValueText);
					if (item.BaseList != null)
						s.Append(item.BaseList.GetText());
                    	
					s.Append('{');
					var field_ = item.DescendantNodes().OfType<FieldDeclarationSyntax>();
					if (field_.Any()) {
						field_ = field_.OrderBy(i => i.Declaration.Variables.First().ToFullString());

						foreach (var itemField in field_) {

							s.Append(itemField.ToFullString().Trim() + '\n');
						}
					}

					var constructor_ = item.DescendantNodes().OfType<ConstructorDeclarationSyntax>();
					if (constructor_.Any()) {
						constructor_ = constructor_.OrderBy(i => i.Identifier.ValueText);//.OrderBy(i => i.Identifier.ValueText).ThenBy(i=>i.Modifiers.ToFullString());
						foreach (var itemMethod in constructor_) {


							s.Append(itemMethod.ToFullString());
						}

					}
					var method_ = item.DescendantNodes().OfType<MethodDeclarationSyntax>();

					if (method_.Any()) {
						method_ = method_.OrderBy(i => i.Modifiers.ToFullString().Trim() + i.Identifier.ValueText.Trim());//.OrderBy(i => i.Identifier.ValueText).ThenBy(i=>i.Modifiers.ToFullString());
						foreach (var itemMethod in method_) {


							s.Append(itemMethod.ToFullString());
						}

					}
					s.Append('}');
				}

			}
			s.Append('}');
			return s.ToString();

		}
		public static IEnumerable<string> FormatMethodList(string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (int i = 0; i < value.Length; i++) {
				sb.Append(value[i]);

				if (value[i] == '{') {
					count++;
				} else if (value[i] == '}') {
					count--;
					if (count == 0) {
						ls.Add(sb.ToString());
						sb.Clear();
					}
				}

			}
			return ls;

		}
		public static void FormatWithClangFormat(string fileName)
		{
			Process.Start(new ProcessStartInfo() {
				FileName = "clang-format",
				Arguments = string.Format("\"{0}\" -i -style=llvm -sort-includes=false", fileName)
			});
		}
		
		public static void FormatVSCTypeDef()
		{
			WinForms.OnClipboardString((str) => {
				var result = str.Trim();
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix", result.SubstringAfterLast(' ').TrimEnd(';'));
				//obj.Add("prefix", string.Join("", matches).ToLower());
				obj.Add("body", result.SubstringAfterLast(' ').TrimEnd(';') + " $0");// changed
				
		 
				var r = new Dictionary<string,dynamic>();
				r.Add(result, obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return	sr.Substring(1, sr.Length - 2) + ",";
			                         
			});
		}
	}


	public static class Win32
	{
		public const int MOUSEEVENTF_LEFTDOWN = 0x02;
		public const int MOUSEEVENTF_LEFTUP = 0x04;
		public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		public const int MOUSEEVENTF_RIGHTUP = 0x10;
		public const int WM_UNICHAR = 0x0109;
		public const int WM_CHAR = 0x0102;
		public  const int PROCESS_WM_READ = 0x0010;
		public   const int PROCESS_VM_WRITE = 0x0020;
		public    const int PROCESS_VM_OPERATION = 0x0008;
		public   const int PROCESS_QUERY_INFORMATION = 0x0400;
		public       const int MEM_COMMIT = 0x00001000;
		public    const int PAGE_READWRITE = 0x04;
		public       const int MEM_PRIVATE = 0x20000;
		public       const int	PAGE_GUARD = 0x100;
		public const int WM_KEYDOWN = 0x100;
		public const int WM_KEYUP = 0x101;
		//https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-registerhotkey
		public const int MOD_CONTROL = 0x0002;
		public struct MEMORY_BASIC_INFORMATION
		{
			public int BaseAddress;
			public int AllocationBase;
			public int AllocationProtect;
			public int RegionSize;
			public int State;
			public int Protect;
			public int lType;
		}

		public struct SYSTEM_INFO
		{
			public ushort processorArchitecture;
			ushort reserved;
			public uint pageSize;
			public IntPtr minimumApplicationAddress;
			public IntPtr maximumApplicationAddress;
			public IntPtr activeProcessorMask;
			public uint numberOfProcessors;
			public uint processorType;
			public uint allocationGranularity;
			public ushort processorLevel;
			public ushort processorRevision;
		}
		
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
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
   
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(Point pnt);
		[DllImport("kernel32.dll", SetLastError = true)]

		public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, 
			byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
		[DllImport("User32.dll", CharSet = CharSet.Auto)]  
		public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(int hProcess, 
			int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
    
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(ref Point lpPoint);
		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int SendMessage(IntPtr hwnd, int wMsg, uint wParam, uint lParam);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "PostMessage")]
		public static extern int PostMessage(IntPtr hwnd, int wMsg, uint wParam, uint lParam);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("kernel32.dll")]
		public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int VirtualQueryEx(IntPtr hProcess, 
			IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
		[DllImport("msvcrt.dll")]
		private static extern IntPtr memcmp(byte[] b1, byte[] b2, IntPtr count);
    
		public static int MemoryCompare(byte[] b1, byte[] b2)
		{
			IntPtr retval = memcmp(b1, b2, new IntPtr(b1.Length));
			return retval.ToInt32();
		}
		public static Color GetColorAt(Point location, Bitmap screenPixel)
		{
			if (screenPixel == null) {
				screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
			}
			using (Graphics gdest = Graphics.FromImage(screenPixel)) {
				using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
					IntPtr hSrcDC = gsrc.GetHdc();
					IntPtr hDC = gdest.GetHdc();
					int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
					gdest.ReleaseHdc();
					gsrc.ReleaseHdc();
				}
			}

			return screenPixel.GetPixel(0, 0);
		}
		
		public static Point GetCursorPosition()
		{
			POINT lpPoint;
			GetCursorPos(out lpPoint);
			//bool success = User32.GetCursorPos(out lpPoint);
			// if (!success)

			return lpPoint;
		}
		public static int ReadMemoryInt(int hProcess, int address)
		{
			var buffer = new byte[4];
			int bytesRead = 0;
			ReadProcessMemory(hProcess, address, buffer, 4, ref bytesRead);
			return BitConverter.ToInt32(buffer, 0);
			
			
		}
		public static void ClickMouse(uint x, uint y)
		{
      
			mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
		}

		public static void PostKey(IntPtr hWnd, uint key)
		{
			PostMessage(hWnd, WM_KEYDOWN, key, 0);
			Thread.Sleep(100);
			PostMessage(hWnd, WM_KEYUP, key, 0);
			
		}
		public static int ScanSegments(int pid, byte[] pattern)
		{
			var hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, pid);
			
			SYSTEM_INFO sysinfo = new SYSTEM_INFO();
			GetSystemInfo(out sysinfo);
			var minAddress = sysinfo.minimumApplicationAddress;
			var maxAddress = sysinfo.maximumApplicationAddress;
			var min = (long)minAddress;
			var max = (long)maxAddress;
			// https://docs.microsoft.com/en-us/windows/desktop/api/winnt/ns-winnt-_memory_basic_information
			var memoryInfo = new MEMORY_BASIC_INFORMATION();
			
			int bytesRead = 0;
			while (min < max) {
				VirtualQueryEx(hProcess, minAddress, out memoryInfo, 28);
				
				//https://docs.microsoft.com/zh-cn/windows/desktop/Memory/memory-protection-constants
				if (((memoryInfo.State & MEM_COMMIT) != 0) && ((memoryInfo.lType & MEM_PRIVATE) != 0) && ((memoryInfo.Protect & PAGE_READWRITE) != 0)
				    && ((memoryInfo.Protect & PAGE_GUARD) == 0)) {
					byte[] buffer = new byte[memoryInfo.RegionSize];
					ReadProcessMemory((int)hProcess, memoryInfo.BaseAddress, buffer, memoryInfo.RegionSize, ref bytesRead);
					
					for (int i = 0; i < memoryInfo.RegionSize; ++i) {
					
						if ((pattern[0] == buffer[i]) && ((i + pattern.Length) < memoryInfo.RegionSize)) {
							bool bSkip = false;
							for (int j = 0; j < pattern.Length; j++) {
								if (pattern[j] != buffer[i + j]) {
									bSkip = true;
									break;
								}
							}
							if (!bSkip)
								return memoryInfo.BaseAddress + i;
						}
					}
				}
				min += memoryInfo.RegionSize;
				minAddress = new IntPtr(min);
			}
			return -1;
		
			
			
		}
	}
  
     
	public static  class StringExtensions
	{
		static sbyte[] unhex_table = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -1, -1, -1, -1, -1, -1
       , -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
		};
		//public static string GetRandomStringAlpha(this int length)
		//{

		//    StringBuilder builder = new StringBuilder();
		//    char ch;
		//    for (int i = 0; i < length; i++)
		//    {
		//        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * s_nameRand.NextDouble() + 65)));
		//        builder.Append(ch);
		//    }
		//    return builder.ToString();
		//}
		public static StringBuilder Append(this String value)
		{
			return new StringBuilder().Append(value);
		}

		public static StringBuilder AppendLine(this String value)
		{
			return  new StringBuilder().AppendLine(value);
		}
		public static string Capitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
			}
			return value;
		}
		public static int ConvertToInt(this string value, int defaultValue = 0)
		{
			var match = Regex.Match(value, "[0-9]+");
			if (match.Success) {
				return int.Parse(match.Value);
			}
			return defaultValue;
		}

		public static int CountStart(this string value, char c)
		{
			var count = 0;

			foreach (var item in value) {
				if (item == c)
					count++;
				else
					break;
			}
			return count;
		}
		public static string EscapeString(this string s)
		{
			char[] cs = new []{ '\\', '"', '\'', '<', '>' };
			string[] ss = cs.Select(i => "\\u" + ((int)i).ToString("x4")).ToArray();
			for (int i = 0; i < cs.Length; i++) {
				s = s.Replace(cs[i].ToString(), ss[i]);
			}
			return s;
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		public static string GetFirstReadable(this string value)
		{
			return  value.TrimStart().Split(new char[] { '\n' }, 2).First().Trim();
		}
		public static string GetRandomString(this int length)
		{
			Random s_nameRand = new Random();//new Random((int)(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()));

			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
			return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[s_nameRand.Next(s.Length)]).ToArray());
		}

		public static int HexToInt(this string hexNumber)
		{
			int decValue = unhex_table[(byte)hexNumber[0]];
			for (int i = 1; i < hexNumber.Length; i++) {
				decValue *= 16;
				decValue += unhex_table[(byte)hexNumber[i]];
			}
			return decValue;
		}
		public static bool IsReadable(this string value)
		{
			return  !string.IsNullOrWhiteSpace(value);
		}
		public static bool IsVacuum(this string value)
		{
			return  string.IsNullOrWhiteSpace(value);
		}
	
		public static bool IsWhiteSpace(this String value)
		{
			if (value == null)
				return true;

			for (int i = 0; i < value.Length; i++) {
				if (!Char.IsWhiteSpace(value[i]))
					return false;
			}

			return true;
		}
        
		public static IEnumerable<string> Matches(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern);

			while (match.Success) {

				yield return match.Value;
				match = match.NextMatch();
			}
		}
		public static IEnumerable<string> MatchesByGroup(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern);

			while (match.Success) {

				yield return match.Groups[1].Value;
				match = match.NextMatch();
			}
		}
		public static IEnumerable<string> MatchesMultiline(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern, RegexOptions.Multiline);

			while (match.Success) {

				yield return match.Value;
				match = match.NextMatch();
			}
		}
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + delimiter.Length);
		}
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfterLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBefore(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBeforeLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static IEnumerable<string> ToLines(this string value)
		{
			return  value.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());
		}
		public static string TrimNonLetterOrDigitStart(this string value)
		{
			var len = value.Length;
			var pos = 0;
//			int a='`';
//			int a1='a';
//			int a2='z';
//			int a3='A';
//			int a4='Z';
//			int a5='0';
//			int a6='9';
			
			for (int i = 0; i < len; i++) {
				if (('a' <= value[i] && value[i] <= 'z') ||
				    ('A' <= value[i] && value[i] <= 'Z') ||
				    ('0' <= value[i] && value[i] <= '9'))
					break;
				pos = i;
			}
			if (pos > 0)
				return value.Substring(pos + 1);
			return value;
		}
	}
	public static class GenericExtensions
	{
		public static IEnumerable<T> Distinct<T, U>(
			this IEnumerable<T> seq, Func<T, U> getKey)
		{
			return
                from item in seq
			             group item by getKey(item) into gp
			             select gp.First();
		}
	}

	public static class NetHttpExtensions
	{
		public static HttpClient GetHttpClient(this string url)
		{
			var client = new HttpClient(new HttpClientHandler {
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				UseProxy = false,
			});
			client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");
			return client;
		}
	}

	public static class FileExtensions
	{
		private static readonly char[] InvalidFileNameChars = {
			'\"',
			'<',
			'>',
			'|',
			'\0',
			':',
			'*',
			'?',
			'\\',
			'/'
		};

		public static void UTF8ToGbk(this string path)
		{
			var content = File.ReadAllText(path, new UTF8Encoding(false));
			File.WriteAllText(path, content, Encoding.GetEncoding("gbk"));
		}
		public static void GbkToUTF8(this string path)
		{
			var content = File.ReadAllText(path, Encoding.GetEncoding("gbk"));
			File.WriteAllText(path, content, new UTF8Encoding(false));
		}
		public static string GetDirectoryFileName(this string v)
		{
			return Path.GetFileName(Path.GetDirectoryName(v));
		}

		public static string GetApplicationPath(this string v)
		{
			return Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), v);
		}

		public static string GetUniqueFileName(this String v)
		{
			int i = 1;
			Regex regex = new Regex(" \\- [0-9]+");
			String t = Path.Combine(Path.GetDirectoryName(v),
				           regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
				           Path.GetExtension(v));

			while (File.Exists(t)) {
				i++;
				t = Path.Combine(Path.GetDirectoryName(v),
					regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
					Path.GetExtension(v));
			}
			return t;
		}
		public static IEnumerable<string> GetFiles(this string dir, string pattern, bool bExclude = false)
		{
			if (bExclude)
				return Directory.GetFiles(dir).Where(i => !Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			else
				return Directory.GetFiles(dir).Where(i => Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			
		}

		public static string GetValidFileName(this String v)
		{
			if (v == null)
				return null;
			// (Char -> Int) 1-31 Invalid;
			List<char> chars = new List<char>(v.Length);

			for (int i = 0; i < v.Length; i++) {
				if (InvalidFileNameChars.Contains(v[i])) {
					chars.Add(' ');
				} else {
					chars.Add(v[i]);
				}
			}

			return new String(chars.ToArray());
		}
		public static string GetFileSha1(this string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open))
			using (BufferedStream bs = new BufferedStream(fs))
			using (var reader = new StreamReader(bs)) {
				using (System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed()) {
					byte[] hash = sha1.ComputeHash(bs);
					StringBuilder formatted = new StringBuilder(2 * hash.Length);
					foreach (byte b in hash) {
						formatted.AppendFormat("{0:X2}", b);
					}
				}
				return reader.ReadToEnd();
			}
		}
		public static void FileCopy(this string path, string dstPath)
		{
			File.Copy(path, dstPath, false);
		}
		public static string GetUniqueImageRandomFileName(this string path)
		{
			var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Select(i => i.GetFileNameWithoutExtension());
			var fileName = 8.GetRandomString();
			while (files.Contains(fileName))
				fileName = 8.GetRandomString();

			return fileName;
		}
		public static String GetCommandPath(this string fileName)
		{
			var dir = System.Reflection.Assembly.GetEntryAssembly().Location.GetDirectoryName();
			return dir.Combine(fileName);
		}
		public static UTF8Encoding sUTF8Encoding = new UTF8Encoding(false);

		//		public static IEnumerable<string> GetFiles(this String path, string pattern = "*")
		//		{
		//			foreach (var item in Directory.GetFiles(path, pattern)) {
		//				yield return item;
		//			}
		//		}
		public static DirectoryInfo GetParent(this String path)
		{
			return  Directory.GetParent(path);
		}
		public static void CreateDirectoryIfNotExists(this String path)
		{
			if (Directory.Exists(path))
				return;
			Directory.CreateDirectory(path);
		}
		public static bool DirectoryExists(this String path)
		{
			return  Directory.Exists(path);
		}
		public static string ChangeFileName(this string path, string fileNameWithoutExtension)
		{
			return Path.Combine(Path.GetDirectoryName(path), fileNameWithoutExtension + Path.GetExtension(path));
		}

		public static void DirectoryDelete(this String path, bool recursive = false)
		{
			if (recursive) {
				Directory.Delete(path, true);
			} else {
				Directory.Delete(path);
			}
		}
		public static void WriteAllText(this String path, String contents)
		{
			File.WriteAllText(path, contents, sUTF8Encoding);
		}
		public static void WriteAllBytes(this String path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
		}
		public static void WriteAllLines(this String path, String[] contents)
		{
			File.WriteAllLines(path, contents, sUTF8Encoding);
		}
		public static void AppendAllText(this String path, String contents)
		{
			File.AppendAllText(path, contents, sUTF8Encoding);
		}
		public static void AppendAllLines(this String path, IEnumerable<String> contents, Encoding encoding)
		{
			File.AppendAllLines(path, contents, sUTF8Encoding);
		}
		public static void FileMove(this String sourceFileName, String destFileName)
		{
			File.Move(sourceFileName, destFileName);
		}
		public static void FileReplace(this String sourceFileName, String destinationFileName, String destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
		}
		public static StreamReader OpenText(this String path)
		{
			return  File.OpenText(path);
		}
		public static StreamWriter CreateText(this String path)
		{
			return  File.CreateText(path);
		}
		public static StreamWriter AppendText(this String path)
		{
			return  File.AppendText(path);
		}
		public static FileStream Create(this String path)
		{
			return  File.Create(path);
		}
		public static FileStream Open(this String path, FileMode mode, FileAccess access)
		{
			return  File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		}
		public static DateTime FileGetCreationTimeUtc(this String path)
		{
			return  File.GetCreationTimeUtc(path);
		}
		public static DateTime FileGetLastAccessTimeUtc(this String path)
		{
			return  File.GetLastAccessTimeUtc(path);
		}
		public static DateTime FileGetLastWriteTimeUtc(this String path)
		{
			return  File.GetLastWriteTimeUtc(path);
		}
		public static FileStream OpenRead(this String path)
		{
			return  File.OpenRead(path);
		}
		public static FileStream OpenWrite(this String path)
		{
			return  File.OpenWrite(path);
		}
		public static String ReadAllText(this String path)
		{
			return  File.ReadAllText(path, new UTF8Encoding(false));
		}
		public static IEnumerable<string> ReadLines(this String path)
		{
			return  File.ReadLines(path, new UTF8Encoding(false));
		}
		public static byte[] ReadAllBytes(this String path)
		{
			return  File.ReadAllBytes(path);
		}
		public static String ChangeExtension(this String path, String extension)
		{
			return  Path.ChangeExtension(path, extension);
		}
		public static string GetDirectoryName(this string path)
		{
			return  Path.GetDirectoryName(path);
		}
		public static String GetExtension(this String path)
		{
			return  Path.GetExtension(path);
		}
		public static String GetFullPath(this String path)
		{
			return  Path.GetFullPath(path);
		}
		public static String GetFileName(this String path)
		{
			return  Path.GetFileName(path);
		}
		public static String GetFileNameWithoutExtension(this String path)
		{
			return  Path.GetFileNameWithoutExtension(path);
		}
		public static String GetPathRoot(this String path)
		{
			return  Path.GetPathRoot(path);
		}
		public static String Combine(this String path1, String path2)
		{
			return  Path.Combine(path1, path2);
		}
		public static string ToLine(this IEnumerable<string> value, string separator = "\r\n")
		{
			return string.Join(separator, value);
		}
		public static bool FileExists(this String path)
		{
			return  File.Exists(path);
		}
		public static string GetExePath(this string path)
		{
			return 	Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), path);
			
		}
		public static string GetValidFileName(this string value, char c)
		{

			var chars = Path.GetInvalidFileNameChars();

			return new string(value.Select<char, char>((i) => {
				if (chars.Contains(i))
					return c;
				return i;
			}).Take(125).ToArray());
		}

	}
}
