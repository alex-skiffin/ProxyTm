using System.Text.RegularExpressions;

namespace ProxyTm
{
	public class Replacer
	{
		private readonly string _localUrl;
		private readonly string _remoteUrl;
		private readonly Regex _regex;

		public Replacer(int wordSize, int port, string url)
		{
			_regex = new Regex(@"(\b\w{" + wordSize + @"}\b)(?![^<]*>|[^<>]*</)");
			_localUrl = "http://localhost:" + port;
			_remoteUrl = url;
		}

		public string Replace(string input)
		{
			return _regex.Replace(ReplaceUrlToLocal(input), m => m.Value + "™");
		}

		private string ReplaceUrlToLocal(string content)
		{
			return content.Replace(_remoteUrl, _localUrl);
		}
	}
}