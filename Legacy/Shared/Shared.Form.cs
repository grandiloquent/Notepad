using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using SQLite;
using Ionic.Zip;

using System.IO;

using System.Text.RegularExpressions;
using System.Diagnostics;
using Common;

namespace Shared
{
    public static class HelperJson
    {

        public static T GetValue<T>(Dictionary<string, T> obj, string key)
        {
            T v;

            return obj.TryGetValue(key, out v) ? v : default(T);
        }
    }
    public static class HelperApplication
    {
        public static void ClipboardActionOnFile(Action<string> act)
        {
            try
            {
                var dir = Clipboard.GetText();
                if (!dir.FileExists())
                {
                    var collection = Clipboard.GetFileDropList();
                    foreach (var item in collection)
                    {
                        if (item.FileExists())
                        {
                            dir = item;
                            break;
                        }
                    }
                }
                if (dir.FileExists())
                {
                    act(dir);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void ClipboardActionOnFiles(Action<List<string>> act)
        {
            try
            {
                var ls = new List<string>();
                var dir = Clipboard.GetText();
                if (!dir.FileExists())
                {
                    var collection = Clipboard.GetFileDropList();
                    foreach (var item in collection)
                    {
                        if (item.FileExists())
                        {
                            ls.Add(item);
                        }
                    }
                }
                else
                {
                    ls.Add(dir);
                }
                if (ls.Any())
                {
                    act(ls);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void ClipboardAction(Func<string, string> act)
        {
            try
            {
                var dir = Clipboard.GetText();
                dir = act(dir);
                if (dir.IsReadable())
                {
                    Clipboard.SetText(dir);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void ClipboardActionOnDirectoy(Action<string> act)
        {
            try
            {
                var dir = Clipboard.GetText();
                if (!dir.DirectoryExists())
                {
                    var collection = Clipboard.GetFileDropList();
                    foreach (var item in collection)
                    {
                        if (item.DirectoryExists())
                        {
                            dir = item;
                            break;
                        }
                    }
                }
                if (dir.DirectoryExists())
                {
                    act(dir);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void ExecuteCommand(string cmd, bool withWindow = true)
        {
            var splited = cmd.Split(new char[] { ' ' }, 2);

            Process.Start(new ProcessStartInfo
            {
                FileName = splited[0].Trim(),
                Arguments = splited[1].Trim(),
                WindowStyle = withWindow ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
            });
        }
    }
    public static class HelperZip
    {
        public static void CompressDirectoriesEncrypt(string path, string dstDir)
        {
            foreach (var item in Directory.GetDirectories(path))
            {
                using (var zip = new ZipFile(Encoding.GetEncoding("gb2312")))
                {
                    zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    zip.Password = "gamahuched64";
                    zip.AddDirectory(item);
                    var fileName = dstDir.Combine(item.GetFileName() + ".zip");
                    if (fileName.FileExists())
                    {

                       // fileName = dstDir.Combine($"{item.GetFileName()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()}.zip");
                    }
                    zip.Save(fileName);
                }
            }

        }
        public static void DeCompressDirectoriesEncrypt(string path, string dstDir)
        {
            foreach (var item in Directory.GetFiles(path, "*.zip"))
            {
                using (var zip = new ZipFile(item, Encoding.GetEncoding("gb2312")))
                {
                    zip.Password = "gamahuched64";
                    dstDir.Combine(item.GetFileNameWithoutExtension()).CreateDirectoryIfNotExists();
                    zip.ExtractAll(dstDir.Combine(item.GetFileNameWithoutExtension()));


                }
            }

        }
        public static void CompressFile(string path, string dstDir)
        {

            using (var zip = new ZipFile(Encoding.GetEncoding("gb2312")))
            {
                zip.AddFile(path, "");
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                var fileName = dstDir.Combine(path.GetFileName() + ".zip");
                if (fileName.FileExists())
                {

                   // fileName = dstDir.Combine($"{path.GetFileName()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()}.zip");
                }
                zip.Save(fileName);
            }


        }
        public static void CompressDirectories(string path, string dstDir)
        {
            foreach (var item in Directory.GetDirectories(path))
            {
                using (var zip = new ZipFile(Encoding.GetEncoding("gb2312")))
                {

                    zip.AddDirectory(item);
                    var fileName = dstDir.Combine(item.GetFileName() + ".zip");
                    if (fileName.FileExists())
                    {

                        //fileName = dstDir.Combine($"{item.GetFileName()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()}.zip");
                    }
                    zip.Save(fileName);
                }
            }

        }
        public static void CompressDirectoryEncrypt(string path, string dstDir)
        {
            using (var zip = new ZipFile(Encoding.GetEncoding("gb2312")))
            {
                zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                zip.Password = "gamahuched64";
                zip.AddDirectory(path);
                var fileName = dstDir.Combine(path.GetFileName() + ".zip");
                if (fileName.FileExists())
                {

                  //  fileName = dstDir.Combine($"{path.GetFileName()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()}.zip");
                }
                zip.Save(fileName);
            }
        }
        public static void CompressCSharpDirectoryEncrypt(string path, string dstDir)
        {
            var dirList = Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories).Where(i => !Regex.IsMatch(i, "\\\\(?:\\.|bin|obj|packages)")).OrderBy(i => i.Length).ToList();
            dirList.Add(path);
            using (var zip = new ZipFile(Encoding.GetEncoding("gb2312")))
            {
                //zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                //zip.Password = "gamahuched64";
                foreach (var item in dirList)
                {
                    zip.AddFiles(Directory.GetFiles(item, "*"), item.Substring(path.Length));
                }
                var fileName = dstDir.Combine(path.GetFileName() + ".zip");
                if (fileName.FileExists())
                {

                    //fileName = dstDir.Combine($"{path.GetFileName()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()}.zip");
                }
                zip.Save(fileName);
            }
        }

        public static void CompressAndroidIntellijDirectoryEncrypt(string path, string dstDir)
        {
            using (var zip = new ZipFile(Encoding.GetEncoding("gb2312")))
            {

                zip.AddFiles(Directory.GetFiles(path), "");
                zip.AddFiles(Directory.GetFiles(path.Combine("app")), "app");
                zip.AddDirectory(path.Combine("app").Combine("src"), "app/src");

                var fileName = dstDir.Combine(path.GetFileName() + ".zip");
                if (fileName.FileExists())
                {

                  //  fileName = dstDir.Combine($"{path.GetFileName()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()}.zip");
                }
                zip.Save(fileName);
            }
        }
    }
    public static class FormExtension
    {

        public static void ChangeTitle(this Form f, string value)
        {

            f.Text = value + " *";



        }
    }
    public static class TextBoxExtension
    {


        public static void Format(this TextBox textBox)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < textBox.Lines.Length; i++)
            {
                if (textBox.Lines[i].IsVacuum())
                {
                    while (i + 1 < textBox.Lines.Length && textBox.Lines[i + 1].IsVacuum())
                    {
                        i++;
                    }
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine(textBox.Lines[i]);
                }
            }
            textBox.Text = sb.ToString().Trim();

        }

        public static void Delete(this TextBox textBox)
        {
            if (textBox.SelectedText.IsVacuum())
            {
                textBox.SelectLine();
            }
            textBox.SelectedText = string.Empty;

        }

        public static void SelectLine(this TextBox textBox, bool trimEnd = false)
        {

            var start = textBox.SelectionStart;

            var length = textBox.Text.Length;
            var end = textBox.SelectionStart;
            var value = textBox.Text;
            while (start - 1 > -1 && value[start - 1] != '\n')
            {
                start--;
            }
            while (end + 1 < length && value[end + 1] != '\n')
            {
                end++;
            }
            if (trimEnd)
            {
                while (char.IsWhiteSpace(value[start]))
                {
                    start++;
                }
                while (char.IsWhiteSpace(value[end]))
                {
                    end--;
                }
            }

            textBox.SelectionStart = start;
            if (end > start)
                textBox.SelectionLength = end - start + 1;




        }
        public static void SelectTableCell(this TextBox textBox)
        {

            var start = textBox.SelectionStart;

            var length = textBox.Text.Length;
            var end = textBox.SelectionStart;
            var value = textBox.Text;
            while (start - 1 > -1 && value[start - 1] != '|' && value[start - 1] != '\n')
            {
                start--;
            }
            while (end + 1 < length && value[end + 1] != '|' && value[end + 1] != '\n')
            {
                end++;
            }
            while (char.IsWhiteSpace(value[start]))
            {
                start++;
            }
            while (char.IsWhiteSpace(value[end]))
            {
                end--;
            }

            textBox.SelectionStart = start;
            if (end > start)
                textBox.SelectionLength = end - start + 1;




        }
    }
    public class Article
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Name = "index_title", Unique = true)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
    public class HelperSqlite
    {
        private static HelperSqlite sHelperSqlite;
        public static HelperSqlite NewInstance(string path){ return new HelperSqlite(path);}
        
        public static HelperSqlite GetInstance(string path){return  (sHelperSqlite = new HelperSqlite(path));}        public static HelperSqlite GetInstance(){return  sHelperSqlite;}
        private readonly SQLiteConnection connection;
        public HelperSqlite(string path)
        {
            connection = new SQLiteConnection(path);
            connection.CreateTable<Article>();
        }

        public IEnumerable<string> GetTitleList()
        {

            return connection.Query<Article>("select Title from Article").Select(i => i.Title);
        }
        public IEnumerable<Article> GetTitleContentList()
        {

            return connection.Query<Article>("select Title,Content from Article");
        }
        public void Insert(Article article)
        {
            connection.Insert(article);
        }
        public void Update(Article article)
        {
            connection.Update(article);
        }
        public Article GetArticle(string title)
        {
            var ls = connection.Query<Article>("select * from Article Where Title = ?", new string[] { title });
            if (ls.Any()) return ls.First();
            return null;

        }
        public void Delete(string title){
        	connection.Delete(GetArticle(title));
        }
        public Article GetContent(Article article)
        {
            return connection.Query<Article>("select Title,Content from Article Where id = ?", new object[] { article.Id }).First();

        }

    }


}
