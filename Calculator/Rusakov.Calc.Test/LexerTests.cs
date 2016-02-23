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
    class LexerTests
    {
        [TestCase("123.23", LexemeType.Number)]
        [TestCase("123...23", LexemeType.Number)] //Формат числа не проблема лексера
        [TestCase("(", LexemeType.OpenBracket)]
        [TestCase(")", LexemeType.CloseBracket)]
        [TestCase("+", LexemeType.BinaryOperator)]
        [TestCase("p", LexemeType.BinaryOperator)]
        public void Parse_SimpleLexeme_LexemeEqualsInputText(string text, LexemeType type)
        {
            var lexer = new Lexer();
            
            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(1));
            Assert.That(lexemes[0].Type, Is.EqualTo(type));
            Assert.That(lexemes[0].Value, Is.EqualTo(text));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\t")]
        public void Parse_EmptyText_EmpytLexemeArray(string text)
        {
            var lexer = new Lexer();

            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(0));
        }

        [Test]
        public void Parse_NumberWithOperator_NumberAndOperatorLexems()
        {
            var lexer = new Lexer();
            var text = "1+";

            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(2));
            Assert.That(lexemes[0].Type, Is.EqualTo(LexemeType.Number));
            Assert.That(lexemes[0].Value, Is.EqualTo("1"));
            Assert.That(lexemes[1].Type, Is.EqualTo(LexemeType.BinaryOperator));
            Assert.That(lexemes[1].Value, Is.EqualTo("+"));
        }

        [Test]
        public void Parse_LongLexeme_CorrectParse()
        {
            var lexer = new Lexer();
            var text = " 1.23abc21)d) ( ) d .2";

            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(12));
            Assert.That(lexemes[0].Type, Is.EqualTo(LexemeType.Number));
            Assert.That(lexemes[0].Value, Is.EqualTo("1.23"));

            Assert.That(lexemes[1].Type, Is.EqualTo(LexemeType.BinaryOperator));
            Assert.That(lexemes[2].Type, Is.EqualTo(LexemeType.BinaryOperator));
            Assert.That(lexemes[3].Type, Is.EqualTo(LexemeType.BinaryOperator));
            Assert.That(lexemes[4].Type, Is.EqualTo(LexemeType.Number));
            Assert.That(lexemes[5].Type, Is.EqualTo(LexemeType.CloseBracket));
            Assert.That(lexemes[6].Type, Is.EqualTo(LexemeType.BinaryOperator));
            Assert.That(lexemes[7].Type, Is.EqualTo(LexemeType.CloseBracket));
            Assert.That(lexemes[8].Type, Is.EqualTo(LexemeType.OpenBracket));
            Assert.That(lexemes[9].Type, Is.EqualTo(LexemeType.CloseBracket));
            Assert.That(lexemes[10].Type, Is.EqualTo(LexemeType.BinaryOperator));
            Assert.That(lexemes[11].Type, Is.EqualTo(LexemeType.Number));
        }
    }
}
