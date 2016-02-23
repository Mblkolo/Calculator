using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Interfaces
{
    /// <summary>
    /// Преоразует текст в последовательность лексем
    /// </summary>
    internal interface ILexer
    {
        Lexeme[] Parse(string input);
    }
}
