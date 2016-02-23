using Rusakov.Calc.Commands;
using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Operations
{
    class MultiplyOperation : IOperation
    {
        public char Operator
        {
            get { return '*'; }
        }

        public bool IsUnary
        {
            get { return false; }
        }

        public bool IsLeft
        {
            get { return true; }
        }

        public byte Priority
        {
            get { return 16 - 3; }
        }

        public ICommand GetCommand()
        {
            return new MultiplyCommand();
        }
    }
}
