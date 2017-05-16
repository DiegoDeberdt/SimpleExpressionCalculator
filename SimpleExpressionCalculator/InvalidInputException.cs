using System;

namespace Deberdt.Yarp.SimpleExpressionCalculator
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(char input, int position)
        {
            Input = input;
            Position = position;
        }

        public char Input
        {
            get;
            private set;
        }

        public int Position
        {
            get;
            private set;
        }
    }
}
