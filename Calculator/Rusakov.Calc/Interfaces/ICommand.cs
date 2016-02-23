using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Interfaces
{
    /// <summary>
    /// Выполняет действие над стеком
    /// </summary>
    internal interface ICommand
    {
        void Execute(Stack<decimal> state);
    }
}
