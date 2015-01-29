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
                else if(peek() == '\n')
                {
                    next();
                    line++;
                }
                else
                {
                    next();
                }
            }

            return null;
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
                return new Tokens.RealConstantToken(sb.ToString());
            }
            //Scientific notation found, call the appending function.
            else if (((peek() == 'e') || (peek() == 'E')) && more())
            {
                scientific(sb);
                return new Tokens.RealConstantToken(sb.ToString());
            }
            //Nothing but digits found, return it as an integer token.
            else
            {
                return new Tokens.IntegerConstantToken(sb.ToString());
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
