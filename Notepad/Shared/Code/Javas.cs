
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace Shared
{
	 
	public static class Javas
	{
		public static String FormatStaticIntField(string value)
		{
			int i = 0;
			return		Regex.Replace(value, "(?<=)[ \\d<]+(?=;)", new MatchEvaluator(v => {
				return string.Format("1 << {0}", ++i);
			}));
			// = 1 << 5;
		}
public static String FormatStaticStringField(string value)
		{
	//  String ACTION_TOGGLE_PAUSE = "";
	var  j=Regex.Matches(value,"(?<=String +)[\\w\\d]+(?= +\\=)").Cast<Match>().Select(i=>i.Value).ToArray();
			 int ix = 0;
			return		Regex.Replace(value, "(?<=\")[\\w\\d\\.\\-]*?(?=\";)", new MatchEvaluator(v => {
			                                                                                 	return "euphoria.psycho.fun."+j[ix++];
			}));
			 
		}
	}
}
