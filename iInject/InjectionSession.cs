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

		/// <summary>
		/// Asynchronously scans for vulnerabilities using the currently defined providers.
		/// </summary>
		public async Task ScanForVulnerabilitiesAsync() {
			await Crawler.CrawlPagesAsync(async (Response) => {
				foreach(IVulnerabilityScanner Scanner in Providers.Where(c => c is IVulnerabilityScanner)) {
					foreach(var Form in Response.Forms) {
						var Vulnerabilities = await Scanner.ScanForVulnerabilitiesAsync(Form);
						foreach(var Vulnerability in Vulnerabilities)
							if(this.VulnerabilityDetected != null)
								this.VulnerabilityDetected(Vulnerability);
					}
				}
			});
		}

		private ProviderCollection _Providers = new ProviderCollection();
		private PageCrawler _Crawler = new PageCrawler();
	}
}
