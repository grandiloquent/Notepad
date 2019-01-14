
using System;
using System.Text;

namespace Utils
{
	 
	public static class Simple
	{
	 
		public static string UpperCase(this string value)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < value.Length; i++) {
				if (i!=0&&char.IsUpper(value[i])) {
					sb.Append('_').Append(char.ToUpper(value[i]));
					
				} else {
					sb.Append(char.ToUpper(value[i]));
				}
				
			}
			return sb.ToString();
		}
	}
}
