﻿using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Commands
{
    internal class MinusCommand : ICommand
    {
        //Вычитание производится в порядке добавления чисел в стек
        public void Execute(Stack<decimal> state)
        {
            if (state.Count < 2)
                throw new CalculationException("Невозможно вычесть 2 числа");

            var a = state.Pop();
            var b = state.Pop();
            var res = b - a;
            state.Push(res);
        }
    }
}
