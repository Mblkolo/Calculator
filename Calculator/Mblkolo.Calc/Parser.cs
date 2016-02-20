using Mblkolo.Calc.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mblkolo.Calc
{
    /// <summary>
    /// Преобразует выражение из инфиксной формы в обратную польскую
    /// </summary>
    class Parser
    {
        public Stack<ICommand> Parse(string p)
        {
            var commands = new Stack<ICommand>();

            for(int i=0; i< p.Length; ++i)
            {
                if (p[i] == ' ' || p[i] == '\t')
                    continue;

                if (p[i] >= '0' && p[i] <= '9')
                {
                    decimal value = parseValue(ref i, p);
                    commands.Push(new PushCommand(value));
                    continue;
                }

                throw new ArgumentException("Неизвестный символ в позиции " + i);
            }

            return commands;
        }

        private decimal parseValue(ref int i, string p)
        {
            int inPos = i;
            for (; i < p.Length && ((p[i] >= '0' && p[i] <= '9') || p[i] == '.' ) ; ++i)
                ;

            string textValue = p.Substring(inPos, i - inPos);
            decimal value;
            if (!decimal.TryParse(textValue, out value))
                throw new ArgumentException("Не удаётся преобразовать в число " + textValue);

            return value;
        }
    }
}
