using System.Text.RegularExpressions;

namespace ProxyTm
{
	public class Speller
	{
		private Regex _regex;
		public Speller() : this(6)
		{ }
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
