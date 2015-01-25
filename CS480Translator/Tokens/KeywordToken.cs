using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class KeywordToken : SetValuesToken
    {
        public KeywordToken(string value)
        {
            validInput = new string[] { "(", ")", ":=", "if", "while", "let", "stdout" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid keyword string passed into token");
            }
        }
    }
}
