namespace Notepad
{
	using System;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using Common;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	public static class TranslatorDelegate
	{
		
			
		[BindMenuItem(Name = "百度 (英文到中文)", SplitButton = "englishButton", Toolbar = "toolStrip", AddSeparatorBefore = true, NeedBinding = true, ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q))))]
		public async static void Baidu(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			
			textBox.SelectLine(true);

			var val = textBox.SelectedText;
			if (string.IsNullOrWhiteSpace(val))
				return;
			var result =await	QueryFromBaidu(val, "en", "zh");
			
			textBox.SelectedText=result+textBox.SelectedText;

//			var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
//			Newtonsoft.Json.Linq.JToken jtoken;
//
//			if (!obj.TryGetValue("sentences", out jtoken))
//				return;
//
//			var sb = new StringBuilder();
//			foreach (var item in jtoken) {
//				sb.AppendLine(item["trans"].ToString()).AppendLine(item["orig"].ToString());
//			}
//			textBox.SelectedText = sb.ToString();
		}
		
		private async static Task<string> QueryFromBaidu(string q, string from, string to)
		{
			var url = "http://api.fanyi.baidu.com/api/trans/vip/translate";
			var appid = "20190312000276185";
			
			var parameters = new Dictionary<string,string>();
			parameters.Add("q", q);
			parameters.Add("from", from);
			parameters.Add("to", to);
			var salt = (DateTime.Now.Ticks / 1000).ToString();
			parameters.Add("salt", salt);
			parameters.Add("appid", appid);
			var securityKey = "sdK6QhtFE64Qm0ID_SjG";
			var sb = new StringBuilder();
			sb.Append(appid)
				.Append(q)
				.Append(salt)
				.Append(securityKey);
			parameters.Add("sign", sb.ToString().Md5());
			var json = await HttpClients.GetHttpClient().PostWithParameters(url, parameters);
			var array = json.ToJObject().GetArray<Newtonsoft.Json.Linq.JObject>("trans_result");
			sb.Clear();
			foreach (var element in array) {
				sb.AppendLine(element.GetString("dst"));
			}
			return sb.ToString();
		}
	}
}