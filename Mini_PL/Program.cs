using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Utils.Source;
using Mini_PL.Lexical_Analysis;

namespace Mini_PL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FileSource source = new FileSource("Example_Programs/test.mpl");
                Scanner scanner = new Scanner(source);
                Parser parser = new Parser(scanner);
                Interpreter interpreter = new Interpreter(parser);
                interpreter.run();
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.ReadKey();
                return;
            }
            
        }
    }
}
