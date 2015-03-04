using System;

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
