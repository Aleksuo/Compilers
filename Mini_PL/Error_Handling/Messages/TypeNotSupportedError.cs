using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils;

namespace Mini_PL.Error_Handling.Messages
{
    class TypeNotSupportedError : IErrorMessage
    {
        AST node;
        AST expr;

        public TypeNotSupportedError(AST node, AST expr)
        {
            this.node = node;
            this.expr = expr;
        }

        public void printMessage()
        {
            Console.WriteLine("Error on line "+expr.token.row+": Type " + expr.builtinType + " is not suported for '" + node.token.getLexeme() + "'.");
        }
    }
}
