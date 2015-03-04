﻿using System;
using System.Linq;

namespace CS480Translator.Tokens
{
    class IT : GenericToken
    {
        public type idType;
        public string codeId;
        public bool assigned;

        public IT(string value)
        {
            idType = type.voidT;
            codeId = null;
            assigned = false;

            if (!validate(value))
            {
                throw new Exception("Error: Invalid id operator string passed into token");
            }
        }

        protected override bool validate(string value)
        {
            char[] arr = value.ToCharArray();
            if(arr.Count() == 0) 
            {
                return false;
            }
            else if ((!Char.IsLetter(arr[0])) && (arr[0] != '_'))
            {
                return false;
            }

            for (int i = 1; i < arr.Count(); i++)
            {
                if ((!Char.IsLetter(arr[i])) && (!Char.IsNumber(arr[i])) && (arr[i] != '_'))
                {
                    return false;
                }
            }

            word = value;
            return true;
        }

        public override string ToString()
        {
            return (this.GetType().Name + ":\t" + word);
        }

    }
}
