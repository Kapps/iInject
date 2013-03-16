using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public static void LoadPlugins(string LibraryPath) {
			if(!File.Exists(LibraryPath))
				throw new FileNotFoundException("The library located at the given path was not found.");

		}
	}
}
