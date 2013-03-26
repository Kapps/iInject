using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {
	/// <summary>
	/// Provides a scanner that looks for XSS exploits by injecting script tags.
	/// </summary>
	public class XssScriptProvider : IVulnerabilityScanner {


		private Random rnd = new Random();

		/// <summary>
		/// Gets the name of the vulnerability this scanner tries to exploit.
		/// </summary>
		public string VulnerabilityName {
			get { return "XSS"; }
		}

		/// <summary>
		/// Gets the session this provider operates on.
		/// </summary>
		public InjectionSession Session { get; private set; }

		/// <summary>
		/// Gets a unique name for this provider.
		/// </summary>
		public string Name {
			get { return "Script Scanner"; }
		}

		/// <summary>
		/// Creates a new XssScriptProvider with the given session.
		/// </summary>
		public XssScriptProvider(InjectionSession Session) {
			this.Session = Session;
		}

		/// <summary>
		/// Scans for any controls that may have XSS vulnerabiities on the given form.
		/// </summary>
		public async Task<IEnumerable<VulnerabilityDetails>> ScanForVulnerabilitiesAsync(WebForm Form) {
			List<VulnerabilityDetails> Results = new List<VulnerabilityDetails>();
			var Parser = Session.Crawler.Parser;
			foreach(var Control in Form.Controls) {
				if(Control.IsSpecialControl())
					continue;
				var OldValue = Control.Value;
				var NewValue = Control.GenerateDefaultValue(true);
				try {
					string RandomString = GetRandomString();
					string InjectionContents = "<script>alert('" + RandomString + "');</script>";
					Control.Value = NewValue + "</input>" + InjectionContents;
					var Result = await Form.SubmitAsync(Parser, TimeSpan.FromSeconds(30));
					if(Result.RawText.Contains(InjectionContents))
						Results.Add(new VulnerabilityDetails(Form, Control, this));
				} finally {
					Control.Value = OldValue;
				}
			}
			return Results;
		}

		private string GetRandomString() {
			string Result = "";
			int NumChars = rnd.Next(24, 32);
			for(int i = 0; i < NumChars; i++)
				Result += (char)rnd.Next((int)'a', (int)'z' + 1);
			return Result;
		}
	}
}
