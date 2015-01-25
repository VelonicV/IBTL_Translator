using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class VariableTypeToken : SetValuesToken
    {
        public VariableTypeToken(string value)
        {
            validInput = new string[] { "int", "real", "bool", "string" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid variable type string passed into token");
            }
        }
    }
}
