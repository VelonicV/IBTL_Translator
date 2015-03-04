using System;
using System.Collections.Generic;
using System.IO;

namespace CS480Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Files to parse
            List<String> files = new List<string>();

            //If no arguments are entered, print the help menu.
            if(args.Length == 0) 
            {
                printHelp();
            }

            //Parse the arguments, adding files and toggling flags.
            foreach (string arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    if (arg == "-h")
                    {
                        printHelp();
                    }
                    else
                    {
                        Console.WriteLine("Invalid flag: " + arg);
                        Console.WriteLine("Use -h flag to see program options and instructions.");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    files.Add(arg);
                }
            }

            //Run the parser for each file.
            foreach (string file in files)
            {
                try {
                    CodeGenerator cg = new CodeGenerator(file);
                    Console.WriteLine(cg.getCode());
                    File.WriteAllText("C:\\output.out", cg.getCode());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                break;
            }

            Console.ReadLine();

        }

        // Print the help menu.
        private static void printHelp()
        {
            Console.WriteLine("Flags:\n       -h: print this help menu\n");
            Console.WriteLine("Instructions: Any non-flags are treated as paths to input files.");
            Console.WriteLine("              Only the first input file is compiled.");
            Console.WriteLine("              Errors caused during compilation will print an error.");
            Environment.Exit(0);
        }
    }
}
