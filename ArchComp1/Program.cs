using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchComp1
{
    class Program
    {
        private static string[] keys = { "-h", "--help" };

        private static string CurrentDirectoryPath = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            List<int> indexes = new List<int>();

            string sourcePath = "", destinationPath = "";

            if (args.Length == 0)
            {
                Console.WriteLine("App doesn't have any arguments.");
            }
            else
            {
                // Проверка аргументов на наличие ключей
                var isKey = false;
                for (int i = 0; i < args.Length; i++)
                {
                    isKey = false;
                    if (args[i][0] == '-') isKey = true;
                    if (isKey)
                    {
                        var index = Array.IndexOf(keys, args[i]);
                        if (index > -1) indexes.Add(index);
                        // else Console.WriteLine("The key {0} not found!", args[i]);
                    }
                }
                // Работа с аргументами
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i][0] != '-')
                    {
                        if (args[i][0] != '\\')
                        {
                            string path = Path.Combine(CurrentDirectoryPath, args[i]);
                            Console.WriteLine("path " + path);
                            if (Directory.Exists(path))
                            {
                                if (sourcePath == "") sourcePath = path;
                                else if (destinationPath == "") destinationPath = path;
                            }
                            else
                            {
                                Console.WriteLine("Source path does not exist!");
                            }
                            
                        }
                    }
                }
            }

            foreach(var i in indexes)
            {
                Console.WriteLine(keys[i]);
            }

            Console.WriteLine("Current Dir: {0}", CurrentDirectoryPath);
            Console.WriteLine("SRC: {0} \nDEST: {1}", sourcePath, destinationPath);

            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}
