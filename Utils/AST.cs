using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    class AST
    {
        public AST left, right;
        public Token token;
        public List<AST> children;

        public AST(AST left, AST right, Token token){
            this.left = left;
            this.right = right;
            this.token = token;
        }

    }

    class stmtsNode : AST{
        public stmtsNode(List<AST> children)
            :base(null,null,null)
        {
            this.children = children;
        }
    }

    class numNode : AST{
        public numNode(Token number) 
        : base(null,null,number){}
    }

    class opNode : AST{
        public opNode(AST left, AST right, Token op) 
        : base(left,right,op){}
    }

    class unaryOpNode : AST{
        public unaryOpNode(AST left, Token op)
        : base(left, null, op){}
    }

    class assignNode : AST{
        public assignNode(AST left, AST right, Token op)
         : base(left,right,op){}
    }

    class printNode : AST
    {
        public printNode(AST expr)
            : base(expr, null, null){}
    }

    class readNode : AST
    {
        public readNode(AST var)
            : base(var, null, null) {}
    }

    class strNode : AST
    {
        public strNode(Token str)
            : base(null, null, str) {}
    }

    class varNode : AST{
        public varNode(Token var)
        : base(null, null, var) { }
    }
}
