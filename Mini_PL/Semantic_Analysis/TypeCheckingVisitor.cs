using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils;
using Mini_PL.Error_Handling;
using Mini_PL.Error_Handling.Messages;
using Mini_PL.Lexical_Analysis;


namespace Mini_PL.Semantic_Analysis
{
    public class TypeCheckingVisitor : NodeVisitor, IHookable
    {
        SymbolTable table;
        public ErrorHook hook { get; set; }

        public TypeCheckingVisitor(SymbolTable table)
        {
            this.table = table;
            this.hook = new ErrorHook();
        }

        public void visit_stmtsNode(AST node)
        {
            foreach (AST c in node.children)
            {
                this.visit(c);
            }
        }

        public void visit_forNode(AST node)
        {
            foreach (AST c in node.children)
            {
                this.visit(c);
            }
            if(node.children[0].builtinType != Utils.Type.INTEGER)
            {
                this.ThrowErrorMessage(new TypeNotSupportedError(node, node.children[0]));
            }
        }

        public void visit_vardeclNode(AST node)
        {
            this.visit(node.left);
            if(node.right != null)
            {
                this.visit(node.right);
                Utils.Type left = node.left.builtinType;
                Utils.Type right = node.right.builtinType;
                if(left == Utils.Type.INTEGER && left == right)
                {
                    node.builtinType = Utils.Type.INTEGER;
                }else if( left == Utils.Type.BOOLEAN && left == right)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }else if(left == Utils.Type.STRING && left == right)
                {
                    node.builtinType = Utils.Type.STRING;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new VarDeclTypeMismatchError(node.left, node.right));
                }
            }
            else
            {
                node.builtinType = node.left.builtinType;
            }
            
        }

        public void visit_assignNode(AST node)
        {
            this.visit(node.left);
            this.visit(node.right);
            Utils.Type left = node.left.builtinType;
            Utils.Type right = node.right.builtinType;
            if (left == Utils.Type.INTEGER && left == right)
            {
                node.builtinType = Utils.Type.INTEGER;
            }
            else if (left == Utils.Type.BOOLEAN && left == right)
            {
                node.builtinType = Utils.Type.BOOLEAN;
            }
            else if (left == Utils.Type.STRING && left == right)
            {
                node.builtinType = Utils.Type.STRING;
            }
            else
            {
                node.builtinType = Utils.Type.ERROR;
                this.ThrowErrorMessage(new VarDeclTypeMismatchError(node.left, node.right));
            }
        }

        public void visit_rangeNode(AST node)
        {
            Utils.Type left = node.left.builtinType;
            Utils.Type right = node.right.builtinType;
            if (left == Utils.Type.INTEGER && left == right)
            {
                node.builtinType = Utils.Type.INTEGER;
            }
            else
            {
                node.builtinType = Utils.Type.ERROR;
                this.ThrowErrorMessage(new VarDeclTypeMismatchError(node.left, node.right));
            }
        }

        public void visit_printNode(AST node)
        {
            this.visit(node.left);
            Utils.Type expr = node.left.builtinType;
            if(expr == Utils.Type.INTEGER)
            {
                node.builtinType = Utils.Type.INTEGER;
            }else if(expr == Utils.Type.STRING)
            {
                node.builtinType = Utils.Type.STRING;
            }
            else
            {
                node.builtinType = Utils.Type.ERROR;
                this.ThrowErrorMessage(new TypeNotSupportedError(node,node.left));
            }
        }

        public void visit_readNode(AST node)
        {
            this.visit(node.left);
            Utils.Type expr = node.left.builtinType;
            if (expr == Utils.Type.INTEGER)
            {
                node.builtinType = Utils.Type.INTEGER;
            }
            else if (expr == Utils.Type.STRING)
            {
                node.builtinType = Utils.Type.STRING;
            }
            else
            {
                node.builtinType = Utils.Type.ERROR;
                this.ThrowErrorMessage(new TypeNotSupportedError(node, node.left));
            }
        }

        public void visit_assertNode(AST node)
        {
            this.visit(node.left);
            Utils.Type expr = node.left.builtinType;
            if (expr == Utils.Type.BOOLEAN)
            {
                node.builtinType = Utils.Type.BOOLEAN;
            }
            else
            {
                node.builtinType = Utils.Type.ERROR;
                this.ThrowErrorMessage(new TypeNotSupportedError(node, node.left));
            }
        }

        public void visit_varNode(AST node)
        {
            Symbol symbol = table.lookup(node.token.getLexeme());
            if(symbol != null)
            {
                node.builtinType = table.lookup(node.token.getLexeme()).type;
            }
            else
            {
                node.builtinType = Utils.Type.ERROR;
            }           
            
        }

        public void visit_numNode(AST node)
        {
            node.builtinType = Utils.Type.INTEGER;
        }

        public void visit_boolNode(AST node)
        {
            node.builtinType = Utils.Type.BOOLEAN;
        }

        public void visit_strNode(AST node)
        {
            node.builtinType = Utils.Type.STRING;
        }
        public void visit_opNode(AST node)
        {
            this.visit(node.left);
            this.visit(node.right);
            TokenType op = node.token.getType();
            Utils.Type left = node.left.builtinType;
            Utils.Type right = node.right.builtinType;
            if(op == TokenType.PLUS)
            {
                if(left == Utils.Type.INTEGER && left == right)
                {
                    node.builtinType = Utils.Type.INTEGER;
                }else if(left == Utils.Type.STRING && left == right)
                {
                    node.builtinType = Utils.Type.STRING;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new BinaryOperandTypeError(node.left, node.right, node));
                }
            }else if(op == TokenType.MINUS)
            {
                if (left == Utils.Type.INTEGER && left == right)
                {
                    node.builtinType = Utils.Type.INTEGER;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new BinaryOperandTypeError(node.left, node.right, node));
                }
            }else if(op == TokenType.DIV)
            {
                if (left == Utils.Type.INTEGER && left == right)
                {
                    node.builtinType = Utils.Type.INTEGER;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new BinaryOperandTypeError(node.left, node.right, node));
                }
            }else if(op == TokenType.MULT)
            {
                if (left == Utils.Type.INTEGER && left == right)
                {
                    node.builtinType = Utils.Type.INTEGER;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new BinaryOperandTypeError(node.left, node.right, node));
                }
            }else if(op == TokenType.LESSTHAN)
            {
                if (left == Utils.Type.INTEGER && left == right)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }else if(left == Utils.Type.STRING && left == right)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }else if(left == Utils.Type.BOOLEAN)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new BinaryOperandTypeError(node.left, node.right, node));
                }
            }else if(op == TokenType.EQUALS)
            {
                if (left == Utils.Type.INTEGER && left == right)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }
                else if (left == Utils.Type.STRING && left == right)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }
                else if (left == Utils.Type.BOOLEAN)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new BinaryOperandTypeError(node.left, node.right, node));
                }
            }else if(op == TokenType.AND)
            {
                if(left == Utils.Type.BOOLEAN && left == right)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new BinaryOperandTypeError(node.left, node.right, node));
                }
            }
        }

        public void visit_unaryOpNode(AST node)
        {
            this.visit(node.left);
            TokenType op = node.token.getType();
            Utils.Type left = node.left.builtinType;
            if(op == TokenType.NOT)
            {
                if(left == Utils.Type.BOOLEAN)
                {
                    node.builtinType = Utils.Type.BOOLEAN;
                }
                else
                {
                    node.builtinType = Utils.Type.ERROR;
                    this.ThrowErrorMessage(new UnaryOperandTypeError(node.left, node));
                }
            }
        }

        public void visit_errorNode(AST node)
        {
            return;
        }

        public void check(AST program)
        {
            this.visit(program);
        }
    }
}
