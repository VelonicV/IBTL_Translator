﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
