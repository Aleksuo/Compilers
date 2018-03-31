using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Lexical_Analysis;

namespace Mini_PL.Error_Handling.Messages
{
    class SyntaxError : IErrorMessage
    {
        private Token token;

        public SyntaxError(Token token)
        {
            this.token = token;
        }

        public void printMessage()
        {
            Console.WriteLine(token.rowAndCol() + " Syntax error: Unexpected symbol '" + token.getLexeme() + "'.");
        }
    }
}
