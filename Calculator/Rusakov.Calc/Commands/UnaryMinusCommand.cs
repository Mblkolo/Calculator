using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Commands
{
    internal class UnaryMinusCommand : ICommand
    {
        //Вычитание производится в порядке добавления чисел в стек
        public void Execute(Stack<decimal> state)
        {
            if (state.Count < 1)
                throw new CalculationException("Отсутсвует число в операции вида (-a)");

            var res = -state.Pop();
            state.Push(res);
        }
    }
}
