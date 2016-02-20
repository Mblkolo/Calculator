using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mblkolo.Calc
{
    interface IOperation<T> where T : ICommand
    {
        char Operator { get; }

        bool IsUnary { get; }

        bool IsLeft { get; }

        byte Priority { get; }
    }
}
