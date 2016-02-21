using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc
{
    class Compiler
    {
        public ICommand[] Compile(Lexeme[] lexemes)
        {
            var commands = new List<ICommand>();

            for(int i=0; i<lexemes.Length; ++i)
            {

            }

            return commands.ToArray();
        }
    }
}
