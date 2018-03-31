using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Lexical_Analysis;

namespace Mini_PL.Error_Handling.Messages
{
    class DuplicateDeclarationError : IErrorMessage
    {
        private Token token;
        public DuplicateDeclarationError(Token token)
        {
            this.token = token;
        }
        public void printMessage()
        {
            Console.WriteLine(token.rowAndCol()+" Error: " + "Duplicate identifier '" + token.getLexeme() + "' found.");
        }
    }
}
