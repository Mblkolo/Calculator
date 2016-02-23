using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Calculator
    {
        private readonly ILexer _lexer;
        private readonly ICompiler _compiler;

        public Calculator(ILexer lexer, ICompiler compiller)
        {
            _lexer = lexer;
            _compiler = compiller;
        }

        public decimal Calculate(string expression)
        {
            Lexeme[] lexem = _lexer.Parse(expression);
            if (lexem.Length == 0)
                throw new CalculationException("Пустое выражение");

            ICommand[] commands = _compiler.Process(lexem);
            
            var stack = new Stack<decimal>();
            foreach (var c in commands)
                c.Execute(stack);

            if(stack.Count == 0)
                throw new CalculationException("В выражении не хватает чисел");

            if (stack.Count > 1)
                throw new CalculationException("В выражении не хватает операторов");

            return stack.Pop();
        }
    }
}
