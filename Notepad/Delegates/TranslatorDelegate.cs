﻿namespace Notepad
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Common;
	public static class TranslatorDelegate
	{
		private static string[][] _fomratMap=new string[][]{
			new string[]{"您","你"},
			new string[]{"类型","文字"},
			new string[]{"过滤器","滤镜"},
			new string[]{"遮罩","蒙版"},
			new string[]{"部电影","个视频"},
			new string[]{"刷子","画笔"},
			
			new string[]{"Control","Ctrl"},
			new string[]{"选择","选区"},
			new string[]{"乘法","正片叠底"},
			new string[]{"Multiply","正片叠底"},
			
			new string[]{"级别调整","色阶调整层"},
			// 飞溅 喷溅 置换贴图 置换图
			
		};
		
		private static string FormatResult(string s){
			foreach (var element in _fomratMap) {
				s=s.Replace(element[0],element[1]);
			}
			return s;
		}
		private static HttpClient _client;

			// ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)))
		
			
		[BindMenuItem(Name = "谷歌 (英文到中文)", SplitButton = "englishButton", Toolbar = "toolStrip", AddSeparatorBefore = true, NeedBinding = true, ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W))))]
		public  async static void Google(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			var textBox = mainForm.textBox;
			
		
			textBox.SelectLine(true);

			var val = textBox.SelectedText;
			if (string.IsNullOrWhiteSpace(val))
				return;
			var json =await QueryEnglish(val);

			var obj = json.ToJObject();
			Newtonsoft.Json.Linq.JToken jtoken;

			if (!obj.TryGetValue("sentences", out jtoken))
				return;

			var sb = new StringBuilder();
			foreach (var item in jtoken) {
				//.AppendLine(item["orig"].ToString()
				sb.AppendLine(item["trans"].ToString());
			}
			textBox.SelectedText=Environment.NewLine+FormatResult(sb.ToString())+Environment.NewLine+textBox.SelectedText;

		}
		 private async static Task<string> QueryEnglish(string q)
        {
            var url = "https://translate.google.cn/translate_a/single?client=gtx&sl=auto&tl="
                + "zh" + "&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" + WebUtility.UrlEncode(q);

            var res =await _client.GetAsync(url);

            return await res.Content.ReadAsStringAsync();
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
			if(_client==null){
				_client=Common.Https.GetHttpClient();
			}
			var json = await _client.PostWithParameters(url, parameters);
			var array = json.ToJObject().GetArray<Newtonsoft.Json.Linq.JObject>("trans_result");
			sb.Clear();
			foreach (var element in array) {
				sb.AppendLine(element.GetString("dst"));
			}
			return sb.ToString();
		}
	}
}