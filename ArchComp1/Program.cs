using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchComp1
{
    class Program
    {
        private static string[] keys = { "-h", "--help" };

        static int Main(string[] args)
        {
            List<int> indexes = new List<int>();

            if (args.Length == 0)
            {
                Console.WriteLine("App doesn't have any arguments.");
            }
            else
            {
                var isKey = false;
                foreach (var a in args)
                {
                    isKey = false;
                    if (a[0] == '-') isKey = true;
                    if (isKey)
                    {
                        var index = Array.IndexOf(keys, a);
                        if (index > -1) indexes.Add(index);
                        else Console.WriteLine("The key {0} not found!", a);
                    }
                }
            }

            Console.WriteLine(indexes);

            Console.Read();
            return 0;
        }
    }
}
