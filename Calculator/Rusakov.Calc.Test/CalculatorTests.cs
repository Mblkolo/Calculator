using NUnit.Framework;
using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Test
{
    [TestFixture]
    class CalculatorTests
    {
        [Test]
        public void Calculate_WithoutLexem_FailProcess()
        {
            var lexer = new LexerStub(new Lexeme[0]);
            var compiler = new CompilerStub(new ICommand[0]);
            var calc = new Calculator(lexer, compiler);

            TestDelegate action = () => calc.Calculate("any");

            var exeption = Assert.Throws<CalculationException>(action);
            Assert.That(exeption.Message, Is.EqualTo("Пустое выражение"));
        }

        [Test]
        public void Calculate_WithEmptyStack_FailProcess()
        {
            var lexer = new LexerStub(new Lexeme[1]);
            var compiler = new CompilerStub(new ICommand[0]);
            var calc = new Calculator(lexer, compiler);

            TestDelegate action = () => calc.Calculate("any");

            var exeption = Assert.Throws<CalculationException>(action);
            Assert.That(exeption.Message, Is.EqualTo("В выражении не хватает чисел"));
            Assert.That(compiler.Lexemes, Is.SameAs(lexer.Lexemes));
        }

        [Test]
        public void Calculate_WithMayItemsStack_FailProcess()
        {
            var commands = new ICommand[]
            {
                new StubCommand(),
                new StubCommand()
            };

            var lexer = new LexerStub(new Lexeme[1]);
            var compiler = new CompilerStub(commands);
            var calc = new Calculator(lexer, compiler);

            TestDelegate action = () => calc.Calculate("any");

            var exeption = Assert.Throws<CalculationException>(action);
            Assert.That(exeption.Message, Is.EqualTo("В выражении не хватает операторов"));
            Assert.That(compiler.Lexemes, Is.SameAs(lexer.Lexemes));
        }


        [Test]
        public void Calculate_WithMayItemsStack_NumberReturn()
        {
            var commands = new ICommand[]
            {
                new StubCommand()
            };

            var lexer = new LexerStub(new Lexeme[1]);
            var compiler = new CompilerStub(commands);
            var calc = new Calculator(lexer, compiler);

            decimal result = calc.Calculate("any");

            Assert.That(result, Is.EqualTo(0m));
            Assert.That(compiler.Lexemes, Is.SameAs(lexer.Lexemes));            
        }

        class LexerStub : ILexer
        {
            public readonly Lexeme[] Lexemes;

            public LexerStub(Lexeme[] lexemes)
            {
                Lexemes = lexemes;
            }

            public Lexeme[] Parse(string input)
            {
                return Lexemes;
            }
        }

        class CompilerStub : ICompiler
        {
            public readonly ICommand[] Commands;

            public Lexeme[] Lexemes;

            public CompilerStub(ICommand[] commands)
            {
                Commands = commands;
            }

            public ICommand[] Process(Lexeme[] lexemes)
            {
                Lexemes = lexemes;
                return Commands;
            }
        }

        class StubCommand : ICommand
        {
            public void Execute(Stack<decimal> state)
            {
                state.Push(0);
            }
        }
    }
}
