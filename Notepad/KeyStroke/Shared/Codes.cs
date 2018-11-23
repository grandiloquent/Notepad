﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
using System.Xml;

using System.Xml.Linq;

namespace Shared
{
	
	public static class Codes
	{
		public static string FormatCSharpCode(string value)
		{

			var s = new StringBuilder();

			var rootNode = CSharpSyntaxTree.ParseText(value).GetRoot();

			var namespace_ = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

			if (namespace_.Any()) {

				s.Append(namespace_.First().NamespaceKeyword.Text).Append(' ').Append(namespace_.First().Name).Append('{');
			}

			var using_ = rootNode.DescendantNodes().OfType<UsingDirectiveSyntax>();
			if (using_.Any()) {

				using_ = using_.OrderBy(i => i.Name.ToString());//.Distinct(i => i.Name.GetText());

				foreach (var item in using_) {
					s.Append(item.ToFullString());
				}
			}
			
			
			var class_ = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

			if (class_.Any()) {
				class_ = class_.OrderBy(i => i.Identifier.ValueText);

				foreach (var item in class_) {
                    
					s.Append(item.Modifiers.ToFullString()).Append(" class ").Append(item.Identifier.ValueText);
					if (item.BaseList != null)
						s.Append(item.BaseList.GetText());
                    	
					s.Append('{');
					var field_ = item.ChildNodes().OfType<FieldDeclarationSyntax>();
					if (field_.Any()) {
						field_ = field_.OrderBy(i => i.Declaration.Variables.First().ToFullString());

						foreach (var itemField in field_) {

							s.Append(itemField.ToFullString().Trim() + '\n');
						}
					}
					var enum_ = item.ChildNodes().OfType<EnumDeclarationSyntax>();
					if (enum_.Any()) {
						enum_ = enum_.OrderBy(i => i.Identifier.ToFullString());
						
						foreach (var itemEnum in enum_) {
							s.Append(itemEnum.ToFullString());
						}
					}
					var delegate_ = item.ChildNodes().OfType<DelegateDeclarationSyntax>();
					if (delegate_.Any()) {
						delegate_ = delegate_.OrderBy(i => i.Identifier.ToFullString());

						foreach (var itemDelegate in delegate_) {

							s.Append(itemDelegate.ToFullString() + '\n');
						}
					}                                    
					var struct_ = item.ChildNodes().OfType<StructDeclarationSyntax>();
					if (struct_.Any()) {
						struct_ = struct_.OrderBy(i => i.Identifier.ToFullString());
						
						foreach (var itemStruct in struct_) {
							s.Append(itemStruct.ToFullString());
						}
					}
					var constructor_ = item.ChildNodes().OfType<ConstructorDeclarationSyntax>();
					if (constructor_.Any()) {
						constructor_ = constructor_.OrderBy(i => i.Identifier.ValueText);//.OrderBy(i => i.Identifier.ValueText).ThenBy(i=>i.Modifiers.ToFullString());
						foreach (var itemMethod in constructor_) {


							s.Append(itemMethod.ToFullString());
						}

					}
					var method_ = item.ChildNodes().OfType<MethodDeclarationSyntax>();

					if (method_.Any()) {
						method_ = method_.OrderByDescending(i => i.Modifiers.ToFullString().Contains("extern")).ThenBy(i => i.Identifier.ValueText.Trim());//.OrderBy(i => i.Identifier.ValueText).ThenBy(i=>i.Modifiers.ToFullString());
						
						//method_ = method_.OrderBy(i => i.Modifiers.ToFullString().Trim() + i.Identifier.ValueText.Trim());//.OrderBy(i => i.Identifier.ValueText).ThenBy(i=>i.Modifiers.ToFullString());
						foreach (var itemMethod in method_) {


							s.Append(itemMethod.ToFullString());
						}

					}
					s.Append('}');
				}

			}
			s.Append('}');
			return s.ToString();

		}
		public static IEnumerable<string> FormatMethodList(string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (int i = 0; i < value.Length; i++) {
				sb.Append(value[i]);

				if (value[i] == '{') {
					count++;
				} else if (value[i] == '}') {
					count--;
					if (count == 0) {
						ls.Add(sb.ToString());
						sb.Clear();
					}
				}

			}
			return ls;

		}
		public static void FormatWithClangFormat(string fileName)
		{
			Process.Start(new ProcessStartInfo() {
				FileName = "clang-format",
				Arguments = string.Format("\"{0}\" -i -style=llvm -sort-includes=false", fileName)
			});
		}
		public static string OrderKotlinFun(string value)
		{
			var list = FormatMethodList(value);
			list = list.OrderBy(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '));
			return string.Join(Environment.NewLine, list).RemoveEmptyLines();
		}
		public static void FormatVSCTypeDef()
		{
			WinForms.OnClipboardString((str) => {
				var result = str.Trim();
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix", result.SubstringAfterLast(' ').TrimEnd(';'));
				//obj.Add("prefix", string.Join("", matches).ToLower());
				obj.Add("body", result.SubstringAfterLast(' ').TrimEnd(';') + " $0");// changed
				
		 
				var r = new Dictionary<string,dynamic>();
				r.Add(result, obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return	sr.Substring(1, sr.Length - 2) + ",";
			                         
			});
		}
		public static void CombineAndroidResource(string dir){
			var sb=new StringBuilder();
			foreach (var element in System.IO.Directory.GetFiles(dir,"*.xml")) {
				var xd = new  XmlDocument();
				xd.LoadXml(element.ReadAllText());
				sb.AppendLine(xd.DocumentElement.InnerXml);
			}
			System.IO.Path.Combine(dir,dir.GetFileName()+".txt").WriteAllText(sb.ToString());
		}
		public static string FormatAndroidResource(string value)
		{
			var xd = new  XmlDocument();
			xd.LoadXml(value);
			var ls =	new List<XmlNode>();
//			var first =	xd.FirstChild;
//			var sb = new StringBuilder();
//			if (first is XmlDeclaration) {
//				sb.AppendLine(first.OuterXml.ToString());
//			}
			var i = xd.DocumentElement.ChildNodes.GetEnumerator();
			while (i.MoveNext()) {
				var nodes = i.Current as XmlNode;
				if(nodes is XmlComment) continue;
				ls.Add(nodes);
			}
			 
			var array=ls.ToArray();
			xd.DocumentElement.InnerXml=string.Join(Environment.NewLine,ls.OrderBy(ie=>ie.Name).ThenBy(ie=> ie.Attributes["name"].Value).Distinct(ie=>ie.Attributes["name"].Value).Select(ie=>ie.OuterXml.ToString()));
			return xd.OuterXml;
			
		}
		public static string[] ParseJavaParameters(string value){
			
			var a=value.SubstringAfter("(").SubstringBeforeLast(")");
			var b=a.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
			var c=b.Select(i=>Regex.Split(i,"\\s").Where(ix=>ix.IsReadable()).Last()).ToArray();
			 
			
			var h="super."+value.SubstringBefore("(")+"("+string.Join(",",c)+")";
			var j=string.Join(",", b.Select((i)=>{
			                                	var list= Regex.Split(i,"\\s").Where(ix=>ix.IsReadable()).ToArray();
			                                	return list[1]+":"+list[0].Capitalize();
			           
			               	
			                                }));
			return new string[]{h,j};
		}
		public static string GenerateAndroidId(string value)
		{
			var pattern = "(?<=@\\+id\\/)[\\w_\\d]+";
			
			var matches = Regex.Matches(value, pattern).Cast<Match>().Select(i => i.Value).OrderBy(i => i);
			
			var p1 = new []{ "R.id.{0} -> {0}.isChecked = true" };
				
			var list = new List<String>();
			for (int i = 0; i < p1.Length; i++) {
				foreach (var element in matches) {
				
					list.Add(string.Format(p1[i], element));
				}
			}
			return string.Join(Environment.NewLine, list);
			
		}
	}


	
}