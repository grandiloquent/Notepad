
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace Notepad
{
	
	public static class CodeHelper
	{
		public static string ExportFile(string file)
		{
			
			var j = "\u0060";
				
			 
			var sb = new StringBuilder();
				
					
			sb.AppendLine("## " + Path.GetFileName(file)).AppendLine();
			var str = file.ReadAllText().Trim();
			while (str.StartsWith("/*")) {
				str = str.SubstringAfter("*/").Trim();
			}
			sb.AppendLine()
			                     			.AppendLine("```")
			                     			.AppendLine()
			                     			.AppendLine(Regex.Replace(str.Replace("`", j), "[\r\n]+", "\r\n"))
			                     			.AppendLine("```")
			                     			.AppendLine();
			                     		
			return sb.ToString();
		}
			
			                   
	}
}
