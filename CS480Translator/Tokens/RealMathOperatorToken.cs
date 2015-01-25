using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class RealMathOperatorToken : SetValuesToken
    {
        public RealMathOperatorToken(string value)
        {
            validInput = new string[] { "sin", "cos", "tan" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid real math operator string passed into token");
            }
        }
    }
}
