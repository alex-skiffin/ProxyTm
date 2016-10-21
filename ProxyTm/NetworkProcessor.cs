using System;
using System.IO;
using System.Net;
using System.Text;

namespace ProxyTm
{
	public class NetworkProcessor
	{
		readonly HttpListener _listener;
		private readonly Speller _speller;
		public NetworkProcessor(int port, int wordSize)
		{
			_listener = new HttpListener();
			_speller = new Speller(wordSize);
			_listener.Prefixes.Add(string.Format("http://*:{0}/", port));
			Console.WriteLine("Listening on port {0}...", port);
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

			Stream inputStream = request.InputStream;
			Encoding encoding = request.ContentEncoding;
			StreamReader reader = new StreamReader(inputStream, encoding);
			var requestBody = reader.ReadToEnd();

			Console.WriteLine("Request was caught: {0}", request.Url);

			string result = "";
			try
			{
				string responseData = string.Empty;
				using (var client = new WebClient() { Encoding = Encoding.UTF8 })
				{
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

			Encoding win1251 = Encoding.GetEncoding("utf-8");
			UTF8Encoding utf = new UTF8Encoding();
			byte[] encodedBytes = win1251.GetBytes(result);
			string temp = utf.GetString(encodedBytes);
			byte[] utfEncodedBytes = Encoding.UTF8.GetBytes(temp);

			context.Response.ContentLength64 = utfEncodedBytes.Length;
			context.Response.OutputStream.Write(utfEncodedBytes, 0, utfEncodedBytes.Length);

			context.Response.Headers.Add(HttpResponseHeader.ContentEncoding, Encoding.UTF8.EncodingName);
		}

		public void Stop()
		{
			if (_listener != null)
			{
				_listener.Stop();
			}
		}

		~NetworkProcessor()
		{
			Stop();
		}
	}
}