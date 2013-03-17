using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {
	/// <summary>
	/// a simple ' or '1'='1' -- ' attempt to access users
	/// </summary>
	class EqualsAndQuotes {
		public string VulnerabilityName {
			get { return "Sql Injection"; }
		}

		public InjectionSession Session { get; private set; }

		public EqualsAndQuotes(InjectionSession Session) {
			this.Session = Session;
		}

		public async Task<IEnumerable<VulnerabilityDetails>> ScanForVulnerabilitiesAsync(WebForm Form) {
			List<VulnerabilityDetails> Results = new List<VulnerabilityDetails>();
			var Parser = Session.Crawler.Parser;
			foreach(var Control in Form.Controls) {
				Control.Value = "' or '1'='1' -- '";
				var SubmitTask = Form.SubmitAsync(Parser);
				PageResponse Response = null;

			}
			return Results;
		}

		public string Name {
			get { return "Sql Injection equals scanner"; }
		}
	}
}
