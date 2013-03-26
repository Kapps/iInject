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
	class Program {

		static void Main(string[] args) {
			// Note that iInjectionProviders is referenced right now solely for the purpose of Copy Local.
			// Otherwise, we'd have to update it ourselves when it changed.
			var Config = LoadConfig(args);
			var Session = ParseSession(Config);
			Session.VulnerabilityDetected += Session_VulnerabilityDetected;
			Session.ScanForVulnerabilitiesAsync().Wait();
			Console.WriteLine("Done! Press any key to continue.");
			Console.ReadKey();
		}

		static void Session_VulnerabilityDetected(VulnerabilityDetails obj) {
			// TODO: Better handle vulnerabilities. Another provider?
			Console.WriteLine(obj);
		}

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
