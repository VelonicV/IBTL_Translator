using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class BOT : SetValuesToken
    {
        public BOT(string value)
        {
            validInput = new string[] { "and", "or", "not" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid boolean operator string passed into token");
            }
        }
    }
}
