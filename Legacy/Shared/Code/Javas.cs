
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

using Common;

namespace Shared
{
	 
	public static class Javas
	{

		public static string GeneateViewFromXML(string value)
		{
			var list1 = new List<String>();
			var list2 = new List<String>();
			var list3 = new List<String>();
			var list4 = new List<String>();
			var list5 = new List<String>();
			var list6 = new List<String>();
			
			var xd = XDocument.Parse(value);
			var nodes = xd.Descendants().Where(i => i.Attributes().Any(ix => ix.Name.LocalName == "id"));
			foreach (var element in nodes) {
				var name = element.Name.LocalName.SubstringAfterLast('.');
				var id = element.Attributes().First(i => i.Name.LocalName == "id").Value.SubstringAfterLast('/');
				var formatName = "m" + Regex.Replace(id.Capitalize(), "_[\\w]", new MatchEvaluator(v => {
				                                                                           
					return v.Value.TrimStart('_').ToUpper();
				}));
				list1.Add(string.Format("{2}=({1})itemView.findViewById(R.id.{0});", id, name, formatName));
				list2.Add(string.Format("private {1} {0};", formatName, name));
				list3.Add(string.Format("{1} {2}=({1})itemView.findViewById(R.id.{0});", id, name, formatName.TrimStart('m').DeCapitalize()));
				list4.Add(string.Format("{1}=findViewById(R.id.{0});", id,  formatName));
				list5.Add(string.Format("{0} {1};",  name, formatName.TrimStart('m').DeCapitalize()));
				list6.Add(string.Format("{1}=itemView.findViewById(R.id.{0});", id,formatName.TrimStart('m').DeCapitalize()));
			
			}
			
			return  string.Join("\n", list1.Concat(list2).Concat(list3).Concat(list4).Concat(list5).Concat(list6));
		}
		public static string GenerateBuilder(string value, string className)
		{
			var lines = value.ToLines();
			var parameters = lines.Select((i) => {
				string[] array = new string[2];
				var str = i.SubstringBefore('=').TrimEnd(';').Trim().Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
				array[0] = str[str.Length - 2];
				array[1] = str[str.Length - 1];
				return array;
			});
			var list1 = new List<String>();
			var list2 = new List<String>();

			foreach (var element in parameters) {
				list2.Add(string.Format("public {0} get{2}(){{\nreturn {1};\n}}\n", element[0], element[1], element[1].TrimStart('m').Capitalize()));
				
				list1.Add(string.Format("public {4} set{2}({0} {3}){{\nthis.{1}={3};\nreturn this;\n}}\n", element[0], element[1], element[1].TrimStart('m').Capitalize(), element[1].TrimStart('m').DeCapitalize(), className));
			}
			return string.Join("\n", list1.Concat(list2));
			
		}
		public static string  ParseJavaParameters(string value)
		{
			
			var a = value.SubstringAfter("(").SubstringBeforeLast(")");
			if (a.IsVacuum())
				return "super." + value;
			var b = a.Split(new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
			var c = b.Select(i => Regex.Split(i, "\\s").Where(ix => ix.IsReadable()).Last()).ToArray();
			 
			
			return "super." + value.SubstringBefore("(") + "(" + string.Join(",", c) + ")";
			 
		}
		public static string GenerateMethods(string value)
		{
			var ls = new List<string>();
			var ls1 = new List<string>();
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(value);
			//[contains(@class, 'api')][@data-version-added]
			var m = "public";
			if (!value.Contains("id=\"pubmethods\"")) {
				m = "protected";
			}
			var isFirst = true;
			var nodes = hd.DocumentNode.SelectNodes("//tbody/tr").ToArray();
			foreach (var element in nodes) {
				if (isFirst) {
					isFirst = false;
					continue;
				}
				try {
					 
					var children = element.ChildNodes.Where(i => i.NodeType != HtmlAgilityPack.HtmlNodeType.Text).ToArray();
					var rs = children[0].InnerText.Trim().SubstringAfterLast(" ");
					if (rs.Contains("final"))
						continue;
					var rs1 = children[1].ChildNodes[1].InnerText.Trim();
					var rs2 = "";
					string rs3 = "";
					try {
						rs3 = ParseJavaParameters(rs1);
						rs2 = Regex.Replace(children[1].ChildNodes[3].InnerText.Trim(), "[\r\n]+", " ");
							
					} catch (Exception e) {
						
					}
					var sb = new StringBuilder();
						
					sb.AppendLine("@Override");
					sb.AppendFormat("{2} {0} {1} {{", rs, rs1, m);
						
					sb.AppendLine("\n// " + rs2);
					if (rs.Contains("void")) {
						sb.AppendFormat("{0};\n", rs3);
					} else {
						sb.AppendFormat("return {0};\n", rs3);
							
					}
					sb.AppendLine("}\n");
						
			           
					ls.Add(sb.ToString());
				} catch (Exception ex) {
						
				}
			                    
			                      		
			}
			return string.Join("\n", ls);     
		}
		public static string GenerateLog(string value)
		{
			// if (DEBUG) Log.e(TAG, "[play]");
			value = Regex.Replace(value, "if \\(DEBUG\\) Log.e\\(TAG, \"\\[[^\\[]*?\\]\"\\);", "");
			var list = CodeHelper.FormatMethodList(value);
			//var orders=list.Select(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' ')).ToArray();
			
			list = list.OrderBy(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '))
				.Select(i => i.ReplaceFirst("{", string.Format("{{\n if(DEBUG) Log.d(TAG,\"[{0}]\");\n", i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '))));
			return string.Join(Environment.NewLine, list).RemoveEmptyLines();
		}
		public static string   GenerateVariableLog(string value)
		{
			// (?<=[\\w] );.OrderBy(i=>i)
			var matches = Regex.Matches(value, "[\\w\\d_]+(?= \\=)").Cast<Match>().Select(i => i.Value).Distinct();
			var sb = new StringBuilder();
			sb.Append("Log.d(TAG,\"\\n\"+");
			foreach (var element in matches) {
				sb.AppendFormat("\"\\n {0} =>\" +{0}+\n", element);
			}
			sb.Remove(sb.Length - 2, 2);
			sb.AppendLine(");");
			return sb.ToString();
		}
		public static string GeneratePublicLog(string value)
		{
			  
			var list = CodeHelper.FormatMethodList(CodeHelper.StripComments(value));
			 
			
			list = list.Where(i => i.TrimStart().StartsWith("public")).Select(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '));
				 
			return  "Log.d(TAG," + string.Join(Environment.NewLine, list.Select(i => string.Format("\"\\nobj.{0}() = \"+obj.{0}()+" + "\n", i))) + ");\n";
		}
		public static IEnumerable<String> ExtractPublicMethodName(string value)
		{
			value = CodeHelper.StripComments(value);
			var list = CodeHelper.FormatMethodList(CodeHelper.StripComments(value));
			 
			
			list = list.Where(i => Regex.IsMatch(i, "\n\\s+public")).ToList();
			list = list.Select((i) => {
				var array = i.SubstringBefore('(').Trim().Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
				if (array.Length > 1)
					return array[array.Length - 2] + " " + array[array.Length - 1];
				return "";
			}).ToList();
				 
			return list;
		}
		public static String FormatStaticIntField(string value)
		{
			int i = 0;
			return		Regex.Replace(value, "(?<=)[ \\d<]+(?=;)", new MatchEvaluator(v => {
				//return string.Format("1 << {0}", ++i);
				return string.Format("{0}", i++);
			}));
			// = 1 << 5;
		}
		public static String FormatStaticStringFieldShort(string value)
		{
			//  String ACTION_TOGGLE_PAUSE = "";
			var j = Regex.Matches(value, "(?<=String +)[\\w\\d]+(?= +\\=)").Cast<Match>().Select(i => i.Value).ToArray();
			int ix = 0;
			return		Regex.Replace(value, "(?<=\")[\\w\\d\\.\\-]*?(?=\";)", new MatchEvaluator(v => {
				return j[ix++].SubstringAfter('_').ToLower();
			}));
			 
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
