namespace StringCompare
{
	using StringCompare;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using Microsoft.Ajax.Utilities;
	using Common;
	public static class CSharpDelegate
	{
		public static  string SortMethodsInternal(string v){
			
			return v.ToBlocks().OrderBy(i=>i.SubstringBefore('(')
			                            .TrimEnd()
			                            .SubstringAfterLast(' ').TrimStart()).ConcatenateLines();
		}
		[BindMenuItem(Name = "排序方法", Control = "codeCSharpStripSplitButton", Toolbar = "toolbar1")]
		public static void CombineDirectory()
		{
			Methods.OnClipboardString(SortMethodsInternal);
		}
	}
}