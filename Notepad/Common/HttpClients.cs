

namespace Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;
	using System.IO;
	public static class HttpClients
	{
		public static HttpClient GetHttpClient()
		{
			return new HttpClient(new HttpClientHandler() {
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				UseProxy = false,
				UseDefaultCredentials = true,
				UseCookies = false,
			});
		}
		
		public async static Task<string> Authenticate(this HttpClient httpClient,
			string url, 
			string userName,
			string password)
		{
			var client = new HttpClient();
		
			var msg = new HttpRequestMessage(HttpMethod.Post, url);
		
			var dic = new Dictionary<string,string>();
			dic.Add("username", userName);
			dic.Add("password", password);
			ServicePointManager.ServerCertificateValidationCallback =
    		delegate {
				return true;
			};
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			msg.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dic), Encoding.UTF8, "application/json");
	 
			var res = await client.SendAsync(msg);
	
			return  await res.Content.ReadAsStringAsync();
		}
		public async static Task<string> Post(this HttpClient httpClient, string url, string accessToken = null)
		{
			
			
			var httpMessage = GetHttpRequestMessage(HttpMethod.Post, url);
        	
			if (accessToken != null)
				httpMessage.Headers.Add("Authorization", "Bearer " + accessToken);
			var response = await httpClient.SendAsync(httpMessage);
		
			var message = await response.Content.ReadAsStringAsync();
		
			if (string.IsNullOrWhiteSpace(message)) {
				message = response.StatusCode.ToString();
			}
			return message;
		}
		public async static Task<string> PostJson(this HttpClient httpClient, string content, string url, string accessToken = null)
		{
			
			//System.Windows.Forms.Clipboard.SetText(content);
			var httpMessage = GetHttpRequestMessage(HttpMethod.Post, url);
        	
			if (accessToken != null)
				httpMessage.Headers.Add("Authorization", "Bearer " + accessToken);

			
			var stream = content.ToStream();
			var sc = new StreamContent(stream);
			sc.Headers.ContentLength = stream.Length;
			sc.Headers.Add("Content-Encoding", "gzip");
			sc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			
			httpMessage.Content = sc;// new StringContent(content, Encoding.UTF8, "application/json");
			var response = await httpClient.SendAsync(httpMessage);
			return response.StatusCode.ToString();
//			var message = await response.Content.ReadAsStringAsync();
//		
//			if (string.IsNullOrWhiteSpace(message)) {
//				message = response.StatusCode.ToString();
//			}
//			return message;
		}
		private static HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string url)
		{
			var ip = "220.181.100." + new Random().Next(1, 255);
			var httpMessage = new HttpRequestMessage(method, url);
			httpMessage.Headers.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
			httpMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
			httpMessage.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
			httpMessage.Headers.Add("Connection", "keep-alive");
			httpMessage.Headers.Add("User-Agent",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.92 Safari/537.36");
			httpMessage.Headers.Add("Client-IP", ip);
			httpMessage.Headers.Add("X-Forwarded-For", ip);
			return httpMessage;
		}

		public async static Task<string> GetJsonAsync(this HttpClient client, string url, string referrer = null)
		{
			var msg = new HttpRequestMessage(HttpMethod.Get, url);
			if (referrer != null) {
				msg.Headers.Add("Referer", referrer);
			}
			var response = await client.SendAsync(msg).ConfigureAwait(false);
			var buffer = await response.Content.ReadAsByteArrayAsync();
			return Encoding.UTF8.GetString(buffer);

		}

		public async static Task<string> GetHtmlAsync(this  HttpClient client, string url)
		{
			var response = await client.GetStringAsync(url).ConfigureAwait(false);
			return response;

		}
		public static async Task<string[]> ReadStringAndCookie(this HttpClient httpClient, string url,
			string referrer = null)
		{
			var httpMessage = GetHttpRequestMessage(HttpMethod.Get, url);


			if (referrer != null)
				httpMessage.Headers.Add("Referer", referrer);


			var response = await httpClient.SendAsync(httpMessage).ConfigureAwait(false);
			var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			var cookieStr = string.Empty;
			IEnumerable<string> values;
			if (response.Headers.TryGetValues("Set-Cookie", out values)) {
				cookieStr = values.First();
			}

			return new[] {
				cookieStr,
				Encoding.UTF8.GetString(bytes)
			};
		}
		
		public static async Task<string> PostWithParameters(this HttpClient httpClient, string url, Dictionary<string,string> parameters)
		{
			
			using(	var content = new FormUrlEncodedContent(parameters)){
			
			var response = await httpClient.PostAsync(url, content);
			return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString();
			}
			
		}

		public static async Task<string> ReadStringWithCookie(this HttpClient httpClient, string url,
			string referrer = null, string cookie = null)
		{
			var httpMessage = GetHttpRequestMessage(HttpMethod.Get, url);


			if (referrer != null)
				httpMessage.Headers.Add("Referer", referrer);

			httpMessage.Headers.Add("Cookie", cookie);
			var response = await httpClient.SendAsync(httpMessage).ConfigureAwait(false);
			var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);


			return Encoding.UTF8.GetString(bytes);
		}
	}

}