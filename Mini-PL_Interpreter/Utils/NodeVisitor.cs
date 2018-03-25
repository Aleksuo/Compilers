using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Mini_PL
{
    public class NodeVisitor
    {
        public NodeVisitor(){}

        public object visit(AST node){
            MethodInfo mi = this.GetType().GetMethod("visit_"+node.GetType().Name);
            return mi.Invoke(this,new object[]{node});
        }
    }
}
