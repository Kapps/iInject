using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides a queue of pages to be scanned for vulnerabilities, generated through one or more IPageProviders.
	/// </summary>
	public class PageQueue {

		/// <summary>
		/// Adds the given provider to the provider list.
		/// </summary>
		public void AddProvider(IPageProvider Provider) {
			bool Added = _Providers.Add(Provider);
			if(!Added)
				throw new ArgumentException("The given provider was already in the provider list.");
		}

		/// <summary>
		/// Returns a list of all pages to scan.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Uri> GetPages() {
			foreach(var Provider in _Providers)
				foreach(var Page in Provider.GetPagesToScan())
					yield return Page;
		}

		private HashSet<IPageProvider> _Providers = new HashSet<IPageProvider>();
	}
}
