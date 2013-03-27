using iInject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {
	/// <summary>
	/// A page provider that gets it's Uris from reading a file.
	/// </summary>
	public class FilePageProvider : IPageProvider {

		/// <summary>
		/// Gets a unique name for this provider.
		/// </summary>
		public string Name {
			get { return "File Queue"; }
		}

		/// <summary>
		/// Creates a new FilePageProvider with the given options.
		/// Options is expected to contain a FileName property indicating what file to read from.
		/// </summary>
		public FilePageProvider(ProviderOptions Options) {
			this._FileName = (string)Options["FileName"];
		}

		/// <summary>
		/// Reads all of the lines from the input file, returning each line as a separate Uri.
		/// </summary>
		public IEnumerable<Uri> GetPagesToScan() {
			return File.ReadAllLines(_FileName).Where(c=>!String.IsNullOrWhiteSpace(c)).Select(c => new Uri(c));
		}

		private string _FileName;
	}
}