using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Lexical_Analysis;

namespace Mini_PL.Error_Handling.Messages
{
    class UndeclaredVariableError : IErrorMessage
    {
        private Token var;
        public UndeclaredVariableError(Token var)
        {
            this.var = var;
        }

        public void printMessage()
        {
            Console.WriteLine(var.rowAndCol()+" Error: Variable '" + this.var.getLexeme()+ "' is undeclared.");
        }
    }
}
