using System.Text.RegularExpressions;

namespace ProxyTm
{
	public class Speller
	{
		private readonly Regex _regex;

		public Speller(int wordSize)
		{
			_regex = new Regex(@"(\b\w{" + wordSize + @"}\b)(?![^<]*>|[^<>]*</)");
		}

		public string Replace(string input)
		{
			return _regex.Replace(input, m => m.Value + "™");
		}
	}
}