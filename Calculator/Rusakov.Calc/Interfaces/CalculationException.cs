using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rusakov.Calc.Interfaces
{
    /// <summary>
    /// Содержит человечитаемое сообщение об ошибке вычисления результата
    /// </summary>
    [Serializable]
    public class CalculationException : Exception
    {
        public CalculationException() { }

        public CalculationException(string message) : base(message) { }

        public CalculationException(string message, Exception inner) : base(message, inner) { }

        protected CalculationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
