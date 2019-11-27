using System;
using System.Windows.Forms;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Common;

namespace Notepad
{
	public static class WebServerDelegate
	{
		private	const string SplitMark = ">====>";
		private const  string LocalUrl = "http://127.0.0.1:5000";
		private static  string ServerUrl = "https://106.12.125.201";
		private readonly static	string AccessToken = "J3ebj2iT";
		
		private static string BuildJson(TextBox textBox)
		{
			var s = textBox.Text.Trim();
			if (!s.StartsWith("# ") || s.IndexOf(SplitMark) == -1)
				return string.Empty;
			var title = s.SubstringBefore('\n').SubstringAfter(' ').Trim();
			var dic = new Dictionary<string,dynamic>();
			dic.Add("title", title);
			dic.Add("content",s.SubstringBefore(SplitMark).Trim().Replace("\r",""));
			var metas = s.SubstringAfter(SplitMark).SubstringBefore(SplitMark).ToLines();
			foreach (var element in metas) {
				var prefix = element.SubstringBefore(':').Trim();
				if (prefix == "toc") {
					dic.Add("toc", element.SubstringAfter(':').Trim());
				} else if (prefix == "id") {
					dic.Add("id", Regex.Match(element.SubstringAfter(':'),"\\d+").Value);
				
				}else if (prefix == "tag") {
					var tags = element.SubstringAfter(':').Split(',')
						.Where(i => !String.IsNullOrWhiteSpace(i))
						.Select(i => i.Trim())
						.Distinct().ToList();
					dic.Add("tags", tags);
				}
			}
			return JsonConvert.SerializeObject(dic);
		}
			
		[BindMenuItem( Control = "updateSplitButton", Toolbar = "toolStrip1", NeedBinding = true)]
		
		public	static void UpdateNote(ToolStripItem menuItem, MainForm mainForm)
		{ 
		
			try {
				Https.SetCertificatePolicy();
				var json = BuildJson(mainForm.textBox);
				ServerUrl = LocalUrl;
				
				var msg = (ServerUrl + "/api/update").PostJson(json, AccessToken);
		
				
			} catch (Exception e) {
				mainForm.HandleException(e);
			}
		}
		
		[BindMenuItem( Control = "createSplitButton", Toolbar = "toolStrip1", NeedBinding = true)]
		
		public	static void CreateNote(ToolStripItem menuItem, MainForm mainForm)
		{ 
		
			try {
				Https.SetCertificatePolicy();
				var json = BuildJson(mainForm.textBox);
				ServerUrl = LocalUrl;
				
				var msg = (ServerUrl + "/api/insert").PostJson(json, AccessToken);
		
				
			} catch (Exception e) {
				mainForm.HandleException(e);
			}
		}
		
		
	}
}