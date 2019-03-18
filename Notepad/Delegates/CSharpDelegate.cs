namespace Notepad
{
	using Common;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;
	public static  class CSharpDelegate
	{
		[BindMenuItem(Name = "格式化 C# (文件)", SplitButton = "csharpButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
		public static void CollectFunctionNames()
		{
			Forms.OnClipboardFile(f => f.WriteAllText(FormatCode(f.ReadAllText()).RemoveWhiteSpaceLines()));
		}
		[BindMenuItem(Name = "格式化 C# (目录)", SplitButton = "csharpButton", Toolbar = "toolStrip1", AddSeparatorBefore = true)]
		public static void FormatCsharpCodeInDirectory()
		{
			Forms.OnClipboardDirectory(dir =>{
			                      
			                      	var files=Directory.GetFiles(dir,"*.cs");
			                      	foreach (var f in files) {
			                      		f.WriteAllText(FormatCode(f.ReadAllText()).RemoveWhiteSpaceLines());
			                      	}
			                      } );
		}
		
		[BindMenuItem(Name = "分隔源代码", Toolbar = "toolStrip1", SplitButton = "csharpButton")]
		public static void SplitSourceCodeFile()
		{
			Forms.OnClipboardFile(SplitSourceCodeFileInternal);
			
		}
		private static void SplitSourceCodeFileInternal(string fileName)
		{
			var strNameSpace = string.Empty;
			
			var rootNode = CSharpSyntaxTree.ParseText(fileName.ReadAllText()).GetRoot();
			var namespace_ = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>();
			if (namespace_.Any()) {
				var s = new StringBuilder();
				s.Append(namespace_.First().NamespaceKeyword.Text).Append(' ').Append(namespace_.First().Name).Append('{');
				strNameSpace = s.ToString();
			}
			var using_ = rootNode.DescendantNodes().OfType<UsingDirectiveSyntax>();
			var strUsing = string.Empty;
		
			if (using_.Any()) {
				var s = new StringBuilder();
				using_ = using_.OrderBy(i => i.Name.ToString());//.Distinct(i => i.Name.GetText());
				foreach (var item in using_) {
					s.Append(item.ToFullString());
				}
				strUsing = s.ToString();
			}
			var class_ = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();
			if (class_.Any()) {
				class_ = class_.OrderBy(i => i.Identifier.ValueText);
				foreach (var item in class_) {
					var s = new StringBuilder();
					s.AppendLine(strNameSpace).AppendLine(strUsing)
						.AppendLine(item.ToFullString())
						.AppendLine("}");
					(item.Identifier.ValueText + ".cs").GetDesktopPath().WriteAllText(s.ToString());
				}
				
			}
			
				var interfaces = rootNode.DescendantNodes().OfType<InterfaceDeclarationSyntax>();
			if (interfaces.Any()) {
				interfaces = interfaces.OrderBy(i => i.Identifier.ValueText);
				foreach (var item in interfaces) {
					var s = new StringBuilder();
					s.AppendLine(strNameSpace).AppendLine(strUsing)
						.AppendLine(item.ToFullString())
						.AppendLine("}");
					(item.Identifier.ValueText + ".cs").GetDesktopPath().WriteAllText(s.ToString());
				}
				
			}
				
		}
		private static string FormatCode(string value)
		{
			var s = new StringBuilder();
			var rootNode = CSharpSyntaxTree.ParseText(value).GetRoot();
			var namespace_ = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>().OrderBy(i => i.Usings.ToFullString());
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
					s.Append(item.AttributeLists.ToFullString());
					s.Append(item.Modifiers.ToFullString()).Append(" class ").Append(item.Identifier.ValueText);
					if (item.BaseList != null)
						s.Append(item.BaseList.GetText());
					s.Append('{');
					var field_ = item.ChildNodes().OfType<FieldDeclarationSyntax>();
					if (field_.Any()) {
						field_ = field_.OrderBy(i => i.ChildNodes().First().ToFullString()).ThenBy(i => i.Declaration.Variables.First().ToFullString());
						foreach (var itemField in field_) {
							s.Append(itemField.ToFullString().Trim() + '\n');
						}
					}
					var property_ = item.ChildNodes().OfType<PropertyDeclarationSyntax>();
					if (property_.Any()) {
						//Order
						property_ = property_.OrderBy(i => i.ChildNodes().First().ToFullString()).ThenBy(i => i.Identifier.ToFullString());
						foreach (var itemProperty in property_) {
							s.Append(itemProperty.ToFullString().Trim() + '\n');
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
					var methods = item.ChildNodes().OfType<IncompleteMemberSyntax>();
					if (methods.Any()) {
						foreach (var element in methods) {
							s.Append(element.ToFullString());
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
			var interfaces = rootNode.DescendantNodes().OfType<InterfaceDeclarationSyntax>();
			if (interfaces.Any()) {
				interfaces = interfaces.OrderBy(i => i.Identifier.ToFullString());
				foreach (var element in interfaces) {
					s.Append(element.Modifiers.ToFullString()).Append(" interface ").Append(element.Identifier.ValueText).Append('{');
					var method_ = element.ChildNodes().OfType<MethodDeclarationSyntax>();
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
	}
}