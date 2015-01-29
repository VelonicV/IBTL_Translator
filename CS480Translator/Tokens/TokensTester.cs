using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class TokensTester
    {
        private const int LOOP = 10;
        private const int MAX_RAND_STRING_LENGTH = 8;

        private static string genRandString(int maxLength, Random random)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_+-*/%^=<>!():";
            string[] validKeywords = { "bool", "int", "real", "string", "and", "or", "not", "-", "*",
                                       "/", "%", "^", "=", ">", "<", "<=", ">=", "!=", "sin", "cos",
                                       "tan", "+", "(", ")", ":=", "if", "while", "let", "stdout", 
                                       "true", "false"};
            char[] stringChars = new char[random.Next(maxLength)];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);

            if (validKeywords.Contains(finalString))
            {
                return genRandString(maxLength, random);
            }
            else
            {
                return finalString;
            }

        }

        public static void runTokenTest()
        {
            bctTest();
            botTest();
            csotTest();
            itTest();
            ktTest();
            motTest();
            rmotTest();
            rotTest();
            vttTest();
        }

        private static void bctTest()
        {
            Random random = new Random();
            BCT bct;
            bct = new BCT("true");
            bct = new BCT("false");
            for (int i = 0; i < LOOP; i++ )
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    bct = new BCT(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("BCT Test failed using string: " + temp);
                }
            }

        }

        private static void botTest()
        {
            Random random = new Random();
            BOT bot;
            bot = new BOT("and");
            bot = new BOT("or");
            bot = new BOT("not");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    bot = new BOT(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("BOT Test failed using string: " + temp);
                }
            }
        }

        private static void csotTest()
        {
            Random random = new Random();
            CSOP csot;
            csot = new CSOP("+");
            csot = new CSOP("-");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    csot = new CSOP(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("CSOT Test failed using string: " + temp);
                }
            }
        }

        private static void itTest()
        {
            IT it;
            it = new IT("_testest123");
            it = new IT("HELLOPEOPLE");
            it = new IT("TeStingThe_ID");
            it = new IT("Hai_123_fun_22_");

            string[] tests = { "888", "8TestingID", "$haisup" };
            foreach (string x in tests) { 
                bool caught = false;
                try
                {
                    it = new IT(x);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("IT Test failed using string: " + x);
                }
            }

        }

        private static void ktTest()
        {
            Random random = new Random();
            KT kt;
            kt = new KT("(");
            kt = new KT(")");
            kt = new KT(":=");
            kt = new KT("if");
            kt = new KT("while");
            kt = new KT("let");
            kt = new KT("stdout");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    kt = new KT(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("KT Test failed using string: " + temp);
                }
            }
        }

        private static void motTest()
        {
            Random random = new Random();
            MOT mot;
            mot = new MOT("*");
            mot = new MOT("/");
            mot = new MOT("%");
            mot = new MOT("^");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    mot = new MOT(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("MOT Test failed using string: " + temp);
                }
            }
        }

        private static void rmotTest()
        {
            Random random = new Random();
            RMOT rmot;
            rmot = new RMOT("sin");
            rmot = new RMOT("cos");
            rmot = new RMOT("tan");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    rmot = new RMOT(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("RMOT Test failed using string: " + temp);
                }
            }

        }

        private static void rotTest()
        {
            Random random = new Random();
            ROT rot;
            rot = new ROT("=");
            rot = new ROT("<");
            rot = new ROT(">");
            rot = new ROT("<=");
            rot = new ROT(">=");
            rot = new ROT("!=");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    rot = new ROT(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("ROT Test failed using string: " + temp);
                }
            }
        }

        private static void vttTest()
        {
            Random random = new Random();
            VTT vtt;
            vtt = new VTT("bool");
            vtt = new VTT("int");
            vtt = new VTT("real");
            vtt = new VTT("string");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    vtt = new VTT(temp);
                    caught = true;
                }
                catch { }
                if (caught)
                {
                    throw new Exception("VTT Test failed using string: " + temp);
                }
            }
        }

    }
}
