using Rusakov.Calc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Lexer : ILexer
    {
        public Lexeme[] Parse(string input)
        {
            var lexemes = new List<Lexeme>();

            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];

                if (c == ' ' || c == '\t')
                    continue;

                if (IsDigit(c))
                {
                    int p = i;
                    while (p < input.Length && IsDigit(input[p]))
                        ++p;

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

        protected bool IsDigit(char dc)
        {
            return (dc >= '0' && dc <= '9') || dc == '.';
        }
    }
}
