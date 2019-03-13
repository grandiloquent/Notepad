namespace Notepad
{
	using System;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	
	public static class FindDelegate
	{
		
			
		[BindMenuItem(Name = "移除非中文行",SplitButton="findButton", Toolbar = "toolStrip2", AddSeparatorBefore = true,NeedBinding=true)]
		public static void RemoveNoChineseLines(ToolStripMenuItem menuItem,MainForm mainForm)
		{
			
			var lines=mainForm.textBox.Lines.Where(i=>Regex.IsMatch(i,"\\p{Lo}"));
			mainForm.textBox.Text=string.Join(Environment.NewLine,lines);
			
		}

	}
}