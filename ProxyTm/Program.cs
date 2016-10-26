using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ProxyTm
{
	class Program
	{
		static void Main(string[] args)
		{
			var settings = ProxySettingsHelper.ReadConfig();
			if (settings == null)
			{
				settings = new ProxySettings();
				ProxySettingsHelper.WriteConfig(settings);
			}
			int port = settings.Port;
			int wordSize = settings.WordSize;
			string url = settings.Url;

			if (args.Length > 0)
				int.TryParse(args[0], out port);
			if (args.Length > 1)
				url = args[1];
			if (args.Length > 2)
				int.TryParse(args[2], out wordSize);

			var replacer = new Replacer(wordSize, port, url);

			var server = new NetworkProcessor(port, url, replacer);
			var listen = new Task(() => server.Start());
			listen.Start();
			Console.WriteLine("Для остановки сервера нажмите ENTER...");
			Console.ReadLine();
		}
	}
}