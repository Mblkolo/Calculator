using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mblkolo.Calc.Commands
{
    class PushCommand : ICommand
    {
        public readonly decimal Value;

        public PushCommand(decimal value)
        {
            Value = value;
        }

        public void Execute(Stack<decimal> state)
        {
            state.Push(Value);
        }
    }
}
