using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils;
using Mini_PL.Lexical_Analysis;
using Mini_PL.Error_Handling;

namespace Mini_PL.Interpreting
{
    class Interpreter : NodeVisitor
    {
        
        public Dictionary<string, object> memory;

        //public ErrorHook hook { get; set; }

        public Interpreter()
        {           
            this.memory = new Dictionary<string, object>();
            //this.hook = new ErrorHook();
        }
        
        public object visit_varNode(AST node)
        {
            string name = node.token.getLexeme();
            object value = this.memory[name];
            return value;
        }

        
        public object  visit_numNode(AST node){
            return Int32.Parse(node.token.getLexeme());
        }

        public object visit_strNode(AST node)
        {
            return node.token.getLexeme();
        }

        public object visit_boolNode(AST node)
        {
            if(node.token.getLexeme() == "true")
            {
                return true;
            }
            return false;
        }

        public void visit_forNode(AST node)
        {
            AST var = node.children[0];
            AST range = node.children[1];
            AST stmts = node.children[2];
            for(int i = (int)visit(range.left); i <= (int)visit(range.right); i++)
            {
                this.memory[var.token.getLexeme()] = i;
                this.visit(stmts);
            }
        }

        public void visit_printNode(AST node)
        {
            Console.WriteLine(this.visit(node.left).ToString());
        }

        public void visit_readNode(AST node)
        {
            string input = Console.ReadLine();
            int n;
            bool isNumber = int.TryParse(input, out n);
            if (isNumber)
            {
                this.memory[node.left.token.getLexeme()] = n;
            }
            else
            {
                this.memory[node.left.token.getLexeme()] = input;
            }
            
        }

        public void visit_assertNode(AST node)
        {
            Console.WriteLine((bool)this.visit(node.left));
        }

        public void visit_stmtsNode(AST node)
        {
            foreach(AST n in node.children)
            {
                this.visit(n);
            }
        }

        public void visit_vardeclNode(AST node)
        {
            string name = node.left.token.getLexeme();
            if (node.right != null)
            {
                memory[name] = this.visit(node.right);
            }
            else
            {
                Utils.Type bType = node.builtinType;
                if(bType == Utils.Type.INTEGER)
                {
                    memory[name] = 0;
                }else if(bType == Utils.Type.STRING)
                {
                    memory[name] = "";
                }else if(bType == Utils.Type.BOOLEAN)
                {
                    memory[name] = false;
                }
                
            }
        }

        public void visit_assignNode(AST node)
        {
            string name = node.left.token.getLexeme();
            memory[name] = this.visit(node.right);
        }

        public object visit_unaryOpNode(AST node){
            TokenType type = node.token.getType();
            if(type == TokenType.PLUS){
                return +(int)this.visit(node.left);
            }else if(type == TokenType.MINUS){
                return -(int)this.visit(node.left);
            }else if(type == TokenType.NOT)
            {
                return !(bool)this.visit(node.left);
            }
            return null;
        }

        public object visit_opNode(AST node){
            TokenType type = node.token.getType();
            if (type == TokenType.PLUS) {
                if (node.builtinType == Utils.Type.INTEGER)
                {
                    return (int)this.visit(node.left) + (int)this.visit(node.right);
                } else if (node.builtinType == Utils.Type.STRING)
                {
                    return (string)this.visit(node.left) + (string)this.visit(node.right);
                }
            } else if (type == TokenType.MINUS) {
                return (int)this.visit(node.left) + (int)this.visit(node.right);
            } else if (type == TokenType.MULT) {
                return (int)this.visit(node.left) * (int)this.visit(node.right);
            } else if (type == TokenType.DIV) {
                int left = (int)this.visit(node.left);
                int right = (int)this.visit(node.right);
                if (right == 0)
                {
                    throw new DivideByZeroException();
                }
                return (int)this.visit(node.left) / (int)this.visit(node.right);
            } else if (type == TokenType.EQUALS)
            {
                object left = this.visit(node.left);
                object right = this.visit(node.right);
                if(node.left.builtinType == Utils.Type.BOOLEAN)
                {
                    return (bool)left == (bool)right;
                }else if(node.left.builtinType == Utils.Type.INTEGER)
                {
                    return (int)left == (int)right;
                }else if(node.left.builtinType == Utils.Type.STRING)
                {
                    return (string)left == (string)right;
                }

            } else if (type == TokenType.LESSTHAN)
            {
                if (node.left.builtinType == Utils.Type.BOOLEAN)
                {
                    int boolValueLeft = boolToInt((bool)this.visit(node.left));
                    int boolValueRight = boolToInt((bool)this.visit(node.right));
                    return boolValueLeft < boolValueRight;
                }
                else if (node.left.builtinType == Utils.Type.INTEGER)
                {
                    return ((int)this.visit(node.left) < (int)this.visit(node.right));
                }else if (node.left.builtinType == Utils.Type.STRING)
                {
                    if (String.Compare((string)this.visit(node.left), (string)this.visit(node.right)) < 0)
                    {
                        return true;
                    }
                    return false;
                }
                
            } else if (type == TokenType.AND)
            {
                return ((bool)this.visit(node.left) && (bool)(this.visit(node.right)));
            }
            return null;
        }

        public int boolToInt(bool boolean)
        {
            //C-style 
            if (boolean)
            {
                return 1;
            }
            return 0;
        }

        public object run(AST program){
            return this.visit(program);
        }
    }
}
