using System;

namespace Deberdt.Yarp.SimpleExpressionCalculator
{
    public class Token<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        public Token(TEnum tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TEnum TokenType
        {
            get;
            private set;
        }

        public string Value
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return $"TOKEN({TokenType.ToString()}, \"{Value}\")";
        }
    }
}
