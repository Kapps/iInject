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
			var Lines = File.ReadAllLines("blah.txt");
			foreach(var line in Lines) {
				Uri path = new Uri(line);
				Console.WriteLine(path);
				Console.ReadLine();
			}
		}
	}
}
