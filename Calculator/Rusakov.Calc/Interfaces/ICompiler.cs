using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Interfaces
{
    /// <summary>
    /// Преобразует набор лексем в набора команд
    /// </summary>
    internal interface ICompiler
    {
        ICommand[] Process(Lexeme[] lexemes);
    }
}
