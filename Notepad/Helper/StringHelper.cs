using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace Notepad
{
	
	public static class StringHelper
	{
		public static String DurationToMillis(this string str){
				var matched=Regex.IsMatch(str,"(\\d+:)*\\d+:\\d+");
			                        	if(matched){
			                        		var p=str.Split(':');
			                        		var j=0;
			                        	var	length=p.Length-1;
			                        		
			                        		for (int i = length; i > -1; i--) {
			                        			
			                        		j+=(int.Parse(p[i])*(int)(Math.Pow(60,length-i)));
			                        		}
			                        	return (j*1000).ToString();
				}
				return "";
		}
		
		public  static String KeepMatchesArray(string text, string find)
		{
			 
			return Regex.Matches(text, find)
				.Cast<Match>()
				.Select(i =>i.Value)
				///.OrderBy(i => i)
				.Distinct()
				.Aggregate(new StringBuilder(), 
				           (builder, nextValue) => builder.Append(nextValue).Append(',').AppendLine())
				.ToString();
			
		}
		public  static String KeepMatchesIntoPostgresSQL(string text, string find)
		{
			 
			return Regex.Matches(text, find)
				.Cast<Match>()
				.Select(i => i.Value + " text,")
				///.OrderBy(i => i)
				.Distinct()
				.Aggregate(new StringBuilder(), 
				(builder, nextValue) => builder.AppendLine(nextValue))
				.ToString();
			
		}
		public  static String KeepMatchesIntoSwitch(string text, string find)
		{
			 
			return Regex.Matches(text, find)
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

	
 