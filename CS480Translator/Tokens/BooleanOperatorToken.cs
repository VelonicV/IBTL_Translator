using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class BooleanOperatorToken : SetValuesToken
    {
        public BooleanOperatorToken(string value)
        {
            validInput = new string[] { "and", "or", "not" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid boolean operator string passed into token");
            }
        }
    }
}
