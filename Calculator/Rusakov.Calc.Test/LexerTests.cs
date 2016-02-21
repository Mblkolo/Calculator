using NUnit.Framework;
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
        [TestCase("(", LexemeType.OpenBracket)]
        [TestCase(")", LexemeType.CloseBracket)]
        [TestCase("+", LexemeType.Operator)]
        [TestCase("p", LexemeType.Operator)]
        public void SimpleLexeme(string text, LexemeType type)
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
        public void LexerEmptyString(string text)
        {
            var lexer = new Lexer();

            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(0));
        }
    }
}
