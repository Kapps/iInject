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
		/// Gets an event called when a vulnerability is found on a page.
		/// </summary>
		public event Action<VulnerabilityDetails> VulnerabilityDetected;

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

		/// <summary>
		/// Notifies the session that the given vulnerability was detected.
		/// </summary>
		public void NotifyVulnerability(VulnerabilityDetails Details) {
			if(this.VulnerabilityDetected != null)
				this.VulnerabilityDetected(Details);
		}

		private ProviderCollection _Providers = new ProviderCollection();
		private PageCrawler _Crawler = new PageCrawler();
	}
}
