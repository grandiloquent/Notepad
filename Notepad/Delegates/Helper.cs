namespace Notepad
{
	using System;
	using System.Windows.Forms;

	[AttributeUsage(AttributeTargets.All)]
	public  class BindMenuItemAttribute:Attribute
	{
		public bool AddSeparatorBefore{ get; set; }
		public String Name{ get; set; }
		public String SplitButton{ get; set; }
		public String Toolbar{ get; set; }
		public bool NeedBinding{ get; set; }
		public System.Windows.Forms.Keys ShortcutKeys {get;set;}
	}
}