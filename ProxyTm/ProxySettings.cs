using System;
using System.Configuration;

namespace ProxyTm
{
	public class ProxySettings : ConfigurationSection
	{

		[ConfigurationProperty("wordSize",
			DefaultValue = (int)6,
			IsRequired = false)]
		public int WordSize
		{
			get
			{
				return (int)this["wordSize"];
			}
			set
			{
				this["wordSize"] = value;
			}
		}

		[ConfigurationProperty("url",
			DefaultValue = "https://habrahabr.ru",
			IsRequired = true)]
		[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
		public string Url
		{
			get
			{
				return (string)this["url"];
			}
			set
			{
				this["url"] = value;
			}
		}

		[ConfigurationProperty("port",
			DefaultValue = (int)7070,
			IsRequired = false)]
		public int Port
		{
			get
			{
				return (int)this["port"];
			}
			set
			{
				this["port"] = value;
			}
		}
	}
	public static class ProxySettingsHelper
	{
		public static void WriteConfig(ProxySettings proxySettings)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
			config.AppSettings.Settings.Add("wordSize", proxySettings.WordSize.ToString());
			config.AppSettings.Settings.Add("port", proxySettings.Port.ToString());
			config.AppSettings.Settings.Add("url", proxySettings.Url);
			config.Save(ConfigurationSaveMode.Minimal);
			ConfigurationManager.RefreshSection("ProxySettings");
		}

		public static ProxySettings ReadConfig()
		{
			var settings = new ProxySettings();
			Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
			if (config.AppSettings.Settings["wordSize"] == null || config.AppSettings.Settings["port"] == null || config.AppSettings.Settings["Url"] == null)
				return null;
			int wordSize;
			if (int.TryParse(config.AppSettings.Settings["wordSize"]?.Value, out wordSize))
				settings.WordSize = wordSize;
			int port;
			if (int.TryParse(config.AppSettings.Settings["port"]?.Value, out port))
				settings.Port = port;
			string url = config.AppSettings.Settings["Url"]?.Value;
			if (!string.IsNullOrEmpty(url))
				settings.Url = url;
			return settings;
		}
	}
}