namespace Notepad
{
	using System;
	using System.Net;
	using System.Windows.Forms;

	using System.IO;
	using System.Text.RegularExpressions;
	using System.Text;
	using System.Linq;
	using System.Collections.Generic;
	using Common;
	using System.Diagnostics;
	
	public static class VideoDelegate
	{
 
			
		private static readonly string Cookie = "bcookie=aefc32df590e448e88c02c1a740c99ff84652c14be894a8d98ef20e66b4d64fe; throttle-f9151a904e07fa0812b7b9fb20b6f1ab=1; throttle-3d2abb02cfc8e3e205c091152d7fcd1f=1; throttle-7566ffb605d4cb8c15225d8859a6efd3=1; throttle-b8b96eed8d81f42a88aadaadc5139c25=1; player_settings_0_5=player_type=2&video_format=2&cc_status=2&window_extra_height=148&volume_percentage=50&resolution=0&reset_on_plugins=True; litrk-srcveh=srcValue=re-other&vehValue=cn.bing.com&prevSrc=&prevVeh=; throttle-d3ebbd09ec7ecff8c4948ff79599614d=1; throttle-ad15fee1459e8f3e1ae3d8d711f77883=1; throttle-20fc2dfb0a81016faeebb960e94da216=1; _ga=GA1.2.217827090.1552301577; _gid=GA1.2.476737545.1552301577; player_settings_0_1=player_type=2&video_format=2&cc_status=2&window_extra_height=148&volume_percentage=50&resolution=0; throttle-9620ede73ab0b3b8d0fe1e62763ad939=1; throttle-54c678a5add39d58a7d7411cae569603=1; show-member-tour-cta=true; plugin_list=; __utmc=203495949; __utmz=203495949.1552301613.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); throttle-e0cb8e4541d2401ae2437d4836b8d8cd=1; throttle-2fa0e9b608dffa03090202330a823d2c=1; NSC_tw5_xxx-iuuqt_wt=ffffffff096e9e2e45525d5f4f58455e445a4a423661; signin-tooltip=1; __utma=203495949.217827090.1552301577.1552301613.1552304852.2; __utmb=203495949.0.10.1552304852; throttle-7f0c15d211a720cccfeff5e84db0ec6e=1; LyndaAccess=LyndaAccess=3/11/2019 4:47:47 AM&p=0&data=9,10/2/2019,1,191505; SSOLogin=OrgUrl=http%3A%2F%2Fwww.lynda.com%2Fportal%2Fsip%3Forg%3Dbexar.org&OrgName=Bexar%20BiblioTech; throttle-2b03d60a3a4380742663b5f4066e4d2a=1; player_settings_1993799633_1=player_type=2&video_format=2&cc_status=2&window_extra_height=148&volume_percentage=50&resolution=540; LyndaLoginStatus=Member-Logged-In; __utmv=203495949.|1=Persona=Enterprise-User-Status-Active-Type-Regular=1^3=Product=lyndaLibrary=1; track_me=category=undefined&label=library:Design:software:Photoshop&action=click; throttle-9ecc4e578ac856334c0a44bd43f10619=1; throttle-85975e5888b0a2a3d27c273fd5637879=1; throttle-cb8048294d8a62ec98a5d38754e4a964=1; ncp=1; throttle-fcc41b5952df7ea0746eff3c71b72bc7=1; throttle-bf01e020137cb85eaa7a5e6a2f331834=1; utag_main=v_id:01696c62534a0012074824de59580308900160640086e$_sn:2$_ss:0$_st:1552306786313$_pn:6%3Bexp-session$ses_id:1552304845297%3Bexp-session; throttle-cc6e24142c9b2f691b86349a86409bdb=1; tcookie=84792826-def7-4507-90b0-a11c9b5f038a; player=%7B%22volume%22%3A0.8%2C%22muted%22%3Afalse%2C%22ccLang%22%3A%22en%22%7D; _gat=1; token=2a6f090a-08cd-4337-b16d-e77358714b32,b5c14db66b97d91529f53773a1aaf8a8,3ezTHmseAI2zN3OcrTMZJ3Yh2PceFxDv7wLh0moBKSns/Cgq9a5WNDz5fYgX2ifzTOHuSOI04fxYAi2egJ9PRCxqwvDdXHMCecgKz3tmc8gkXWFXtbz/xFFRAlxzIYJCO2Z0Re/bfQTyhRArL9OsEw==";
				
