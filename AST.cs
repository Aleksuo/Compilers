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

    class numNode : AST{
        public numNode(Token number) 
        : base(null,null,number){}
    }

    class opNode : AST{
        public opNode(AST left, AST right, Token op) 
        : base(left,right,op){}
    }
}
