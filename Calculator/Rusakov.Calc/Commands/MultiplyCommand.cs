using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Commands
{
    internal class MultiplyCommand : ICommand
    {
        public void Execute(Stack<decimal> state)
        {
            if (state.Count < 2)
                throw new ArgumentException("Невозможно умножить 2 числа. Чисел всего " + state.Count);

            var res = state.Pop() * state.Pop();
            state.Push(res);
        }
    }
}
