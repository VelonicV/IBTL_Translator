using System;

namespace CS480Translator.Tokens
{
    class KT : SetValuesToken
    {
        public KT(string value)
        {
            validInput = new string[] { "(", ")", ":=", "if", "while", "let", "stdout" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid keyword string passed into token");
            }
        }
    }
}
