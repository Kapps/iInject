using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// A page provider that gets it's Uris from reading a file.
	/// </summary>
	public class FilePageProvider : IPageProvider {

		public string Name {
			get { return "File Page Provider"; }
		}

		public FilePageProvider(string FileName) {
			this._FileName = FileName;
		}

		/// <summary>
		/// provides list of \n seperated Uris from a file specified with parameter
		/// </summary>
		/// <param name="FileToScan"></param>
		/// <returns></returns>
		public IEnumerable<Uri> GetPagesToScan() {   //reads in file to scan
			// each Uri is separated by a newline
			var Lines = File.ReadAllLines(_FileName);
			foreach(var line in Lines) {
				Uri path = new Uri(line);
				yield return path;
			}
		}

		private string _FileName;
	}
}