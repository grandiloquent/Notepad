namespace Temporary{
	using System.Text;
	using System.Text.RegularExpressions;
	using System.IO;
	using System.Linq;
	public static class Strings{
		
		public static string RemoveWhiteSpace(this string value){
			return Regex.Replace(value,"\\s+","");
		}
		public static string GetValidateFileName(this string fileName){
			var chars=Path.GetInvalidFileNameChars();
			var results=new char[fileName.Length];
			for (int i = 0,j=fileName.Length; i <j; i++) {
				if(chars.Any(c=>c==fileName[i]))results[i]=' ';
				else results[i]=fileName[i];
			}
			return new string(results);
		}
		public static void CreateDirectoryIfNotExists(this string dir){
			if(Directory.Exists(dir))return;
			Directory.CreateDirectory(dir);
		}
		public static string Sub(this string value){
			var s=Regex.Replace(value,"<sub[^>]*?>","△",RegexOptions.IgnoreCase);
			s=Regex.Replace(s,"</sub>","▲",RegexOptions.IgnoreCase);
			
			 s=Regex.Replace(s,"<sup[^>]*?>","◇",RegexOptions.IgnoreCase);
			s=Regex.Replace(s,"</sup>","◆",RegexOptions.IgnoreCase);
			return s;
		}
			
		public static string FromSub(this string value){
			var s=value.Replace("△","<sub>")
				.Replace("▲","</sub>")
				.Replace("◇","<sup>")
				.Replace("◆","</sup>");
			
		
			return s;
		}
		
		public static string SubstringBeforeWhiteSpace(string value)
		{
			for (int i = 0; i < value.Length; i++) {
				if (char.IsWhiteSpace(value[i])) {
					
					return value.Substring(0, i);
				}
			}
			return value;
		}
		public static string SubstringAtfer( this string value,char separator)
		{
			var index=value.IndexOf(separator);
			return index == -1 ? value : value.Substring(index+1);
		}
		public static string RemoveRedundancyLines(this string str )
		{
			var lines = str.Trim().Split('\n');
			// disable once SuggestUseVarKeywordEvident
			StringBuilder sb=new StringBuilder();
			
			for (int i = 0, j = lines.Count(); i < j; i++) {
				string line = lines[i];
				
				sb.Append(line).Append('\n');
				
				if (string.IsNullOrWhiteSpace(lines[i])) {
					while (i + 1 < j && string.IsNullOrWhiteSpace(lines[i + 1])) {
						i++;
					}
				}
			}
			return sb.ToString();
		}
		public static string SubstringAtfer( this string value,string separator)
		{
			var index=value.IndexOf(separator);
			return index == -1 ? value : value.Substring(index+separator.Length);
		}
		public static string SubstringAtferLast( this string value,char separator)
		{
			var index=value.LastIndexOf(separator);
			return index == -1 ? value : value.Substring(index+1);
		}
			public static string SubstringBeforeLast( this string value,char separator)
		{
			var index=value.LastIndexOf(separator);
			return index == -1 ? value : value.Substring(0,index);
		}
				public static string SubstringBefore( this string value,char separator)
		{
			var index=value.IndexOf(separator);
			return index == -1 ? value : value.Substring(0,index);
		}
		public static string SubstringAfterWhiteSpace(string value)
		{
			
			
			for (int i = 0; i < value.Length; i++) {
				if (char.IsWhiteSpace(value[i])) {
					
					return value.Substring(i);
				}
			}
			return value;
		}
	}
}