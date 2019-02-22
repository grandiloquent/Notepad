using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Common;

namespace Shared
{
	 
	public static class C
	{
		 public static void RunGenerateGccCommand(string f)
		{
			
//			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
//			if (!Directory.Exists(dir)) {
//				Directory.CreateDirectory(dir);
//			}
			var dir = Path.Combine(Path.GetDirectoryName(f), "bin");
			dir.CreateDirectoryIfNotExists();
			var arg = "";
			var argLines = f.ReadLines();
			foreach (var element in argLines) {
				if (element.IsVacuum())
					continue;
				if (element.StartsWith("// ")) {
					arg += element.Substring(3) + " ";
				} else
					break;
			}
			var exe = Path.GetFileNameWithoutExtension(f) + ".exe";
				
			try {
			 
				var ps =	Process.GetProcesses().Where(i => i.ProcessName == Path.GetFileNameWithoutExtension(f) || i.ProcessName == "cmd");
				if (ps.Any()) {
					foreach (var p in ps) {
						p.Kill();
					}
				}
			} catch {
			}
			//var cmd = string.Format("/K gcc -Wall -g -finput-charset=GBK -fexec-charset=GBK \"{0}\" -o \"{1}\\{3}\" {2} && \"{1}\\{3}\" ", f, dir, arg, exe);
			var cmd = string.Format("/K gcc -Wall -g -finput-charset=UTF-8 -fexec-charset=GBK \"{0}\" -o \"{1}\\{3}\" {2} && \"{1}\\{3}\" ", f, dir, arg, exe);
		
			Process.Start("cmd", cmd);
				
			
		}
	}
}
