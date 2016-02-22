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

        //Реализация алгоритма сортировочной станции
        //https://ru.wikipedia.org/wiki/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D1%81%D0%BE%D1%80%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%BE%D1%87%D0%BD%D0%BE%D0%B9_%D1%81%D1%82%D0%B0%D0%BD%D1%86%D0%B8%D0%B8
        public ICommand[] Compile(Lexeme[] lexemes)
        {
            var commands = new List<ICommand>();
            var lexStack = new Stack<Lexeme>();

            foreach(var lex in lexemes)
            {
                if(lex.Type == LexemeType.Number)
                {
                    ProcessNumberLexem(lex, commands, lexStack);
                    continue;
                }
                if(lex.Type == LexemeType.OpenBracket)
                {
                    ProcessOpenBracketLexem(lex, commands, lexStack);
                    continue;
                }
                if(lex.Type == LexemeType.CloseBracket)
                {
                    ProcessCloseBracketLexem(lex, commands, lexStack);
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

        //Если токен — число, то добавить его в очередь вывода.
        protected void ProcessNumberLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
        {
            if(lexeme.Type != LexemeType.Number)
                throw new ArgumentException("lexeme");

            decimal number;
            if (!decimal.TryParse(lexeme.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                throw new ArgumentException("Не удалось преобразовать в число " + lexeme.Value);

            commands.Add(new PushCommand(number));
        }

        //Если токен — открывающая скобка, то положить его в стек.
        protected void ProcessOpenBracketLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
        {
            if (lexeme.Type != LexemeType.OpenBracket)
                throw new ArgumentException("lexeme");

            lexemeStack.Push(lexeme);
        }

        //Если токен — закрывающая скобка:
        //Пока токен на вершине стека не является открывающей скобкой, перекладывать операторы из стека в выходную очередь.
        //Выкинуть открывающую скобку из стека, но не добавлять в очередь вывода.
        //Если стек закончился до того, как был встречен токен открывающая скобка, то в выражении пропущена скобка.
        protected void ProcessCloseBracketLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
        {
            if (lexeme.Type != LexemeType.CloseBracket)
                throw new ArgumentException("lexeme");

            while (lexemeStack.Count > 0 && lexemeStack.Peek().Type != LexemeType.OpenBracket)
            {
                //Перекидываем операции в результат
                var l = lexemeStack.Pop();
                if (l.Type == LexemeType.Operator)
                    commands.Add(GetOperation(l.Value).GetCommand());
            }

            if (lexemeStack.Count == 0 || lexemeStack.Peek().Type != LexemeType.OpenBracket)
                throw new ArgumentException("Обнаружена непарная закрывающая скобка");

            lexemeStack.Pop();
        }

        //Если токен — оператор op1, то:
        //Пока присутствует на вершине стека токен оператор op2, и
        //Либо оператор op1 лево-ассоциативен и его приоритет меньше чем у оператора op2 либо равен,
        //или оператор op1 право-ассоциативен и его приоритет меньше чем у op2,
        //переложить op2 из стека в выходную очередь;
        //положить op1 в стек.
        protected void ProcessOperatorLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
        {

        }

        //Если больше не осталось токенов на входе:
        //Пока есть токены операторы в стеке:
        //Если токен оператор на вершине стека — скобка, то в выражении присутствует незакрытая скобка.
        //Переложить оператор из стека в выходную очередь.
        protected void ProcessRemainingLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
        {

        }
    }
}
