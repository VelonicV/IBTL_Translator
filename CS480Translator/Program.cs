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

            foreach (string arg in args)
            {
                Console.WriteLine("Input file: " + arg + "\n");

                try
                {
                    Parser parser = new Parser(arg);
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
    }
}
