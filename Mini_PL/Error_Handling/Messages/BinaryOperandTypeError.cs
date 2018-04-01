using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils;

namespace Mini_PL.Error_Handling.Messages
{
    class BinaryOperandTypeError : IErrorMessage
    {
        AST left;
        AST right;
        AST op;

        public BinaryOperandTypeError(AST left, AST right, AST op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }
        public void printMessage()
        {
            Console.WriteLine("Error on line "+this.op.token.line+": Operator '" + op.token.getLexeme() + "' is not supported for types " + left.builtinType + " and " + right.builtinType + ".");
        }
    }
}
