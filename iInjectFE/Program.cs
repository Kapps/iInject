using iInject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace iInjectFE {
	class Program {
		static void Main(string[] args) {
			InjectionSession Session = new InjectionSession();
			Session.Crawler.CrawlPagesAsync().Wait();
			Console.WriteLine("Done! Press any key to continue.");
			Console.ReadKey();
			/*var Lines = File.ReadAllLines("blah.txt");
			foreach(var line in Lines) {
				Uri path = new Uri(line);
				Console.WriteLine(path);
				Console.ReadLine();
			}*/
		}
	}
}
