using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils;

namespace Mini_PL.Error_Handling.Messages
{
    class UnaryOperandTypeError : IErrorMessage
    {
        AST expr;
        AST op;

        public UnaryOperandTypeError(AST expr, AST op)
        {
            this.expr = expr;
            this.op = op;
        }
        public void printMessage()
        {
            Console.WriteLine("Error on line "+ op.token.line + ": Type " + expr.builtinType + " is not supported for operator '" + op.token.getLexeme() + "'.");
        }
    }
}
