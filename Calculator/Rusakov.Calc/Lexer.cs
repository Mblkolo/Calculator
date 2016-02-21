using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Lexer
    {
        public Lexeme[] Parse(string input)
        {
            var lexemes = new List<Lexeme>();

            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];

                if (c == ' ' || c == '\t')
                    continue;

                if (c >= '0' && c <= '9')
                {
                    bool mayBeDigit = true;
                    int p = i;
                    for (p = i; p < input.Length && mayBeDigit; ++p )
                    {
                        var dc = input[p];
                        mayBeDigit = (dc >= '0' && dc <= '9') || dc == '.';
                        ++p;
                    }

                    var value = input.Substring(i, p-i);
                    lexemes.Add(new Lexeme(value, LexemeType.Number));

                    i = p-1;
                    continue;
                }

                if(c == ')')
                {
                    lexemes.Add(new Lexeme(")", LexemeType.CloseBracket));
                    continue;
                }

                if (c == '(')
                {
                    lexemes.Add(new Lexeme("(", LexemeType.OpenBracket));
                    continue;
                }

                //Любой другой символ считается оператором
                lexemes.Add(new Lexeme(new String(c, 1), LexemeType.Operator));
                continue;
            }

            return lexemes.ToArray();
        }
    }
}
