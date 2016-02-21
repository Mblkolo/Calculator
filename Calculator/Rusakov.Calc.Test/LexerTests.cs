﻿using NUnit.Framework;
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
        public void EmptyLexeme(string text)
        {
            var lexer = new Lexer();

            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(0));
        }

        [Test]
        public void NumberWithOperator()
        {
            var lexer = new Lexer();
            var text = "1+";

            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(2));
            Assert.That(lexemes[0].Type, Is.EqualTo(LexemeType.Number));
            Assert.That(lexemes[0].Value, Is.EqualTo("1"));
            Assert.That(lexemes[1].Type, Is.EqualTo(LexemeType.Operator));
            Assert.That(lexemes[1].Value, Is.EqualTo("+"));

        }

        [Test]
        public void LongLexeme()
        {
            var lexer = new Lexer();
            var text = " 1.23abc21)d) ( ) d .2";

            Lexeme[] lexemes = lexer.Parse(text);

            Assert.That(lexemes.Length, Is.EqualTo(12));
            Assert.That(lexemes[0].Type, Is.EqualTo(LexemeType.Number));
            Assert.That(lexemes[0].Value, Is.EqualTo("1.23"));

            Assert.That(lexemes[1].Type, Is.EqualTo(LexemeType.Operator));
            Assert.That(lexemes[2].Type, Is.EqualTo(LexemeType.Operator));
            Assert.That(lexemes[3].Type, Is.EqualTo(LexemeType.Operator));
            Assert.That(lexemes[4].Type, Is.EqualTo(LexemeType.Number));
            Assert.That(lexemes[5].Type, Is.EqualTo(LexemeType.CloseBracket));
            Assert.That(lexemes[6].Type, Is.EqualTo(LexemeType.Operator));
            Assert.That(lexemes[7].Type, Is.EqualTo(LexemeType.CloseBracket));
            Assert.That(lexemes[8].Type, Is.EqualTo(LexemeType.OpenBracket));
            Assert.That(lexemes[9].Type, Is.EqualTo(LexemeType.CloseBracket));
            Assert.That(lexemes[10].Type, Is.EqualTo(LexemeType.Operator));
            Assert.That(lexemes[11].Type, Is.EqualTo(LexemeType.Number));
        }
    }
}
