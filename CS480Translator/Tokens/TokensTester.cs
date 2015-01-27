using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator.Tokens
{
    class TokensTester
    {
        private const int LOOP = 100;
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
            BooleanConstantToken bct;
            bct = new BooleanConstantToken("true");
            bct = new BooleanConstantToken("false");
            for (int i = 0; i < LOOP; i++ )
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    bct = new BooleanConstantToken(temp);
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
            BooleanOperatorToken bot;
            bot = new BooleanOperatorToken("and");
            bot = new BooleanOperatorToken("or");
            bot = new BooleanOperatorToken("not");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    bot = new BooleanOperatorToken(temp);
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
            ContextSensitiveOperatorToken csot;
            csot = new ContextSensitiveOperatorToken("+");
            csot = new ContextSensitiveOperatorToken("-");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    csot = new ContextSensitiveOperatorToken(temp);
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
            IdToken it;
            it = new IdToken("_testest123");
            it = new IdToken("HELLOPEOPLE");
            it = new IdToken("TeStingThe_ID");
            it = new IdToken("Hai_123_fun_22_");

            string[] tests = { "888", "8TestingID", "$haisup" };
            foreach (string x in tests) { 
                bool caught = false;
                try
                {
                    it = new IdToken(x);
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
            KeywordToken kt;
            kt = new KeywordToken("(");
            kt = new KeywordToken(")");
            kt = new KeywordToken(":=");
            kt = new KeywordToken("if");
            kt = new KeywordToken("while");
            kt = new KeywordToken("let");
            kt = new KeywordToken("stdout");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    kt = new KeywordToken(temp);
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
            MathOperatorToken mot;
            mot = new MathOperatorToken("*");
            mot = new MathOperatorToken("/");
            mot = new MathOperatorToken("%");
            mot = new MathOperatorToken("^");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    mot = new MathOperatorToken(temp);
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
            RealMathOperatorToken rmot;
            rmot = new RealMathOperatorToken("sin");
            rmot = new RealMathOperatorToken("cos");
            rmot = new RealMathOperatorToken("tan");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    rmot = new RealMathOperatorToken(temp);
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
            RelationalOperatorToken rot;
            rot = new RelationalOperatorToken("=");
            rot = new RelationalOperatorToken("<");
            rot = new RelationalOperatorToken(">");
            rot = new RelationalOperatorToken("<=");
            rot = new RelationalOperatorToken(">=");
            rot = new RelationalOperatorToken("!=");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    rot = new RelationalOperatorToken(temp);
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
            VariableTypeToken vtt;
            vtt = new VariableTypeToken("bool");
            vtt = new VariableTypeToken("int");
            vtt = new VariableTypeToken("real");
            vtt = new VariableTypeToken("string");
            for (int i = 0; i < LOOP; i++)
            {
                string temp = "";
                bool caught = false;
                try
                {
                    temp = genRandString(MAX_RAND_STRING_LENGTH, random);
                    vtt = new VariableTypeToken(temp);
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
