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
            Console.WriteLine("Error on line "+ op.token.row + ": Type " + expr.builtinType + " not suported for operator '" + op.token.getLexeme() + "'.");
        }
    }
}
