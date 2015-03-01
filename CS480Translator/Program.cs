using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CS480Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Files to parse
            List<String> files = new List<string>();

            //Quiet mode, automatically disabled
            bool quiet = false;

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
                    if (arg == "-q")
                    {
                        quiet = true;
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
                Console.WriteLine("Input file: " + file);
                try
                {
                    CodeGenerator cg = new CodeGenerator(file);
                    Console.WriteLine(cg.getCode());
                    File.WriteAllText("C:\\output.out", cg.getCode());
                    //Parser parser = new Parser(file);
                    //if (quiet)
                    //{
                    //    Console.WriteLine("Grammar is valid.");
                    //}
                    //else
                    //{
                    //    printTree(parser.returnTree(), 0);
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.ReadLine();
        }

        // Parse the tree recursively, outputting the terminals and their depth.
        private static void printTree(Tree.NonTerm root, int level)
        {
            foreach (Tree.IParseTree node in root.getList())
            {
                if (node is Tree.Term)
                {
                    for (int i = 0; i < level; i++)
                    {
                        Console.Write("  ");
                    }
                    Console.WriteLine(node.ToString());
                }
                else
                {
                    printTree((Tree.NonTerm) node, level + 1);
                }
            }
        }

        // Print the help menu.
        private static void printHelp()
        {
            Console.WriteLine("Flags:\n       -h: print this help menu\n       -q: quiet, only show errors or valid grammar confirmation\n");
            Console.WriteLine("Instructions: Any non-flags are treated as paths to input files.");
            Console.WriteLine("              All input files are parsed in the order given.");
            Console.WriteLine("              Errors caused during parsing will not halt the processing");
            Console.WriteLine("              of valid files.");
            Environment.Exit(0);
        }
    }
}
