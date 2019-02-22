
namespace  Shared
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Common;
	
	public static class GenericExtensions
	{
		public static IEnumerable<T> Distinct<T, U>(
			this IEnumerable<T> seq, Func<T, U> getKey)
		{
			return
                from item in seq
			             group item by getKey(item) into gp
			             select gp.First();
		}
		
		public static void AddIfNotExist<T>(this List<T> list, T t)
		{
			if (list.IndexOf(t) < 0) {
				list.Add(t);
			}
		}
		public static string Joining(this IEnumerable<String> values, string separator = "\n", bool pretty = true)
		{
			if (separator == null)
				separator = String.Empty;
			if (pretty) {
				values = values.Where(i => i.IsReadable()).Select(i => i).OrderBy(i => i).Distinct();
			}
			
			using (IEnumerator<String> en = values.GetEnumerator()) {
				if (!en.MoveNext())
					return String.Empty;
 
				StringBuilder result = new StringBuilder();
				if (en.Current != null) {
					result.Append(en.Current);
				}
 
				while (en.MoveNext()) {
					result.Append(separator);
					if (en.Current != null) {
						result.Append(en.Current);
					}
				}            
				return result.ToString();
			}  
		}
	}

}
