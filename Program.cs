using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = Console.ReadLine();
            Scanner scanner = new Scanner(input);
            Parser parser = new Parser(scanner);
            Interpreter interpreter = new Interpreter(parser);
            Console.WriteLine(interpreter.run());
            Console.ReadKey();
        }
    }
}
