﻿using iInject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using iInjectProviders;

namespace iInjectFE {
	class Program {
		static void Main(string[] args) {
			// 64.201.201.162
			InjectionSession Session = new InjectionSession();
			SleepInjectionProvider SleepProvider = new SleepInjectionProvider(Session);
			Session.Providers.Add(SleepProvider);
			Session.Crawler.Queue.AddProvider(new ConsolePageProvider());
			Session.ScanForVulnerabilitiesAsync().Wait();
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
