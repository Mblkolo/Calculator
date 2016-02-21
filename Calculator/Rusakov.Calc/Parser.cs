using Rusakov.Calc.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    /// <summary>
    /// Преобразует выражение из инфиксной формы в обратную польскую
    /// </summary>
    class Parser
    {
        public readonly Dictionary<char, IOperation> Operations;

        public Parser(Dictionary<char, IOperation> operations)
        {
            Operations = operations;
        }

        public List<ICommand> Parse(string p)
        {
            var commands = new List<ICommand>();
            var opStack = new Stack<IOperation>();

            for (int i = 0; i < p.Length; ++i)
            {
                char c = p[i];

                if (c == ' ' || c == '\t')
                    continue;

                if (c >= '0' && c <= '9')
                {
                    decimal value = parseValue(ref i, p);
                    commands.Add(new PushCommand(value));
                    continue;
                }

                IOperation op;
                if(Operations.TryGetValue(c, out op))
                {
                    while (opStack.Count > 0 && CompareOperation(op, opStack.Peek() ))
                    {
                        commands.Add(opStack.Pop().GetCommand());
                    }
                    opStack.Push(op);
                    continue;
                }



                throw new ArgumentException("Неизвестный символ в позиции " + i);
            }

            while (opStack.Count > 0)
                commands.Add(opStack.Pop().GetCommand());

            return commands;
        }

        protected decimal parseValue(ref int i, string p)
        {
            int inPos = i;
            for (; i < p.Length && ((p[i] >= '0' && p[i] <= '9') || p[i] == '.'); ++i)
                ;

            string textValue = p.Substring(inPos, i - inPos);
            decimal value;
            if (!decimal.TryParse(textValue, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                throw new ArgumentException("Не удаётся преобразовать в число " + textValue);

            return value;
        }

        protected bool CompareOperation(IOperation operation, IOperation topStackOperation)
        {
            if (operation.IsLeft && operation.Priority <= topStackOperation.Priority)
                return true;

            if (!operation.IsLeft && operation.Priority < topStackOperation.Priority)
                return true;

            return false;
        }
    }
}
