﻿using Rusakov.Calc.Commands;
using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Compiler : ICompiler
    {
        private Dictionary<string, IOperation> _operations;
        private Dictionary<string, IOperation> _unaryOperations;

        public Compiler(IOperation[] operations)
        {
            if (operations == null)
                throw new ArgumentNullException("operations");

            this._operations = operations.Where(x => !x.IsUnary).ToDictionary(x => new String(x.Operator, 1));
            this._unaryOperations = operations.Where(x => x.IsUnary).ToDictionary(x => new String(x.Operator, 1));
        }

        public Compiler()
            : this(new IOperation[0])
        {
        }

        //Реализация алгоритма сортировочной станции
        //https://ru.wikipedia.org/wiki/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D1%81%D0%BE%D1%80%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%BE%D1%87%D0%BD%D0%BE%D0%B9_%D1%81%D1%82%D0%B0%D0%BD%D1%86%D0%B8%D0%B8
        public ICommand[] Process(Lexeme[] lexemes)
        {
            var commands = new List<ICommand>();
            var lexStack = new Stack<Lexeme>();

            foreach (var lex in lexemes)
            {
                switch (lex.Type)
                {
                    case LexemeType.Number:
                        ProcessNumberLexem(lex, commands, lexStack);
                        break;

                    case LexemeType.OpenBracket:
                        ProcessOpenBracketLexem(lex, commands, lexStack);
                        break;

                    case LexemeType.CloseBracket:
                        ProcessCloseBracketLexem(lex, commands, lexStack);
                        break;

                    case LexemeType.BinaryOperator:
                    case LexemeType.UnaryOperator:
                        ProcessOperatorLexem(lex, commands, lexStack);
                        break;

                    default:
                        throw new NotSupportedException("Неизвестный тип лексемы " + lex.Type);
                }
            }

            ProcessRemainingLexem(commands, lexStack);

            return commands.ToArray();
        }

        //Либо оператор op лево-ассоциативен и его приоритет меньше чем у оператора topStackOp либо равен,
        //или оператор op право-ассоциативен и его приоритет меньше чем у topStackOp
        protected bool CompareLexem(Lexeme opLexeme, Lexeme topStackLexeme)
        {
            IOperation op = GetOperation(opLexeme);
            IOperation topStackOp = GetOperation(topStackLexeme);

            if (op.IsLeft && op.Priority <= topStackOp.Priority)
                return true;

            if (!op.IsLeft && op.Priority < topStackOp.Priority)
                return true;

            return false;
        }

        protected IOperation GetOperation(Lexeme lexeme)
        {
            if (lexeme.Type == LexemeType.BinaryOperator)
            {
                IOperation op;
                if (!_operations.TryGetValue(lexeme.Value, out op))
                    throw new CalculationException("Неизвестная операция " + lexeme.Value);
                return op;
            }

            if (lexeme.Type == LexemeType.UnaryOperator)
            {
                IOperation op;
                if (!_unaryOperations.TryGetValue(lexeme.Value, out op))
                    throw new CalculationException("Неизвестная операция " + lexeme.Value);
                return op;
            }

            throw new ArgumentException("lexeme");
        }

        //Если токен — число, то добавить его в очередь вывода.
        protected void ProcessNumberLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
        {
            if (lexeme.Type != LexemeType.Number)
                throw new ArgumentException("lexeme");

            decimal number;
            if (!decimal.TryParse(lexeme.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                throw new CalculationException("Не удалось преобразовать в число " + lexeme.Value);

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
                if (IsOperation(l))
                    commands.Add(GetOperation(l).GetCommand());
            }

            if (lexemeStack.Count == 0 || lexemeStack.Peek().Type != LexemeType.OpenBracket)
                throw new CalculationException("Обнаружена непарная закрывающая скобка");

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
            if (!IsOperation(lexeme))
                throw new ArgumentException("lexeme");

            //Снимает часть лексем со стека
            while (lexemeStack.Count > 0 && IsOperation(lexemeStack.Peek()))
            {
                Lexeme topStackOperation = lexemeStack.Peek();
                if (!CompareLexem(lexeme, topStackOperation))
                    break;

                lexemeStack.Pop();
                commands.Add(GetOperation(topStackOperation).GetCommand());
            }

            lexemeStack.Push(lexeme);
        }

        //Если больше не осталось токенов на входе:
        //Пока есть токены операторы в стеке:
        //Если токен оператор на вершине стека — скобка, то в выражении присутствует незакрытая скобка.
        //Переложить оператор из стека в выходную очередь.
        protected void ProcessRemainingLexem(List<ICommand> commands, Stack<Lexeme> lexemeStack)
        {
            while (lexemeStack.Count > 0)
            {
                var lex = lexemeStack.Pop();
                if (lex.Type == LexemeType.OpenBracket)
                    throw new CalculationException("Обнаружена непарная открывающая скобка");

                //Перекидываем операции в результат
                if (IsOperation(lex))
                    commands.Add(GetOperation(lex).GetCommand());
            }
        }

        private bool IsOperation(Lexeme lexeme)
        {
            return lexeme.Type == LexemeType.BinaryOperator || lexeme.Type == LexemeType.UnaryOperator;
        }
    }
}
