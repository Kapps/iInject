using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {
	/// <summary>
	/// Represents a page provider that reads pages from the console.
	/// </summary>
	public class ConsoleQueueProvider : IPageProvider {

		/// <summary>
		/// Gets a unique name for this provider.
		/// </summary>
		public string Name {
			get { return "Console Queue"; }
		}

		/// <summary>
		/// Gets the pages that should be scanned by reading lines from the console until a blank line is found.
		/// </summary>
		public IEnumerable<Uri> GetPagesToScan() {
			while(true) {
				string Line = Console.ReadLine();
				if(String.IsNullOrWhiteSpace(Line))
					yield break;
				var Uri = new Uri(Line);
				if(!Uri.IsAbsoluteUri)
					throw new ArgumentException("All Uris must be absolute.");
				yield return Uri;
			}
		}
	}
}
