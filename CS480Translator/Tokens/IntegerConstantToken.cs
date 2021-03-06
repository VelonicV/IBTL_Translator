﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class ICT : GenericToken
    {

        public int integerValue;

        public ICT(string value)
        {
            if (!validate(value))
            {
                throw new Exception("Error: Invalid integer constant string passed into token");
            }
        }

        protected override bool validate(string value)
        {
            try
            {
                integerValue = Convert.ToInt32(value);
                return true;
            }
            catch (OverflowException e)
            {
                Console.WriteLine("Error: Integer constant too large to store in int value type.");
                
                Environment.Exit(1);
            }
            catch { }

            return false;
        }

        public override string ToString()
        {
            return (this.GetType().Name + ":\t" + integerValue);
        }

    }
}
