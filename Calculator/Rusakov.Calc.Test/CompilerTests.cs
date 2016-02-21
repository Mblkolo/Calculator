using NUnit.Framework;
using Rusakov.Calc.Commands;
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
        public void ComlipeNumber()
        {
            var compiler = new Compiler();
            var lexemes = new[] { new Lexeme("23.2", LexemeType.Number) };

            var commands = compiler.Compile(lexemes);

            Assert.That(commands.Length, Is.EqualTo(1));
            Assert.That(commands[0], Is.TypeOf<PushCommand>());
            Assert.That((commands[0] as PushCommand).Value, Is.EqualTo(23.2m));
        }

        [Test]
        public void FailComlipeNumber()
        {
            var compiler = new Compiler();
            var lexemes = new[] { new Lexeme("23..2", LexemeType.Number) };

            TestDelegate commands = () => compiler.Compile(lexemes);

            Assert.Throws<ArgumentException>(commands);
        }

        [Test]
        public void ComlipeBrackets()
        {
            var compiler = new Compiler();
            var lexemes = new[] { 
                new Lexeme("(", LexemeType.OpenBracket),
                new Lexeme("1", LexemeType.Number),
                new Lexeme(")", LexemeType.CloseBracket),
            };

            var commands = compiler.Compile(lexemes);

            Assert.That(commands.Length, Is.EqualTo(1));
            Assert.That(commands[0], Is.TypeOf<PushCommand>());
            Assert.That((commands[0] as PushCommand).Value, Is.EqualTo(1m));
        }

        [Test]
        public void EmptyBracketsNotAllowed()
        {
            var compiler = new Compiler();
            var lexemes = new[] { 
                new Lexeme("(", LexemeType.OpenBracket),
                new Lexeme(")", LexemeType.CloseBracket)
            };
            TestDelegate commands = () => compiler.Compile(lexemes);

            Assert.Throws<ArgumentException>(commands);
        }

        [Test]
        public void MultiplyBracketsNotAllowed()
        {
            var compiler = new Compiler();
            var lexemes = new[] { 
                new Lexeme("(", LexemeType.OpenBracket),
                new Lexeme("(", LexemeType.OpenBracket),
                new Lexeme("1", LexemeType.Number),
                new Lexeme(")", LexemeType.CloseBracket),
                new Lexeme(")", LexemeType.CloseBracket)
            };

            TestDelegate commands = () => compiler.Compile(lexemes);

            Assert.Throws<ArgumentException>(commands);
        }

        [Test]
        public void NotBalansedCloseBrackets()
        {
            var compiler = new Compiler();
            var lexemes = new[] { 
                new Lexeme("(", LexemeType.OpenBracket),
                new Lexeme("1", LexemeType.Number),
                new Lexeme(")", LexemeType.CloseBracket),
                new Lexeme(")", LexemeType.CloseBracket)
            };

            TestDelegate commands = () => compiler.Compile(lexemes);

            Assert.Throws<ArgumentException>(commands);
        }

        [Test]
        public void NotBalansedOpenBrackets()
        {
            var compiler = new Compiler();
            var lexemes = new[] { 
                new Lexeme("(", LexemeType.OpenBracket),
                new Lexeme("(", LexemeType.OpenBracket),
                new Lexeme("1", LexemeType.Number),
                new Lexeme(")", LexemeType.CloseBracket)
            };

            TestDelegate commands = () => compiler.Compile(lexemes);

            Assert.Throws<ArgumentException>(commands);
        }

        //Либо оператор op лево-ассоциативен и его приоритет меньше чем у оператора topStackOp либо равен,
        //или оператор op право-ассоциативен и его приоритет меньше чем у topStackOp
        //static object[] CompareOperationCases =
        //{
        //    new object[] { new OperationStub('+', false, true, 2), new OperationStub('-', false, true, 1), false },
        //    new object[] { new OperationStub('+', false, true, 2), new OperationStub('-', false, true, 2), true},
        //    new object[] { new OperationStub('+', false, true, 2), new OperationStub('-', false, true, 3), true},
        //    new object[] { new OperationStub('+', false, false, 2), new OperationStub('-', false, true, 1), false },
        //    new object[] { new OperationStub('+', false, false, 2), new OperationStub('-', false, true, 2), false},
        //    new object[] { new OperationStub('+', false, false, 2), new OperationStub('-', false, true, 3), true},
        //};


        //[Test, TestCaseSource("CompareOperationCases")]
        //public void CompareOperation(IOperation op, IOperation topStackOp, bool result)
        //{
        //    var parser = new PublicParser(new Dictionary<char, IOperation>());

        //    bool res = parser.CompareOperation(op, topStackOp);

        //    Assert.That(res, Is.EqualTo(result));
        //}
    }
}
