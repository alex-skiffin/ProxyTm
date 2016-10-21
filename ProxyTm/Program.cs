using System;
using System.Threading.Tasks;

namespace ProxyTm
{
	class Program
	{
		static void Main(string[] args)
		{
			int port = 7070;
			string url = "habrahabr.ru";
			if (args.Length > 0)
				int.TryParse(args[0], out port);
			if (args.Length > 1)
				url = args[1];

			WebHelper.LocalUrl = "http://localhost:" + port;
			WebHelper.RemoteUrl = "http://" + url;

			var server = new NetworkProcessor(port);
			Task listen = new Task(() => server.Start());
			listen.Start();
			Console.WriteLine("Для остановки сервера нажмите ENTER...");
			Console.ReadLine();
		}
	}
}
