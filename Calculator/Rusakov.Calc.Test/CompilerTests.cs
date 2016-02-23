using NUnit.Framework;
using Rusakov.Calc.Commands;
using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Test
{
    [TestFixture]
    class CompilerTests
    {
        [Test]
        public void ProcessNumberLexem_WithoutNumberLexem_FailProcess()
        {
            var compiller = new OpenCompiler();
            var lexeme = new Lexeme("1", LexemeType.OpenBracket);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            TestDelegate process = () => compiller.ProcessNumberLexem(lexeme, commands, stack);

            Assert.Throws<ArgumentException>(process);
        }

        [Test]
        public void ProcessNumberLexem_WithNumberLexem_NumberInOutCommands()
        {
            var compiller = new OpenCompiler();
            var lexeme = new Lexeme("20", LexemeType.Number);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            compiller.ProcessNumberLexem(lexeme, commands, stack);

            Assert.That(commands.Count, Is.EqualTo(1));
            Assert.That(commands[0], Is.TypeOf<PushCommand>());
            Assert.That((commands[0] as PushCommand).Value, Is.EqualTo(20m));

            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void ProcessNumberLexem_WithBadNumberLexem_FailProcess()
        {
            var compiller = new OpenCompiler();
            var lexeme = new Lexeme("2..0", LexemeType.Number);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            TestDelegate process = () => compiller.ProcessNumberLexem(lexeme, commands, stack);

            Assert.Throws<CalculationException>(process);
        }

        [Test]
        public void ProcessOpenBracketLexem_WithoutOpenBracketLexem_FailProcess()
        {
            var compiller = new OpenCompiler();
            var lexeme = new Lexeme("2.0", LexemeType.Number);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            TestDelegate process = () => compiller.ProcessOpenBracketLexem(lexeme, commands, stack);

            Assert.Throws<ArgumentException>(process);
        }

        [Test]
        public void ProcessOpenBracketLexem_WithOpenBracketLexem_OpenBracketInStack()
        {
            var compiller = new OpenCompiler();
            var lexeme = new Lexeme("(", LexemeType.OpenBracket);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            compiller.ProcessOpenBracketLexem(lexeme, commands, stack);

            Assert.That(commands.Count, Is.EqualTo(0));

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek(), Is.SameAs(lexeme));
        }

        [Test]
        public void ProcessCloseBracketLexem_WithoutCloseBracketLexem_FailProcess()
        {
            var compiller = new OpenCompiler();
            var lexeme = new Lexeme("2.0", LexemeType.Number);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            TestDelegate process = () => compiller.ProcessCloseBracketLexem(lexeme, commands, stack);

            Assert.Throws<ArgumentException>(process);
        }

        [Test]
        public void ProcessCloseBracketLexem_WithsCloseBracketLexem_OperatorsInOutCommands()
        {
            var operations = new IOperation[] { new MockOperation('+', true, 1) };
            var compiller = new OpenCompiler(operations);
            var lexeme = new Lexeme(")", LexemeType.CloseBracket);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();
            stack.Push(new Lexeme("(", LexemeType.OpenBracket));
            stack.Push(new Lexeme("+", LexemeType.Operator));

            compiller.ProcessCloseBracketLexem(lexeme, commands, stack);

            Assert.That(commands.Count, Is.EqualTo(1));
            Assert.That(commands[0], Is.TypeOf<MockCommand>());

            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void ProcessCloseBracketLexem_WithoutOpenBracketInStack_FailProcess()
        {
            var operations = new IOperation[] { new MockOperation('+', true, 1) };
            var compiller = new OpenCompiler(operations);
            var lexeme = new Lexeme(")", LexemeType.CloseBracket);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();
            //stack.Push(new Lexeme("(", LexemeType.OpenBracket)); без закрывающей скобки
            stack.Push(new Lexeme("+", LexemeType.Operator));

            TestDelegate process = () => compiller.ProcessCloseBracketLexem(lexeme, commands, stack);

            Assert.Throws<CalculationException>(process);
        }


        [Test]
        public void ProcessOperatorLexem_WithoutOperatorLexem_FailProcess()
        {
            var compiller = new OpenCompiler();
            var lexeme = new Lexeme("2", LexemeType.Number);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            TestDelegate process = () => compiller.ProcessOperatorLexem(lexeme, commands, stack);

            Assert.Throws<ArgumentException>(process);
        }

        [Test]
        public void ProcessOperatorLexem_WithEmptyStack_OperatorLexemInStack()
        {
            var operations = new IOperation[] { new MockOperation('+', true, 1) };
            var compiller = new OpenCompiler(operations);
            var lexeme = new Lexeme("+", LexemeType.Operator);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            compiller.ProcessOperatorLexem(lexeme, commands, stack);

            Assert.That(commands.Count, Is.EqualTo(0));

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek(), Is.SameAs(lexeme));
        }


        [Test]
        public void ProcessOperatorLexem_WithLeftAssociative_LessOrEqualPrioryInOutCommand()
        {
            var operations = new IOperation[] {
                new MockOperation('-', true, 1),
                new MockOperation('+', true, 2),
                new MockOperation('*', true, 3),
            };


            var compiller = new OpenCompiler(operations);
            var lexeme = new Lexeme("+", LexemeType.Operator);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();
            stack.Push(new Lexeme("-", LexemeType.Operator));
            stack.Push(new Lexeme("+", LexemeType.Operator));
            stack.Push(new Lexeme("*", LexemeType.Operator));

            compiller.ProcessOperatorLexem(lexeme, commands, stack);

            Assert.That(commands.Count, Is.EqualTo(2)); // * и +
            Assert.That(stack.Count, Is.EqualTo(2)); // - и +
        }

        [Test]
        public void ProcessOperatorLexem_WithRightAssociative_LessPrioryInOutCommand()
        {
            var operations = new IOperation[] {
                new MockOperation('-', false, 1),
                new MockOperation('+', false, 2),
                new MockOperation('*', false, 3),
            };

            var compiller = new OpenCompiler(operations);
            var lexeme = new Lexeme("+", LexemeType.Operator);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();
            stack.Push(new Lexeme("-", LexemeType.Operator));
            stack.Push(new Lexeme("+", LexemeType.Operator));
            stack.Push(new Lexeme("*", LexemeType.Operator));

            compiller.ProcessOperatorLexem(lexeme, commands, stack);

            Assert.That(commands.Count, Is.EqualTo(1)); // -
            Assert.That(stack.Count, Is.EqualTo(3)); // *, + и +
        }


        [Test]
        public void ProcessRemainingLexem_WithEmptyStack_Nothing()
        {
            var compiller = new OpenCompiler();
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();

            compiller.ProcessRemainingLexem(commands, stack);

            Assert.That(commands.Count, Is.EqualTo(0));
            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void ProcessRemainingLexem_WithOpenBracketInStack_FailProcess()
        {
            var compiller = new OpenCompiler();
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();
            stack.Push(new Lexeme("(", LexemeType.OpenBracket));

            TestDelegate process = () => compiller.ProcessRemainingLexem(commands, stack);

            Assert.Throws<CalculationException>(process);
        }

        [Test]
        public void ProcessRemainingLexem_WithOperatorsInStack_OperatorsInOutCommand()
        {
            var operations = new IOperation[] { new MockOperation('+', true, 1) };
            var compiller = new OpenCompiler(operations);
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();
            stack.Push(new Lexeme("+", LexemeType.Operator));
            stack.Push(new Lexeme("+", LexemeType.Operator));

            compiller.ProcessRemainingLexem(commands, stack);

            Assert.That(commands.Count, Is.EqualTo(2));
            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void ProcessRemainingLexem_WithUnknownOperator_FailProcess()
        {
            var compiller = new OpenCompiler();
            var commands = new List<ICommand>();
            var stack = new Stack<Lexeme>();
            stack.Push(new Lexeme("+", LexemeType.Operator));

            TestDelegate process = () => compiller.ProcessRemainingLexem(commands, stack);

            Assert.Throws<CalculationException>(process);
        }


        class MockOperation : IOperation
        {
            public char Operator { get; set; }

            public bool IsUnary { get; set; }

            public bool IsLeft { get; set; }

            public byte Priority { get; set; }

            public MockOperation(char op, bool isLeft, byte priory)
            {
                Operator = op;
                //IsUnary = IsUnary;
                IsLeft = isLeft;
                Priority = priory;
            }

            public ICommand GetCommand()
            {
                return new MockCommand(this);
            }
        }

        class MockCommand : ICommand
        {
            public readonly MockOperation Operation;

            public MockCommand(MockOperation operation)
            {
                Operation = operation;
            }

            public void Execute(Stack<decimal> state)
            {
                throw new NotImplementedException();
            }
        }

        class OpenCompiler : Compiler
        {
            public OpenCompiler()
            {
            }

            public OpenCompiler(IOperation[] operations)
                : base(operations)
            {
            }

            public new void ProcessNumberLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
            {
                base.ProcessNumberLexem(lexeme, commands, lexemeStack);
            }

            public new void ProcessOpenBracketLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
            {
                base.ProcessOpenBracketLexem(lexeme, commands, lexemeStack);
            }

            public new void ProcessCloseBracketLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
            {
                base.ProcessCloseBracketLexem(lexeme, commands, lexemeStack);
            }

            public new void ProcessOperatorLexem(Lexeme lexeme, List<ICommand> commands, Stack<Lexeme> lexemeStack)
            {
                base.ProcessOperatorLexem(lexeme, commands, lexemeStack);
            }

            public new void ProcessRemainingLexem(List<ICommand> commands, Stack<Lexeme> lexemeStack)
            {
                base.ProcessRemainingLexem(commands, lexemeStack);
            }

        }
    }
}
