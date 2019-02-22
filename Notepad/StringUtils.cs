
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace Notepad
{
	
	public static class StringUtils
	{
		
		public  static String KeepMatchesIntoArray(string text,string find)
		{
			 
			return Regex.Matches(text,find)
				.Cast<Match>()
				.Select(i => "\"" + i.Value + "\",")
				.OrderBy(i => i)
				.Distinct()
				.Aggregate(new StringBuilder(), 
				           (builder, nextValue) => builder.AppendLine(nextValue))
				.ToString();
			
		}
			public  static String KeepMatchesIntoSwitch(string text,string find)
		{
			 
			return Regex.Matches(text,find)
				.Cast<Match>()
				.Select(i => "case \"" + i.Value + "\":")
				.OrderBy(i => i)
				.Distinct()
				.Aggregate(new StringBuilder(), 
				           (builder, nextValue) => builder.AppendLine(nextValue))
				.ToString();
			
		}
	}
}
