using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNParser
{
    public class Token
    {
        public readonly int Length;
        public readonly bool IsDigit;
        public readonly int Position;
        public readonly string Value;
        //public BinaryOperator Operation;


        /// <param name="value">Проинтерпретированное значение токена</param>
        /// <param name="position">Позиция начала токена в исходной строке</param>
        /// <param name="length">Длина токена в исходной строке. Может не совпадать с длиной <paramref name="value" /></param>
        public Token(string value, int position, int length, bool isDigit = false)
        {
            Position = position;
            Length = length;
            Value = value;
            IsDigit = isDigit;
        }

        public override bool Equals(object obj)
        {
            if ((Token)obj == null)
                return false;
            return Equals((Token)obj);
        }

        protected bool Equals(Token other)
        {
            return Length == other.Length && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Length;
                hashCode = (hashCode * 397) ^ Position;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        public int GetIndexNextToToken()
        {
            return Position + Length;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
