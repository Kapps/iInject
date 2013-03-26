using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {

	/// <summary>
	/// Provides a vulnerability scanner that injects invalid SQL and scans for injection by seeing if an error is returned.
	/// </summary>
	public class ErrorInjectionProvider : IVulnerabilityScanner {

		public string VulnerabilityName {
			get { return "Sql Injection"; }
		}

		public string Name {
			get { return "Error Scanner"; }
		}

		public InjectionSession Session { get; private set; }

		public ErrorInjectionProvider(InjectionSession Session) {
			this.Session = Session;
		}

		public async Task<IEnumerable<VulnerabilityDetails>> ScanForVulnerabilitiesAsync(WebForm Form) {
			// Try to inject invalid SQL into each control, then check if the server returns a 5__ status code, indicating a server error.
			List<VulnerabilityDetails> Results = new List<VulnerabilityDetails>();
			var Parser = Session.Crawler.Parser;
			foreach(var Control in Form.Controls) {
				if(Control.IsSpecialControl())
					continue;
				var OldValue = Control.Value;
				var NewValue = Control.GenerateDefaultValue(true);
				try {
					Control.Value = NewValue + "' 123asdkocxckxvisd";
					var Response = await Form.SubmitAsync(Parser, TimeSpan.FromSeconds(30));
					if((int)Response.StatusCode >= 500 && (int)Response.StatusCode < 600)
						Results.Add(new VulnerabilityDetails(Form, Control, this));
				} finally {
					Control.Value = OldValue;
				}
			}
			return Results;
		}
	}
}
