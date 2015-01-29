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
            Lexalizer lex = new Lexalizer(@"C:\stutest.in");
            Tokens.GenericToken token = lex.getNextToken();

            while (token != null)
            {
                Console.WriteLine(token.ToString());
                token = lex.getNextToken();
            }

            Console.ReadLine();

        }
    }
}
