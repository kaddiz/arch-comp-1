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

        private static string sourcePath = "", destinationPath = "";
        private static string sourceFile = "", destinationFile = "", fileName = "";

        static void Main(string[] args)
        {
            List<int> indexes = new List<int>();            

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
                    }
                }
                // Работа с аргументами
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i][0] != '-')
                    {
                        if (args[i][0] == '\\' || args[i][1] == ':' && args[i][2] == '\\')
                        {
                            string path;
                            if (args[i][1] == ':') path = args[i];
                            else path = CurrentDirectoryPath + args[i];
                            if (path == CurrentDirectoryPath + "\\.") path = CurrentDirectoryPath;
                            if (sourcePath == "" && destinationPath == "")
                            {
                                sourcePath = path;
                                destinationPath = path;
                            }
                            if (destinationPath == sourcePath) destinationPath = path;

                            if (!Directory.Exists(destinationPath))
                            {
                                Directory.CreateDirectory(destinationPath);
                            }
                        }
                        else if (fileName == "") fileName = args[i];
                    }
                }
                if (sourcePath == destinationPath) sourcePath = CurrentDirectoryPath;
                else
                {
                    if (!Directory.Exists(sourcePath))
                    {
                        Console.WriteLine("Source path doesn't exist!");
                        return;
                    }
                }
                if (fileName != "")
                {
                    Copy(fileName, false);
                }
                else
                {
                    Copy("*", true);
                }
            }

            foreach(var i in indexes)
            {
                Console.WriteLine(keys[i]);
            }

            Console.WriteLine("Current Dir: {0}", CurrentDirectoryPath);
            Console.WriteLine("SRC: {0} \nDEST: {1}", sourcePath, destinationPath);

            //Console.WriteLine("Press any key to exit.");
            //Console.Read();
        }

        static void Copy(string mask, bool allSubDirectories)
        {
            if (!allSubDirectories)
            {
                string[] files = Directory.GetFiles(sourcePath, mask);

                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destinationFile = Path.Combine(destinationPath, fileName);
                    System.IO.File.Copy(s, destinationFile, true);
                }
            }
            if (allSubDirectories)
            {
                var dir = new DirectoryInfo(sourcePath);

                foreach (FileInfo file in dir.EnumerateFiles(mask, SearchOption.AllDirectories))
                {
                    Console.WriteLine(file.FullName);

                    fileName = file.Name;
                    sourceFile = file.FullName;
                    var localPath = sourceFile.Replace(sourcePath, "");
                    var directoryName = localPath.Replace(fileName, "");
                    var destPath = destinationPath + directoryName;
                    if (!Directory.Exists(destPath))
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    destinationFile = Path.Combine(destPath, fileName);
                    File.Copy(sourceFile, destinationFile, true);
                }
            }
        }
    }
}
