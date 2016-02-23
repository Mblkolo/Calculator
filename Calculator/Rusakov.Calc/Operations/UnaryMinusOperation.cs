using Rusakov.Calc.Commands;
using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Operations
{
    class UnaryMinusOperation : IOperation
    {
        public char Operator
        {
            get { return '-'; }
        }

        public bool IsUnary
        {
            get { return true; }
        }

        public bool IsLeft
        {
            get { return false; }
        }

        public byte Priority
        {
            get { return 16 - 2; }
        }

        public ICommand GetCommand()
        {
            return new UnaryMinusCommand();
        }
    }
}
