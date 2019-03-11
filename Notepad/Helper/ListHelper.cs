
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Linq;

namespace Notepad
{
	
	public static class ListHelper
	{
		public static string ToList(string value)
		{
			
			return string.Join(Environment.NewLine, value.Trim()
			                   .Split(Environment.NewLine.ToArray(), 
				StringSplitOptions.RemoveEmptyEntries).
			                   Select(i => {
			                          
				if (i.StartsWith("* ")) {
					return i;
				} else {
					return	"* " + i.Trim();
				}
			}));
		}
		
		public static string ToCodeList(string value)
		{
			
			return string.Join(Environment.NewLine, value.Trim()
			                   .Split(Environment.NewLine.ToArray(), 
				StringSplitOptions.RemoveEmptyEntries).
			                   Select(i => "* `" + i.Trim() + "`"));
		}
		public static string ToCodeListByOrder(string value)
		{
			var count = 1;
			
			return string.Join(Environment.NewLine, value.Trim()
			                   .Split(Environment.NewLine.ToArray(), 
				StringSplitOptions.RemoveEmptyEntries).
				                   Select(i => (count++) + ". `" + i.Trim() + "`"));
		}
	
	}
}
