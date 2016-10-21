namespace ProxyTm
{
	public static class WebHelper
	{
		public static string LocalUrl { get; set; }
		public static string RemoteUrl { get; set; }
		public static string ReplaceUrl(string content)
		{
			return content.Replace(RemoteUrl, LocalUrl);
		}
	}
}