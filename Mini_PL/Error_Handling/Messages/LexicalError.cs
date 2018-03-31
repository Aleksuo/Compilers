using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Lexical_Analysis;

namespace Mini_PL.Error_Handling.Messages
{
    class LexicalError : IErrorMessage
    {
        private Token token;

        public LexicalError(Token token)
        {
            this.token = token;
        }
        public void printMessage()
        {
            Console.WriteLine(token.rowAndCol() + " Error: Unrecognized token '" + token.getLexeme() + "'.");
        }
    }
}
