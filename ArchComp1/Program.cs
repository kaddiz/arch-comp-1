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
        private static string[] keys = { "-h", "--help", "-a", "--all", "-o", "--overwrite" };

        private static string CurrentDirectoryPath = Directory.GetCurrentDirectory();

        private static string sourcePath = "", destinationPath = "";
        private static string sourceFile = "", destinationFile = "";
        private static List<string> fileNames = new List<string>();

        private static bool isAll = false;
        private static bool isHelp = false;
        private static bool rewrite = false;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("App doesn't have any arguments.");
                WriteHelp();
                Console.Write("Press Enter to exit . . . ");
                Console.ReadLine();
            }
            else
            {
                var isKey = false;
                for (int i = 0; i < args.Length; i++)
                {
                    isKey = false;
                    if (args[i][0] == '-') isKey = true;
                    if (isKey)
                    {
                        var index = Array.IndexOf(keys, args[i]);
                        if (index == 0 || index == 1) isHelp = true;
                        if (index == 2 || index == 3) isAll = true;
                        if (index == 4 || index == 5) rewrite = true;
                    }
                }
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
                        }
                        else fileNames.Add(args[i]);
                    }
                }
                if (sourcePath == destinationPath)
                {
                    sourcePath = CurrentDirectoryPath;
                }
                else
                {
                    if (!Directory.Exists(sourcePath))
                    {
                        Console.Write("Source path doesn't exist!");
                        return;
                    }
                }
                if (isHelp)
                {
                    WriteHelp();
                    Console.Write("Press Enter to continue . . .");
                    Console.ReadLine();
                    Console.WriteLine();
                }
                if (destinationPath != "")
                {
                    if (fileNames.Capacity > 0)
                    {
                        foreach (string mask in fileNames)
                        {
                            Copy(mask, isAll);
                        }
                    }
                    else
                    {
                        Copy("*", isAll);
                    }
                }
            }
        }

        static void Copy(string mask, bool allSubDirectories)
        {
            if (!allSubDirectories)
            {
                string[] files = Directory.GetFiles(sourcePath, mask);

                if (files.Length == 0)
                {
                    Console.WriteLine("File(s) with name (mask) {0} not found!", mask);
                }
                else
                {
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    foreach (string s in files)
                    {
                        var fileName = Path.GetFileName(s);
                        destinationFile = Path.Combine(destinationPath, fileName);
                        Console.WriteLine("Copying file: " + fileName);
                        try
                        {
                            File.Copy(s, destinationFile, rewrite);
                        }
                        catch(IOException e)
                        {
                            Console.WriteLine("File {0} already is exist!", fileName);
                        }
                        
                    }
                }
            }
            if (allSubDirectories)
            {
                var dir = new DirectoryInfo(sourcePath);
                var flag = false;

                foreach (FileInfo file in dir.EnumerateFiles(mask, SearchOption.AllDirectories))
                {
                    flag = true;
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    var fileName = file.Name;
                    sourceFile = file.FullName;
                    var localPath = sourceFile.Replace(sourcePath, "");
                    var directoryName = localPath.Replace(fileName, "");
                    var destPath = destinationPath + directoryName;
                    if (!Directory.Exists(destPath))
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    destinationFile = Path.Combine(destPath, fileName);
                    if (directoryName == "\\") Console.WriteLine("Copying file: {0}", fileName); 
                    else Console.WriteLine("Copying file: {0} from subdirectory {1}", fileName, directoryName);
                    try
                    {
                        File.Copy(sourceFile, destinationFile, rewrite);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("File {0} already is exist!", fileName);
                    }
                }

                if (!flag)
                {
                    Console.WriteLine("File(s) with name (mask) {0} not found!", mask);
                }
            }
        }

        static void WriteHelp()
        {
            Console.WriteLine();
            Console.WriteLine("\tHELP of program for copying files");
            Console.WriteLine();
            Console.WriteLine("\tParams type list:");
            Console.WriteLine();
            Console.WriteLine("\t1: \\[Directory name] - param that contains the name of the directory");
            Console.WriteLine("\t\t * If directory name param is only one then");
            Console.WriteLine("\t\t\t * Source directory will be Current directory");
            Console.WriteLine("\t\t * If directory name param more than one then");
            Console.WriteLine("\t\t\t * First directory name param will be Source directory");
            Console.WriteLine("\t\t\t * Second directory name param will be Destination directory");
            Console.WriteLine("\t\t\t * Third and more directory name params will be ignored");
            Console.WriteLine();
            Console.WriteLine("\t   Examples:");
            Console.WriteLine("\t   - \\SomeDirectoryName");
            Console.WriteLine("\t   - D:\\DirName\\SubDir");
            Console.WriteLine("\t   - \"\\Dir with space in name\" ");
            Console.WriteLine();
            Console.Write("Press Enter to continue . . .");
            Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("\t2: [File name] - param that contains the name of the file or mask of file group");
            Console.WriteLine("\t\t * File name need specify with extension");
            Console.WriteLine("\t\t * For mask of file group you can use special symbols:");
            Console.WriteLine("\t\t\t ? - one any symbol");
            Console.WriteLine("\t\t\t * - any sequence of symbols");
            Console.WriteLine("\t\t * If file name param is not exist all files will be copy");
            Console.WriteLine();
            Console.WriteLine("\t   Examples:");
            Console.WriteLine("\t   - SomeTextFile.txt");
            Console.WriteLine("\t   - SomeText?.txt (SomeText1.txt or SomeTextA.txt etc.)");
            Console.WriteLine("\t   - *.txt (all text files)");
            Console.WriteLine("\t   - SomeFileName.* (SomeFileName with any extension)");
            Console.WriteLine();
            Console.Write("Press Enter to continue . . .");
            Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("\t3: [-k] or [--keys] - params that contains special options for copying");
            Console.WriteLine();
            Console.WriteLine("\t   List of keys:");
            Console.WriteLine("\t   -a or --all - this key indicates that all subdirectories will be considered in the source directory");
            Console.WriteLine("\t   -o or --overwrite - this key indicates that the files available for overwriting");
            Console.WriteLine("\t   -h or --help - this key displays help on the screen");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tExample of command: ArchComp1 \\Source \\Destination --all *.txt -o");
            Console.WriteLine("\t * Copying all text files from directory Source to Destination directory (in current directory) with overwriting");
        }
    }
}
