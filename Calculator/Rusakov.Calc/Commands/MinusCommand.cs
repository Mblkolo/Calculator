using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Commands
{
    class MinusCommand : ICommand
    {
        public void Execute(Stack<decimal> state)
        {
            if (state.Count < 2)
                throw new ArgumentException("Невозможно вычесть 2 числа. Чисел всего " + state.Count);

            var a = state.Pop();
            var b = state.Pop();
            var res = b - a;
            state.Push(res);
        }
    }
}
