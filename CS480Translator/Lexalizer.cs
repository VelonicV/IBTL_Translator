using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS480Translator
{
    class Lexalizer
    {
        //Maximum buffer size.
        private const int BUFFER_SIZE = 1024;

        //File stream representing the source code.
        private FileStream file;

        //The buffer object, and its associated pointers.
        private byte[] buffer;
        private int buff_pos;
        private int buff_end;

        //Line number
        private int line;

        //Attempts to open the file at the given path and initializes the fields.
        public Lexalizer(string filePath)
        {
            try
            {
                file = File.Open(filePath, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                Console.WriteLine("Error: could not open given file in read-only mode.");
                Console.ReadLine();
                Environment.Exit(1);
            }

            buffer = new byte[BUFFER_SIZE];
            buff_pos = 0;
            refillBuffer();

            line = 1;

        }

        //Returns the next token in the file, or null if none are left.
        public Tokens.GenericToken getNextToken()
        {
            while (more())
            {
                if (Char.IsDigit(peek()) || (peek() == '.'))
                {
                    return createNumToken();
                }
                else if (Char.IsLetter(peek()) || (peek() == '_'))
                {
                    return createLetterToken();
                }
                else if (peek() == '"')
                {
                    next();
                    return createStringToken();
                }
                else if ((peek() == '+') || (peek() == '-'))
                {
                    return new Tokens.CSOP(Char.ToString(next()));
                }
                else if ((peek() == '*') || (peek() == '/') || (peek() == '%') || (peek() == '^'))
                {
                    return new Tokens.MOT(Char.ToString(next()));
                }
                else if((peek() == '=') || (peek() == '!') || (peek() == '>') || (peek() == '<')) 
                {
                    return createRelOpToken();
                }
                else if ((peek() == '(') || (peek() == ')') || (peek() == ':'))
                {
                    return createSymKeyToken();
                }
                else if(peek() == '\n')
                {
                    next();
                    line++;
                }
                else if((peek() == ' ') || (peek() == '\t') || (peek() == '\r'))
                {
                    next();
                }
                else
                {
                    Console.WriteLine("Error: Invalid syntax starting with {0} declared on line {1}.", peek(), line);
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }

            return null;
        }

        //Letter-based token parser.
        private Tokens.GenericToken createLetterToken()
        {

            string[] variableTypes = { "int", "real", "bool", "string" };
            string[] booleanOperators = { "and", "or", "not" };
            string[] realMathOperators = { "sin", "cos", "tan" };
            string[] letterKeywords = { "if", "while", "let", "stdout" };
            string[] booleanConstants = { "true", "false" };

            StringBuilder sb = new StringBuilder();

            while ((Char.IsLetter(peek()) || Char.IsNumber(peek()) || (peek() == '_')) && more())
            {
                sb.Append(next());
            }

            string final = sb.ToString();
            if (variableTypes.Contains(final))
            {
                return new Tokens.VTT(final);
            }
            else if (booleanOperators.Contains(final))
            {
                return new Tokens.BOT(final);
            }
            else if (realMathOperators.Contains(final))
            {
                return new Tokens.RMOT(final);
            }
            else if (letterKeywords.Contains(final))
            {
                return new Tokens.KT(final);
            }
            else if (booleanConstants.Contains(final))
            {
                return new Tokens.BCT(final);
            }
            else
            {
                return new Tokens.IT(final);
            }

        }

        //Symbol keyword parser function.
        private Tokens.KT createSymKeyToken()
        {
            StringBuilder sb = new StringBuilder();
            if ((peek() == '(') || (peek() == ')'))
            {
                sb.Append(next());
                return new Tokens.KT(sb.ToString());
            }
            else
            {
                sb.Append(next());
                if ((peek() == '=') && more())
                {
                    sb.Append(next());
                    return new Tokens.KT(sb.ToString());
                }
                else
                {
                    Console.WriteLine("Error: Invalid syntax starting with : declared on line {0}.", line);
                    Console.ReadLine();
                    Environment.Exit(1);
                    return null;
                }
            }
        }

        //Relational operator parser function.
        private Tokens.ROT createRelOpToken()
        {
            StringBuilder sb = new StringBuilder();

            if (peek() == '=')
            {
                sb.Append(next());
                return new Tokens.ROT(sb.ToString());
            }
            else if ((peek() == '<') || (peek() == '>'))
            {
                sb.Append(next());
                if ((peek() == '=') && more())
                {
                    sb.Append(next());
                }

                return new Tokens.ROT(sb.ToString());
            }
            else
            {
                sb.Append(next());
                if (peek() == '=')
                {
                    sb.Append(next());
                    return new Tokens.ROT(sb.ToString());
                }
                else
                {
                    Console.WriteLine("Error: Invalid syntax starting with ! declared on line {0}.", line);
                    Console.ReadLine();
                    Environment.Exit(1);
                    return null;
                }
            }

        }

        //String token parser function.
        private Tokens.SCT createStringToken()
        {
            StringBuilder sb = new StringBuilder();

            while ((peek() != '"') && (peek() != '\n') && more())
            {
                sb.Append(next());
            }

            if ((peek() == '"') && more())
            {
                next();
                return new Tokens.SCT(sb.ToString());
            }
            else
            {
                Console.WriteLine("Error: End of line reached without finding second pair of quotation marks on line {0}.", line);
                Console.ReadLine();
                Environment.Exit(1);
                return null;
            }

        }

        //Number token parser function.
        private Tokens.GenericToken createNumToken()
        {
            StringBuilder sb = new StringBuilder();

            //Read in all starting integers of the number.
            while (Char.IsDigit(peek()) && more())
            {
                sb.Append(next());
            }

            //Decimal found.
            if ((peek() == '.') && more())
            {
                //Append decimal and the digits following it.
                sb.Append(next());
                while (Char.IsDigit(peek()) && more())
                {
                    sb.Append(next());
                }

                //if we find scientific notation, call the appending function.
                if(((peek() == 'e') || (peek() == 'E')) && more())
                {
                    scientific(sb);
                }

                //Return the real constant token.
                return new Tokens.RCT(sb.ToString());
            }
            //Scientific notation found, call the appending function.
            else if (((peek() == 'e') || (peek() == 'E')) && more())
            {
                scientific(sb);
                return new Tokens.RCT(sb.ToString());
            }
            //Nothing but digits found, return it as an integer token.
            else
            {
                return new Tokens.ICT(sb.ToString());
            }

        }

        //Returns the next character in the file stream.
        private char peek()
        {
            return (char) buffer[buff_pos];
        }

        //Returns the next character in the file stream and increments the position.
        //Refills buffer if necessary.
        private char next()
        {
            char next_char = (char) buffer[buff_pos];
            buff_pos++;
            if (buff_pos > buff_end)
            {
                refillBuffer();
            }

            return next_char;
        }

        //Resets the buffer position and refills it.
        private void refillBuffer()
        {
            buff_pos = 0;
            buff_end = file.Read(buffer, 0, BUFFER_SIZE) - 1;
        }

        //True if there's more to read in the buffer.
        private bool more()
        {
            return (buff_end != -1);
        }

        //Appends the digits and sign following the 'e' in a real number.
        private void scientific(StringBuilder sb)
        {
            sb.Append(next());
            if (((peek() == '-') || (peek() == '+')) && more())
            {
                sb.Append(next());
            }
            if (Char.IsDigit(peek()) && more())
            {
                while (Char.IsDigit(peek()) && more())
                {
                    sb.Append(next());
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid real constant declared on line {0}.", line);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

    }
}
