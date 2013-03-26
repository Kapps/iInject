using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides a static helper than can be used to load plugins implementing providers.
	/// </summary>
	public static class PluginManager {
		/// <summary>
		/// Loads the plugin located at the given file path, including .dll extension.
		/// </summary>
		public static void LoadPlugin(string LibraryPath) {
			if(!File.Exists(LibraryPath))
				throw new FileNotFoundException("The library located at the given path was not found.");
			if(!Path.IsPathRooted(LibraryPath))
				LibraryPath = Path.GetFullPath(LibraryPath);
			var Library = Assembly.LoadFile(LibraryPath);
			// TODO: Plugins should be loaded in a different AppDomain with no real permissions.
			foreach(var Type in Library.GetTypes()) {
				if(Type.GetInterfaces().Contains(typeof(IInjectionProvider)))
					InjectionProviders.RegisterProvider(Type);
			}
		}
	}
}
