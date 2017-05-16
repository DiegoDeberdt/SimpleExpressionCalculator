using System;
using System.Collections.Generic;

namespace Deberdt.Yarp.SimpleExpressionCalculator
{
    class ExpressionParser
    {
        private List<Token<TokenType>> _tokens = new List<Token<TokenType>>();

        public Token<TokenType>[] Tokens
        {
            get { return _tokens.ToArray(); }
        }

        public ExpressionParser()
        {
        }

        public Token<TokenType>[] Parse(string expression)
        {
            _tokens.Clear();
            var scanner = new ExpressionScanner(expression);

            Token<TokenType> token;
            while ((token = scanner.GetNextToken()).TokenType != TokenType.EndOfExpression)
            {
                //Console.WriteLine(token);
                if (_tokens.Count == 0)
                {
                    if (token.TokenType == TokenType.Integer)
                    {
                        _tokens.Add(token);
                    }
                    else if (token.TokenType == TokenType.Operator)
                    {
                        if (token.Value == "+" || token.Value == "-")
                        {
                            // When the first integer is signed we insert a zero token.
                            _tokens.Add(new Token<TokenType>(TokenType.Integer, "0"));
                            _tokens.Add(token);
                        }
                        else
                        {                            
                            throw new InvalidOperationException($"The first token cannot be '*' or '/'!");
                        }
                    }
                }
                else if (_tokens.Count % 2 == 0)
                {
                    // Integers are always found on even postions in the expression
                    if (token.TokenType == TokenType.Integer)
                        _tokens.Add(token);
                    else
                        throw new InvalidOperationException($"Found {token}, expected Integer!");
                }
                else
                {
                    // Operators are always found on uneven positions in the expression
                    if (token.TokenType == TokenType.Operator)
                        _tokens.Add(token);
                    else
                        throw new InvalidOperationException($"Found {token}, expected Operator!");
                }
            }

            // A valid expression always consists of an uneven number of tokens
            if (_tokens.Count > 0 && _tokens.Count % 2 == 0) 
                throw new InvalidOperationException("The last token must be of type Integer!");

            return Tokens;
        }
    }
}
