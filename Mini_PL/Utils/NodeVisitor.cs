using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Mini_PL.Utils
{
    public class NodeVisitor
    {
        public NodeVisitor(){}

        public object visit(AST node){
            if(node != null)
            {
                MethodInfo mi = this.GetType().GetMethod("visit_" + node.GetType().Name);
                if (mi == null)
                {
                    return null;
                }
                return mi.Invoke(this, new object[] { node });
            }
            return null;
        }
    }
}
