using System;

namespace CS480Translator.Tokens
{
    class CSOP : SetValuesToken
    {
        public CSOP(string value)
        {
            validInput = new string[] { "+", "-" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid context sensitive operator string passed into token");
            }
        }
    }
}
