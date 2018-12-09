
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
	 
	public static class Javas
	{
		public static string  ParseJavaParameters(string value)
		{
			
			var a = value.SubstringAfter("(").SubstringBeforeLast(")");
			if(a.IsVacuum())
				return "super."+value;
			var b = a.Split(new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
			var c = b.Select(i => Regex.Split(i, "\\s").Where(ix => ix.IsReadable()).Last()).ToArray();
			 
			
			return "super." + value.SubstringBefore("(") + "(" + string.Join(",", c) + ")";
			 
		}
		public static string GenerateMethods(string value){
			var ls = new List<string>();
				var ls1 = new List<string>();
				var hd = new HtmlAgilityPack.HtmlDocument();
				hd.LoadHtml(value);
				//[contains(@class, 'api')][@data-version-added]
				var m="public";
				if(!value.Contains("id=\"pubmethods\"")){
					m="protected";
				}
				var isFirst=true;
				var nodes = hd.DocumentNode.SelectNodes("//tbody/tr").ToArray();
				foreach (var element in nodes) {
					if(isFirst){
						isFirst=false;
						continue;
					}
					try {
					 
						var children = element.ChildNodes.Where(i => i.NodeType != HtmlAgilityPack.HtmlNodeType.Text).ToArray();
						var rs = children[0].InnerText.Trim().SubstringAfterLast(" ");
						if (rs.Contains("final"))
							continue;
						var rs1 = children[1].ChildNodes[1].InnerText.Trim();
						var rs2 = "";
						string  rs3="";
						try {
							rs3=ParseJavaParameters(rs1);
							rs2 =Regex.Replace( children[1].ChildNodes[3].InnerText.Trim(),"[\r\n]+"," ");
							
						} catch(Exception e) {
						
						}
						var sb = new StringBuilder();
						
						sb.AppendLine("@Override");
						sb.AppendFormat("{2} {0} {1} {{",rs,rs1,m);
						
						sb.AppendLine("\n// "+rs2);
						if(rs.Contains("void")){
							sb.AppendFormat("{0};\n",rs3);
						}else{
							sb.AppendFormat("return {0};\n",rs3);
							
						}
						sb.AppendLine("}\n");
						
			           
						ls.Add(sb.ToString());
					} catch (Exception ex) {
						
					}
			                    
			                      		
				}
				 return string.Join("\n",ls);     
		}
		public static string GenerateLog(string value)
		{
			// if (DEBUG) Log.e(TAG, "[play]");
			value = Regex.Replace(value, "if \\(DEBUG\\) Log.e\\(TAG, \"\\[[^\\[]*?\\]\"\\);", "");
			var list = CodeHelper.FormatMethodList(value);
			//var orders=list.Select(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' ')).ToArray();
			
			list = list.OrderBy(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '))
				.Select(i => i.ReplaceFirst("{", string.Format("{{\n if(DEBUG) Log.e(TAG,\"[{0}]\");\n", i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '))));
			return string.Join(Environment.NewLine, list).RemoveEmptyLines();
		}
		public static string GeneratePublicLog(string value)
		{
			  
			var list = CodeHelper.FormatMethodList(CodeHelper.StripComments(value));
			 
			
			list = list.Where(i=>i.TrimStart().StartsWith("public")).Select(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '));
				 
			return  "Log.e(TAG,"+string.Join(Environment.NewLine, list.Select(i=>string.Format("\"\\nobj.{0}() = \"+obj.{0}()+"+"\n",i)))+");\n";
		}
		
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
			var j = Regex.Matches(value, "(?<=String +)[\\w\\d]+(?= +\\=)").Cast<Match>().Select(i => i.Value).ToArray();
			int ix = 0;
			return		Regex.Replace(value, "(?<=\")[\\w\\d\\.\\-]*?(?=\";)", new MatchEvaluator(v => {
				return "euphoria.psycho.fun." + j[ix++];
			}));
			 
		}
	}
}
