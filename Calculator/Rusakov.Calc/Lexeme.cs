using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Lexeme
    {
        public LexemeType Type;
        public string Value;
    }

    enum LexemeType
    {
        Operator,
        Number,
        OpenBracket,
        CloseBracket
    }
}
