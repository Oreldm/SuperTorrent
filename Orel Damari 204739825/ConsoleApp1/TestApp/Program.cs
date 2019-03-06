﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorrentLibrary;

namespace TestApp
{

    using System;
    using System.Reflection;

    class Program
    {
        static void Main(string[] args)
        {
            var DLL = Assembly.LoadFile("C:\\Users\\oreldm\\Desktop\\minitorrent\\Orel Damari 204739825\\ConsoleApp1\\TorrentLibrary\\bin\\Debug\\TorrentLibrary.dll");

            foreach (Type type in DLL.GetExportedTypes())
            {
                Console.WriteLine(type);
                if (type.ToString().Contains("Class1"))
                {
                    var c = Activator.CreateInstance(type);
                    type.InvokeMember("PrintHour", BindingFlags.InvokeMethod, null, c, new object[] { @"Hello" });
                }
            }

            Console.ReadLine();
        }
    }

}
