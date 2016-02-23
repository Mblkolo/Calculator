using Rusakov.Calc.Interfaces;
using Rusakov.Calc.Operations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calc = getCalculator();

            while(true)
            {
                Console.Write("Введите выражение: ");
                string expression = Console.ReadLine();
                
                decimal result = calc.Calculate(expression);

                Console.WriteLine("Результат: " + result.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static Calculator getCalculator()
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
            return new Calculator(lexer, compiler);
        }
    }
}
