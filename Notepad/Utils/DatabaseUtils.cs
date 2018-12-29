using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace Utils
{
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
	public class DatabaseUtils
	{
		private static DatabaseUtils sHelperSqlite;
		public static DatabaseUtils GetInstance(string path)
		{
			return  (sHelperSqlite = new DatabaseUtils(path));
		}
		public static DatabaseUtils NewInstance(string path){ return new DatabaseUtils(path);}
        
		public static DatabaseUtils GetInstance()
		{
			return  sHelperSqlite;
		}
		private readonly SQLiteConnection connection;
        
		public DatabaseUtils(string path)
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
			if (ls.Any())
				return ls.First();
			return null;

		}
		public void Delete(string title)
		{
			connection.Delete(GetArticle(title));
		}
		public Article GetContent(Article article)
		{
			return connection.Query<Article>("select Title,Content from Article Where id = ?", new object[] { article.Id }).First();

		}
	}
}
