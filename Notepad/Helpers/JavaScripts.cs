//namespace Helpers
//{
//
//	using System;
//	using System.Linq;
//	using System.Text;
//	using System.Text.RegularExpressions;
//	using Microsoft.Ajax.Utilities;
//	using System.IO;
//	using System.Diagnostics;
//	
//	public static class JavaScripts
//	{
//		public static void CompileTypeScriptInDirectory(string dir, string[] excludes)
//		{
//			Directory.GetFiles(dir, "*.ts").Where(i => !excludes.Contains(i.GetFileName())).AsParallel().ForAll(fileName => {
//				Process.Start(new ProcessStartInfo() {
//					FileName = "tsc",
//					Arguments = "\"" + fileName + "\""
//				});                                                                     
//			                                                                                                 
//			});
//			
//		}
//		
//		public static void PublishTypeScript(string fileName, string destinationDirectory, string templateFile)
//		{
//				
//			Directory.GetFiles(destinationDirectory, "*.js")
//					.Where(i => i.GetFileName().StartsWith(fileName.GetFileNameWithoutExtension() + "_v"))
//				.AsParallel().ForAll(File.Delete);
//			
//			var destinationFileName =
//				Path.Combine(destinationDirectory,fileName.GetFileNameWithoutExtension()+".js");
//				
//			//  
//			
//		var p=	Process.Start(new ProcessStartInfo() {
//				FileName = "tsc",
//				Arguments = "\"" + fileName + "\" --outFile " + "\"" + destinationFileName + "\""
//			});
//			p.WaitForExit();
//			var min = new Minifier();
//			var minJs =	min.MinifyJavaScript(destinationFileName.ReadAllText());
//			
//			var source =	destinationFileName.ChangeFileName(Cryptos.GenerateFileVersion(destinationFileName).GetFileNameWithoutExtension() + ".min");
//			source.WriteAllText(minJs);
//		
////			File.Move(destinationFileName,Path.Combine(destinationDirectory,Cryptos.GenerateFileVersion(fileName)));
////			destinationFileName=Path.Combine(destinationDirectory,Cryptos.GenerateFileVersion(fileName));
////	 
//				var reg="(?<=src=\"/static/)"
//				          + fileName.GetFileNameWithoutExtension()
//					+ "[\\.\\w\\-]+(?=\">)";
//			 var str = Regex.Replace(templateFile.ReadAllText(), 
//				          reg,source.GetFileName());
//			templateFile.WriteAllText(str);
//		}
//		public static void CompileTypeScript(string fileName)
//		{
//			if (!fileName.EndsWith(".ts"))
//				return;
//			
//			Process.Start(new ProcessStartInfo() {
//				FileName = "tsc",
//				Arguments = "\"" + fileName + "\""
//			});                                                                     
//			                                                                                                 
//		}
//		public static String CompileTypeScript(string fileName, string outputFileName)
//		{
//			Process.Start(new ProcessStartInfo() {
//				FileName = "tsc",
//				Arguments = "\"" + fileName + "\" --outFile " + "\"" + outputFileName + "\""
//			});
//			              
//			return null;
////			Process process = new Process();
////			StringBuilder outputStringBuilder = new StringBuilder();
////			const int PROCESS_TIMEOUT = 10 * 1000;
////			string exeFileName = "cmd";
////			string workingDirectory = fileName.GetDirectoryName();
////			try {
////				process.StartInfo.FileName = exeFileName;
////				process.StartInfo.WorkingDirectory = workingDirectory;
////				process.StartInfo.Arguments = "/K tsc \""+fileName+"\" --outFile "+"\""+outputFileName+"\"";
////				process.StartInfo.RedirectStandardError = true;
////				process.StartInfo.RedirectStandardOutput = true;
////				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
////				process.StartInfo.CreateNoWindow = true;
////				process.StartInfo.UseShellExecute = false;
////				process.EnableRaisingEvents = false;
////				process.OutputDataReceived += (sender, eventArgs) => outputStringBuilder.AppendLine(eventArgs.Data);
////				process.ErrorDataReceived += (sender, eventArgs) => outputStringBuilder.AppendLine(eventArgs.Data);
////				process.Start();
////				process.BeginOutputReadLine();
////				process.BeginErrorReadLine();
////				var processExited = process.WaitForExit(PROCESS_TIMEOUT);
////
////				if (processExited == false) { // we timed out...
////					process.Kill();
////					throw new Exception("ERROR: Process took too long to finish");
////				} else if (process.ExitCode != 0) {
////					var output = outputStringBuilder.ToString();
////					var prefixMessage = "";
////
////					throw new Exception("Process exited with non-zero exit code of: " + process.ExitCode + Environment.NewLine +
////					"Output from process: " + outputStringBuilder.ToString());
////				}
////			} finally {                
////				process.Close();
////			}
////			return outputStringBuilder.ToString();
////			
//		}
//		public static void  CompressJavaScript(string source, 
//			string fileName)
//		{
//		 
//			var m = new  Minifier();
//			var r =	m.MinifyJavaScript(source);
//			fileName.WriteAllText(r);
//			
//		}
//		public static void CompressScript(string source
//		                                      , string destinationDirectory
//		                                     , string templateFile)
//		{
//			
//			var fileName = Path.Combine(destinationDirectory, Path.ChangeExtension(Cryptos.GenerateFileVersion(source), ".js"));
//			
//			var searchPattern = "*.js";
//			 
//
//			var matched = source.GetFileNameWithoutExtension().TrimStart('.') + "_v";
////			var xxx=Directory.GetFiles(destinationDirectory, searchPattern)
////				.Where(i => i.StartsWith(source.GetFileNameWithoutExtension().TrimStart('.') + "_v_")).ToArray();
//			//	var files=Directory.GetFiles(destinationDirectory, searchPattern);
////				.Where(i => i.StartsWith(matched))
////				.AsParallel().ToArray();
//			Directory.GetFiles(destinationDirectory, searchPattern)
//				.Where(i => i.GetFileName().StartsWith(matched))
//				.AsParallel().ForAll(File.Delete);
//			
//	 
//			var js = CompileTypeScript(source, fileName);
//			//CompressJavaScript(js,fileName);
//				
//			var str = Regex.Replace(templateFile.ReadAllText(), 
//				          "(?<=src=\"/static/)"
//				          + source.GetFileNameWithoutExtension()
//				          + "[\\.\\w\\-]+(?=\">)", fileName.GetFileName());
//			templateFile.WriteAllText(str);
//
//		}
//		public static void  CompressJavaScriptFile(string sourceFile, 
//			string destinationDirectory)
//		{
//		 
//			var m = new  Minifier();
//			var r =	m.MinifyJavaScript(sourceFile.ReadAllText());
//			Path.Combine(destinationDirectory, 
//				sourceFile.GetFileNameWithoutExtension().TrimStart('.')
//				+ ".min" + sourceFile.GetExtension()).WriteAllText(r);
//			
//		}
//		public static void  CompressJavaScriptFile(string sourceFile, 
//			string destinationDirectory, string fileName)
//		{
//		 
//			var m = new  Minifier();
//			var r =	m.MinifyJavaScript(sourceFile.ReadAllText());
//			Path.Combine(destinationDirectory, 
//				fileName).WriteAllText(r);
//			
//		}
//		public static void  CompressStyleScriptFile(string sourceFile, 
//			string destinationDirectory)
//		{
//		 
//			var m = new  Minifier();
//			var r =	m.MinifyStyleSheet(sourceFile.ReadAllText());
//			Path.Combine(destinationDirectory, 
//				sourceFile.GetFileNameWithoutExtension().TrimStart('.')
//				+ ".min" + sourceFile.GetExtension()).WriteAllText(r);
//			
//		}
//		public static void  CompressStyleScriptFile(string sourceFile, 
//			string destinationDirectory, string fileName)
//		{
//		 
//			var m = new  Minifier();
//			var r =	m.MinifyStyleSheet(sourceFile.ReadAllText());
//			Path.Combine(destinationDirectory, fileName).WriteAllText(r);
//			
//		}
//	}
//}