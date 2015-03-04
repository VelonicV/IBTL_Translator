using System;

namespace CS480Translator.Tokens
{
    class MOT : SetValuesToken
    {

        public MOT(string value)
        {
            validInput = new string[] { "*", "/", "%", "^" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid math operator string passed into token");
            }
        }

    }
}
