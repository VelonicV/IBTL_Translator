using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class RelationalOperatorToken : SetValuesToken
    {
        public RelationalOperatorToken(string value)
        {
            validInput = new string[] { "=", "<", ">", ">=", "<=", "!=" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid keyword string passed into token");
            }
        }
    }
}
