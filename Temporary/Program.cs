 
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Linq;
namespace Temporary
{
	class Program
	{
		[STAThread]
		
		public static void Main(string[] args)
		{
			//var dir=@"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\妇产科门急诊手册(第2版) (实用门急诊丛书) - 沈宗姬等\text";
//			var dir=@"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生药物手册 (1500余种全科医生常用药物，4000多种药物商品名快速索引) - 邵柏\text";
//
//			foreach (var element in System.IO.Directory.GetFiles(dir,
//			                                                    
//			                                                    "*.html") ){
//			
//			Markdowns.ConvertToMarkdowns(element);
//				
//			}
//			var utf = new UTF8Encoding(false);
//			var dirtxt = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生药物手册 (1500余种全科医生常用药物，4000多种药物商品名快速索引) - 邵柏\text";
//			var ls = Directory.GetFiles(dirtxt, "*.txt");
//			foreach (var f in ls) {
//				var lines =	File.ReadAllText(f, utf).Split('\n');
//				for (int i = 0; i < lines.Length; i++) {
//					if (lines[i].Trim() == "## 异名") {
//					
//						for (int a = i - 1; a > -1; a--) {
//							if (!string.IsNullOrWhiteSpace(lines[a])) {
//								if (!lines[a].StartsWith("# ")) {
//									lines[a] = "# " + lines[a];
//									
//								}
//								break;
//							}
//						}
//						
//					}
//				}
//				File.WriteAllText(f, string.Join("\n", lines), utf);
//		
//			
//			}
			
			var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生药物手册 (1500余种全科医生常用药物，4000多种药物商品名快速索引) - 邵柏\text";
			var outdir = dir + "\\Formated";
			var ls = Directory.GetFiles(dir, "*.txt");
			foreach (var f in ls) {
				Markdowns.FormatFile(f, outdir);
			}
	
			
		}
	
		static void Check(){
			var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生药物手册 (1500余种全科医生常用药物，4000多种药物商品名快速索引) - 邵柏\text\Formated";
					var outdir = dir + "\\Formated";
					outdir.CreateDirectoryIfNotExists();
					var utf = new UTF8Encoding(false);
					foreach (var f in Directory.GetFiles(dir,"*.txt"))
						if (Regex.Matches(File.ReadAllText(f, utf), "## 用法用量").Count > 1) {
							Console.WriteLine(f);
						}
		
					Console.ReadKey();
		}
		
		
		//Clipboard.SetText(FormatMarkdowns(Clipboard.GetText()));
		// Markdowns.ConvertToMarkdowns("C:\\3.html");
			
		//			}
		//			var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生药物手册 (1500余种全科医生常用药物，4000多种药物商品名快速索引) - 邵柏\text\Formated";
		//			var outdir = dir + "\\Formated";
		//			outdir.CreateDirectoryIfNotExists();
		//			var utf = new UTF8Encoding(false);
		//			foreach (var f in Directory.GetFiles(dir,"*.txt"))
		//				if (Regex.Matches(File.ReadAllText(f, utf), "## 用法用量").Count > 1) {
		//					Console.WriteLine(f);
		//				}
		//			var dirtxt = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\全科医生药物手册 (1500余种全科医生常用药物，4000多种药物商品名快速索引) - 邵柏\text\新建文件夹";
		//			var ls= Directory.GetFiles(dirtxt,"*.txt");
		//			foreach (var f in ls) {
		//				var lines =	File.ReadAllText(f,utf).Split('\n');
		//				for (int i = 0; i < lines.Length; i++) {
		//					if (lines[i].Trim() == "## 异名") {
		//
		//						for (int a = i-1; a >-1; a--) {
		//							if(!string.IsNullOrWhiteSpace(lines[a])){
		//								if(!lines[a].StartsWith("# ")){
		//									lines[a]="# "+lines[a];
		//
		//								}
		//								break;
		//							}}
		//						break;
		//					}
		//				}
		//				File.WriteAllLines(f,lines,utf);
		
		//Markdowns.FormatFile(f,outdir);
		
		//Clipboard.SetText(FormatMarkdowns(Clipboard.GetText()));
		// Markdowns.ConvertToMarkdowns("C:\\3.html");
			
		//			}
		//				Console.ReadKey(true);
		//		}
		
		//		public static string FormatMarkdowns(string value)
		//		{
		//
		//			var lines = value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		//
		//			var sb1 = new StringBuilder();
		//			var sb2 = new StringBuilder();
		//
		//			foreach (var line in lines) {
		//				var lineStr = line.Trim();
		//				var first = SubstringBeforeWhiteSpace(lineStr);
		//				var second = SubstringAfterWhiteSpace(lineStr).Trim();
		//
		//				first =	Regex.Replace(first, "\\d+$", "<sub>$0</sub>");
		//				sb1.AppendFormat("|{0}||{1}|\r\n", first, second);
		//
		//				sb2.AppendFormat("|{0}|{1}（{0}）|\r\n", first, second);
		//			}
		//
		//			return sb1.ToString() + Environment.NewLine + Environment.NewLine + sb2.ToString();
		//
		//		}
		
	}
}