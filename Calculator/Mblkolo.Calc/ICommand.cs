using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mblkolo.Calc
{
    interface ICommand
    {
        void Execute(Stack<decimal> state);
    }
}
