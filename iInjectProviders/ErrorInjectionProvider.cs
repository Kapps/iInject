using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {

	/// <summary>
	/// Provides a vulnerability scanner that injects invalid SQL and scans for injection by seeing if an internal server error occurs.
	/// </summary>
	public class ErrorInjectionProvider : IVulnerabilityScanner {

		/// <summary>
		/// Gets the name of the vulnerability this scanner tries to exploit.
		/// </summary>
		public string VulnerabilityName {
			get { return "Sql Injection"; }
		}

		/// <summary>
		/// Gets a unique name for this provider.
		/// </summary>
		public string Name {
			get { return "Error Scanner"; }
		}

		/// <summary>
		/// Gets the session that this provider operates on.
		/// </summary>
		public InjectionSession Session { get; private set; }

		/// <summary>
		/// Creates a new ErrorInjectionProvider for the given session.
		/// </summary>
		public ErrorInjectionProvider(InjectionSession Session) {
			this.Session = Session;
		}

		/// <summary>
		/// Scans each control in the given form for Sql Injection exploits.
		/// </summary>
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
