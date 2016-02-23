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
    class CommandsTests
    {
        [Test]
        public void PushCommand_InsertValueInStack_ValueInStack()
        {
            var command = new PushCommand(20m);
            var stack = new Stack<decimal>();

            command.Execute(stack);

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek(), Is.EqualTo(20m));
        }

        [Test]
        public void PlusCommand_WithTwoNumberInStack_SumInStack()
        {
            var command = new PlusCommand();
            var stack = new Stack<decimal>();
            stack.Push(1);
            stack.Push(2);

            command.Execute(stack);

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek(), Is.EqualTo(3m));
        }

        [Test]
        public void PlusCommand_WithOneNumberInStack_FailExecute()
        {
            var command = new PlusCommand();
            var stack = new Stack<decimal>();
            stack.Push(1);

            TestDelegate action = () => command.Execute(stack);

            var exeption = Assert.Throws<CalculationException>(action);
            Assert.That(exeption.Message, Is.EqualTo("Невозможно сложить 2 числа"));
        }

        [Test]
        public void MultiplyCommand_WithTwoNumberInStack_MultiplyInStack()
        {
            var command = new MultiplyCommand();
            var stack = new Stack<decimal>();
            stack.Push(3);
            stack.Push(2);

            command.Execute(stack);

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek(), Is.EqualTo(6m));
        }

        [Test]
        public void MultiplyCommand_WithOneNumberInStack_FailExecute()
        {
            var command = new MultiplyCommand();
            var stack = new Stack<decimal>();
            stack.Push(1);

            TestDelegate action = () => command.Execute(stack);

            var exeption = Assert.Throws<CalculationException>(action);
            Assert.That(exeption.Message, Is.EqualTo("Невозможно умножить 2 числа"));
        }

        [Test]
        public void MinusCommand_WithTwoNumberInStack_DifferenceInStack()
        {
            var command = new MinusCommand();
            var stack = new Stack<decimal>();
            stack.Push(3);
            stack.Push(2);

            command.Execute(stack);

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek(), Is.EqualTo(1m));
        }

        [Test]
        public void MinusCommand_WithOneNumberInStack_FailExecute()
        {
            var command = new MinusCommand();
            var stack = new Stack<decimal>();
            stack.Push(1);

            TestDelegate action = () => command.Execute(stack);

            var exeption = Assert.Throws<CalculationException>(action);
            Assert.That(exeption.Message, Is.EqualTo("Невозможно вычесть 2 числа"));
        }

        [Test]
        public void DivideCommand_WithTwoNumberInStack_DivideInStack()
        {
            var command = new DivideCommand();
            var stack = new Stack<decimal>();
            stack.Push(3);
            stack.Push(2);

            command.Execute(stack);

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek(), Is.EqualTo(1.5m));
        }

        [Test]
        public void DivideCommand_WithOneNumberInStack_FailExecute()
        {
            var command = new DivideCommand();
            var stack = new Stack<decimal>();
            stack.Push(1);

            TestDelegate action = () => command.Execute(stack);

            var exeption = Assert.Throws<CalculationException>(action);
            Assert.That(exeption.Message, Is.EqualTo("Невозможно разделить 2 числа"));
        }
    }
}
