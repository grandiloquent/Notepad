namespace StringCompare
{
	using System.Net;
	using System.Net.Http;
	using System.Collections.Generic;
	using System.Reflection;
	using System.IO;
	using System.Text;
	using System.Linq;
	using System.Text.RegularExpressions;
	using Common;
	public static class SplitDelegate
	{
		private static	HttpClient sClient;
		private static List<string> mWords;
		private readonly static string WordFile = "words.txt".GetEntryPath();
		private readonly static string DictionaryFile = "dictionaries.txt".GetEntryPath();
		private const string BaseUri = "https://cn.bing.com/dict/search?q=";
		
		private static Encoding sUtf8 = Encoding.GetEncoding("UTF-8");
		public async static void ParseInternal(string value)
		{
			if (sClient == null) {
				sClient = new HttpClient(new HttpClientHandler() {
					UseProxy = false,
					UseCookies = false,
					AllowAutoRedirect = false
				});
			}
			if (mWords == null) {
				if (File.Exists(WordFile)) {
					mWords = File.ReadLines(WordFile, sUtf8)
						.Where(i => !string.IsNullOrWhiteSpace(i))
						.Select(i => i.Trim())
						.ToList();
				} else {
					mWords = new List<string>();
				}
			}
			var words = Regex.Matches(value, "[a-zA-Z]+", RegexOptions.IgnoreCase)
				.Cast<Match>()
				.Select(i => i.Value.ToLower())
				.OrderBy(i => i)
				.Distinct()
				.Where(i => !mWords.Contains(i))
				.ToList();
			var hd = new HtmlAgilityPack.HtmlDocument();
			
			var explains = new List<string>();
			var sb = new StringBuilder();
			foreach (var word in words) {
				var str = await sClient.GetStringAsync(BaseUri + word);
				hd.LoadHtml(str);
				var nodes = hd.DocumentNode.SelectNodes("//div[@class='qdef']//span[@class='def']");
				
				if (nodes != null && nodes.Any()) {
					sb.Clear();
					sb.Append(word).Append('|');
					foreach (var n in nodes) {
						sb.Append(n.InnerText).Append(';');
						
					}
				}
				explains.Add(sb.ToString());
				
				mWords.Add(word);
				if (explains.Count > 50) {
					File.AppendAllLines(DictionaryFile, explains, sUtf8);
					explains.Clear(); 
					File.WriteAllLines(WordFile, mWords, sUtf8);
					
				}
			}
			if (explains.Count > 0) {
				File.AppendAllLines(DictionaryFile, explains, sUtf8);
				explains.Clear();
				File.WriteAllLines(WordFile, mWords, sUtf8);
			}
		
		}


		[BindMenuItem(Control = "dictionaryStripButton", Toolbar = "toolbar2")]
		public static void Parse()
		{
		
		
			Methods.OnClipboardText(ParseInternal);
		}
	}
}