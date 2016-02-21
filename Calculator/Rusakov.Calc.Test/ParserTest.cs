using Rusakov.Calc.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rusakov.Calc.Operations;

namespace Rusakov.Calc.Test
{
    [TestFixture]
    internal class ParserTest
    {
        //Самый сложный вопрос: где же остановиться?
        //Вычислитель выражений может поодерживать
        //Лево и право ассоциативные операторы
        //Унарные операторы, операторы из нескольких символов
        //Всякие там неопределённые поведения i++ + ++i
        //Поэтому делаем несколько допущений
        //1. Все операторы односимвольные
        //2. В выражении содержатся только числа, операторы и пробелы с табами в качестве разделителей
        //3. Унарные операторы только префиксные и правоассоциативные
        //4. Никаких фунаций (но должно быть расширяемо)

        //Выражение - это [число] или [(выражение)] или [выражение бинарный оператор выражение] или [унарный оператор выражение]

        static object[] ExpressionIsValueCases =
        {
            new object[] { "0", 0m },
            new object[] { "9", 9m },
            new object[] { "5.5", 5.5m } 
        };

        [Test, TestCaseSource("ExpressionIsValueCases")]
        public void ExpressionIsValue(string expression, decimal result)
        {
            var parser = new Parser(new Dictionary<char, IOperation>());

            List<ICommand> commands = parser.Parse(expression);

            Assert.That(commands.Count, Is.EqualTo(1));
            var command = commands[0];
            Assert.That(command, Is.TypeOf<PushCommand>());
            Assert.That((command as PushCommand).Value, Is.EqualTo(result));
        }

        //TODO заменить на тест операций
        [Test]
        public void PlusParseTest()
        {
            var operations = new Dictionary<char, IOperation> { {'+', new PlusOperation() } };
            var parser = new Parser(operations);

            List<ICommand> commands = parser.Parse("1 + 2");

            Assert.That(commands.Count, Is.EqualTo(3));
            Assert.That(commands[0], Is.TypeOf<PushCommand>());
            Assert.That(commands[1], Is.TypeOf<PushCommand>());
            Assert.That(commands[2], Is.TypeOf<PlusCommand>());
        }

        //Либо оператор op лево-ассоциативен и его приоритет меньше чем у оператора topStackOp либо равен,
        //или оператор op право-ассоциативен и его приоритет меньше чем у topStackOp
        static object[] CompareOperationCases =
        {
            new object[] { new OperationStub('+', false, true, 2), new OperationStub('-', false, true, 1), false },
            new object[] { new OperationStub('+', false, true, 2), new OperationStub('-', false, true, 2), true},
            new object[] { new OperationStub('+', false, true, 2), new OperationStub('-', false, true, 3), true},
            new object[] { new OperationStub('+', false, false, 2), new OperationStub('-', false, true, 1), false },
            new object[] { new OperationStub('+', false, false, 2), new OperationStub('-', false, true, 2), false},
            new object[] { new OperationStub('+', false, false, 2), new OperationStub('-', false, true, 3), true},
        };


        [Test, TestCaseSource("CompareOperationCases")]
        public void CompareOperation(IOperation op, IOperation topStackOp, bool result)
        {
            var parser = new PublicParser(new Dictionary<char, IOperation>());

            bool res = parser.CompareOperation(op, topStackOp);

            Assert.That(res, Is.EqualTo(result));
        }
    }


    internal class PublicParser : Parser
    {
        public PublicParser(Dictionary<char, IOperation> operations)
            : base (operations)
        {

        }

        public new bool CompareOperation(IOperation op, IOperation topStackOp)
        {
            return base.CompareOperation(op, topStackOp);
        }
    }

    internal class OperationStub : IOperation
    {
        public char Operator { get; private set; }

        public bool IsUnary { get; private set; }

        public bool IsLeft { get; private set; }

        public byte Priority { get; private set; }

        public OperationStub(char op, bool isUnary, bool isLeft, byte priory)
        {
            Operator = op;
            IsUnary = isUnary;
            IsLeft = isLeft;
            Priority = priory;
        }
        
        public ICommand GetCommand()
        {
            throw new NotImplementedException();
        }
    }
}
