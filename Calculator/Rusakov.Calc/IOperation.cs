using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rusakov.Calc
{
    internal interface IOperation
    {
        char Operator { get; }

        bool IsUnary { get; }

        bool IsLeft { get; }

        byte Priority { get; }

        /// <summary>
        /// Да, работает как фабрика
        /// </summary>
        /// <returns>Команада, выполняющая данный оператор</returns>
        ICommand GetCommand();
    }
}
