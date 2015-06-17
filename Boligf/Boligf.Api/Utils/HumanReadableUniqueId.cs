using System;
using System.Text;

namespace Boligf.Api.Utils
{
	public interface IHumanReadableUniqueId
	{
		string Generate();
	}

	public class HumanReadableUniqueId : IHumanReadableUniqueId
	{
		private readonly char[] _base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
		private readonly Random _random = new Random();

		public string Generate()
		{
			const int length = 6;
			var sb = new StringBuilder(length);

			for (var i = 0; i < length; i++)
				sb.Append(_base62Chars[_random.Next(62)]);
			
			return sb.ToString();
		}

		public static IHumanReadableUniqueId Instance;
		public static string NewUid()
		{
			return (Instance ?? (Instance = new HumanReadableUniqueId())).Generate();
		}
	}
}