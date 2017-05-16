using Deberdt.Yarp.Automata;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Deberdt.Yarp.SimpleExpressionCalculator
{
    public enum State
    {
        Start,
        InInteger,
        Integer,
        InOperator,
        Operator,
        EndOfExpression
    }

    public enum Input
    {
        Digit,
        Operator,
        EndOfExpression
    }

    public enum TokenType
    {
        Integer,
        Operator,
        EndOfExpression
    }

    class ExpressionScanner
    {
        private string _expression;
        private char _currentChar;
        private int _position;
        private List<char> _tokenValue = new List<char>();
        private FiniteStateMachine<State, Input> _stateMachine = new FiniteStateMachine<State, Input>();

        public ExpressionScanner(string expression)
        {
            _expression = expression;

            _stateMachine.Initialize(State.Start);

            _stateMachine.WhenIn(State.Start).On(Input.EndOfExpression).Goto(State.EndOfExpression); // End-state
            _stateMachine.WhenIn(State.Start).On(Input.Digit).Goto(State.InInteger);
            _stateMachine.WhenIn(State.InInteger).On(Input.Digit);
            _stateMachine.WhenIn(State.InInteger).On(Input.Operator, Input.EndOfExpression).Goto(State.Integer); // End-state
            _stateMachine.WhenIn(State.Start).On(Input.Operator).Goto(State.InOperator);
            _stateMachine.WhenIn(State.InOperator).On(Input.Digit, Input.EndOfExpression).Goto(State.Operator); // End-state
        }

        public Token<TokenType> GetNextToken()
        {
            _tokenValue.Clear();
            _stateMachine.Reset();

            while (true)
            {
                if (_position < _expression.Length) _currentChar = _expression[_position];
                else _currentChar = '\n';

                _stateMachine.Transition(GetCharacterCategory(_currentChar));

                if (_stateMachine.State == State.Integer)
                    return new Token<TokenType>(TokenType.Integer, String.Concat(_tokenValue));
                else if (_stateMachine.State == State.Operator)
                    return new Token<TokenType>(TokenType.Operator, String.Concat(_tokenValue));
                else if (_stateMachine.State == State.EndOfExpression)
                    return new Token<TokenType>(TokenType.EndOfExpression, null);

                _tokenValue.Add(_currentChar);
                _position++;
            };
        }

        private Input GetCharacterCategory(char c)
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (Char.IsDigit(c)) return Input.Digit;
            if (c == '+' || c == '-' || c == '/' || c == '*') return Input.Operator;
            else if (c == '\n') return Input.EndOfExpression;

            throw new InvalidInputException(c, _position);
        }
    }
}
