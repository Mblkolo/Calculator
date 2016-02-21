using Rusakov.Calc.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer();
            var operations = new IOperation[] 
            {
                new PlusOperation(),
                new MinusOperation(),
                new MultiplyOperation(),
                new DivideOperation()
            };
            var compiler = new Compiler(operations);

            while(true)
            {
                string expression = Console.ReadLine();

                Lexeme[] lexem = lexer.Parse(expression);
                ICommand[] commands = compiler.Compile(lexem);
                var stack = new Stack<decimal>();
                foreach(var c in commands)
                    c.Execute(stack);

                Console.WriteLine(stack.Pop());
            }
        }
    }
}
