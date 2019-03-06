using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentLibrary
{
    public class Class1
    {
        public void PrintHour(string s)
        {
            Console.WriteLine("the time is now " + string.Format("{0:HH:mm:ss} ", DateTime.Now));
        }
    }
}
