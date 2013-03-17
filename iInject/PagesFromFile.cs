using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject
{
    class PagesFromFile : IPageProvider
    {


        public IEnumerable<Uri> GetPagesToScan()
        {   //reads in file to scan
            // each Uri is separated by a newline
            string Filename = Console.ReadLine();
            var Lines = File.ReadAllLines(Filename);
            foreach (var line in Lines)
            {
                Uri path = new Uri(line);
                yield return path;
            }
        }
    }
}
