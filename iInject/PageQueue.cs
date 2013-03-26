using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides a queue of pages to be scanned for vulnerabilities, generated through one or more IPageProviders on the session.
	/// </summary>
	public class PageQueue {

		public PageQueue(InjectionSession Session) {
			this.Session = Session;
		}

		/// <summary>
		/// Returns a list of all pages to scan.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Uri> GetPages() {
			foreach(var Provider in Session.Providers.Where(c=>c is IPageProvider).Select(c=>(IPageProvider)c))
				foreach(var Page in Provider.GetPagesToScan())
					yield return Page;
		}

		private InjectionSession Session;
	}
}
