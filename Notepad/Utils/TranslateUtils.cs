using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Xml;
using SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace  Notepad
{
	
	public class TranslateUtils
	{
		 private readonly HttpClient _client;

        public TranslateUtils()
        {
            _client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                UseProxy = false,
            });
            _client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");

        }

        public string QueryEnglish(string q)
        {
            var url = "https://translate.google.cn/translate_a/single?client=gtx&sl=auto&tl="
                + "zh" + "&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" + WebUtility.UrlEncode(q);

            var res = _client.GetAsync(url).GetAwaiter().GetResult();

            return res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
        public string QueryChinese(string q)
        {
            var url = "https://translate.google.cn/translate_a/single?client=gtx&sl=auto&tl="
                + "en" + "&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" + WebUtility.UrlEncode(q);

            var res = _client.GetAsync(url).GetAwaiter().GetResult();

            return res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
        public static TranslateUtils s;

        public static TranslateUtils GetInstance() {
        	if(s==null)
        		s= new TranslateUtils();
        	return s;
        }
	}
}
