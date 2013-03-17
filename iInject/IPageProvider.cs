using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides an interface for a provider that can generate a list of pages to search.
	/// </summary>
	public interface IPageProvider : IInjectionProvider {

		/// <summary>
		/// Returns a collection of pages that can be scanned.
		/// </summary>
		IEnumerable<Uri> GetPagesToScan();

	}
}
