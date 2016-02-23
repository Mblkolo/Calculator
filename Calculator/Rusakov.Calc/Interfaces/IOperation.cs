using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rusakov.Calc.Interfaces
{
    /// <summary>
    /// Описание операции
    /// </summary>
    internal interface IOperation
    {
        char Operator { get; }

        bool IsUnary { get; }

        /// <summary>
        /// Левоассоциативность
        /// </summary>
        bool IsLeft { get; }

        /// <summary>
        /// Приоритет, чем выше, тем выше
        /// </summary>
        byte Priority { get; }

        /// <summary>
        /// Да, работает как фабрика
        /// </summary>
        /// <returns>Команада, выполняющая данный оператор</returns>
        ICommand GetCommand();
    }
}
