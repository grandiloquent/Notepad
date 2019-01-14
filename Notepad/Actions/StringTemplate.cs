
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Notepad
{
	
	public static class StringTemplate
	{
		public static void EscapePattern(TextBox textBox){
			textBox.SelectedText=EscapePattern(textBox.SelectedText);
		}
		private static String EscapePattern(string value){
			value=Regex.Replace(value,"(?<!{){(?!{)","{{");
			return Regex.Replace(value,"(?<!})}(?!})","}}");
			
		}
		 
	}
}
