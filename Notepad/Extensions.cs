using System;
namespace Common
{
	public static class Extensions
	{
		public static Int64 GetTimeStampInMillis(this DateTime dateTime)
		{
			//DateTime.UtcNow
			return (Int64)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
		}
		public static DateTime GetTimeStampInMillis(Int64 ticks)
		{
			var dateTime = new DateTime(ticks);
			return dateTime;
		}
	}
}