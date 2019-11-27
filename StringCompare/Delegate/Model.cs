namespace StringCompare
{
	using System;
	using System.Windows.Forms;

	[AttributeUsage(AttributeTargets.All)]
	public  class BindMenuItemAttribute:Attribute
	{
		public bool AddSeparatorBefore{ get; set; }
		public String Name{ get; set; }
		public String Control{ get; set; }
		public String Toolbar{ get; set; }
		public bool NeedBinding{ get; set; }
		public Keys ShortcutKeys { get; set; }
	}
}