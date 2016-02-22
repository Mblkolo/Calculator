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
        private Dictionary<string, IOperation> operations;

        public Compiler(IOperation[] operations)
        {
            if (operations == null)
                throw new ArgumentNullException("operations");

            this.operations = operations.ToDictionary(x => new String(x.Operator, 1));

        }

        public Compiler()
            : this(new IOperation[0])
        {
        }


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
                        throw new ArgumentException("Не удалось преобразовать в число " + lex.Value);

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
                    while (lexStack.Count > 0 && lexStack.Peek().Type != LexemeType.OpenBracket)
                    {
                        //Перекидываем операции в результат
                        var l = lexStack.Pop();
                        if (l.Type == LexemeType.Operator)
                            commands.Add(GetOperation(l.Value).GetCommand());
                    }

                    if (lexStack.Count == 0 || lexStack.Peek().Type != LexemeType.OpenBracket)
                        throw new ArgumentException("Обнаружена непарная закрывающая скобка");

                    lexStack.Pop();

                    continue;
                }

                if(lex.Type == LexemeType.Operator)
                {
                    //Снимает часть лексем со стека
                    while(lexStack.Count > 0 && lexStack.Peek().Type == LexemeType.Operator)
                    {
                        string topStackOperation = lexStack.Peek().Value;
                        if (!CompareLexem(lex.Value, topStackOperation))
                            break;

                        lexStack.Pop();
                        commands.Add(GetOperation(topStackOperation).GetCommand());
                    }

                    lexStack.Push(lex);
                    continue;
                }

                throw new NotSupportedException("Неизвестный тип лексемы " + lex.Type);
            }

            while(lexStack.Count > 0)
            {
                var lex = lexStack.Pop();
                if(lex.Type == LexemeType.OpenBracket)
                    throw new ArgumentException("Обнаружена непарная открывающая скобка");

                //Перекидываем операции в результат
                if (lex.Type == LexemeType.Operator)
                    commands.Add(GetOperation(lex.Value).GetCommand());
            }

            return commands.ToArray();
        }

        //Либо оператор op лево-ассоциативен и его приоритет меньше чем у оператора topStackOp либо равен,
        //или оператор op право-ассоциативен и его приоритет меньше чем у topStackOp
        protected bool CompareLexem(string opName, string topStackOpName)
        {
            IOperation op = GetOperation(opName);
            IOperation topStackOp = GetOperation(topStackOpName);

            if (op.IsLeft && op.Priority <= topStackOp.Priority)
                return true;

            if (!op.IsLeft && op.Priority < topStackOp.Priority)
                return true;

            return false;
        }

        protected IOperation GetOperation(string operatorName)
        {
            IOperation op;
            if (!operations.TryGetValue(operatorName, out op))
                throw new ArgumentException("Неизвестная операция " + operatorName);

            return op;
        }
    }
}
