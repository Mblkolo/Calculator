using Rusakov.Calc.Interfaces;
using Rusakov.Calc.Operations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
                if (expression == "exit")
                    return;

                try
                {
                    decimal result = calc.Calculate(expression);
                    Console.WriteLine("Результат: " + result.ToString(CultureInfo.InvariantCulture));
                }
                catch(CalculationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch(Exception e)
                {
                    LogExeption(e);
                    Console.WriteLine("Невозможно вычислить выражение");
                }

                Console.WriteLine();
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

        private static void LogExeption(Exception exeption)
        {
            try
            {
                //Сохраняем одну последнюю ошибку
                File.WriteAllText(".\\log.txt", exeption.ToString());
            }
            catch(Exception e)
            {
                //Проглатывае исключение на случай если у нас нет прав на запись
            }
        }
    }
}
