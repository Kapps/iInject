using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {
	/// <summary>
	/// An IVulnerabilityScanner that attempts to use a Sleep statement for SQL Injection.
	/// </summary>
	public class SleepInjectionProvider : IVulnerabilityScanner {

		public string VulnerabilityName {
			get { return "Sql Injection"; }
		}

		public InjectionSession Session { get; private set; }

		public SleepInjectionProvider(InjectionSession Session) {
			this.Session = Session;
		}

		public async Task<IEnumerable<VulnerabilityDetails>> ScanForVulnerabilitiesAsync(WebForm Form) {
			List<VulnerabilityDetails> Results = new List<VulnerabilityDetails>();
			var Parser = Session.Crawler.Parser;
			foreach(var Control in Form.Controls) {
				var OldValue = Control.Value;
				var NewValue = Control.GenerateDefaultValue(true);
				try {
					Control.Value = NewValue + "' OR SLEEP(30000) = '1";
					DateTime StartTime = DateTime.Now;
					var SubmitTask = Form.SubmitAsync(Parser);
					PageResponse Response = null;
					try {
						Response = await SubmitTask;
					} catch(Exception) { } // If an exception, do nothing as we can expect a timeout.
					
					DateTime StopTime = DateTime.Now;
					if((StopTime - StartTime).TotalSeconds > 15) {
						Results.Add(new VulnerabilityDetails(Form, Control, this));
					}
				} finally {
					Control.Value = OldValue;
				}
			}
			return Results;
		}

		public string Name {
			get { return "Sql Injection Sleep Scaner"; }
		}
	}
}
