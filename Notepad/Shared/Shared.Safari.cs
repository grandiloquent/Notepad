using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;

using System.Windows.Forms;
using System.Diagnostics;

namespace Shared
{
    class HelpersSafari
    {
        public static void GenerateUnDownloadFileListFile(string path,string sourceListFile)
        {
            var ls = Directory.GetFiles(path).Select(i => i.GetFileName());

            var sourceList = File.ReadAllLines(sourceListFile, new UTF8Encoding(false)).Where(i => i.IsReadable());

            var undownloadList = new List<string>();

            foreach (var item in sourceList)
            {
                if (ls.Contains(item.Split('/').Last().Trim())) continue;

                undownloadList.Add(item);
            }

            sourceListFile.ChangeFileName("un" + sourceListFile.GetFileNameWithoutExtension()).WriteAllText(string.Join(Environment.NewLine,undownloadList));

        }
        public static void DoExtractImages(string sourceDirectory)
        {
            var targetdir = sourceDirectory + "\\images";

            if (!System.IO.Directory.Exists(targetdir))
            {
                System.IO.Directory.CreateDirectory(targetdir);
            }

            string[] extensions = new string[]

         {

                ".htm",

                ".html"

         };
            var ls = new List<string>();

            var files = Directory.GetFiles(sourceDirectory, "*.*").Where(i => extensions.Contains(Path.GetExtension(i)));
            var doc = new HtmlAgilityPack.HtmlDocument();
            var encoding = new UTF8Encoding(false);
            foreach (var item in files)
            {
                var value = File.ReadAllText(item, new UTF8Encoding(false));
                doc.LoadHtml(value);
                var nodes = doc.DocumentNode.SelectNodes("//img");
                if (nodes == null) continue;
                foreach (var i in nodes)
                {
                    var str = i.GetAttributeValue("src", "");
                    ls.Add("https://www.safaribooksonline.com" + str);
                    i.SetAttributeValue("src", "./images/" + str.Split('/').Last());

                }
                File.WriteAllText(item, doc.DocumentNode.OuterHtml, encoding);
            }

            System.IO.File.WriteAllLines(targetdir + "\\img-links.txt", ls.Distinct(), encoding);

            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                //aria2c --input-file="img-links.txt" --load-cookies="C:\Users\Administrator\Desktop\Library\cookie.txt"
                FileName = "aria2c",
                WorkingDirectory = targetdir,
                Arguments = "-c --input-file=\"img-links.txt\" --load-cookies=\"C:\\Users\\Administrator\\Desktop\\Safari\\cookie.txt\""

                //    Arguments = "-c --input-file=\"img-links.txt\" --https-proxy=\"https://127.0.0.1:8087\" --load-cookies=\"C:\\Users\\Administrator\\Desktop\\Safari\\cookie.txt\""

            });
        }
        private static void WrapHtml(string fileName)
        {
            var lines = new string[]{
                "<!DOCTYPE html> <html lang=en> <head> <meta charset=utf-8> <meta content=\"IE=edge\" http-equiv=X-UA-Compatible> <meta content=\"width=device-width,initial-scale=1\" name=viewport>",
                "<link href=\"style.css\" rel=\"stylesheet\">",
                "</head>",
                "<body>",
                "",
                 "</body>",
                 "</html>"
            };

            var str = fileName.ReadAllText();
            if (str.Contains("<!DOCTYPE html>")) return;
            lines[4] = str;
            fileName.WriteAllText(string.Join("", lines));

        }
        public static void FormatHTML(string dir)
        {
            var target = dir + "\\style.css";

            var styleFile = "assets\\stylesheets\\markdown.css".GetCommandPath();

            if (!System.IO.File.Exists(target))
                System.IO.File.Copy(styleFile, target);
            var ls = Directory.GetFiles(dir, "*").Where(i => Regex.IsMatch(i, "\\.(?:htm|html|xhtml)"));


            foreach (var item in ls)
            {
                WrapHtml(item);

            }
        }
        public static void ProcessForOffline(string dir)
        {




            var ls = System.IO.Directory.GetFiles(dir).Where(i => i.GetExtension() == ".html" || i.GetExtension() == ".xhtml" || i.GetExtension() == ".htm");

            var target = dir;

            foreach (var item in ls)
            {



                var doc = new HtmlAgilityPack.HtmlDocument();
                //var u = url.Split('|').First();
                //var fn = url.Split('|').Last();
                // var str = url.GetHttpString(_cookie, "127.0.0.1:8787", 35000);
                doc.LoadHtml(item.ReadAllText());


                var n = doc.DocumentNode.SelectSingleNode("//div[@id='sbo-rt-content']");
                if (n == null) continue;
                //+ " " + (System.Text.RegularExpressions.Regex.Split(doc.GetTitle()," \\- ").First().GetValidFileName() + ".html")

                if (item.EndsWith(".xhtml"))
                    item.ChangeExtension(".html").WriteAllText(n.InnerHtml);
                else
                    item.WriteAllText(n.InnerHtml);
                if (item.EndsWith(".xhtml"))
                    System.IO.File.Delete(item);
            }

            //ParseImage(dir);
        }
        private static void GetLinks(string dir)
        {
            var content = Clipboard.GetText();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);
            var ls = new List<string>();

            foreach (var item in doc.DocumentNode.SelectNodes("//a"))
            {
                var url = item.GetAttributeValue("href", "");
                if (url.IsVacuum()) continue;
                url = "https://www.safaribooksonline.com" + url;
                var p = url.LastIndexOf("#");
                if (p != -1)
                {
                    url = url.Substring(0, p);
                }
                if (ls.Contains(url)) continue;
                ls.Add(url);
            }


            (dir + "\\links.txt").WriteAllText(string.Join(Environment.NewLine, ls));


        }
        private static void GetTOC(string dir)
        {
            var content = Clipboard.GetText();

            content = Regex.Replace(content, "<([a-z]+)[^>]*?>", (m) =>
            {
                if (m.Groups[1].Value == "a")
                {
                    return m.Value;
                }
                else
                {
                    return "<" + m.Groups[1].Value + ">";
                }
            });
            content = content.Replace("<button><span>Add to Queue</span></button>", "");
            content = content.Replace("<span>", "");
            content = content.Replace("</span>", "");
            content = content.Replace(" class=\"t-chapter\"", "");
            content = Regex.Replace(content, "tabindex\\=\"[^\"]*?\"", "");
            content = Regex.Replace(content, "href\\=\"([^\"]*?)\"", (m) =>
            {

                return "href=\"" + m.Groups[1].Value.Split('/').Last().Replace(".xhtml", ".html") + "\"";
            });
            (dir + "\\目录.html").WriteAllText(content);
        }
        public static void CreateDirectory(string value)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Combine("Safari");
            dir.CreateDirectoryIfNotExists();

            var target = dir + "\\" + value.GetValidFileName('-').Replace('™', ' ').Replace('®', ' ');

            target = target.Replace("#", "Sharp");


            target.CreateDirectoryIfNotExists();

            (target + "\\README.txt").WriteAllText(value);
        }
        private static void InvokeAria2c(string dir)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "aria2c",
                WorkingDirectory = dir,
                Arguments="--input-file=\"links.txt\" --load-cookies=\"C:\\Users\\Administrator\\Desktop\\Safari\\cookie.txt\""
            //    Arguments="--input-file=\"links.txt\" --https-proxy=\"https://127.0.0.1:8087\" --load-cookies=\"C:\\Users\\Administrator\\Desktop\\Safari\\cookie.txt\""

            });
        }
        public static void CreateTableContents()
        {

            var dlg = new OpenFileDialog();
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Combine("Safari");
            var dir = "";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dir = dlg.FileName.GetDirectoryName();

            }
            else
            {
                return;
            }
            GetTOC(dir);
            GetLinks(dir);
            InvokeAria2c(dir);
        }
    }
}
