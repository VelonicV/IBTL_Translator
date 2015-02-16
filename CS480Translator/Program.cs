using System;
using System.Collections;
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
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine();
            }

            Console.ReadLine();

        }
    }
}
