using Rusakov.Calc.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Test
{
    [TestFixture]
    public class ParserTest
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
            var parser = new Parser();

            List<ICommand> commands = parser.Parse(expression);

            Assert.That(commands.Count, Is.EqualTo(1));
            var command = commands[0];
            Assert.That(command, Is.TypeOf<PushCommand>());
            Assert.That((command as PushCommand).Value, Is.EqualTo(result));
        }


    }
}
