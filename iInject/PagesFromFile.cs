using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject
{
    /// <summary>
    /// get Uris from file
    /// </summary>
    class PagesFromFile : IPageProvider
    {
        /// <summary>
        /// provides list of \n seperated Uris from a file specified with parameter
        /// </summary>
        /// <param name="FileToScan"></param>
        /// <returns></returns>

        public IEnumerable<Uri> GetPagesToScan(string FileToScan)
        {   //reads in file to scan
            // each Uri is separated by a newline
            string Filename = FileToScan;
            var Lines = File.ReadAllLines(Filename);
            foreach (var line in Lines)
            {
                Uri path = new Uri(line);
                yield return path;
            }
        }
    }
}
