using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides a single session to be used for performing injections.
	/// </summary>
	public class InjectionSession {

		/// <summary>
		/// Gets the crawler used to scan pages.
		/// </summary>
		public PageCrawler Crawler {
			get { return _Crawler; }
		}

		/// <summary>
		/// Gets a collection of providers used for this session.
		/// </summary>
		public ProviderCollection Providers {
			get { return _Providers; }
		}


		private ProviderCollection _Providers = new ProviderCollection();
		private PageCrawler _Crawler = new PageCrawler();
	}
}
