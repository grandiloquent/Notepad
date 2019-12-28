 
using System;

namespace Common
{
	using System;
	using System.Linq;
	using System.Windows.Forms;

	[AttributeUsage(AttributeTargets.All)]
	public  class BindMenuItemAttribute:Attribute
	{
		public bool AddSeparatorBefore{ get; set; }
		public String Name{ get; set; }
		public String SplitButton{ get; set; }
		public String Toolbar{ get; set; }
		
		public String Control{ get; set; }
		public bool NeedBinding{ get; set; }
		public Keys ShortcutKeys { get; set; }
	}
	
	public static class Delegates
	{
		public static void Inject(Type type, Form form)
		{
			foreach (var method in type.GetMethods()) {
				var attributes = method.GetCustomAttributes(
					                 typeof(BindMenuItemAttribute), false);
				if (!attributes.Any())
					continue;
				var attribute = (BindMenuItemAttribute)attributes.First();
				var toolStrip = (ToolStrip)form.Controls[attribute.Toolbar];
				if(toolStrip==null){
					
					return;
				}
				ToolStripItem control = toolStrip.Items[attribute.Control];
				ToolStripItem item = null;
				if (control is ToolStripButton) {
					item = control;
				} else if (control is ToolStripSplitButton) {
					var parent = (ToolStripSplitButton)control;
					if (attribute.AddSeparatorBefore) {
						parent.DropDownItems.Add(new ToolStripSeparator());
					}
					item = new ToolStripMenuItem(attribute.Name);
					
					if (attribute.ShortcutKeys != null) {
						((ToolStripMenuItem)item).ShortcutKeys = attribute.ShortcutKeys;
					}
					parent.DropDownItems.Add(item);
				}else{
					item=new ToolStripButton();
					item.Text=attribute.Control;
					item.DisplayStyle=ToolStripItemDisplayStyle.Text;
					toolStrip.Items.Add(item);
				}
				if (attribute.NeedBinding)
					item.Click += (a, b) => method.Invoke(null, new Object[]{ a, form });
				else
					item.Click += (a, b) => method.Invoke(null, null);
//				var splitButton = (ToolStripSplitButton)toolStrip.Items[attribute.Control];
//				if (attribute.AddSeparatorBefore) {
//					splitButton.DropDownItems.Add(new ToolStripSeparator());
//				}
//				var item = new ToolStripMenuItem(attribute.Name);
//				if (attribute.NeedBinding)
//					item.Click += (a, b) => method.Invoke(null, new Object[]{ a, this });
//				else
//					item.Click += (a, b) => method.Invoke(null, null);
				
//				splitButton.DropDownItems.Add(item);
			}
		}
	}
}
