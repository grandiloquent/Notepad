
namespace  Shared
{

	using System;
	using System.Linq;
	using System.Net.Http;
	using System.Net;

	public static class NetHttpExtensions
	{
		public static HttpClient GetHttpClient(this string url)
		{
			var client = new HttpClient(new HttpClientHandler {
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				UseProxy = false,
			});
			client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");
			return client;
		}
	}
}
