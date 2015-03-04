﻿using System;

namespace CS480Translator.Tokens
{
    class RMOT : SetValuesToken
    {
        public RMOT(string value)
        {
            validInput = new string[] { "sin", "cos", "tan" };
            if (!validate(value))
            {
                throw new Exception("Error: Invalid real math operator string passed into token");
            }
        }
    }
}
