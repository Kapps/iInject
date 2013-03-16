using iInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInjectFE {
	class Program {
		static void Main(string[] args) {
			InjectionSession Session = new InjectionSession();
			Session.Crawler.CrawlPagesAsync().Wait();
			Console.WriteLine("Done! Press any key to continue.");
			Console.ReadKey();
		}
	}
}
