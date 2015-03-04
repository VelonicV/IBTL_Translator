using System;

namespace CS480Translator.Tokens
{
    class VTT : SetValuesToken
    {
        public VTT(string value)
        {
            validInput = new string[] { "int", "real", "bool", "string" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid variable type string passed into token");
            }
        }
    }
}
