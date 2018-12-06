
namespace  Shared
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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
	}

}
