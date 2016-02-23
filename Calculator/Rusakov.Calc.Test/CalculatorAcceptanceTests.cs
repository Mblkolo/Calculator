using NUnit.Framework;
using Rusakov.Calc.Interfaces;
using Rusakov.Calc.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Test
{
    [TestFixture]
    class CalculatorAcceptanceTests
    {
        private Calculator _calc;

        [TestFixtureSetUp]
        public void SetUp()
        {
            var lexer = new Lexer();
            var operations = new IOperation[] 
            {
                new PlusOperation(),
                new MinusOperation(),
                new MultiplyOperation(),
                new DivideOperation(),
                new UnaryPlusOperation(),
                new UnaryMinusOperation(),
            };

            var compiler = new Compiler(operations);
            _calc = new Calculator(lexer, compiler);
        }

        public static object[] CorrectExpressionSource = 
        {
            new object[] { "5", 5m },
            new object[] { "5+2", 7m },
            new object[] { "5-2", 3m },
            new object[] { "5*2", 10m },
            new object[] { "5/2", 2.5m },
            new object[] { "(((3)))", 3m },
            new object[] { "()5()*()2()", 10m }, //пустые выражения не приводят к ошибкам
            new object[] { "1 + 2 * 3", 7m },
            new object[] { "1 * 2 + 3", 5m },
            new object[] { "3 * 2 / 3", 2m },
            new object[] { "4 / 2 / 2", 1m },
            new object[] { "-34", -34m },
            new object[] { "+-+-34", 34m },
            new object[] { "1+-2", -1m },
            new object[] { "1+(-2)", -1m },
            new object[] { "1 () + ( -2)", -1m },
        };

        [Test, TestCaseSource("CorrectExpressionSource")]
        public void CorrectExpression(string expression, decimal answer)
        {
            decimal result = _calc.Calculate(expression);

            Assert.That(result, Is.EqualTo(answer));
        }

        [TestCase("")]
        [TestCase("  \t")]
        [TestCase("(1")]
        [TestCase("1)")]
        [TestCase("((1)")]
        [TestCase("(1))")]
        [TestCase("+")]
        [TestCase("10 20")]
        [TestCase("1..34")]
        [TestCase("34 -")]
        [TestCase("1/0")]
        [TestCase("1/(1-1)")]
        [TestCase("(1+)-2")]
        [TestCase("1 () + () -2")]
        public void IncorrectExpression(string expression)
        {
            TestDelegate action = () => _calc.Calculate(expression);

             Assert.Throws<CalculationException>(action);
        }
    }
}
