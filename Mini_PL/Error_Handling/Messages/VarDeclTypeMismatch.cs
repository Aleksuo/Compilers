using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Lexical_Analysis;
using Mini_PL.Utils;

namespace Mini_PL.Error_Handling.Messages
{
    class VarDeclTypeMismatch : IErrorMessage
    {
        AST var;
        AST expr;

        public VarDeclTypeMismatch(AST var, AST expr)
        {
            this.var = var;
            this.expr = expr;
        }

        public void printMessage()
        {
            Console.WriteLine("Error on line " + var.token.row + ": Cannot assign " + expr.builtinType + " to variable of type " + var.builtinType + ".");
        }
    }
}
