using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> files = new List<string>();

            foreach (string arg in args)
            {
                if (!arg.StartsWith("-"))
                {
                    files.Add(arg);
                }
                else
                {
                    if (arg == "-h")
                    {
                        printHelp();
                    }
                    else
                    {
                        Console.WriteLine("Invalid flag: " + arg);
                        Environment.Exit(1);
                    }
                }
            }

            foreach (string file in files)
            {
                Console.WriteLine("Input file: " + file);
                try
                {
                    Parser parser = new Parser(file);
                    printTree(parser.returnTree(), 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine();
            }

            Console.ReadLine();

        }

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

        private static void printHelp()
        {
            Console.WriteLine("Flags:\n\t-h: print help\n");
            Console.WriteLine("Instructions: Any non-flags are treated as paths to input files.");
            Console.WriteLine("              All input files are processed in the order given.");
            Environment.Exit(0);
        }
    }
}
