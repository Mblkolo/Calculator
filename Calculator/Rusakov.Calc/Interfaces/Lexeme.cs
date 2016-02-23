using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Interfaces
{
    internal class Lexeme
    {
        public readonly string Value;
        public readonly LexemeType Type;

        public Lexeme(string value, LexemeType type)
        {
            Value = value;
            Type = type;
        }
    }

    internal enum LexemeType
    {
        BinaryOperator,
        UnaryOperator,
        Number,
        OpenBracket,
        CloseBracket
    }
}
