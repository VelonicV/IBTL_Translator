using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class IdToken : GenericToken
    {
        public string idName;

        public IdToken(string value)
        {
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
            else if (!Char.IsLetter(arr[0]))
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

            idName = value;
            return true;
        }
    }
}
