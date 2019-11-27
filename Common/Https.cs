using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
	public static class Https{
			private static bool RemoteCertificateValidate(
			object sender, X509Certificate cert,
			X509Chain chain, SslPolicyErrors error)
		{
			// trust any certificate!!!
			System.Console.WriteLine("Warning, trust any certificate");
			return true;
		}
		public static string Cookie = "pgv_pvid=3531479395; euin_cookie=41BD1C4435DC375FBEDB49054DB76D6A7E144504ED74E35D; ptcz=b6a78a1389245b1d160bd02b1bd65a22d62fe28d6c0914e7264b6c74f1216b1f; pgv_pvi=4809115648; uin_cookie=2728578956; sensorsdata2015jssdkcross=%7B%22distinct_id%22%3A%22168c5952d117ea-0a44be39a4fa52-4f7b614a-1049088-168c5952d1218a%22%2C%22%24device_id%22%3A%22168c5952d117ea-0a44be39a4fa52-4f7b614a-1049088-168c5952d1218a%22%2C%22props%22%3A%7B%22%24latest_traffic_source_type%22%3A%22%E7%9B%B4%E6%8E%A5%E6%B5%81%E9%87%8F%22%2C%22%24latest_referrer%22%3A%22%22%2C%22%24latest_referrer_host%22%3A%22%22%2C%22%24latest_search_keyword%22%3A%22%E6%9C%AA%E5%8F%96%E5%88%B0%E5%80%BC_%E7%9B%B4%E6%8E%A5%E6%89%93%E5%BC%80%22%7D%7D; luin=o2728578956; RK=sKKMfg2M0M; ptui_loginuin=3545039435; lskey=00010000d1e86791d11afac56238fb6c21b4cd18569d8cfcb9408efb9e6655484611b1b5866e11b41a54c33c; pgv_si=s2290829312; _qpsvr_localtk=0.2537786283402402; ptisp=cm; uin=o2728578956; skey=@ZkYewUA2E; ts_last=y.qq.com/portal/profile.html; p_lskey=0004000037d15eb69d9dd01f3d1e0a3944c9698d5273a3ec8195a08e74454a505dc4420591f6b65f7514105e; ts_refer=xui.ptlogin2.qq.com/cgi-bin/xlogin; ts_uid=3700488506; p_luin=o2728578956; p_uin=o2728578956; pt4_token=4r1g3HfBwvrtAtxNBu0HdtWblkUbd4bNzJza-bU3LPo_; p_skey=sf61L*8fbh52eBNoq47ywuHrS*5VwShQ5K9svtDkONU_; yqq_stat=0";
		public static string PostJson(this string url, string json, string accessToken = null)
		{
//			ServicePointManager.SecurityProtocol =
//				SecurityProtocolType.Ssl3
//			| SecurityProtocolType.Tls
//			| SecurityProtocolType.Tls11
//			| SecurityProtocolType.Tls12;
			var req = WebRequest.Create(url);
			req.Method = "POST";
			if (accessToken != null)
				req.Headers.Add("Authorization", "Bearer " + accessToken);
			if (json != null) {
				req.ContentType = "application/json";
				req.Headers.Add("Content-Encoding", "gzip");
				using (var s = req.GetRequestStream())
				using (var gzip = new GZipStream(s, CompressionMode.Compress)) {
					var bytes = new UTF8Encoding(false).GetBytes(json);
					gzip.Write(bytes, 0, bytes.Length);
					//s.Write(bytes, 0, bytes.Length);
				}
			}
			var res = req.GetResponse();
			var result = string.Empty;
			using (var stream = res.GetResponseStream())
			using (var ms = new MemoryStream()) {
				stream.CopyTo(ms);
				result = new UTF8Encoding(false).GetString(ms.ToArray());
			}
			if (string.IsNullOrWhiteSpace(result)) {
				result = ((HttpWebResponse)res).StatusCode.ToString();
			}
			return result;
		}
		public static async Task<string> GetWebDatacAsync(string url, Encoding c = null)
		{
			if (c == null)
				c = Encoding.UTF8;
			HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
			hwr.Timeout = 20000;
			hwr.KeepAlive = true;
			hwr.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
			hwr.Headers.Add(HttpRequestHeader.Upgrade, "1");
			hwr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.110 Safari/537.36";
			hwr.Accept = "*/*";
			hwr.Referer = "https://y.qq.com/portal/player.html";
			hwr.Host = "c.y.qq.com";
			hwr.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
			//hwr.Headers.Add(HttpRequestHeader.Cookie, Cookie);
			var o = await hwr.GetResponseAsync();
			StreamReader sr = new StreamReader(o.GetResponseStream(), c);
			var st = await sr.ReadToEndAsync();
			sr.Dispose();
			return st;
		}
		public static async Task<int> GetWebCode(String url)
		{
			try {
				HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
				var o = (await hwr.GetResponseAsync()) as HttpWebResponse;
				return (int)o.StatusCode;
			} catch {
				return 404;
			}
		}
		public static async Task<string> GetUrlAsync(string Musicid)
		{
			// disable once SuggestUseVarKeywordEvident
			List<String[]> MData = new List<String[]>();
//			MData.Add(new String[] { "M800", "mp3" });
//			MData.Add(new String[] { "C600", "m4a" });
			MData.Add(new String[] { "M500", "mp3" });
			MData.Add(new String[] { "C400", "m4a" });
			MData.Add(new String[] { "M200", "mp3" });
			MData.Add(new String[] { "M100", "mp3" });

			var guid = "xxx";
//			var mid = JObject.Parse(await GetWebDatacAsync(string.Format("https://c.y.qq.com/v8/fcg-bin/fcg_play_single_song.fcg?songmid={0}&platform=yqq&format=json", Musicid)))["data"][0]["file"]["media_mid"].ToString();
//			for (int i = 0; i < MData.Count; i++) {
//				String[] datakey = MData[i];
//				var key = JObject.Parse(await GetWebDatacAsync(string.Format("https://c.y.qq.com/base/fcgi-bin/fcg_musicexpress.fcg?json=3&guid={0}&format=json", guid)))["key"].ToString();
//				string uri = string.Format("https://dl.stream.qqmusic.qq.com/{0}{1}.{2}?vkey={3}&guid={4}&uid=0&fromtag=30", datakey[0], mid, datakey[1], key, guid);
//				if (await GetWebCode(uri) == 200)
//					return uri;
//			}
		//	return "http://ws.stream.qqmusic.qq.com/C100" + mid + ".m4a?fromtag=0&guid=" + guid;
		return string.Empty;
		}
		public static string Handshake(string url, 
			string method = "GET",
			bool allowAutoRedirect = false)
		{
			
			var sb = new StringBuilder();
			
			var req = (HttpWebRequest)WebRequest.Create(url);
			req.Method = method;
			req.AllowAutoRedirect = allowAutoRedirect;
			req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.56 Safari/537.36";
			
			//req.UserAgent="Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.56 Mobile Safari/537.36";
			
			var res = (HttpWebResponse)req.GetResponse();
			
			sb.AppendFormat("{0} {1}\r\n", (int)res.StatusCode, res.StatusCode);
			foreach (var key in res.Headers.AllKeys) {
				
				sb.AppendFormat("{0}: {1}\r\n", key, res.Headers[key]);
			}
			
			using (var s = res.GetResponseStream())
			using (var r = new StreamReader(s, new UTF8Encoding(false))) {
				sb.AppendLine(r.ReadToEnd());
			}
			
			return sb.ToString();
		}
		public static HttpClient GetHttpClient()
		{
			return new HttpClient(new HttpClientHandler() {
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				UseProxy = false,
				UseDefaultCredentials = true,
				UseCookies = false,
			});
		}
		public static void SetCertificatePolicy()
		{
			ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
		}
		
	}
}