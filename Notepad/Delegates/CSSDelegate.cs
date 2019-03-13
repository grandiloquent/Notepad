using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Ajax.Utilities;
using Common;
namespace Notepad
{
	public static class CSSDelegate
	{
		public static void CombineCssFiles()
		{
			// @"C:\Users\psycho\Desktop\Scripts\Go\Halalla\public\css";
			
			var dir = @"C:\NetCore\wwwroot\stylesheets";
			
//			string[] files = new string[] {
//				"_reset.css",
//				"album-list.css",
//				"player.css",
//				"layout.css",
//				"button.css",
//				"toolbar.css",
//				"owner.css",
//				"list.css",
//				"badge.css",
//				"actions.css",
//				"spinner.css",
//				"modal.css",
//				"unmute.css",
//				"loading-more.css",
//				"search-sub-menu-renderer.css",
//				"toast.css"
//			};
			
			var files = Directory.GetFiles(dir, "*.css").Where(i => !i.GetFileName().StartsWith(".")).OrderBy(i => i.GetFileName());
			var sb = new StringBuilder();
			foreach (var element in files) {
				sb.AppendLine((Path.Combine(dir, element).ReadAllText()));
			}
			var min = new Minifier();
			var str = min.MinifyStyleSheet(sb.ToString());
			
			Path.Combine(Path.GetDirectoryName(dir), "app.min.css").WriteAllText(str);
		}
	}
}