		[BindMenuItem(Name = "SRT 转换为文本 (文件)", SplitButton = "videoButton", Toolbar = "toolStrip", AddSeparatorBefore = true, NeedBinding = true)]
		public static void DownloadSingleLynda(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			Forms.OnClipboardFile(f => {
				var srtContents = f.ReadAllText();
				var lines = srtContents.ToLines();
				var sb = new StringBuilder();
			                      	
				foreach (var element in lines) {
					if (Regex.IsMatch(element, "(^[0-9]+$)|(^[0-9]+[^a-zA-Z]*?[0-9]+$)"))
						continue;
					sb.Append(element.Trim() + " ");
				}
				mainForm.SelectedText(Regex.Replace(sb.ToString(), "[\\.]+", ".\r\n\r\n"));
			                      	
			});
		}
		[BindMenuItem(Name = "下载字幕 (文件)", SplitButton = "videoButton", Toolbar = "toolStrip", AddSeparatorBefore = true, NeedBinding = true)]
		public static void DownloadSubtitles(ToolStripMenuItem menuItem, MainForm mainForm)
		{
			Forms.OnClipboardText(DownloadSrtFilesInternal);
		}
			
		[BindMenuItem(Name = "下载单个文件 (文本)", SplitButton = "videoButton", Toolbar = "toolStrip", AddSeparatorBefore = true)]
		public static void DownloadSingleLynda()
		{
			Forms.OnClipboardText(url => {
				DownloadSingleLyndaInternal(url);
			});
		}
		private async static void DownloadSrtFilesInternal(string str)
		{
			var nodes =	str.GetHtmlDocument().DocumentNode.SelectNodesByClassName("item-name");
			//nodes=str.GetHtmlDocument().DocumentNode.SelectNodes("//*[contains(@class,'video-name')]");
			if(nodes==null)return;
			var index=0;
			var dir="Subtitles".GetDesktopPath();
			dir.CreateDirectoryIfNotExists();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			var client=HttpClients.GetHttpClient();
			foreach (var element in nodes) {
				var originalUrl=element.GetHref();
				var pieces = originalUrl.Split('/').Reverse().Take(2).ToArray();
		var	url = string.Format("https://www.lynda.com/ajax/player/transcript?courseId={0}&videoId={1}", pieces[1], pieces[0].SubstringBefore("-"));
				var content = await client.ReadStringWithCookie(url,originalUrl, Cookie);
				Path.Combine(dir,	((++index).ToString().PadLeft(3,'0')+". "+element.GetInnerText().Trim().GetValidFileName()+".srt")).WriteAllText(content);
			}
		}
		
		private async static void DownloadSingleLyndaInternal(string originalUrl)
		{
			var pieces = originalUrl.Split('/').Reverse().Take(2).ToArray();
			// 758626/5036974-4.html
			//

			var	url = string.Format("https://www.lynda.com/ajax/course/{0}/{1}/play", pieces[1], pieces[0].SubstringBefore("-"));
			
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
				
			var content = await HttpClients.GetHttpClient().ReadStringWithCookie(url, originalUrl, Cookie);
		
			var videoUrl =	content.ToJArray().First().ConvertToJObject().GetStringFromChain(new string[] {
				"urls",
				"720"
			});
			
			url = string.Format("https://www.lynda.com/ajax/player/transcript?courseId={0}&videoId={1}", pieces[1], pieces[0].SubstringBefore("-"));
			content = await HttpClients.GetHttpClient().ReadStringWithCookie(url, originalUrl, Cookie);
			"1.srt".GetDesktopPath().WriteAllText(content);
			"1.txt".GetDesktopPath().WriteAllText(videoUrl);
			Process.Start(new ProcessStartInfo {
				FileName = "aria2c",
				Arguments = "\"" + videoUrl + "\"",
				WorkingDirectory = "".GetDesktopPath()
			});
		}
		
	}
}