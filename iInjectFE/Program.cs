using iInject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace iInjectFE {
	/// <summary>
	/// Handles the main entry point for the program.
	/// </summary>
	public class Program {

		/// <summary>
		/// The main entry point for the front-end. Simply parses the config, loads plugins, and begins scanning.
		/// </summary>
		static void Main(string[] args) {
			// For now, iInjectProviders is referenced for the sole purpose of allowing Copy Local to copy it to the output folder automatically.
			// It doesn't need to be referenced, nothing is special about it, and we still need to load it at runtime.
			// It's just a convenience thing.
			var Config = LoadConfig(args);
			var Session = ParseSession(Config);
			Session.ScanForVulnerabilitiesAsync().Wait();
		}

		/// <summary>
		/// Scans the console arguments for a config file, or uses the default. Returns the JSON Object representing the root of the config.
		/// </summary>
		private static JObject LoadConfig(string[] args) {
			string ConfigFile = args.Length == 0 ? "config.json" : args[0];
			if(!".JSON".Equals(Path.GetExtension(ConfigFile), StringComparison.InvariantCultureIgnoreCase))
				throw new FormatException("Expected the only argument to be the path to a .json config file. If one is not provided, config.json is used.");
			ConfigFile = Path.GetFullPath(ConfigFile);
			if(!File.Exists(ConfigFile))
				throw new FileNotFoundException();
			string JSON = File.ReadAllText(ConfigFile);
			if(!JSON.Trim().StartsWith("{"))
				JSON = "{\n" + JSON + "\n}";
			JObject Input = JObject.Parse(JSON);
			return Input;
		}

		/// <summary>
		/// Parses session information from a config file.
		/// </summary>
		private static InjectionSession ParseSession(JObject Input) {
			foreach(dynamic Plugin in Input["plugins"])
				PluginManager.LoadPlugin((string)Plugin);
			InjectionSession Session = new InjectionSession();
			foreach(dynamic ProviderObj in Input["providers"]) {
				string Name = null;
				ProviderOptions Options = new ProviderOptions();
				foreach(JProperty Child in ProviderObj) {
					string PropertyName = Child.Name;
					dynamic PropertyValue = Child.Value;
					if(String.Equals(PropertyName, "name", StringComparison.InvariantCultureIgnoreCase))
						Name = (string)PropertyValue;
					else
						Options[PropertyName] = PropertyValue;
				}
				var Provider = InjectionProviders.CreateProvider(Name, Session, Options);
				Session.Providers.Add(Provider);
			}
			return Session;
		}
	}
}
