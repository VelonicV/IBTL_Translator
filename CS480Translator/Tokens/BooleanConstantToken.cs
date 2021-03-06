﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class BCT : GenericToken
    {
        public bool booleanValue;
        private string trueString = "true";
        private string falseString = "false";

        public BCT(string value)
        {
            if (!validate(value))
            {
                throw new Exception("Error: Invalid boolean constant string passed into token");
            }
        }

        protected override bool validate(string value)
        {
            if (value == trueString)
            {
                booleanValue = true;
            }
            else if (value == falseString)
            {
                booleanValue = false;
            }
            else
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return (this.GetType().Name + ":\t" + booleanValue);
        }

    }
}
