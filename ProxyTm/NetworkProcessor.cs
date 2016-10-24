using System;
using System.Net;
using System.Text;

namespace ProxyTm
{
	public class NetworkProcessor
	{
		private readonly HttpListener _listener;
		private readonly Speller _speller;
		public NetworkProcessor(int port, int wordSize)
		{
			_listener = new HttpListener();
			_listener.Prefixes.Add($"http://*:{port}/");

			_speller = new Speller(wordSize);

			Console.WriteLine($"Listening on port {port}...");
		}

		public void Start()
		{
			_listener.Start();

			while (true)
			{
				try
				{
					Method();
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.Message);
				}
			}
		}

		private void Method()
		{
			HttpListenerContext context = _listener.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;

			Console.WriteLine($"Request was caught: {request.Url}");

			var result = string.Empty;
			try
			{
				using (var client = new WebClient())
				{
					client.Encoding = Encoding.UTF8;
					result = client.DownloadString(WebHelper.RemoteUrl + request.Url.AbsolutePath);
				}
				response.StatusCode = (int)HttpStatusCode.OK;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			
			result = WebHelper.ReplaceUrl(result);
			result = _speller.Replace(result);

			var win1251 = Encoding.GetEncoding("utf-8");
			var utf = new UTF8Encoding();
			var encodedBytes = win1251.GetBytes(result);
			var temp = utf.GetString(encodedBytes);
			var utfEncodedBytes = Encoding.UTF8.GetBytes(temp);

			context.Response.ContentLength64 = utfEncodedBytes.Length;
			context.Response.OutputStream.Write(utfEncodedBytes, 0, utfEncodedBytes.Length);

			context.Response.Headers.Add(HttpResponseHeader.ContentEncoding, Encoding.UTF8.EncodingName);
		}
	}
}