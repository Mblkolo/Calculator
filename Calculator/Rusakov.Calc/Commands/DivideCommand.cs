using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Commands
{
    internal class DivideCommand : ICommand
    {
        public void Execute(Stack<decimal> state)
        {
            if (state.Count < 2)
                throw new CalculationException("Невозможно разделить 2 числа");

            var a = state.Pop();
            var b = state.Pop();

            try
            {
                var res = b / a;
                state.Push(res);
                return;
            }
            catch(DivideByZeroException)
            {
                throw new CalculationException("Обнаружено деление ноль");
            }
        }
    }
}
