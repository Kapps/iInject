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
    class VersionDetect
    {

		// TODO: Make an IInjectionProvider that can post-process results, and make this be one of those.

        /// <summary>
        /// listen to event in session
        /// </summary>
        /// <param name="SessionToListen"></param>
        public void listen(InjectionSession SessionToListen)
        {
            SessionToListen.VulnerabilityDetected += SessionToListen_VulnerabilityDetected;
        }
        /// <summary>
        /// inputs a string into the form causing errors on wrong version until correct version found
        /// </summary>
        /// <param name="toScan"></param>
        void SessionToListen_VulnerabilityDetected(VulnerabilityDetails obj)
        {
            //implement
        }

    }
}
