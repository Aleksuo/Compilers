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
        
        public AST(AST left, AST right, Token token){
            this.left = left;
            this.right = right;
            this.token = token;
        }

    }

    class stmtsNode : AST{
        public List<AST> children;
        public stmtsNode(List<AST> children)
        {this.children = children;
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

    class varNode : AST{
        public varNode(Token var) 
        : base(null,null,var);
    }
}
