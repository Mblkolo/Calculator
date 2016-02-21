using Rusakov.Calc.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Compiler
    {
        public ICommand[] Compile(Lexeme[] lexemes)
        {
            var commands = new List<ICommand>();
            var lexStack = new Stack<Lexeme>();

            foreach(var lex in lexemes)
            {
                if(lex.Type == LexemeType.Number)
                {
                    decimal number;
                    if(!decimal.TryParse(lex.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                        throw new ArgumentException(String.Format("Не удалось преобразовать в число '{0}'", lex.Value));

                    commands.Add(new PushCommand(number));
                    continue;
                }
                if(lex.Type == LexemeType.OpenBracket)
                {
                    lexStack.Push(lex);
                    continue;
                }
                if(lex.Type == LexemeType.CloseBracket)
                {
                    try
                    {
                        for (var l = lexStack.Pop(); l.Type != LexemeType.OpenBracket; l = lexStack.Pop() )
                        {
                            //Что делаем
                        }
                    }
                    catch(InvalidOperationException)
                    {
                        throw new ArgumentException("Обнаружена непарная закрывающая скобка");
                    }

                    continue;
                }

                if(lex.Type == LexemeType.Operator)
                {
                    //На самом деле нужно ещё доставать лишнее
                    lexStack.Push(lex);
                    continue;
                }
            }

            while(lexStack.Count > 0)
            {
                var lex = lexStack.Pop();
                if(lex.Type == LexemeType.OpenBracket)
                    throw new ArgumentException("Обнаружена непарная открывающая скобка");

                //Перекидываем операции в результат
            }

            return commands.ToArray();
        }
    }
}
